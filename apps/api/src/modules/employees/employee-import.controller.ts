import {
  Controller,
  Post,
  Get,
  Param,
  Res,
  UseGuards,
  Logger,
  BadRequestException,
  HttpCode,
  HttpStatus,
  Inject,
} from '@nestjs/common';
import { FastifyReply, FastifyRequest } from 'fastify';
import { ClerkAuthGuard } from '../../core/auth/clerk-auth.guard';
import { EmployeeImportService } from './employee-import.service';
import { IMPORT_ERROR_CODES, IMPORT_ERROR_MESSAGES } from './dto';
import { Req } from '@nestjs/common';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';
import * as readline from 'readline';
import { createId } from '@paralleldrive/cuid2';
import { STORAGE_SERVICE, StorageServiceInterface } from '../../infrastructure/storage';
import { ContextService } from '../../core/context/context.service';

@Controller('api/v1/employees/import')
@UseGuards(ClerkAuthGuard)
export class EmployeeImportController {
  private readonly logger = new Logger(EmployeeImportController.name);
  private readonly tempDir: string;

  constructor(
    private readonly employeeImportService: EmployeeImportService,
    @Inject(STORAGE_SERVICE) private readonly storageService: StorageServiceInterface,
    private readonly contextService: ContextService,
  ) {
    this.tempDir = path.join(os.tmpdir(), 'fastconsig-imports');
    if (!fs.existsSync(this.tempDir)) {
      fs.mkdirSync(this.tempDir, { recursive: true });
    }
  }

  /**
   * POST /api/v1/employees/import
   * Upload a CSV file for bulk employee import
   */
  @Post()
  @HttpCode(HttpStatus.ACCEPTED)
  async uploadCsv(@Req() request: FastifyRequest) {
    try {
      // Get multipart file from request
      const data = await (request as any).file();

      if (!data) {
        throw new BadRequestException({
          code: IMPORT_ERROR_CODES.INVALID_FILE,
          message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.INVALID_FILE],
        });
      }

      // Validate file
      this.employeeImportService.validateFile({
        filename: data.filename,
        size: data.file.bytesRead || 0,
        mimetype: data.mimetype,
      });

      // Create temp directory for uploads if it doesn't exist
      const uploadDir = path.join(os.tmpdir(), 'fastconsig-imports');
      if (!fs.existsSync(uploadDir)) {
        fs.mkdirSync(uploadDir, { recursive: true });
      }

      // Generate unique filename
      const fileName = `${createId()}_${Date.now()}.csv`;
      const localFilePath = path.join(uploadDir, fileName);

      // Save file to temp location
      const writeStream = fs.createWriteStream(localFilePath);
      await new Promise<void>((resolve, reject) => {
        data.file.pipe(writeStream);
        data.file.on('end', () => resolve());
        data.file.on('error', (err: Error) => reject(err));
        writeStream.on('error', (err) => reject(err));
      });

      // Get actual file size after writing
      const stats = fs.statSync(localFilePath);
      if (stats.size > 50 * 1024 * 1024) {
        // Clean up file
        fs.unlinkSync(localFilePath);
        throw new BadRequestException({
          code: IMPORT_ERROR_CODES.FILE_TOO_LARGE,
          message: IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.FILE_TOO_LARGE],
        });
      }

      this.logger.log(`File uploaded: ${data.filename} -> ${localFilePath} (${stats.size} bytes)`);

