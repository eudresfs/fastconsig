import { z } from 'zod';

// CPF validation function
function validateCPF(cpf: string): boolean {
  // Remove non-digits
  const cleaned = cpf.replace(/\D/g, '');

  if (cleaned.length !== 11) return false;

  // Check if all digits are the same (invalid CPF)
  if (/^(\d)\1{10}$/.test(cleaned)) return false;

  // Validate check digits
  let sum = 0;
  let remainder;

  // First check digit
  for (let i = 1; i <= 9; i++) {
    sum += parseInt(cleaned.substring(i - 1, i)) * (11 - i);
  }
  remainder = (sum * 10) % 11;
  if (remainder === 10 || remainder === 11) remainder = 0;
  if (remainder !== parseInt(cleaned.substring(9, 10))) return false;

  // Second check digit
  sum = 0;
  for (let i = 1; i <= 10; i++) {
    sum += parseInt(cleaned.substring(i - 1, i)) * (12 - i);
  }
  remainder = (sum * 10) % 11;
  if (remainder === 10 || remainder === 11) remainder = 0;
  if (remainder !== parseInt(cleaned.substring(10, 11))) return false;

  return true;
}

// Input sanitization helper
function sanitizeString(value: string): string {
  return value
    .trim()
    .replace(/[<>]/g, '') // Remove HTML tags
    .substring(0, 255); // Limit length
}

// Zod schema for creating employees
export const createEmployeeSchema = z.object({
  cpf: z.string()
    .min(11, 'CPF must have 11 digits')
    .max(14, 'CPF is too long')
    .transform(val => val.replace(/\D/g, ''))
    .refine(validateCPF, 'Invalid CPF'),
  enrollmentId: z.string()
    .min(1, 'Enrollment ID is required')
    .max(50, 'Enrollment ID is too long')
    .transform(sanitizeString),
  name: z.string()
    .min(1, 'Name is required')
    .max(100, 'Name is too long')
    .transform(sanitizeString),
  email: z.string()
    .email('Invalid email format')
    .max(255, 'Email is too long')
    .transform(sanitizeString)
    .optional(),
  phone: z.string()
    .max(20, 'Phone is too long')
    .transform(sanitizeString)
    .optional(),
  grossSalary: z.number()
    .int('Salary must be an integer (in cents)')
    .min(0, 'Salary cannot be negative'),
  mandatoryDiscounts: z.number()
    .int('Discounts must be an integer (in cents)')
    .min(0, 'Discounts cannot be negative')
    .optional()
    .default(0),
});

// Zod schema for updating employees
export const updateEmployeeSchema = z.object({
  cpf: z.string()
    .min(11, 'CPF must have 11 digits')
    .max(14, 'CPF is too long')
    .transform(val => val.replace(/\D/g, ''))
    .refine(validateCPF, 'Invalid CPF')
    .optional(),
  enrollmentId: z.string()
    .min(1, 'Enrollment ID is required')
    .max(50, 'Enrollment ID is too long')
    .transform(sanitizeString)
    .optional(),
  name: z.string()
    .min(1, 'Name is required')
    .max(100, 'Name is too long')
    .transform(sanitizeString)
    .optional(),
  email: z.string()
    .email('Invalid email format')
    .max(255, 'Email is too long')
    .transform(sanitizeString)
    .optional(),
  phone: z.string()
    .max(20, 'Phone is too long')
    .transform(sanitizeString)
    .optional(),
  grossSalary: z.number()
    .int('Salary must be an integer (in cents)')
    .min(0, 'Salary cannot be negative')
    .optional(),
  mandatoryDiscounts: z.number()
    .int('Discounts must be an integer (in cents)')
    .min(0, 'Discounts cannot be negative')
    .optional(),
  version: z.number()
    .int('Version must be an integer')
    .min(1, 'Version must be at least 1'),
});

export type CreateEmployeeInput = z.infer<typeof createEmployeeSchema>;
export type UpdateEmployeeInput = z.infer<typeof updateEmployeeSchema>;
