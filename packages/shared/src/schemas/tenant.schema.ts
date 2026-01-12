import { z } from 'zod';

export const CreateTenantSchema = z.object({
  name: z.string().min(3, "Name must be at least 3 characters"),
  cnpj: z.string().length(14, "CNPJ must be exactly 14 digits").regex(/^\d+$/, "CNPJ must contain only numbers"),
  slug: z.string().min(3, "Slug must be at least 3 characters").regex(/^[a-z0-9-]+$/, "Slug must contain only lowercase letters, numbers, and hyphens"),
  adminEmail: z.string().email("Invalid email address"),
});

export type CreateTenantInput = z.infer<typeof CreateTenantSchema>;

export const TenantResponseSchema = z.object({
  id: z.string(),
  clerkOrgId: z.string(),
  name: z.string(),
  cnpj: z.string(),
  slug: z.string(),
  active: z.boolean(),
  createdAt: z.date(),
});

export type TenantResponse = z.infer<typeof TenantResponseSchema>;
