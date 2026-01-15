CREATE OR REPLACE FUNCTION prevent_audit_modification()
RETURNS TRIGGER AS $$
BEGIN
  RAISE EXCEPTION 'audit_trails is append-only - modifications not allowed';
END;
$$ LANGUAGE plpgsql;

-- Function to set tenant context (used by middleware for RLS)
CREATE OR REPLACE FUNCTION set_tenant_context(p_tenant_id TEXT)
RETURNS VOID AS $$
BEGIN
  PERFORM set_config('app.tenant_id', p_tenant_id, true);
END;
$$ LANGUAGE plpgsql;
