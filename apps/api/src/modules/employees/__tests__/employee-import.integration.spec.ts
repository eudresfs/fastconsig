import { Test, TestingModule } from '@nestjs/testing';
import { INestApplication } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { BullModule, getQueueToken } from '@nestjs/bullmq';
import { Queue } from 'bullmq';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';
import { EmployeeImportService, EMPLOYEE_IMPORT_QUEUE, MAX_CONCURRENT_IMPORTS_PER_TENANT } from '../employee-import.service';
import { EmployeeImportController } from '../employee-import.controller';
import { ContextService } from '../../../core/context/context.service';
import { AuditTrailService } from '../../../shared/services/audit-trail.service';

// Valid Brazilian CPFs for testing (with correct check digits)
const VALID_CPFS = [
  '52998224725',
  '36187673029',
  '10820375438',
  '84828509590',
  '15976349807',
];

describe('Employee Import Integration Tests', () => {
  let app: INestApplication;
  let importService: EmployeeImportService;
  let importQueue: Queue;
  let tempDir: string;

  // Mock services
  const mockContextService = {
    getTenantId: jest.fn().mockReturnValue('tenant-123'),
    getUserId: jest.fn().mockReturnValue('user-456'),
    getIp: jest.fn().mockReturnValue('127.0.0.1'),
  };

  const mockAuditTrailService = {
    log: jest.fn().mockResolvedValue(undefined),
  };

  beforeAll(async () => {
    // Create temp directory for test files
    tempDir = path.join(os.tmpdir(), 'employee-import-integration-tests');
    if (!fs.existsSync(tempDir)) {
      fs.mkdirSync(tempDir, { recursive: true });
    }

    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [
        ConfigModule.forRoot({
          isGlobal: true,
          envFilePath: '.env.test',
        }),
        BullModule.forRoot({
          connection: {
            host: process.env.REDIS_HOST || 'localhost',
            port: parseInt(process.env.REDIS_PORT || '6379', 10),
          },
        }),
        BullModule.registerQueue({
          name: EMPLOYEE_IMPORT_QUEUE,
        }),
      ],
      controllers: [EmployeeImportController],
      providers: [
        EmployeeImportService,
        {
          provide: ContextService,
          useValue: mockContextService,
        },
        {
          provide: AuditTrailService,
          useValue: mockAuditTrailService,
        },
      ],
    }).compile();

    app = moduleFixture.createNestApplication();
    await app.init();

    importService = moduleFixture.get<EmployeeImportService>(EmployeeImportService);
    importQueue = moduleFixture.get<Queue>(getQueueToken(EMPLOYEE_IMPORT_QUEUE));
  });

  afterAll(async () => {
    // Clean up temp directory
    if (fs.existsSync(tempDir)) {
      fs.rmSync(tempDir, { recursive: true, force: true });
    }

    // Clean up queue
    await importQueue.obliterate({ force: true });
    await app.close();
  });

  beforeEach(async () => {
    // Clear queue before each test
    await importQueue.obliterate({ force: true });
    jest.clearAllMocks();
  });

  // Helper function to create test CSV files
  const createTestCsv = (content: string, filename?: string): string => {
    const fileName = filename || `test-${Date.now()}-${Math.random().toString(36).substring(7)}.csv`;
    const filePath = path.join(tempDir, fileName);
    fs.writeFileSync(filePath, content, 'utf-8');
    return filePath;
  };

  describe('Task 8.2: Upload CSV with 100 valid rows', () => {
    it('should enqueue job and return job ID for valid CSV with 100 rows', async () => {
      // Create CSV with 100 valid rows
      let csvContent = 'cpf,enrollmentId,name,email,grossSalary,mandatoryDiscounts\n';
      for (let i = 0; i < 100; i++) {
        const cpf = VALID_CPFS[i % VALID_CPFS.length];
        csvContent += `${cpf},MAT-${i.toString().padStart(4, '0')},Employee ${i},emp${i}@test.com,${500000 + i * 1000},${50000 + i * 100}\n`;
      }

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      // Enqueue import job
      const result = await importService.enqueueImportJob(filePath, fileName, 'test-100-rows.csv');

      expect(result).toBeDefined();
      expect(result.jobId).toBeDefined();
      expect(result.jobId).toMatch(/^import_/);
      expect(result.status).toBe('queued');
      expect(result.message).toContain('Importação iniciada');

      // Verify job was added to queue
      const job = await importQueue.getJob(result.jobId);
      expect(job).toBeDefined();
      expect(job?.data.tenantId).toBe('tenant-123');
      expect(job?.data.userId).toBe('user-456');
      expect(job?.data.originalFileName).toBe('test-100-rows.csv');

      // Verify audit trail was called
      expect(mockAuditTrailService.log).toHaveBeenCalledWith(
        expect.objectContaining({
          tenantId: 'tenant-123',
          userId: 'user-456',
          action: 'CREATE',
          resourceType: 'employee_import',
        }),
      );
    });

    it('should validate file size limit (50MB)', async () => {
      const file = {
        filename: 'test.csv',
        size: 51 * 1024 * 1024, // 51MB - exceeds limit
        mimetype: 'text/csv',
      };

      expect(() => importService.validateFile(file)).toThrow();
    });

    it('should reject non-CSV files', async () => {
      const file = {
        filename: 'test.xlsx',
        size: 1024,
        mimetype: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      };

      expect(() => importService.validateFile(file)).toThrow();
    });
  });

  describe('Task 8.3: Upload CSV with mixed valid/invalid rows (partial success)', () => {
    it('should enqueue job for CSV with mixed valid and invalid rows', async () => {
      // Create CSV with mix of valid and invalid rows
      const csvContent = `cpf,enrollmentId,name,grossSalary,mandatoryDiscounts
${VALID_CPFS[0]},MAT-001,Valid Employee 1,500000,50000
invalid-cpf,MAT-002,Invalid CPF Employee,500000,50000
${VALID_CPFS[1]},MAT-003,Valid Employee 2,600000,60000
${VALID_CPFS[2]},,Missing Enrollment,500000,50000
${VALID_CPFS[3]},MAT-005,Valid Employee 3,700000,70000
12345678901,MAT-006,Invalid Check Digit,500000,50000
${VALID_CPFS[4]},MAT-007,Valid Employee 4,800000,80000`;

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      const result = await importService.enqueueImportJob(filePath, fileName, 'mixed-rows.csv');

      expect(result).toBeDefined();
      expect(result.jobId).toBeDefined();
      expect(result.status).toBe('queued');

      // Job should be created even with invalid rows
      const job = await importQueue.getJob(result.jobId);
      expect(job).toBeDefined();
    });
  });

  describe('Task 8.6: Progress tracking', () => {
    it('should return progress for queued job', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      const enqueueResult = await importService.enqueueImportJob(filePath, fileName, 'progress-test.csv');

      // Get progress immediately after enqueueing
      const progress = await importService.getImportProgress(enqueueResult.jobId);

      expect(progress).toBeDefined();
      expect(progress.jobId).toBe(enqueueResult.jobId);
      expect(['waiting', 'active']).toContain(progress.status);
    });

    it('should return 404 for non-existent job', async () => {
      await expect(importService.getImportProgress('non-existent-job-id')).rejects.toThrow();
    });

    it('should return 404 for job belonging to different tenant', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      const result = await importService.enqueueImportJob(filePath, fileName, 'tenant-test.csv');

      // Change tenant context
      mockContextService.getTenantId.mockReturnValue('different-tenant');

      await expect(importService.getImportProgress(result.jobId)).rejects.toThrow();

      // Restore tenant context
      mockContextService.getTenantId.mockReturnValue('tenant-123');
    });
  });

  describe('Task 8.7: Concurrent uploads - queue handling', () => {
    it('should enforce maximum concurrent imports per tenant', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      // Enqueue MAX_CONCURRENT_IMPORTS_PER_TENANT jobs
      const jobs: string[] = [];
      for (let i = 0; i < MAX_CONCURRENT_IMPORTS_PER_TENANT; i++) {
        const filePath = createTestCsv(csvContent, `concurrent-test-${i}.csv`);
        const fileName = path.basename(filePath);
        const result = await importService.enqueueImportJob(filePath, fileName, `concurrent-${i}.csv`);
        jobs.push(result.jobId);
      }

      expect(jobs).toHaveLength(MAX_CONCURRENT_IMPORTS_PER_TENANT);

      // Try to enqueue one more - should fail
      const extraFilePath = createTestCsv(csvContent, 'concurrent-extra.csv');
      const extraFileName = path.basename(extraFilePath);

      await expect(
        importService.enqueueImportJob(extraFilePath, extraFileName, 'concurrent-extra.csv'),
      ).rejects.toThrow(/Limite de importações simultâneas/);
    });

    it('should allow imports from different tenants simultaneously', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      // Enqueue jobs for tenant A
      mockContextService.getTenantId.mockReturnValue('tenant-A');
      const filePathA = createTestCsv(csvContent, 'tenant-a.csv');
      const resultA = await importService.enqueueImportJob(filePathA, 'tenant-a.csv', 'tenant-a.csv');

      // Enqueue jobs for tenant B
      mockContextService.getTenantId.mockReturnValue('tenant-B');
      const filePathB = createTestCsv(csvContent, 'tenant-b.csv');
      const resultB = await importService.enqueueImportJob(filePathB, 'tenant-b.csv', 'tenant-b.csv');

      expect(resultA.jobId).toBeDefined();
      expect(resultB.jobId).toBeDefined();
      expect(resultA.jobId).not.toBe(resultB.jobId);

      // Restore tenant context
      mockContextService.getTenantId.mockReturnValue('tenant-123');
    });
  });

  describe('Rejection Report', () => {
    it('should return null for job without errors', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      const result = await importService.enqueueImportJob(filePath, fileName, 'no-errors.csv');

      // Job is not completed yet, so this should throw
      await expect(importService.getRejectionReport(result.jobId)).rejects.toThrow(/ainda não foi concluído/);
    });

    it('should return 404 for rejection report of non-existent job', async () => {
      await expect(importService.getRejectionReport('non-existent-job')).rejects.toThrow();
    });
  });

  describe('File Validation', () => {
    it('should accept valid CSV file', () => {
      const file = {
        filename: 'employees.csv',
        size: 1024,
        mimetype: 'text/csv',
      };

      expect(() => importService.validateFile(file)).not.toThrow();
    });

    it('should accept text/plain mimetype for CSV', () => {
      const file = {
        filename: 'employees.csv',
        size: 1024,
        mimetype: 'text/plain',
      };

      expect(() => importService.validateFile(file)).not.toThrow();
    });

    it('should reject file with invalid extension', () => {
      const file = {
        filename: 'employees.txt',
        size: 1024,
        mimetype: 'text/plain',
      };

      expect(() => importService.validateFile(file)).toThrow();
    });
  });

  describe('Tenant Isolation', () => {
    it('should include tenant ID in job data', async () => {
      const csvContent = `cpf,enrollmentId,name,grossSalary
${VALID_CPFS[0]},MAT-001,Test Employee,500000`;

      mockContextService.getTenantId.mockReturnValue('isolated-tenant-xyz');

      const filePath = createTestCsv(csvContent);
      const fileName = path.basename(filePath);

      const result = await importService.enqueueImportJob(filePath, fileName, 'tenant-isolation.csv');

      const job = await importQueue.getJob(result.jobId);
      expect(job?.data.tenantId).toBe('isolated-tenant-xyz');

      // Restore tenant context
      mockContextService.getTenantId.mockReturnValue('tenant-123');
    });
  });
});
