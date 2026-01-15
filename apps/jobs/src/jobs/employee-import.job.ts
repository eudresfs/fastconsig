import { Processor, WorkerHost, OnWorkerEvent } from '@nestjs/bullmq';
import { Job } from 'bullmq';
import { Inject, Logger } from '@nestjs/common';
import { db, employees, sql } from '@fast-consig/database';
import { StreamProcessor, ImportRow, BatchResult } from '../processors/stream-processor';
import { MarginCalculationService } from '../services/margin-calculation.service';
import { STORAGE_SERVICE, StorageServiceInterface } from '../infrastructure/storage';
import * as fs from 'fs';

export interface ImportJobData {
  tenantId: string;
  userId: string;
  filePath: string; // Storage path (not local path)
  fileName: string;
  originalFileName: string;
  rejectionReportContent?: string; // Populated after processing if there are errors
  rejectionReportUrl?: string; // Pre-signed URL for rejection report download
}

export interface ImportJobResult {
  jobId: string;
  totalRows: number;
  successfulRows: number;
  failedRows: number;
  processingTimeMs: number;
  rejectionReportPath: string | null;
  rejectionReportUrl: string | null;
}

export const EMPLOYEE_IMPORT_QUEUE = 'employee-import';

@Processor(EMPLOYEE_IMPORT_QUEUE)
export class EmployeeImportJob extends WorkerHost {
  private readonly logger = new Logger(EmployeeImportJob.name);

  constructor(
    private readonly streamProcessor: StreamProcessor,
    private readonly marginCalculationService: MarginCalculationService,
    @Inject(STORAGE_SERVICE) private readonly storageService: StorageServiceInterface,
  ) {
    super();
  }

  async process(job: Job<ImportJobData>): Promise<ImportJobResult> {
    const { tenantId, userId, filePath, fileName, originalFileName } = job.data;

    this.logger.log(`Starting import job ${job.id} for tenant ${tenantId}, file: ${originalFileName}`);

    let localFilePath: string | null = null;

    try {
      // Download file from storage to temp location
      this.logger.log(`Downloading file from storage: ${filePath}`);
      localFilePath = await this.storageService.downloadToTemp(filePath);
      this.logger.log(`File downloaded to: ${localFilePath}`);

      // Validate CSV structure before processing
      const structureValidation = await this.streamProcessor.validateCSVStructure(localFilePath);
      if (!structureValidation.valid) {
        throw new Error(`Estrutura do CSV inválida: ${structureValidation.error}`);
      }

      // Set RLS context for this tenant
      await db.execute(sql`SELECT set_config('app.current_tenant_id', ${tenantId}, TRUE)`);

      // Process CSV with progress tracking
      const result = await this.streamProcessor.processCSV(
        localFilePath,
        async (batch: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
          // Refresh RLS context before each batch to prevent context loss in long transactions
          await db.execute(sql`SELECT set_config('app.current_tenant_id', ${tenantId}, TRUE)`);
          return this.processBatch(batch, lineNumbers, tenantId, job);
        },
        (processed, successful, failed) => {
          // Update job progress
          job.updateProgress({
            percentage: 0,
            totalRows: 0,
            processedRows: processed,
            successfulRows: successful,
            failedRows: failed,
          });
        },
      );

      // Generate and upload rejection report if there are errors (H3 fix)
      let rejectionReportPath: string | null = null;
      let rejectionReportUrl: string | null = null;

      if (result.errors.length > 0) {
        const reportContent = this.streamProcessor.generateRejectionReport(result.errors);

        // Build storage path for rejection report
        const now = new Date();
        rejectionReportPath = `reports/${tenantId}/${now.getFullYear()}/${(now.getMonth() + 1).toString().padStart(2, '0')}/rejection-report-${job.id}.csv`;

        // Upload rejection report to storage
        await this.storageService.uploadFromString(reportContent, rejectionReportPath, {
          contentType: 'text/csv; charset=utf-8',
        });

        // Generate pre-signed URL for download (valid for 24 hours)
        rejectionReportUrl = await this.storageService.getSignedUrl(rejectionReportPath, {
          expiresInSeconds: 24 * 60 * 60,
        });

        this.logger.log(`Rejection report uploaded: ${rejectionReportPath}`);

        // Store report content in job data for backward compatibility
        await job.updateData({
          ...job.data,
          rejectionReportContent: reportContent,
          rejectionReportUrl,
        });
      }

      // Delete source CSV from storage after successful processing
      try {
        await this.storageService.delete(filePath);
        this.logger.log(`Source file deleted from storage: ${filePath}`);
      } catch (deleteError: any) {
        this.logger.warn(`Failed to delete source file from storage: ${deleteError.message}`);
      }

      this.logger.log(
        `Import job ${job.id} completed: ${result.successfulRows}/${result.totalRows} successful in ${result.processingTimeMs}ms`,
      );

      return {
        jobId: job.id as string,
        totalRows: result.totalRows,
        successfulRows: result.successfulRows,
        failedRows: result.failedRows,
        processingTimeMs: result.processingTimeMs,
        rejectionReportPath,
        rejectionReportUrl,
      };
    } finally {
      // Cleanup: Always delete local temp file after processing
      if (localFilePath) {
        this.cleanupTempFile(localFilePath);
      }
    }
  }

