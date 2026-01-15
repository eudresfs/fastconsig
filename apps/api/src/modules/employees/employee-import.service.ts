import { Injectable, Logger, NotFoundException, BadRequestException } from '@nestjs/common';
import { InjectQueue } from '@nestjs/bullmq';
import { Queue, Job } from 'bullmq';
import { createId } from '@paralleldrive/cuid2';
import { ContextService } from '../../core/context/context.service';
import { AuditTrailService } from '../../shared/services/audit-trail.service';
import {
  ImportJobResponseDto,
  ImportProgressDto,
  ImportResultDto,
  IMPORT_ERROR_CODES,
  IMPORT_ERROR_MESSAGES,
  MAX_FILE_SIZE,
} from './dto';

// Queue name must match the one in jobs app
export const EMPLOYEE_IMPORT_QUEUE = 'employee-import';

// Maximum concurrent imports per tenant (H7 requirement)
export const MAX_CONCURRENT_IMPORTS_PER_TENANT = 3;

export interface ImportJobData {
  tenantId: string;
  userId: string;
  filePath: string;
  fileName: string;
  originalFileName: string;
}

export interface ImportJobResult {
  jobId: string;
  totalRows: number;
  successfulRows: number;
  failedRows: number;
  processingTimeMs: number;
  rejectionReportPath: string | null;
}

@Injectable()
export class EmployeeImportService {
  private readonly logger = new Logger(EmployeeImportService.name);

  constructor(
    @InjectQueue(EMPLOYEE_IMPORT_QUEUE) private readonly importQueue: Queue<ImportJobData, ImportJobResult>,
    private readonly contextService: ContextService,
    private readonly auditTrailService: AuditTrailService,
  ) {}

  /**
   * Validate uploaded file before processing
   */
  validateFile(file: { filename: string; size: number; mimetype: string }): void {
    // Check file size
    if (file.size > MAX_FILE_SIZE) {
      throw new BadRequestException({
        code: IMPORT_ERROR_CODES.FILE_TOO_LARGE,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.FILE_TOO_LARGE],
      });
    }

