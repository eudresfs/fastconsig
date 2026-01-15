import { createReadStream } from 'fs';
import * as csvParser from 'csv-parser';
import { Injectable, Logger } from '@nestjs/common';
import { createEmployeeSchema } from '@fast-consig/shared';

export interface ImportRow {
  cpf: string;
  enrollmentId: string;
  name: string;
  email?: string;
  phone?: string;
  grossSalary: number;
  mandatoryDiscounts?: number;
}

export interface ImportError {
  lineNumber: number;
  rowData: Record<string, unknown>;
  errorMessage: string;
}

export interface ImportResult {
  totalRows: number;
  successfulRows: number;
  failedRows: number;
  errors: ImportError[];
  processingTimeMs: number;
}

export interface BatchResult {
  success: boolean;
  insertedCount: number;
  errors: ImportError[];
}

@Injectable()
export class StreamProcessor {
  private readonly logger = new Logger(StreamProcessor.name);
  private readonly BATCH_SIZE = 100;

  /**
   * Process a CSV file using streams for memory efficiency
   * @param filePath - Path to the CSV file
   * @param onBatch - Callback to process each batch of valid rows
   * @param onProgress - Optional callback for progress updates
   */
  async processCSV(
    filePath: string,
    onBatch: (rows: ImportRow[], lineNumbers: number[]) => Promise<BatchResult>,
    onProgress?: (processed: number, successful: number, failed: number) => void,
  ): Promise<ImportResult> {
    const startTime = Date.now();
    let totalRows = 0;
    let successfulRows = 0;
    const errors: ImportError[] = [];

    let currentBatch: ImportRow[] = [];
    let currentLineNumbers: number[] = [];
    let lineNumber = 0;

    return new Promise((resolve, reject) => {
      const stream = createReadStream(filePath, { encoding: 'utf-8' })
        .pipe((csvParser as any).default ? (csvParser as any).default() : (csvParser as any)());

      // Handle backpressure properly with async processing
      let processing = false;
      const pendingRows: Array<{ row: Record<string, string>; line: number }> = [];

      const processPendingRows = async () => {
        if (processing) return;
        processing = true;

        while (pendingRows.length > 0) {
          const { row, line } = pendingRows.shift()!;

          // Parse numeric values from CSV strings
          const parsedRow = {
            cpf: row.cpf?.trim() || '',
            enrollmentId: row.enrollmentId?.trim() || row.enrollment_id?.trim() || '',
            name: row.name?.trim() || '',
            email: row.email?.trim() || undefined,
            phone: row.phone?.trim() || undefined,
            grossSalary: parseInt(row.grossSalary || row.gross_salary || '0', 10),
            mandatoryDiscounts: parseInt(row.mandatoryDiscounts || row.mandatory_discounts || '0', 10),
          };

          // Validate row with Zod schema
          const validation = createEmployeeSchema.safeParse(parsedRow);

          if (!validation.success) {
            // Invalid row - log error and skip
            errors.push({
              lineNumber: line,
              rowData: row,
              errorMessage: validation.error.errors.map(e => `${e.path.join('.')}: ${e.message}`).join('; '),
            });
          } else {
            // Valid row - add to batch
            currentBatch.push(validation.data as ImportRow);
            currentLineNumbers.push(line);

            // Process batch when it reaches BATCH_SIZE
            if (currentBatch.length >= this.BATCH_SIZE) {
              const batchToProcess = [...currentBatch];
              const linesToProcess = [...currentLineNumbers];
              currentBatch = [];
              currentLineNumbers = [];

              try {
                const batchResult = await onBatch(batchToProcess, linesToProcess);
                successfulRows += batchResult.insertedCount;
                errors.push(...batchResult.errors);

                // Update progress
                if (onProgress) {
                  onProgress(totalRows, successfulRows, errors.length);
                }
              } catch (error: any) {
                // Batch failed - mark all rows as failed
                batchToProcess.forEach((failedRow, index) => {
                  errors.push({
                    lineNumber: linesToProcess[index],
                    rowData: failedRow as unknown as Record<string, unknown>,
                    errorMessage: error.message || 'Erro ao inserir no banco de dados',
                  });
                });
              }
            }
          }
        }

        processing = false;
      };

      stream.on('data', (row: Record<string, string>) => {
        lineNumber++;
        totalRows++;
        pendingRows.push({ row, line: lineNumber });

        // Process in next tick to allow backpressure
        setImmediate(processPendingRows);
      });

      stream.on('end', async () => {
        // Wait for pending rows to be processed
        await processPendingRows();

        // Process remaining rows in batch
        if (currentBatch.length > 0) {
          try {
            const batchResult = await onBatch(currentBatch, currentLineNumbers);
            successfulRows += batchResult.insertedCount;
            errors.push(...batchResult.errors);
          } catch (error: any) {
            currentBatch.forEach((failedRow, index) => {
              errors.push({
                lineNumber: currentLineNumbers[index],
                rowData: failedRow as unknown as Record<string, unknown>,
                errorMessage: error.message || 'Erro ao inserir no banco de dados',
              });
            });
          }
        }

        const result: ImportResult = {
          totalRows,
          successfulRows,
          failedRows: errors.length,
          errors,
          processingTimeMs: Date.now() - startTime,
        };

        this.logger.log(`Import completed: ${successfulRows}/${totalRows} rows successful in ${result.processingTimeMs}ms`);
        resolve(result);
      });

      stream.on('error', (error: Error) => {
        this.logger.error('CSV parsing error', error);
        reject(error);
      });
    });
  }

