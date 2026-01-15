import { pgTable, varchar, timestamp, text, jsonb, index } from "drizzle-orm/pg-core";
import { createId } from "@paralleldrive/cuid2";
import { tenants } from "./tenants";

/**
 * Audit Trails Table - Append-only audit log for compliance and security
 *
 * Captures all data mutations and sensitive read operations across the system.
 * Implements FR15, FR16, NFR05 from PRD.
 *
 * @see ADR-006 (Audit & Observability) - Hybrid approach with application events
 * @see ADR-002 (Multi-Tenancy) - RLS enabled with tenant_id column
 */
export const auditTrails = pgTable(
  'audit_trails',
  {
    id: varchar('id', { length: 30 })
      .primaryKey()
      .$defaultFn(() => createId()),

    // Multi-tenant isolation
    tenantId: varchar('tenant_id', { length: 30 })
      .notNull()
      .references(() => tenants.id),

    // Actor information
    userId: varchar('user_id', { length: 255 }).notNull(), // Clerk user ID

    // Action details
    action: varchar('action', { length: 50 }).notNull(), // CREATE, UPDATE, DELETE, QUERY_MARGIN, etc.
    resourceType: varchar('resource_type', { length: 100 }).notNull(), // employees, loans, tokens, etc.
    resourceId: varchar('resource_id', { length: 30 }), // ID of affected resource (nullable)

    // Request context
    ipAddress: varchar('ip_address', { length: 45 }).notNull(), // IPv4/IPv6
    userAgent: text('user_agent'), // Browser/client info

    // Data changes (for mutations)
    oldValue: jsonb('old_value'), // Previous state (UPDATE/DELETE)
    newValue: jsonb('new_value'), // New state (CREATE/UPDATE)

    // Additional context
    metadata: jsonb('metadata'), // Extra contextual data

    // Timestamp
    createdAt: timestamp('created_at', { withTimezone: true, mode: 'date' })
      .notNull()
      .defaultNow(),
  },
  (table) => ({
    // Performance indexes for multi-tenant queries
    tenantIdIdx: index('audit_trails_tenant_id_idx').on(table.tenantId),
    userIdIdx: index('audit_trails_user_id_idx').on(table.userId),
    resourceTypeIdx: index('audit_trails_resource_type_idx').on(table.resourceType),
    createdAtIdx: index('audit_trails_created_at_idx').on(table.createdAt),

    // Composite index for specific resource lookups (tenant-scoped)
    tenantResourceIdx: index('audit_trails_tenant_resource_idx').on(
      table.tenantId,
      table.resourceType,
      table.resourceId
    ),
  })
);

// Type exports for TypeScript
export type AuditTrail = typeof auditTrails.$inferSelect;
export type NewAuditTrail = typeof auditTrails.$inferInsert;
