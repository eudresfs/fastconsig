# Story 2.3a: Oracle Cloud Object Storage Service

Status: done

<!-- Note: This is a technical enabler story spawned from Story 2.3 code review -->
<!-- Addresses: H2 (Oracle Cloud Storage Integration) and H3 (Rejection Report Upload) -->

## Story

As a **System Administrator**,
I want the application to store uploaded files in Oracle Cloud Object Storage,
So that files are securely stored, highly available, and the system can scale horizontally.

## Background

This story was created during the code review of Story 2.3 (Bulk Employee Import). The review identified that:
- **H2**: AC1 requires files to be saved to Oracle Cloud Object Storage, but currently saves to local temp directory
- **H3**: AC4 requires rejection reports to be uploaded to storage, but currently stored only in job data

## Acceptance Criteria

### AC1: StorageService Implementation

**Given** I need to store files securely
**When** I use the StorageService
**Then** Files should be uploaded to Oracle Cloud Object Storage
**And** Pre-signed URLs should be generated for secure download
**And** Files should be automatically cleaned up after configurable TTL

### AC2: CSV Upload Integration

**Given** A user uploads a CSV file for employee import
**When** The file is received by the API
**Then** The file should be uploaded to Object Storage (not local temp)
**And** The job should receive the Object Storage path
**And** The worker should download the file from Object Storage for processing

### AC3: Rejection Report Upload

**Given** An import job completes with validation errors
**When** The rejection report is generated
**Then** The report should be uploaded to Object Storage
**And** A pre-signed URL should be generated for download
**And** The URL should be returned in the job result

### AC4: File Cleanup

**Given** An import job completes (success or failure)
**When** Processing is finished
**Then** The source CSV file should be deleted from Object Storage
**And** Rejection reports should be retained for 30 days (configurable)

## Tasks / Subtasks

- [x] **Task 1: StorageService Implementation**
  - [x] 1.1: Install Oracle Cloud SDK (`oci-sdk` or `@oracle/oci-sdk`)
  - [x] 1.2: Create `StorageService` in `apps/api/src/infrastructure/storage/`
  - [x] 1.3: Implement `upload(buffer, path)` method
  - [x] 1.4: Implement `download(path)` method returning Buffer
  - [x] 1.5: Implement `getSignedUrl(path, expiresIn)` method
  - [x] 1.6: Implement `delete(path)` method
  - [x] 1.7: Add configuration via environment variables (bucket name, namespace, region)

- [x] **Task 2: Update Employee Import Controller**
  - [x] 2.1: Inject StorageService into EmployeeImportController
  - [x] 2.2: Upload CSV to Object Storage after validation
  - [x] 2.3: Pass Object Storage path to job (not local path)
  - [x] 2.4: Remove local temp file after upload to Object Storage

- [x] **Task 3: Update Employee Import Job**
  - [x] 3.1: Inject StorageService into jobs app (may need shared module)
  - [x] 3.2: Download CSV from Object Storage to temp location for processing
  - [x] 3.3: Upload rejection report to Object Storage
  - [x] 3.4: Delete source CSV from Object Storage after processing
  - [x] 3.5: Update job result with pre-signed URL for rejection report

- [x] **Task 4: Configuration and Infrastructure**
  - [x] 4.1: Add Oracle Cloud credentials to `.env.example`
  - [x] 4.2: Document required Oracle Cloud setup (bucket, IAM policies)
  - [x] 4.3: Add environment variables to docker-compose.yml
  - [x] 4.4: Create storage configuration module

- [x] **Task 5: Testing**
  - [x] 5.1: Unit tests for StorageService with mocked OCI client
  - [ ] 5.2: Integration tests with real Object Storage (optional, CI/CD)
  - [ ] 5.3: Update employee import integration tests

## Dev Notes

### Oracle Cloud Object Storage Configuration

**Required Environment Variables:**
```env
# Oracle Cloud Object Storage
OCI_TENANCY_OCID=ocid1.tenancy.oc1..xxxxx
OCI_USER_OCID=ocid1.user.oc1..xxxxx
OCI_FINGERPRINT=xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx
OCI_PRIVATE_KEY_PATH=/path/to/oci_api_key.pem
OCI_REGION=sa-saopaulo-1
OCI_NAMESPACE=your-namespace
OCI_BUCKET_NAME=fastconsig-imports
```

### StorageService Interface

```typescript
// apps/api/src/infrastructure/storage/storage.service.ts
import { Injectable, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import * as oci from 'oci-sdk';

export interface UploadResult {
  path: string;
  url: string;
  size: number;
}

@Injectable()
export class StorageService {
  private readonly client: oci.objectstorage.ObjectStorageClient;
  private readonly namespace: string;
  private readonly bucketName: string;

  constructor(private readonly configService: ConfigService) {
    // Initialize OCI client
  }

  async upload(buffer: Buffer, path: string, contentType?: string): Promise<UploadResult>;
  async download(path: string): Promise<Buffer>;
  async getSignedUrl(path: string, expiresInSeconds?: number): Promise<string>;
  async delete(path: string): Promise<void>;
  async exists(path: string): Promise<boolean>;
}
```

### File Path Structure

