import { pgTable, text, boolean, timestamp } from "drizzle-orm/pg-core";

export const tenants = pgTable('tenants', {
  id: text('id').primaryKey(), // CUID generated in application
  clerkOrgId: text('clerk_org_id').notNull().unique(),
  name: text('name').notNull(),
  cnpj: text('cnpj').notNull().unique(),
  slug: text('slug').notNull().unique(),
  active: boolean('active').default(true).notNull(),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull(),
});

export const auditTrails = pgTable('audit_trails', {
  id: text('id').primaryKey(), // CUID generated in application
  tenantId: text('tenant_id').references(() => tenants.id),
  actorId: text('actor_id').notNull(), // Clerk User ID
  action: text('action').notNull(),
  resource: text('resource').notNull(),
  details: text('details'), // JSON string or text description
  createdAt: timestamp('created_at').defaultNow().notNull(),
});
