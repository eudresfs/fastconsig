import { Test, TestingModule } from '@nestjs/testing';
import { ConfigService } from '@nestjs/config';
import * as fs from 'fs';
import * as path from 'path';
import * as os from 'os';
import { LocalStorageService } from '../local-storage.service';

describe('LocalStorageService', () => {
  let service: LocalStorageService;
  let testStorageDir: string;

  beforeEach(async () => {
    // Create a temp directory for testing
    testStorageDir = path.join(os.tmpdir(), 'test-storage-' + Date.now());
    fs.mkdirSync(testStorageDir, { recursive: true });

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        LocalStorageService,
        {
          provide: ConfigService,
          useValue: {
            get: jest.fn((key: string) => {
              if (key === 'LOCAL_STORAGE_PATH') return testStorageDir;
              return null;
            }),
          },
        },
      ],
    }).compile();

    service = module.get<LocalStorageService>(LocalStorageService);
  });

  afterEach(() => {
    // Clean up test directory
    if (fs.existsSync(testStorageDir)) {
      fs.rmSync(testStorageDir, { recursive: true, force: true });
    }
  });

  describe('upload', () => {
    it('should upload a file successfully', async () => {
      const content = Buffer.from('Test content');
      const filePath = 'test/file.txt';

      const result = await service.upload(content, filePath);

      expect(result).toEqual({
        url: expect.stringContaining(filePath),
        eTag: expect.any(String),
        size: content.length,
      });

      const fullPath = path.join(testStorageDir, filePath);
      expect(fs.existsSync(fullPath)).toBe(true);
      expect(fs.readFileSync(fullPath).toString()).toBe('Test content');
    });

    it('should create nested directories if they do not exist', async () => {
      const content = Buffer.from('Nested content');
      const filePath = 'level1/level2/level3/file.txt';

      await service.upload(content, filePath);

      const fullPath = path.join(testStorageDir, filePath);
      expect(fs.existsSync(fullPath)).toBe(true);
    });
  });

  describe('uploadFromString', () => {
    it('should upload string content', async () => {
      const content = 'String content';
      const filePath = 'test/string.txt';

      const result = await service.uploadFromString(content, filePath);

      expect(result.size).toBe(Buffer.from(content).length);
      const fullPath = path.join(testStorageDir, filePath);
      expect(fs.readFileSync(fullPath, 'utf-8')).toBe(content);
    });
  });

  describe('download', () => {
    it('should download a file successfully', async () => {
      const content = Buffer.from('Download test');
      const filePath = 'test/download.txt';

      // Upload first
      await service.upload(content, filePath);

      // Then download
      const downloaded = await service.download(filePath);

      expect(downloaded.toString()).toBe('Download test');
    });

    it('should throw error if file does not exist', async () => {
      await expect(service.download('nonexistent/file.txt')).rejects.toThrow();
    });
  });

  describe('downloadToTemp', () => {
    it('should download file to temp directory', async () => {
      const content = Buffer.from('Temp download test');
      const filePath = 'test/temp-download.txt';

      // Upload first
      await service.upload(content, filePath);

      // Download to temp
      const tempPath = await service.downloadToTemp(filePath);

      expect(fs.existsSync(tempPath)).toBe(true);
      expect(fs.readFileSync(tempPath).toString()).toBe('Temp download test');

      // Clean up temp file
      fs.unlinkSync(tempPath);
    });
  });

  describe('delete', () => {
    it('should delete a file successfully', async () => {
      const content = Buffer.from('Delete test');
      const filePath = 'test/delete.txt';

      // Upload first
      await service.upload(content, filePath);

      const fullPath = path.join(testStorageDir, filePath);
      expect(fs.existsSync(fullPath)).toBe(true);

      // Delete
      await service.delete(filePath);

      expect(fs.existsSync(fullPath)).toBe(false);
    });

    it('should not throw error if file does not exist', async () => {
      await expect(service.delete('nonexistent/file.txt')).resolves.not.toThrow();
    });
  });

  describe('exists', () => {
    it('should return true if file exists', async () => {
      const content = Buffer.from('Exists test');
      const filePath = 'test/exists.txt';

      await service.upload(content, filePath);

      const exists = await service.exists(filePath);
      expect(exists).toBe(true);
    });

    it('should return false if file does not exist', async () => {
      const exists = await service.exists('nonexistent/file.txt');
      expect(exists).toBe(false);
    });
  });

  describe('getSignedUrl', () => {
    it('should generate a signed URL', async () => {
      const content = Buffer.from('Signed URL test');
      const filePath = 'test/signed.txt';

      await service.upload(content, filePath);

      const url = await service.getSignedUrl(filePath);

      expect(url).toContain(filePath);
    });
  });

  describe('listFiles', () => {
    it('should list files in a prefix', async () => {
      // Upload multiple files
      await service.upload(Buffer.from('File 1'), 'test/file1.txt');
      await service.upload(Buffer.from('File 2'), 'test/file2.txt');
      await service.upload(Buffer.from('File 3'), 'other/file3.txt');

      const files = await service.listFiles('test/');

      expect(files).toHaveLength(2);
      expect(files).toContain('test/file1.txt');
      expect(files).toContain('test/file2.txt');
      expect(files).not.toContain('other/file3.txt');
    });

    it('should return empty array if no files match prefix', async () => {
      const files = await service.listFiles('nonexistent/');
      expect(files).toEqual([]);
    });
  });
});
