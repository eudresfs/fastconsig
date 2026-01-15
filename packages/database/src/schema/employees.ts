import { pgTable, text, integer, timestamp, uniqueIndex, index } from "drizzle-orm/pg-core";
import { tenants } from "./tenants";

export const employees = pgTable('employees', {
  id: text('id').primaryKey(),
  tenantId: text('tenant_id').notNull().references(() => tenants.id),
  cpf: text('cpf').notNull(),
  enrollmentId: text('enrollment_id').notNull(), // matricula
  name: text('name').notNull(),
  email: text('email'),
  phone: text('phone'),
  grossSalary: integer('gross_salary').notNull(), // in cents
  mandatoryDiscounts: integer('mandatory_discounts').notNull().default(0), // in cents
  availableMargin: integer('available_margin').notNull().default(0), // in cents
  usedMargin: integer('used_margin').notNull().default(0), // in cents
  version: integer('version').notNull().default(1),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull().$onUpdate(() => new Date()),
  deletedAt: timestamp('deleted_at'),
}, (table) => {
  return {
    tenantIdIdx: index('employees_tenant_id_idx').on(table.tenantId),
    tenantCpfUnique: uniqueIndex('employees_tenant_cpf_unique').on(table.tenantId, table.cpf),
    tenantEnrollmentUnique: uniqueIndex('employees_tenant_enrollment_unique').on(table.tenantId, table.enrollmentId),
  };
});
