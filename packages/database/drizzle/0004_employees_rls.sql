-- Enable Row Level Security for employees
ALTER TABLE employees ENABLE ROW LEVEL SECURITY;

-- Create Policy for Tenant Isolation
-- Only allows access to rows where tenant_id matches the current session's tenant_id
CREATE POLICY tenant_isolation_policy ON employees
    USING (tenant_id = current_setting('app.tenant_id', true));
