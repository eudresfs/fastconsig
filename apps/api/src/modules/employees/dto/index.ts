export * from './import.dto';

export class CreateEmployeeDto {
  cpf!: string;
  enrollmentId!: string;
  name!: string;
  email?: string;
  phone?: string;
  grossSalary!: number; // in cents
  mandatoryDiscounts?: number; // in cents
}

export class UpdateEmployeeDto {
  cpf?: string;
  enrollmentId?: string;
  name?: string;
  email?: string;
  phone?: string;
  grossSalary?: number;
  mandatoryDiscounts?: number;
  version!: number; // Required for optimistic locking
}

export class EmployeeResponseDto {
  id!: string;
  tenantId!: string;
  cpf!: string;
  enrollmentId!: string;
  name!: string;
  email?: string;
  phone?: string;
  grossSalary!: number;
  mandatoryDiscounts!: number;
  version!: number;
  createdAt!: Date;
  updatedAt!: Date;
  deletedAt?: Date;
}
