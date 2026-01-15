# Audit Trails Table Documentation

## Overview

The `audit_trails` table is an **append-only** audit log that captures all data mutations and sensitive read operations across the Fast Consig system. This table is critical for compliance (LGPD, FR15, FR16) and security monitoring.

## Schema

```sql
CREATE TABLE audit_trails (
  id VARCHAR(30) PRIMARY KEY,
  tenant_id VARCHAR(30) NOT NULL REFERENCES tenants(id),
  user_id VARCHAR(255) NOT NULL,
  action VARCHAR(50) NOT NULL,
  resource_type VARCHAR(100) NOT NULL,
  resource_id VARCHAR(30),
  ip_address VARCHAR(45) NOT NULL,
  user_agent TEXT,
  old_value JSONB,
  new_value JSONB,
  metadata JSONB,
  created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX audit_trails_tenant_id_idx ON audit_trails(tenant_id);
CREATE INDEX audit_trails_user_id_idx ON audit_trails(user_id);
CREATE INDEX audit_trails_resource_type_idx ON audit_trails(resource_type);
CREATE INDEX audit_trails_created_at_idx ON audit_trails(created_at);
CREATE INDEX audit_trails_tenant_resource_idx ON audit_trails(tenant_id, resource_type, resource_id);
```

## Column Descriptions

| Column | Type | Required | Description |
|--------|------|----------|-------------|
| `id` | VARCHAR(30) | Yes | Primary key (CUID2 generated) |
| `tenant_id` | VARCHAR(30) | Yes | Tenant isolation (multi-tenancy) |
| `user_id` | VARCHAR(255) | Yes | Clerk user ID (actor) |
| `action` | VARCHAR(50) | Yes | Action type (CREATE, UPDATE, DELETE, QUERY_MARGIN, etc.) |
| `resource_type` | VARCHAR(100) | Yes | Resource type (employees, loans, tokens, etc.) |
| `resource_id` | VARCHAR(30) | No | ID of affected resource |
| `ip_address` | VARCHAR(45) | Yes | IPv4/IPv6 address (supports x-forwarded-for) |
| `user_agent` | TEXT | No | Browser/client user agent |
| `old_value` | JSONB | No | Previous state (for UPDATE/DELETE) |
| `new_value` | JSONB | No | New state (for CREATE/UPDATE) |
| `metadata` | JSONB | No | Additional context (URL, method, status code) |
| `created_at` | TIMESTAMP WITH TIME ZONE | Yes | Timestamp (UTC) |

## Action Types

| Action | Description | Use Case |
|--------|-------------|----------|
| `CREATE` | Resource creation | POST requests, new records |
| `UPDATE` | Resource modification | PUT/PATCH requests |
| `DELETE` | Resource deletion | DELETE requests |
| `QUERY_MARGIN` | Sensitive margin query | Blind margin queries (FR08) |
| `QUERY_EMPLOYEE` | Employee data query | Employee lookups |
| `LOGIN` | User login | Authentication events |
| `LOGOUT` | User logout | Session termination |
| `TOKEN_GENERATE` | Token generation | 2FA token creation |
| `TOKEN_VALIDATE` | Token validation | Token usage |
| `READ` | Generic read operation | Other GET requests |

## Security Considerations

### Sensitive Data Redaction

The `AuditTrailService` automatically sanitizes sensitive fields before storing:

- `password` → `[REDACTED]`
- `token` → `[REDACTED]`
- `secret` → `[REDACTED]`
- `apiKey` / `api_key` → `[REDACTED]`
- `accessToken` / `access_token` → `[REDACTED]`
- `refreshToken` / `refresh_token` → `[REDACTED]`
- `privateKey` / `private_key` → `[REDACTED]`

### Append-Only Enforcement

**CRITICAL:** This table is **append-only**. UPDATE and DELETE operations are prohibited to maintain audit trail integrity.

Optional PostgreSQL trigger to enforce:

```sql
CREATE OR REPLACE FUNCTION prevent_audit_modification()
RETURNS TRIGGER AS $$
BEGIN
  RAISE EXCEPTION 'audit_trails is append-only - modifications not allowed';
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER prevent_audit_update
BEFORE UPDATE OR DELETE ON audit_trails
FOR EACH ROW EXECUTE FUNCTION prevent_audit_modification();
```

### LGPD Compliance (FR16)

The `getAuditTrail()` method supports anonymization:

```typescript
const results = await auditTrailService.getAuditTrail({
  tenantId: 'tenant_123',
  anonymize: true, // Enable LGPD anonymization
});

// Results will have:
// - userId: hashed (e.g., "user_dXNlcl9yZWFs")
// - ipAddress: "[ANONYMIZED]"
// - userAgent: undefined
```

## Performance Considerations

### Indexes

All indexes are optimized for multi-tenant queries:

1. **tenant_id_idx**: Essential for tenant isolation (RLS)
2. **user_id_idx**: User activity tracking
3. **resource_type_idx**: Filtering by resource type
4. **created_at_idx**: Time-based queries (reports)
5. **tenant_resource_idx**: Composite for specific resource lookups

### Partitioning (Future)

When table exceeds ~1M rows, consider **monthly partitioning**:

```sql
-- Example: Partition by created_at (monthly)
CREATE TABLE audit_trails (
  -- columns as above
) PARTITION BY RANGE (created_at);

CREATE TABLE audit_trails_2026_01 PARTITION OF audit_trails
  FOR VALUES FROM ('2026-01-01') TO ('2026-02-01');

CREATE TABLE audit_trails_2026_02 PARTITION OF audit_trails
  FOR VALUES FROM ('2026-02-01') TO ('2026-03-01');
```

### Retention Policy (NFR05)

- **Hot storage:** 1 year (PostgreSQL)
- **Cold storage:** 5 years (S3 Glacier or equivalent)

Archival job (to be implemented):
- Runs monthly
- Moves records older than 1 year to cold storage
- Deletes from hot storage after successful transfer

## Querying Examples

### Get all actions for a specific resource

```typescript
const logs = await auditTrailService.getAuditTrail({
  tenantId: 'tenant_123',
  resourceType: 'employees',
  resourceId: 'emp_789',
  limit: 50,
});
```

### Get user activity for a date range

```typescript
const logs = await auditTrailService.getAuditTrail({
  tenantId: 'tenant_123',
  userId: 'user_456',
  startDate: new Date('2026-01-01'),
  endDate: new Date('2026-01-31'),
});
```

### Anonymized report for LGPD compliance

```typescript
const logs = await auditTrailService.getAuditTrail({
  tenantId: 'tenant_123',
  startDate: new Date('2026-01-01'),
  anonymize: true, // Removes PII
});
```

## Integration

The audit trail is captured automatically via `AuditLogInterceptor`:

```typescript
// Registered globally in app.module.ts
{
  provide: APP_INTERCEPTOR,
  useClass: AuditLogInterceptor,
}
```

All HTTP requests (tRPC or REST) are automatically logged after successful completion.

## Related Documentation

- [ADR-006: Audit & Observability](../../_bmad-output/planning-artifacts/architecture.md#ADR-006)
- [ADR-002: Multi-Tenancy](../../_bmad-output/planning-artifacts/architecture.md#ADR-002)
- [PRD FR15: Audit Logging](../../_bmad-output/planning-artifacts/prd.md)
- [PRD FR16: Audit Reports](../../_bmad-output/planning-artifacts/prd.md)
