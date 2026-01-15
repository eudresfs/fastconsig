"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.auditTrails = exports.tenants = void 0;
const pg_core_1 = require("drizzle-orm/pg-core");
exports.tenants = (0, pg_core_1.pgTable)('tenants', {
    id: (0, pg_core_1.text)('id').primaryKey(), // CUID generated in application
    clerkOrgId: (0, pg_core_1.text)('clerk_org_id').notNull().unique(),
    name: (0, pg_core_1.text)('name').notNull(),
    cnpj: (0, pg_core_1.text)('cnpj').notNull().unique(),
    slug: (0, pg_core_1.text)('slug').notNull().unique(),
    active: (0, pg_core_1.boolean)('active').default(true).notNull(),
    createdAt: (0, pg_core_1.timestamp)('created_at').defaultNow().notNull(),
    updatedAt: (0, pg_core_1.timestamp)('updated_at').defaultNow().notNull().$onUpdate(() => new Date()),
});
exports.auditTrails = (0, pg_core_1.pgTable)('audit_trails', {
    id: (0, pg_core_1.text)('id').primaryKey(), // CUID generated in application
    tenantId: (0, pg_core_1.text)('tenant_id').references(() => exports.tenants.id),
    actorId: (0, pg_core_1.text)('actor_id').notNull(), // Clerk User ID
    action: (0, pg_core_1.text)('action').notNull(),
    resource: (0, pg_core_1.text)('resource').notNull(),
    details: (0, pg_core_1.text)('details'), // JSON string or text description
    createdAt: (0, pg_core_1.timestamp)('created_at').defaultNow().notNull(),
});
