/**
 * Storage Service Interface and Types
 * Provides abstraction for file storage operations
 */

export interface UploadOptions {
  contentType?: string;
  metadata?: Record<string, string>;
}

export interface UploadResult {
  path: string;
  size: number;
  contentType: string;
}

export interface SignedUrlOptions {
  expiresInSeconds?: number;
  contentType?: string;
}

export interface StorageServiceInterface {
  /**
   * Upload a file to storage
   * @param buffer - File content as Buffer
   * @param path - Destination path in storage
   * @param options - Upload options
   * @returns Upload result with path and metadata
   */
  upload(buffer: Buffer, path: string, options?: UploadOptions): Promise<UploadResult>;

  /**
   * Download a file from storage
   * @param path - File path in storage
   * @returns File content as Buffer
   */
  download(path: string): Promise<Buffer>;

  /**
   * Generate a pre-signed URL for secure download
   * @param path - File path in storage
   * @param options - Signed URL options
   * @returns Pre-signed URL string
   */
  getSignedUrl(path: string, options?: SignedUrlOptions): Promise<string>;

  /**
   * Delete a file from storage
   * @param path - File path in storage
   */
  delete(path: string): Promise<void>;

  /**
   * Check if a file exists in storage
   * @param path - File path in storage
   * @returns True if file exists
   */
  exists(path: string): Promise<boolean>;

  /**
   * Download a file to a local temporary path
   * @param path - File path in storage
   * @returns Local file path
   */
  downloadToTemp(path: string): Promise<string>;

  /**
   * Upload content from a string (for reports, etc.)
   * @param content - String content to upload
   * @param path - Destination path in storage
   * @param options - Upload options
   * @returns Upload result
   */
  uploadFromString(content: string, path: string, options?: UploadOptions): Promise<UploadResult>;
}

export const STORAGE_SERVICE = 'STORAGE_SERVICE';