    // Check file extension
    const extension = file.filename.split('.').pop()?.toLowerCase();
    if (extension !== 'csv') {
      throw new BadRequestException({
        code: IMPORT_ERROR_CODES.INVALID_FORMAT,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.INVALID_FORMAT],
      });
    }

    // Check mimetype (CSV can have multiple valid mimetypes)
    const validMimetypes = ['text/csv', 'text/plain', 'application/csv', 'application/vnd.ms-excel'];
    if (!validMimetypes.includes(file.mimetype)) {
      this.logger.warn(`Unexpected mimetype for CSV file: ${file.mimetype}, allowing anyway`);
    }
  }

  /**
   * Enqueue an import job
   */
  async enqueueImportJob(
    filePath: string,
    fileName: string,
    originalFileName: string,
  ): Promise<ImportJobResponseDto> {
    const tenantId = this.contextService.getTenantId();
    const userId = this.contextService.getUserId();

    if (!tenantId || !userId) {
      throw new BadRequestException('Tenant ID and User ID are required');
    }

    // Check concurrency limit per tenant (H7 fix)
    const activeJobsCount = await this.countActiveJobsForTenant(tenantId);
    if (activeJobsCount >= MAX_CONCURRENT_IMPORTS_PER_TENANT) {
      throw new BadRequestException({
        code: 'CONCURRENT_LIMIT_EXCEEDED',
        message: `Limite de importações simultâneas atingido. Máximo permitido: ${MAX_CONCURRENT_IMPORTS_PER_TENANT}. Aguarde a conclusão de uma importação antes de iniciar outra.`,
      });
    }

    const jobId = `import_${createId()}`;

    const job = await this.importQueue.add(
      'process-import',
      {
        tenantId,
        userId,
        filePath,
        fileName,
        originalFileName,
      },
      {
        jobId,
        attempts: 3,
        backoff: {
          type: 'exponential',
          delay: 2000,
        },
      },
    );

    // Audit trail
    await this.auditTrailService.log({
      tenantId,
      userId,
      action: 'CREATE',
      resourceType: 'employee_import',
      resourceId: jobId,
      ipAddress: this.contextService.getIp() || 'unknown',
      metadata: {
        fileName: originalFileName,
        filePath,
      },
    });

    this.logger.log(`Import job ${jobId} enqueued for tenant ${tenantId}, file: ${originalFileName}`);

    return {
      jobId: job.id as string,
      status: 'queued',
      message: 'Importacao iniciada. Use o jobId para acompanhar o progresso.',
    };
  }

  /**
   * Get import job progress
   */
  async getImportProgress(jobId: string): Promise<ImportProgressDto | ImportResultDto> {
    const tenantId = this.contextService.getTenantId();

    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    const job = await this.importQueue.getJob(jobId);

    if (!job) {
      throw new NotFoundException({
        code: IMPORT_ERROR_CODES.JOB_NOT_FOUND,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.JOB_NOT_FOUND],
      });
    }

    // Verify tenant ownership
    if (job.data.tenantId !== tenantId) {
      throw new NotFoundException({
        code: IMPORT_ERROR_CODES.JOB_NOT_FOUND,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.JOB_NOT_FOUND],
      });
    }

    const state = await job.getState();
    const progress = job.progress as {
      percentage?: number;
      totalRows?: number;
      processedRows?: number;
      successfulRows?: number;
      failedRows?: number;
    } | number;

    if (state === 'completed') {
      const result = job.returnvalue as ImportJobResult;
      return {
        jobId: job.id as string,
        status: 'completed',
        result: {
          totalRows: result?.totalRows || 0,
          successfulRows: result?.successfulRows || 0,
          failedRows: result?.failedRows || 0,
          processingTimeMs: result?.processingTimeMs || 0,
          rejectionReportUrl: result?.rejectionReportPath || undefined,
        },
        completedAt: job.finishedOn ? new Date(job.finishedOn) : new Date(),
      };
    }

    if (state === 'failed') {
      return {
        jobId: job.id as string,
        status: 'failed',
        result: {
          totalRows: 0,
          successfulRows: 0,
          failedRows: 0,
          processingTimeMs: 0,
        },
        completedAt: job.finishedOn ? new Date(job.finishedOn) : new Date(),
      };
    }

    // Job is still in progress
    const progressData = typeof progress === 'object' ? progress : { percentage: 0 };

    return {
      jobId: job.id as string,
      status: state === 'active' ? 'active' : 'waiting',
      progress: {
        percentage: progressData.percentage || 0,
        totalRows: progressData.totalRows || 0,
        processedRows: progressData.processedRows || 0,
        successfulRows: progressData.successfulRows || 0,
        failedRows: progressData.failedRows || 0,
      },
      startedAt: job.processedOn ? new Date(job.processedOn) : undefined,
      estimatedCompletionAt: this.estimateCompletion(job, progressData),
    };
  }

  /**
   * Get rejection report for a completed import job
   */
  async getRejectionReport(jobId: string): Promise<string | null> {
    const tenantId = this.contextService.getTenantId();

    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    const job = await this.importQueue.getJob(jobId);

    if (!job) {
      throw new NotFoundException({
        code: IMPORT_ERROR_CODES.JOB_NOT_FOUND,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.JOB_NOT_FOUND],
      });
    }

    // Verify tenant ownership
    if (job.data.tenantId !== tenantId) {
      throw new NotFoundException({
        code: IMPORT_ERROR_CODES.JOB_NOT_FOUND,
        message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.JOB_NOT_FOUND],
      });
    }

    const state = await job.getState();
    if (state !== 'completed') {
      throw new BadRequestException('O job ainda nao foi concluido');
    }

    // Return rejection report content from job data
    const jobData = job.data as ImportJobData & { rejectionReportContent?: string };
    return jobData.rejectionReportContent || null;
  }

  /**
   * Estimate completion time based on current progress
   */
  private estimateCompletion(
    job: Job<ImportJobData, ImportJobResult>,
    progress: { percentage?: number; processedRows?: number; totalRows?: number },
  ): Date | undefined {
    if (!job.processedOn || !progress.percentage || progress.percentage <= 0) {
      return undefined;
    }

    const elapsed = Date.now() - job.processedOn;
    const estimatedTotal = (elapsed / progress.percentage) * 100;
    const remaining = estimatedTotal - elapsed;

    return new Date(Date.now() + remaining);
  }

  /**
   * Count active (waiting + active) jobs for a specific tenant
   * Used to enforce concurrency limits (H7 requirement)
   */
  private async countActiveJobsForTenant(tenantId: string): Promise<number> {
    // Get all jobs in waiting and active states
    const [waitingJobs, activeJobs] = await Promise.all([
      this.importQueue.getJobs(['waiting', 'delayed']),
      this.importQueue.getJobs(['active']),
    ]);

    // Filter by tenant ID
    const allJobs = [...waitingJobs, ...activeJobs];
    const tenantJobs = allJobs.filter((job) => job.data.tenantId === tenantId);

    this.logger.debug(`Active import jobs for tenant ${tenantId}: ${tenantJobs.length}`);

    return tenantJobs.length;
  }
}
