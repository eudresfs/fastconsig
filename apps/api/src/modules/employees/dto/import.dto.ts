/**
 * DTOs for Employee Bulk Import (Story 2.3)
 */

export class ImportJobResponseDto {
  jobId!: string;
  status!: 'queued' | 'active' | 'completed' | 'failed';
  message!: string;
}

export class ImportProgressDto {
  jobId!: string;
  status!: 'waiting' | 'active' | 'completed' | 'failed';
  progress!: {
    percentage: number;
    totalRows: number;
    processedRows: number;
    successfulRows: number;
    failedRows: number;
  };
  startedAt?: Date;
  completedAt?: Date;
  estimatedCompletionAt?: Date;
}

export class ImportResultDto {
  jobId!: string;
  status!: 'completed' | 'failed';
  result!: {
    totalRows: number;
    successfulRows: number;
    failedRows: number;
    processingTimeMs: number;
    rejectionReportUrl?: string;
  };
  completedAt!: Date;
}

export class ImportErrorDto {
  code!: string;
  message!: string;
  details?: Record<string, unknown>;
}

// Error codes for import operations
export const IMPORT_ERROR_CODES = {
  INVALID_FILE: 'INVALID_FILE',
  FILE_TOO_LARGE: 'FILE_TOO_LARGE',
  INVALID_FORMAT: 'INVALID_FORMAT',
  PROCESSING_FAILED: 'PROCESSING_FAILED',
  JOB_NOT_FOUND: 'JOB_NOT_FOUND',
  NO_VALID_ROWS: 'NO_VALID_ROWS',
} as const;

// Error messages in Portuguese (per config.yaml)
export const IMPORT_ERROR_MESSAGES = {
  [IMPORT_ERROR_CODES.FILE_TOO_LARGE]: 'Arquivo muito grande. Tamanho máximo permitido: 50MB',
  [IMPORT_ERROR_CODES.INVALID_FORMAT]: 'Formato de arquivo inválido. Por favor, envie um arquivo CSV',
  [IMPORT_ERROR_CODES.INVALID_FILE]: 'Arquivo inválido: deve ser um CSV com no máximo 50MB',
  [IMPORT_ERROR_CODES.PROCESSING_FAILED]: 'Erro ao processar arquivo. Por favor, verifique o relatório de erros',
  [IMPORT_ERROR_CODES.JOB_NOT_FOUND]: 'Job de importação não encontrado',
  [IMPORT_ERROR_CODES.NO_VALID_ROWS]: 'Nenhuma linha válida encontrada no arquivo',
} as const;

// CSV validation messages in Portuguese
export const CSV_VALIDATION_MESSAGES = {
  INVALID_CPF: 'CPF inválido ou mal formatado',
  DUPLICATE_CPF: 'CPF já cadastrado no sistema',
  DUPLICATE_ENROLLMENT: 'Matrícula já cadastrada no sistema',
  INVALID_EMAIL: 'Email inválido',
  NEGATIVE_SALARY: 'Salário bruto deve ser maior que zero',
  DISCOUNTS_EXCEED_SALARY: 'Descontos obrigatórios excedem o salário bruto',
} as const;

// Max file size in bytes (50MB)
export const MAX_FILE_SIZE = 50 * 1024 * 1024;
