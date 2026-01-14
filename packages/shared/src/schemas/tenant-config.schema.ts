import { z } from 'zod';

export const UpdateTenantConfigSchema = z.object({
  standardMarginPercent: z.number()
    .min(0, 'Standard margin must be at least 0%')
    .max(100, 'Standard margin cannot exceed 100%')
    .refine(
      (val) => Number.isFinite(val) && Math.round(val * 100) / 100 === val,
      'Standard margin must have at most 2 decimal places'
    ),
  benefitCardMarginPercent: z.number()
    .min(0, 'Benefit card margin must be at least 0%')
    .max(100, 'Benefit card margin cannot exceed 100%')
    .refine(
      (val) => Number.isFinite(val) && Math.round(val * 100) / 100 === val,
      'Benefit card margin must have at most 2 decimal places'
    ),
  payrollCutoffDay: z.number()
    .int('Payroll cutoff day must be an integer')
    .min(1, 'Payroll cutoff day must be between 1 and 28')
    .max(28, 'Payroll cutoff day must be between 1 and 28 (safe for all months)'),
  minInstallmentValueCents: z.number()
    .int('Minimum installment value must be an integer')
    .min(100, 'Minimum installment value must be at least R$ 1.00 (100 cents)'),
  maxInstallments: z.number()
    .int('Max installments must be an integer')
    .min(1, 'Max installments must be at least 1')
    .max(120, 'Max installments cannot exceed 120'),
}).refine(
  (data) => data.standardMarginPercent + data.benefitCardMarginPercent <= 100,
  {
    message: 'Sum of standard margin and benefit card margin cannot exceed 100%',
    path: ['standardMarginPercent'],
  }
);

export const TenantConfigResponseSchema = z.object({
  id: z.string(),
  tenantId: z.string(),
  standardMarginBasisPoints: z.number(),
  benefitCardMarginBasisPoints: z.number(),
  payrollCutoffDay: z.number(),
  minInstallmentValueCents: z.number(),
  maxInstallments: z.number(),
  createdAt: z.date(),
  updatedAt: z.date(),
});

export type UpdateTenantConfigInput = z.infer<typeof UpdateTenantConfigSchema>;
export type TenantConfigResponse = z.infer<typeof TenantConfigResponseSchema>;
