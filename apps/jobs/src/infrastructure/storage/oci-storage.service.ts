import { Injectable, Logger, OnModuleInit } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';
import {
  StorageServiceInterface,
  UploadOptions,
  UploadResult,
  SignedUrlOptions,
} from './storage.interface';

/**
 * Oracle Cloud Infrastructure Object Storage Service
 *
 * Required environment variables:
 * - OCI_TENANCY_OCID
 * - OCI_USER_OCID
 * - OCI_FINGERPRINT
 * - OCI_PRIVATE_KEY_PATH or OCI_PRIVATE_KEY (inline)
 * - OCI_REGION
 * - OCI_NAMESPACE
 * - OCI_BUCKET_NAME
 */
@Injectable()
export class OciStorageService implements StorageServiceInterface, OnModuleInit {
  private readonly logger = new Logger(OciStorageService.name);
  private readonly tempDir: string;

  // OCI Configuration
  private readonly tenancyId: string;
  private readonly userId: string;
  private readonly fingerprint: string;
  private readonly privateKeyPath: string;
  private readonly region: string;
  private readonly namespace: string;
  private readonly bucketName: string;

  // OCI Client (lazy loaded)
  private objectStorageClient: any = null;

  constructor(private readonly configService: ConfigService) {
    this.tenancyId = this.configService.getOrThrow<string>('OCI_TENANCY_OCID');
    this.userId = this.configService.getOrThrow<string>('OCI_USER_OCID');
    this.fingerprint = this.configService.getOrThrow<string>('OCI_FINGERPRINT');
    this.privateKeyPath = this.configService.getOrThrow<string>('OCI_PRIVATE_KEY_PATH');
    this.region = this.configService.getOrThrow<string>('OCI_REGION');
    this.namespace = this.configService.getOrThrow<string>('OCI_NAMESPACE');
    this.bucketName = this.configService.getOrThrow<string>('OCI_BUCKET_NAME');

    this.tempDir = path.join(os.tmpdir(), 'fastconsig-oci-storage');

    // Ensure temp directory exists
    if (!fs.existsSync(this.tempDir)) {
      fs.mkdirSync(this.tempDir, { recursive: true });
    }
  }

  async onModuleInit(): Promise<void> {
    await this.initializeClient();
  }

  private async initializeClient(): Promise<void> {
    try {
      // Dynamic import of OCI SDK (may not be installed in all environments)
      const oci = await import('oci-sdk');

      const privateKey = fs.readFileSync(this.privateKeyPath, 'utf-8');

      const provider = new oci.common.SimpleAuthenticationDetailsProvider(
        this.tenancyId,
        this.userId,
        this.fingerprint,
        privateKey,
        null, // passphrase
        oci.common.Region.fromRegionId(this.region),
      );

      this.objectStorageClient = new oci.objectstorage.ObjectStorageClient({
        authenticationDetailsProvider: provider,
      });

      this.logger.log(`OCI Object Storage client initialized for bucket: ${this.bucketName}`);
    } catch (error: any) {
      this.logger.error(`Failed to initialize OCI client: ${error.message}`);
      throw new Error(`OCI Storage initialization failed: ${error.message}`);
    }
  }

  private ensureClientInitialized(): void {
    if (!this.objectStorageClient) {
      throw new Error('OCI Object Storage client not initialized');
    }
  }

  async upload(buffer: Buffer, storagePath: string, options?: UploadOptions): Promise<UploadResult> {
    this.ensureClientInitialized();

    const contentType = options?.contentType || 'application/octet-stream';

    try {
      const putObjectRequest = {
        namespaceName: this.namespace,
        bucketName: this.bucketName,
        objectName: storagePath,
        putObjectBody: buffer,
        contentLength: buffer.length,
        contentType,
        opcMeta: options?.metadata,
      };

      await this.objectStorageClient.putObject(putObjectRequest);

      this.logger.log(`File uploaded to OCI: ${storagePath} (${buffer.length} bytes)`);

      return {
        path: storagePath,
        size: buffer.length,
        contentType,
      };
    } catch (error: any) {
      this.logger.error(`Failed to upload to OCI: ${error.message}`);
      throw new Error(`Upload failed: ${error.message}`);
    }
  }

  async download(storagePath: string): Promise<Buffer> {
    this.ensureClientInitialized();

    try {
      const getObjectRequest = {
        namespaceName: this.namespace,
        bucketName: this.bucketName,
        objectName: storagePath,
      };

      const response = await this.objectStorageClient.getObject(getObjectRequest);

      // Read stream to buffer
      const chunks: Buffer[] = [];
      for await (const chunk of response.value) {
        chunks.push(Buffer.from(chunk));
      }
      const buffer = Buffer.concat(chunks);

      this.logger.debug(`File downloaded from OCI: ${storagePath} (${buffer.length} bytes)`);

      return buffer;
    } catch (error: any) {
      this.logger.error(`Failed to download from OCI: ${error.message}`);
      throw new Error(`Download failed: ${error.message}`);
    }
  }

  async getSignedUrl(storagePath: string, options?: SignedUrlOptions): Promise<string> {
    this.ensureClientInitialized();

    const expiresInSeconds = options?.expiresInSeconds || 3600; // 1 hour default
    const expirationTime = new Date(Date.now() + expiresInSeconds * 1000);

    try {
      const createPreauthenticatedRequestRequest = {
        namespaceName: this.namespace,
        bucketName: this.bucketName,
        createPreauthenticatedRequestDetails: {
          name: `download-${storagePath}-${Date.now()}`,
          objectName: storagePath,
          accessType: 'ObjectRead',
          timeExpires: expirationTime,
        },
      };

      const response = await this.objectStorageClient.createPreauthenticatedRequest(
        createPreauthenticatedRequestRequest,
      );

      const parUrl = `https://objectstorage.${this.region}.oraclecloud.com${response.preauthenticatedRequest.accessUri}`;

      this.logger.debug(`Generated pre-signed URL for: ${storagePath}`);

      return parUrl;
    } catch (error: any) {
      this.logger.error(`Failed to generate signed URL: ${error.message}`);
      throw new Error(`Signed URL generation failed: ${error.message}`);
    }
  }

  async delete(storagePath: string): Promise<void> {
    this.ensureClientInitialized();

    try {
      const deleteObjectRequest = {
        namespaceName: this.namespace,
        bucketName: this.bucketName,
        objectName: storagePath,
      };

      await this.objectStorageClient.deleteObject(deleteObjectRequest);

      this.logger.log(`File deleted from OCI: ${storagePath}`);
    } catch (error: any) {
      // Ignore "not found" errors during delete
      if (error.statusCode === 404) {
        this.logger.debug(`File not found during delete (ignoring): ${storagePath}`);
        return;
      }
      this.logger.error(`Failed to delete from OCI: ${error.message}`);
      throw new Error(`Delete failed: ${error.message}`);
    }
  }

  async exists(storagePath: string): Promise<boolean> {
    this.ensureClientInitialized();

    try {
      const headObjectRequest = {
        namespaceName: this.namespace,
        bucketName: this.bucketName,
        objectName: storagePath,
      };

      await this.objectStorageClient.headObject(headObjectRequest);
      return true;
    } catch (error: any) {
      if (error.statusCode === 404) {
        return false;
      }
      throw error;
    }
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
   * Clean up temp files
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