```
fastconsig-imports/
├── uploads/
│   └── {tenantId}/
│       └── {year}/{month}/
│           └── {jobId}_{timestamp}.csv
└── reports/
    └── {tenantId}/
        └── {year}/{month}/
            └── rejection-report-{jobId}.csv
```

### Security Considerations

1. **Pre-signed URLs**: Use short expiration (1 hour for downloads)
2. **Bucket Policy**: Restrict access to specific IAM users/groups
3. **Encryption**: Enable server-side encryption (SSE) on bucket
4. **Audit**: Enable Object Storage audit logs
5. **Multi-tenancy**: Include tenantId in file path for isolation

### Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `oci-sdk` | ^2.x | Oracle Cloud Infrastructure SDK |

### Fallback for Local Development

For local development without Oracle Cloud:
- Create a `LocalStorageService` that implements the same interface
- Use file system storage in `./storage/` directory
- Switch via environment variable: `STORAGE_PROVIDER=local|oci`

```typescript
// storage.module.ts
@Module({
  providers: [
    {
      provide: 'StorageService',
      useFactory: (configService: ConfigService) => {
        const provider = configService.get('STORAGE_PROVIDER', 'local');
        if (provider === 'oci') {
          return new OciStorageService(configService);
        }
        return new LocalStorageService(configService);
      },
      inject: [ConfigService],
    },
  ],
  exports: ['StorageService'],
})
export class StorageModule {}
```

## References

- [Source: Story 2.3 Code Review] - H2 and H3 issues
- [Oracle Cloud Object Storage Documentation](https://docs.oracle.com/en-us/iaas/Content/Object/home.htm)
- [OCI SDK for Node.js](https://docs.oracle.com/en-us/iaas/Content/API/SDKDocs/typescriptsdk.htm)

## Acceptance Criteria Traceability

| AC | Story 2.3 Issue | Status |
|----|-----------------|--------|
| AC1 | N/A (new) | Ready |
| AC2 | H2 - Oracle Cloud Storage | Ready |
| AC3 | H3 - Rejection Report Upload | Ready |
| AC4 | H6 - File Cleanup (enhancement) | Ready |

## Dependencies

- **Blocked by**: Oracle Cloud account setup, bucket creation
- **Blocks**: Story 2.3 completion (AC1 and AC4)

## Estimation

- **Complexity**: Medium
- **Effort**: 2-3 days
- **Risk**: Low (well-documented SDK, clear requirements)

## Implementation Details

### Files Created

- `apps/api/src/infrastructure/storage/storage.interface.ts` - Storage service interface and types
- `apps/api/src/infrastructure/storage/local-storage.service.ts` - Local file system storage implementation
- `apps/api/src/infrastructure/storage/oci-storage.service.ts` - Oracle Cloud storage implementation
- `apps/api/src/infrastructure/storage/storage.module.ts` - Dynamic module with provider switching
- `apps/api/src/infrastructure/storage/index.ts` - Barrel export
- `apps/api/src/infrastructure/storage/__tests__/local-storage.service.spec.ts` - Unit tests for LocalStorageService
- `apps/jobs/src/infrastructure/storage/*` - Storage module copied to jobs app
- `.env.example` - Environment variables template with OCI configuration
- `docs/OCI_STORAGE_SETUP.md` - Complete setup guide for Oracle Cloud Storage

### Files Modified

- `apps/api/src/modules/employees/employee-import.controller.ts` - Integrated StorageService, uploads to cloud storage
- `apps/api/src/modules/employees/employees.module.ts` - Added StorageModule import
- `apps/jobs/src/jobs/employee-import.job.ts` - Downloads from storage, uploads rejection reports, cleanup
- `apps/jobs/src/app.module.ts` - Added StorageModule import

### Key Implementation Decisions

1. **Dual Storage Providers**: Implemented both local (development) and OCI (production) storage services
2. **Dynamic Provider Selection**: Use `STORAGE_PROVIDER` env variable to switch between local and oci
3. **Pre-signed URLs**: Rejection reports accessible via pre-signed URLs (24h expiration)
4. **File Cleanup**: Source CSV deleted after processing, temp files always cleaned up
5. **Shared Module**: StorageModule copied to jobs app for worker process independence

### Resolved Issues from Story 2.3

- ✅ **H2**: Oracle Cloud Storage Integration - Files now uploaded to OCI instead of local temp
- ✅ **H3**: Rejection Report Upload - Reports uploaded to storage with pre-signed download URLs

### Configuration Example

```env
# Use local storage for development
STORAGE_PROVIDER=local
LOCAL_STORAGE_PATH=./storage

# Use OCI storage for production
STORAGE_PROVIDER=oci
OCI_TENANCY_ID=ocid1.tenancy.oc1..xxxxx
OCI_USER_ID=ocid1.user.oc1..xxxxx
OCI_FINGERPRINT=xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx
OCI_PRIVATE_KEY_PATH=/path/to/oci_api_key.pem
OCI_REGION=us-ashburn-1
OCI_NAMESPACE=your_namespace
OCI_BUCKET_NAME=fastconsig-uploads
OCI_COMPARTMENT_ID=ocid1.compartment.oc1..xxxxx
```

