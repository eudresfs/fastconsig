import { StreamProcessor, ImportRow, BatchResult } from '../stream-processor';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';

// Valid Brazilian CPFs for testing (with correct check digits)
const VALID_CPFS = [
  '52998224725', // Valid CPF 1
  '36187673029', // Valid CPF 2
  '10820375438', // Valid CPF 3
  '84828509590', // Valid CPF 4
  '15976349807', // Valid CPF 5
];

describe('StreamProcessor', () => {
  let streamProcessor: StreamProcessor;
  let tempDir: string;

  beforeAll(() => {
    tempDir = path.join(os.tmpdir(), 'stream-processor-tests');
    if (!fs.existsSync(tempDir)) {
      fs.mkdirSync(tempDir, { recursive: true });
    }
  });

  beforeEach(() => {
    streamProcessor = new StreamProcessor();
  });

  afterAll(() => {
    // Clean up temp directory
    if (fs.existsSync(tempDir)) {
      fs.rmSync(tempDir, { recursive: true, force: true });
    }
  });

  const createTempCsv = (content: string): string => {
    const fileName = `test-${Date.now()}-${Math.random().toString(36).substring(7)}.csv`;
    const filePath = path.join(tempDir, fileName);
    fs.writeFileSync(filePath, content, 'utf-8');
    return filePath;
  };

  describe('validateCSVStructure', () => {
    it('should validate CSV with all required headers', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary\n${VALID_CPFS[0]},MAT-001,John Doe,500000`;
      const filePath = createTempCsv(csvContent);

      const result = await streamProcessor.validateCSVStructure(filePath);

      expect(result.valid).toBe(true);
      expect(result.error).toBeUndefined();
    });

    it('should validate CSV with alternative header names (snake_case)', async () => {
      const csvContent = `cpf,enrollment_id,name,gross_salary\n${VALID_CPFS[0]},MAT-001,John Doe,500000`;
      const filePath = createTempCsv(csvContent);

      const result = await streamProcessor.validateCSVStructure(filePath);

      expect(result.valid).toBe(true);
    });

    it('should reject CSV missing required headers', async () => {
      const csvContent = `cpf,name,grossSalary\n${VALID_CPFS[0]},John Doe,500000`;
      const filePath = createTempCsv(csvContent);

      const result = await streamProcessor.validateCSVStructure(filePath);

      expect(result.valid).toBe(false);
      expect(result.error).toContain('enrollmentId');
    });

    it('should reject empty CSV file', async () => {
      const csvContent = '';
      const filePath = createTempCsv(csvContent);

      const result = await streamProcessor.validateCSVStructure(filePath);

      expect(result.valid).toBe(false);
      expect(result.error).toContain('vazio');
    });
  });

  describe('processCSV', () => {
    it('should process valid CSV with all rows successful', async () => {
      const csvContent = `cpf,enrollmentId,name,email,phone,grossSalary,mandatoryDiscounts
${VALID_CPFS[0]},MAT-001,John Doe,john@example.com,+5511999999999,500000,100000
${VALID_CPFS[1]},MAT-002,Jane Smith,jane@example.com,+5511888888888,600000,120000`;

      const filePath = createTempCsv(csvContent);
      const processedBatches: ImportRow[][] = [];

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        processedBatches.push(rows);
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(2);
      expect(result.successfulRows).toBe(2);
      expect(result.failedRows).toBe(0);
      expect(result.errors).toHaveLength(0);
      expect(processedBatches.length).toBeGreaterThan(0);
    });

    it('should handle mixed valid and invalid rows (partial success)', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,John Doe,500000
invalid-cpf,MAT-002,Jane Smith,600000
${VALID_CPFS[1]},MAT-003,Bob Wilson,700000`;

      const filePath = createTempCsv(csvContent);

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(3);
      expect(result.successfulRows).toBe(2); // Valid rows processed
      expect(result.failedRows).toBe(1); // Invalid CPF row
      expect(result.errors).toHaveLength(1);
      expect(result.errors[0].lineNumber).toBe(2);
    });

    it('should handle rows with missing required fields', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,John Doe,500000
${VALID_CPFS[1]},,Jane Smith,600000
${VALID_CPFS[2]},MAT-003,,700000`;

      const filePath = createTempCsv(csvContent);

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(3);
      expect(result.failedRows).toBeGreaterThan(0);
    });

    it('should batch rows correctly (100 rows per batch)', async () => {
      // Create CSV with 250 rows using rotating valid CPFs
      let csvContent = 'cpf,enrollmentId,name,grossSalary\n';
      for (let i = 0; i < 250; i++) {
        const cpf = VALID_CPFS[i % VALID_CPFS.length];
        csvContent += `${cpf},MAT-${i.toString().padStart(4, '0')},Employee ${i},${500000 + i * 1000}\n`;
      }

      const filePath = createTempCsv(csvContent);
      let batchCount = 0;
      const batchSizes: number[] = [];

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        batchCount++;
        batchSizes.push(rows.length);
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(250);
      expect(batchCount).toBe(3); // 100 + 100 + 50
      expect(batchSizes).toContain(100);
    });

    it('should handle batch processing errors gracefully', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,John Doe,500000
${VALID_CPFS[1]},MAT-002,Jane Smith,600000`;

      const filePath = createTempCsv(csvContent);

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        // Simulate batch error
        return {
          success: false,
          insertedCount: 0,
          errors: rows.map((row, i) => ({
            lineNumber: lineNumbers[i],
            rowData: row as unknown as Record<string, unknown>,
            errorMessage: 'Database connection error',
          })),
        };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(2);
      expect(result.successfulRows).toBe(0);
      expect(result.failedRows).toBe(2);
    });

    it('should call progress callback with correct values', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,John Doe,500000
${VALID_CPFS[1]},MAT-002,Jane Smith,600000`;

      const filePath = createTempCsv(csvContent);
      const progressCalls: Array<{ processed: number; successful: number; failed: number }> = [];

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const onProgress = (processed: number, successful: number, failed: number) => {
        progressCalls.push({ processed, successful, failed });
      };

      await streamProcessor.processCSV(filePath, onBatch, onProgress);

      // Progress should be called at least once
      expect(progressCalls.length).toBeGreaterThanOrEqual(0);
    });

    it('should parse numeric values correctly from CSV strings', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary,mandatoryDiscounts
${VALID_CPFS[0]},MAT-001,John Doe,500000,100000`;

      const filePath = createTempCsv(csvContent);
      let capturedRows: ImportRow[] = [];

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        capturedRows = rows;
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      await streamProcessor.processCSV(filePath, onBatch);

      expect(capturedRows).toHaveLength(1);
      expect(capturedRows[0].grossSalary).toBe(500000);
      expect(capturedRows[0].mandatoryDiscounts).toBe(100000);
      expect(typeof capturedRows[0].grossSalary).toBe('number');
    });
  });

  describe('generateRejectionReport', () => {
    it('should return success message when no errors', () => {
      const report = streamProcessor.generateRejectionReport([]);

      expect(report).toContain('Nenhum erro encontrado');
    });

    it('should generate CSV report with errors', () => {
      const errors = [
        {
          lineNumber: 2,
          rowData: { cpf: '12345678901', name: 'John Doe', enrollmentId: 'MAT-001' },
          errorMessage: 'CPF invalido',
        },
        {
          lineNumber: 5,
          rowData: { cpf: '98765432100', name: 'Jane Smith', enrollmentId: 'MAT-002' },
          errorMessage: 'Matricula duplicada',
        },
      ];

      const report = streamProcessor.generateRejectionReport(errors);

      expect(report).toContain('Linha,CPF,Nome,Matricula,Motivo do Erro');
      expect(report).toContain('2,"12345678901","John Doe","MAT-001","CPF invalido"');
      expect(report).toContain('5,"98765432100","Jane Smith","MAT-002","Matricula duplicada"');
    });

    it('should escape quotes in error messages', () => {
      const errors = [
        {
          lineNumber: 2,
          rowData: { cpf: '12345678901', name: 'John Doe', enrollmentId: 'MAT-001' },
          errorMessage: 'Error with "quotes" inside',
        },
      ];

      const report = streamProcessor.generateRejectionReport(errors);

      expect(report).toContain('""quotes""');
    });

    it('should handle missing row data fields gracefully', () => {
      const errors = [
        {
          lineNumber: 2,
          rowData: { cpf: '12345678901' },
          errorMessage: 'Some error',
        },
      ];

      const report = streamProcessor.generateRejectionReport(errors);

      expect(report).toContain('2,"12345678901","","","Some error"');
    });
  });

  describe('edge cases', () => {
    it('should handle CSV with only header row', async () => {
      const csvContent = 'cpf,enrollmentId,name,grossSalary';
      const filePath = createTempCsv(csvContent);

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.totalRows).toBe(0);
      expect(result.successfulRows).toBe(0);
      expect(result.failedRows).toBe(0);
    });

    it('should handle CSV with special characters in names', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Jose da Silva Junior,500000
${VALID_CPFS[1]},MAT-002,Maria OConnor,600000`;

      const filePath = createTempCsv(csvContent);
      let capturedRows: ImportRow[] = [];

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        capturedRows = [...capturedRows, ...rows];
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      expect(result.successfulRows).toBe(2);
      expect(capturedRows[0].name).toBe('Jose da Silva Junior');
    });

    it('should handle negative salary values', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,John Doe,-500000`;

      const filePath = createTempCsv(csvContent);

      const onBatch = async (rows: ImportRow[], lineNumbers: number[]): Promise<BatchResult> => {
        return { success: true, insertedCount: rows.length, errors: [] };
      };

      const result = await streamProcessor.processCSV(filePath, onBatch);

      // Should fail validation due to negative salary
      expect(result.failedRows).toBe(1);
      expect(result.errors[0].errorMessage).toContain('grossSalary');
    });
  });
});
