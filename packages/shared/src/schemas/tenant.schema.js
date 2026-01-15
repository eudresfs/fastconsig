"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TenantResponseSchema = exports.CreateTenantSchema = void 0;
const zod_1 = require("zod");
function isValidCNPJ(cnpj) {
    if (!cnpj)
        return false;
    // Remove non-digits
    cnpj = cnpj.replace(/[^\d]+/g, '');
    if (cnpj.length !== 14)
        return false;
    // Eliminate known invalid CNPJs (e.g. 00000000000000)
    if (/^(\d)\1+$/.test(cnpj))
        return false;
    // Validate 1st digit
    let tamanho = cnpj.length - 2;
    let numeros = cnpj.substring(0, tamanho);
    let digitos = cnpj.substring(tamanho);
    let soma = 0;
    let pos = tamanho - 7;
    for (let i = tamanho; i >= 1; i--) {
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2)
            pos = 9;
    }
    let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != parseInt(digitos.charAt(0)))
        return false;
    // Validate 2nd digit
    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (let i = tamanho; i >= 1; i--) {
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != parseInt(digitos.charAt(1)))
        return false;
    return true;
}
exports.CreateTenantSchema = zod_1.z.object({
    name: zod_1.z.string().min(3, "Name must be at least 3 characters"),
    cnpj: zod_1.z.string()
        .length(14, "CNPJ must be exactly 14 digits")
        .regex(/^\d+$/, "CNPJ must contain only numbers")
        .refine(isValidCNPJ, "Invalid CNPJ checksum"),
    slug: zod_1.z.string().min(3, "Slug must be at least 3 characters").regex(/^[a-z0-9-]+$/, "Slug must contain only lowercase letters, numbers, and hyphens"),
    adminEmail: zod_1.z.string().email("Invalid email address"),
});
exports.TenantResponseSchema = zod_1.z.object({
    id: zod_1.z.string(),
    clerkOrgId: zod_1.z.string(),
    name: zod_1.z.string(),
    cnpj: zod_1.z.string(),
    slug: zod_1.z.string(),
    active: zod_1.z.boolean(),
    createdAt: zod_1.z.date(),
});
