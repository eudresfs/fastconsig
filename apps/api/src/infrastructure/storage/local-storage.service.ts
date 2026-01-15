import { Injectable, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';
import * as crypto from 'crypto';
import {
  StorageServiceInterface,
  UploadOptions,
  UploadResult,
  SignedUrlOptions,
} from './storage.interface';

/**
 * Local Storage Service for development
 * Uses file system storage instead of cloud storage
 */
@Injectable()
export class LocalStorageService implements StorageServiceInterface {
  private readonly logger = new Logger(LocalStorageService.name);
  private readonly baseDir: string;
  private readonly tempDir: string;

  constructor(private readonly configService: ConfigService) {
    // Use configurable base directory or default to ./storage
    this.baseDir = this.configService.get<string>('LOCAL_STORAGE_PATH') ||
      path.join(process.cwd(), 'storage');
    this.tempDir = path.join(os.tmpdir(), 'fastconsig-storage');

    // Ensure directories exist
    this.ensureDirectoryExists(this.baseDir);
    this.ensureDirectoryExists(this.tempDir);

    this.logger.log(`Local storage initialized at: ${this.baseDir}`);
  }

  private ensureDirectoryExists(dir: string): void {
    if (!fs.existsSync(dir)) {
      fs.mkdirSync(dir, { recursive: true });
    }
  }

  private getFullPath(storagePath: string): string {
    return path.join(this.baseDir, storagePath);
  }

  async upload(buffer: Buffer, storagePath: string, options?: UploadOptions): Promise<UploadResult> {
    const fullPath = this.getFullPath(storagePath);
    const dir = path.dirname(fullPath);

    this.ensureDirectoryExists(dir);

    fs.writeFileSync(fullPath, buffer);

    this.logger.debug(`File uploaded to local storage: ${storagePath} (${buffer.length} bytes)`);

    return {
      path: storagePath,
      size: buffer.length,
      contentType: options?.contentType || 'application/octet-stream',
    };
  }

  async download(storagePath: string): Promise<Buffer> {
    const fullPath = this.getFullPath(storagePath);

    if (!fs.existsSync(fullPath)) {
      throw new Error(`File not found: ${storagePath}`);
    }

    const buffer = fs.readFileSync(fullPath);
    this.logger.debug(`File downloaded from local storage: ${storagePath} (${buffer.length} bytes)`);

    return buffer;
  }

  async getSignedUrl(storagePath: string, options?: SignedUrlOptions): Promise<string> {
    // For local storage, we generate a simple file:// URL or a mock signed URL
    // In production, this would be a real pre-signed URL
    const fullPath = this.getFullPath(storagePath);

    if (!fs.existsSync(fullPath)) {
      throw new Error(`File not found: ${storagePath}`);
    }

    // Generate a mock signed URL for local development
    // Format: /api/v1/storage/download/{token}
    const token = crypto.randomBytes(32).toString('hex');
    const expiresAt = Date.now() + (options?.expiresInSeconds || 3600) * 1000;

    // In a real implementation, you would store this token mapping
    // For now, just return a local file path or mock URL
    const mockUrl = `file://${fullPath}`;

    this.logger.debug(`Generated mock signed URL for: ${storagePath}`);

    return mockUrl;
  }

  async delete(storagePath: string): Promise<void> {
    const fullPath = this.getFullPath(storagePath);

    if (fs.existsSync(fullPath)) {
      fs.unlinkSync(fullPath);
      this.logger.debug(`File deleted from local storage: ${storagePath}`);
    }
  }

  async exists(storagePath: string): Promise<boolean> {
    const fullPath = this.getFullPath(storagePath);
    return fs.existsSync(fullPath);
  }

  async downloadToTemp(storagePath: string): Promise<string> {
    const buffer = await this.download(storagePath);
    const fileName = path.basename(storagePath);
    const tempPath = path.join(this.tempDir, `${Date.now()}_${fileName}`);

    fs.writeFileSync(tempPath, buffer);
    this.logger.debug(`File downloaded to temp: ${storagePath} -> ${tempPath}`);

    return tempPath;
  }

  async uploadFromString(content: string, storagePath: string, options?: UploadOptions): Promise<UploadResult> {
    const buffer = Buffer.from(content, 'utf-8');
    return this.upload(buffer, storagePath, {
      contentType: 'text/plain; charset=utf-8',
      ...options,
    });
  }

  /**
   * Clean up temp files (call periodically or on shutdown)
   */
  cleanupTempFiles(maxAgeMs: number = 24 * 60 * 60 * 1000): void {
    if (!fs.existsSync(this.tempDir)) return;

    const now = Date.now();
    const files = fs.readdirSync(this.tempDir);

    for (const file of files) {
      const filePath = path.join(this.tempDir, file);
      const stats = fs.statSync(filePath);

      if (now - stats.mtimeMs > maxAgeMs) {
        fs.unlinkSync(filePath);
        this.logger.debug(`Cleaned up old temp file: ${file}`);
      }
    }
  }
}
