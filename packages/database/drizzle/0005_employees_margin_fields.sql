-- Migration: Add margin fields to employees table
-- Story: 2.2 - Dynamic Margin Calculation Engine
-- Date: 2026-01-14

-- Add margin columns to employees table
ALTER TABLE employees
ADD COLUMN available_margin INTEGER NOT NULL DEFAULT 0,
ADD COLUMN used_margin INTEGER NOT NULL DEFAULT 0;

-- Add index for margin queries (performance optimization)
CREATE INDEX idx_employees_margins ON employees(tenant_id, available_margin) WHERE deleted_at IS NULL;

-- Add comment for documentation
COMMENT ON COLUMN employees.available_margin IS 'Available margin for loans in cents (calculated from salary and tenant rules)';
COMMENT ON COLUMN employees.used_margin IS 'Used margin from active loans in cents';
