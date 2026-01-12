import { z } from 'zod';

function isValidCNPJ(cnpj: string): boolean {
  if (!cnpj) return false;

  // Remove non-digits
  cnpj = cnpj.replace(/[^\d]+/g, '');

  if (cnpj.length !== 14) return false;

  // Eliminate known invalid CNPJs (e.g. 00000000000000)
  if (/^(\d)\1+$/.test(cnpj)) return false;

  // Validate 1st digit
  let tamanho = cnpj.length - 2
  let numeros = cnpj.substring(0, tamanho);
  let digitos = cnpj.substring(tamanho);
  let soma = 0;
  let pos = tamanho - 7;

  for (let i = tamanho; i >= 1; i--) {
    soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
    if (pos < 2) pos = 9;
  }

  let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
  if (resultado != parseInt(digitos.charAt(0))) return false;

  // Validate 2nd digit
  tamanho = tamanho + 1;
  numeros = cnpj.substring(0, tamanho);
  soma = 0;
  pos = tamanho - 7;

  for (let i = tamanho; i >= 1; i--) {
    soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
    if (pos < 2) pos = 9;
  }

  resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
  if (resultado != parseInt(digitos.charAt(1))) return false;

  return true;
}

export const CreateTenantSchema = z.object({
  name: z.string().min(3, "Name must be at least 3 characters"),
  cnpj: z.string()
    .length(14, "CNPJ must be exactly 14 digits")
    .regex(/^\d+$/, "CNPJ must contain only numbers")
    .refine(isValidCNPJ, "Invalid CNPJ checksum"),
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
