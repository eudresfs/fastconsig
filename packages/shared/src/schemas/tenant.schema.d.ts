import { z } from 'zod';
export declare const CreateTenantSchema: z.ZodObject<{
    name: z.ZodString;
    cnpj: z.ZodEffects<z.ZodString, string, string>;
    slug: z.ZodString;
    adminEmail: z.ZodString;
}, "strip", z.ZodTypeAny, {
    name: string;
    cnpj: string;
    slug: string;
    adminEmail: string;
}, {
    name: string;
    cnpj: string;
    slug: string;
    adminEmail: string;
}>;
export type CreateTenantInput = z.infer<typeof CreateTenantSchema>;
export declare const TenantResponseSchema: z.ZodObject<{
    id: z.ZodString;
    clerkOrgId: z.ZodString;
    name: z.ZodString;
    cnpj: z.ZodString;
    slug: z.ZodString;
    active: z.ZodBoolean;
    createdAt: z.ZodDate;
}, "strip", z.ZodTypeAny, {
    name: string;
    cnpj: string;
    slug: string;
    id: string;
    clerkOrgId: string;
    active: boolean;
    createdAt: Date;
}, {
    name: string;
    cnpj: string;
    slug: string;
    id: string;
    clerkOrgId: string;
    active: boolean;
    createdAt: Date;
}>;
export type TenantResponse = z.infer<typeof TenantResponseSchema>;
//# sourceMappingURL=tenant.schema.d.ts.map