  /**
   * Cleanup temporary file after processing
   */
  private cleanupTempFile(filePath: string): void {
    try {
      if (fs.existsSync(filePath)) {
        fs.unlinkSync(filePath);
        this.logger.debug(`Temp file cleaned up: ${filePath}`);
      }
    } catch (error: any) {
      this.logger.warn(`Failed to cleanup temp file ${filePath}: ${error.message}`);
    }
  }

  private async processBatch(
    batch: ImportRow[],
    lineNumbers: number[],
    tenantId: string,
    job: Job<ImportJobData>,
  ): Promise<BatchResult> {
    const errors: BatchResult['errors'] = [];
    let insertedCount = 0;

    try {
      // Calculate margins for each employee in batch
      const employeesWithMargins = await Promise.all(
        batch.map(async (row, index) => {
          try {
            const marginResult = await this.marginCalculationService.calculateAvailableMargin(
              row.grossSalary,
              row.mandatoryDiscounts || 0,
              tenantId,
            );

            return {
              id: this.generateId(),
              tenantId,
              cpf: row.cpf,
              enrollmentId: row.enrollmentId,
              name: row.name,
              email: row.email || null,
              phone: row.phone || null,
              grossSalary: row.grossSalary,
              mandatoryDiscounts: row.mandatoryDiscounts || 0,
              availableMargin: marginResult.availableMargin,
              usedMargin: 0,
              version: 1,
            };
          } catch (error: any) {
            errors.push({
              lineNumber: lineNumbers[index],
              rowData: row as unknown as Record<string, unknown>,
              errorMessage: `Erro ao calcular margem: ${error.message}`,
            });
            return null;
          }
        }),
      );

      // Filter out failed margin calculations
      const validEmployees = employeesWithMargins.filter((e): e is NonNullable<typeof e> => e !== null);

      if (validEmployees.length === 0) {
        return { success: false, insertedCount: 0, errors };
      }

      // Batch insert into database with ON CONFLICT handling
      // Using individual inserts with error handling to support partial success
      for (let i = 0; i < validEmployees.length; i++) {
        const employee = validEmployees[i];
        const originalIndex = employeesWithMargins.indexOf(employee);
        const lineNumber = lineNumbers[originalIndex >= 0 ? originalIndex : i];

        try {
          await db.insert(employees).values(employee);
          insertedCount++;
        } catch (error: any) {
          // Handle duplicate errors gracefully
          if (error.code === '23505') {
            // PostgreSQL unique constraint violation
            const constraintMessage = error.constraint?.includes('cpf')
              ? 'CPF ja cadastrado no sistema'
              : error.constraint?.includes('enrollment')
                ? 'Matricula ja cadastrada no sistema'
                : 'Registro duplicado encontrado';

            errors.push({
              lineNumber,
              rowData: batch[originalIndex >= 0 ? originalIndex : i] as unknown as Record<string, unknown>,
              errorMessage: constraintMessage,
            });
          } else {
            errors.push({
              lineNumber,
              rowData: batch[originalIndex >= 0 ? originalIndex : i] as unknown as Record<string, unknown>,
              errorMessage: error.message || 'Erro ao inserir no banco de dados',
            });
          }
        }
      }

      return { success: true, insertedCount, errors };
    } catch (error: any) {
      this.logger.error(`Batch insert error: ${error.message}`, error.stack);
      // Mark entire batch as failed
      batch.forEach((row, index) => {
        errors.push({
          lineNumber: lineNumbers[index],
          rowData: row as unknown as Record<string, unknown>,
          errorMessage: error.message || 'Erro ao processar lote',
        });
      });
      return { success: false, insertedCount: 0, errors };
    }
  }

  /**
   * Generate a unique ID for the employee (cuid-like format)
   */
  private generateId(): string {
    const timestamp = Date.now().toString(36);
    const randomPart = Math.random().toString(36).substring(2, 15);
    return `emp_${timestamp}${randomPart}`;
  }

  @OnWorkerEvent('completed')
  onCompleted(job: Job<ImportJobData>) {
    this.logger.log(`Job ${job.id} completed successfully`);
  }

  @OnWorkerEvent('failed')
  onFailed(job: Job<ImportJobData>, error: Error) {
    this.logger.error(`Job ${job.id} failed: ${error.message}`, error.stack);
  }

  @OnWorkerEvent('progress')
  onProgress(job: Job<ImportJobData>, progress: number | object) {
    if (typeof progress === 'object') {
      const p = progress as { percentage: number; processedRows: number; totalRows: number };
      this.logger.debug(`Job ${job.id} progress: ${p.percentage}% (${p.processedRows}/${p.totalRows})`);
    }
  }
}