      // Validate CSV structure before enqueueing (M6 fix)
      const structureValidation = await this.validateCSVStructure(localFilePath);
      if (!structureValidation.valid) {
        // Clean up file on validation failure
        fs.unlinkSync(localFilePath);
        throw new BadRequestException({
          code: IMPORT_ERROR_CODES.INVALID_FORMAT,
          message: structureValidation.error || IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.INVALID_FORMAT],
        });
      }

      // Upload to cloud storage (H2 fix)
      const tenantId = this.contextService.getTenantId() || 'unknown';
      const now = new Date();
      const storagePath = `uploads/${tenantId}/${now.getFullYear()}/${(now.getMonth() + 1).toString().padStart(2, '0')}/${fileName}`;

      const fileBuffer = fs.readFileSync(localFilePath);
      const uploadResult = await this.storageService.upload(fileBuffer, storagePath, {
        contentType: 'text/csv',
        metadata: {
          originalFileName: data.filename,
          tenantId,
        },
      });

      this.logger.log(`File uploaded to storage: ${storagePath}`);

      // Clean up local temp file after upload to storage
      fs.unlinkSync(localFilePath);

      // Enqueue import job with storage path
      const result = await this.employeeImportService.enqueueImportJob(
        storagePath,  // Now using storage path instead of local path
        fileName,
        data.filename,
      );

      return {
        success: true,
        data: result,
      };
    } catch (error: any) {
      this.logger.error(`Upload error: ${error.message}`, error.stack);

      if (error instanceof BadRequestException) {
        throw error;
      }

      throw new BadRequestException({
        code: IMPORT_ERROR_CODES.INVALID_FILE,
        message: error.message || IMPORT_ERROR_MESSAGES[IMPORT_ERROR_CODES.INVALID_FILE],
      });
    }
  }

  /**
   * GET /api/v1/employees/import/:jobId/progress
   * Get import job progress
   */
  @Get(':jobId/progress')
  async getProgress(@Param('jobId') jobId: string) {
    const progress = await this.employeeImportService.getImportProgress(jobId);

    return {
      success: true,
      data: progress,
    };
  }

  /**
   * GET /api/v1/employees/import/:jobId/report
   * Download rejection report for completed import
   */
  @Get(':jobId/report')
  async getReport(@Param('jobId') jobId: string, @Res() reply: FastifyReply) {
    const report = await this.employeeImportService.getRejectionReport(jobId);

    if (!report) {
      return reply.status(200).send({
        success: true,
        data: {
          message: 'Nenhum erro encontrado - todas as linhas foram processadas com sucesso.',
        },
      });
    }

    // Return as CSV file download
    reply
      .header('Content-Type', 'text/csv; charset=utf-8')
      .header('Content-Disposition', `attachment; filename="rejection-report-${jobId}.csv"`)
      .send(report);
  }

  /**
   * Validate CSV structure before enqueueing job (M6 & M4 fix)
   * Checks: UTF-8 encoding, required headers
   */
  private async validateCSVStructure(filePath: string): Promise<{ valid: boolean; error?: string }> {
    return new Promise((resolve) => {
      const requiredHeaders = ['cpf', 'enrollmentid', 'name', 'grosssalary'];
      const alternativeHeaders: Record<string, string> = {
        enrollmentid: 'enrollment_id',
        grosssalary: 'gross_salary',
        mandatorydiscounts: 'mandatory_discounts',
      };

      // Check UTF-8 encoding by reading first few bytes
      try {
        const buffer = Buffer.alloc(4);
        const fd = fs.openSync(filePath, 'r');
        fs.readSync(fd, buffer, 0, 4, 0);
        fs.closeSync(fd);

        // Check for UTF-8 BOM or valid UTF-8 start bytes
        const hasBOM = buffer[0] === 0xef && buffer[1] === 0xbb && buffer[2] === 0xbf;
        const isValidUtf8Start = buffer[0] < 0x80 || (buffer[0] >= 0xc0 && buffer[0] <= 0xf7);

        if (!hasBOM && !isValidUtf8Start) {
          resolve({
            valid: false,
            error: 'Arquivo deve estar codificado em UTF-8',
          });
          return;
        }
      } catch (error: any) {
        resolve({
          valid: false,
          error: `Erro ao verificar encoding do arquivo: ${error.message}`,
        });
        return;
      }

      // Read first line to check headers
      const rl = readline.createInterface({
        input: fs.createReadStream(filePath, { encoding: 'utf-8' }),
        crlfDelay: Infinity,
      });

      let headerChecked = false;

      rl.on('line', (line) => {
        if (!headerChecked) {
          headerChecked = true;
          rl.close();

          const headers = line.split(',').map((h) => h.trim().toLowerCase().replace(/['"]/g, ''));

          const missingHeaders: string[] = [];
          for (const required of requiredHeaders) {
            const hasHeader =
              headers.includes(required) ||
              (alternativeHeaders[required] && headers.includes(alternativeHeaders[required]));

            if (!hasHeader) {
              missingHeaders.push(required);
            }
          }

          if (missingHeaders.length > 0) {
            resolve({
              valid: false,
              error: `Colunas obrigatórias ausentes: ${missingHeaders.join(', ')}`,
            });
          } else {
            resolve({ valid: true });
          }
        }
      });

      rl.on('error', (error) => {
        resolve({
          valid: false,
          error: `Erro ao ler arquivo CSV: ${error.message}`,
        });
      });

      rl.on('close', () => {
        if (!headerChecked) {
          resolve({
            valid: false,
            error: 'Arquivo CSV vazio ou sem cabeçalho',
          });
        }
      });
    });
  }
}
