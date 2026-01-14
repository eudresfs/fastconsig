import { pgTable, text, integer, timestamp, index } from 'drizzle-orm/pg-core';
import { tenants } from './tenants';

export const tenantConfigurations = pgTable('tenant_configurations', {
  id: text('id').primaryKey(), // CUID generated in application
  tenantId: text('tenant_id').notNull().references(() => tenants.id),
  // Default: 30% standard margin (Brazilian market standard for payroll loans)
  standardMarginBasisPoints: integer('standard_margin_basis_points').notNull().default(3000), // 30.00%
  // Default: 5% benefit card margin (typical consigned benefit card margin)
  benefitCardMarginBasisPoints: integer('benefit_card_margin_basis_points').notNull().default(500), // 5.00%
  // Default: Day 20 for payroll cutoff (safe for all months, common in Brazilian payroll)
  payrollCutoffDay: integer('payroll_cutoff_day').notNull().default(20),
  // Default: R$ 10.00 minimum installment (prevents micro-loans with high admin cost)
  minInstallmentValueCents: integer('min_installment_value_cents').notNull().default(1000), // R$ 10,00
  // Default: 96 months maximum (8 years, typical for long-term payroll loans)
  maxInstallments: integer('max_installments').notNull().default(96),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull().$onUpdate(() => new Date()),
}, (table) => ({
  tenantIdIdx: index('tenant_configurations_tenant_id_idx').on(table.tenantId),
}));
