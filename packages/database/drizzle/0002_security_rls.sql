-- Enable Row Level Security
ALTER TABLE audit_trails ENABLE ROW LEVEL SECURITY;

-- Create Policy for Tenant Isolation
-- Only allows access to rows where tenant_id matches the current session's tenant_id
-- Note: Requires app.tenant_id to be set in the current session
CREATE POLICY tenant_isolation_policy ON audit_trails
    USING (tenant_id = current_setting('app.tenant_id', true));

-- Append-only enforcement (Optional but recommended)
-- Prevent updates
CREATE TRIGGER prevent_audit_update
BEFORE UPDATE ON audit_trails
FOR EACH ROW EXECUTE FUNCTION prevent_audit_modification();

-- Prevent deletes
CREATE TRIGGER prevent_audit_delete
BEFORE DELETE ON audit_trails
FOR EACH ROW EXECUTE FUNCTION prevent_audit_modification();