  /**
   * Generate rejection report CSV from import errors
   */
  generateRejectionReport(errors: ImportError[]): string {
    if (errors.length === 0) {
      return 'Nenhum erro encontrado - todas as linhas foram processadas com sucesso.';
    }

    const header = 'Linha,CPF,Nome,Matricula,Motivo do Erro\n';
    const rows = errors.map(error => {
      const { lineNumber, rowData, errorMessage } = error;
      const cpf = (rowData as any).cpf || '';
      const name = (rowData as any).name || '';
      const enrollmentId = (rowData as any).enrollmentId || (rowData as any).enrollment_id || '';
      // Escape quotes in error message for CSV
      const escapedError = errorMessage.replace(/"/g, '""');
      return `${lineNumber},"${cpf}","${name}","${enrollmentId}","${escapedError}"`;
    }).join('\n');

    return header + rows;
  }

  /**
   * Validate CSV file structure before processing
   */
  async validateCSVStructure(filePath: string): Promise<{ valid: boolean; error?: string }> {
    return new Promise((resolve) => {
      const requiredHeaders = ['cpf', 'enrollmentId', 'name', 'grossSalary'];
      const alternativeHeaders: Record<string, string> = {
        enrollmentId: 'enrollment_id',
        grossSalary: 'gross_salary',
        mandatoryDiscounts: 'mandatory_discounts',
      };

      let headersChecked = false;

      const stream = createReadStream(filePath, { encoding: 'utf-8' })
        .pipe((csvParser as any).default ? (csvParser as any).default() : (csvParser as any)());

      stream.on('headers', (headers: string[]) => {
        headersChecked = true;
        const normalizedHeaders = headers.map(h => h.trim().toLowerCase());

        const missingHeaders: string[] = [];
        for (const required of requiredHeaders) {
          const hasHeader = normalizedHeaders.includes(required.toLowerCase()) ||
            (alternativeHeaders[required] && normalizedHeaders.includes(alternativeHeaders[required].toLowerCase()));

          if (!hasHeader) {
            missingHeaders.push(required);
          }
        }

        if (missingHeaders.length > 0) {
          stream.destroy();
          resolve({
            valid: false,
            error: `Colunas obrigatorias ausentes: ${missingHeaders.join(', ')}`,
          });
        }
      });

      stream.on('data', () => {
        // Just need to check headers, destroy after first row
        if (headersChecked) {
          stream.destroy();
          resolve({ valid: true });
        }
      });

      stream.on('error', (error: Error) => {
        resolve({
          valid: false,
          error: `Erro ao ler arquivo CSV: ${error.message}`,
        });
      });

      stream.on('end', () => {
        if (!headersChecked) {
          resolve({
            valid: false,
            error: 'Arquivo CSV vazio ou sem cabecalho',
          });
        } else {
          resolve({ valid: true });
        }
      });
    });
  }
}
