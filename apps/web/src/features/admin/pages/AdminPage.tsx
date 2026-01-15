import { useState } from "react";
import { CreateTenantDialog } from "../components/CreateTenantDialog";
import { TenantList } from "../components/TenantList";
import { TenantDetailPage } from "./TenantDetailPage";

export function AdminPage() {
  const [selectedTenant, setSelectedTenant] = useState<{ id: string; name: string } | null>(null);

  if (selectedTenant) {
    return (
      <div className="container mx-auto py-10">
        <TenantDetailPage
          tenantId={selectedTenant.id}
          tenantName={selectedTenant.name}
          onBack={() => setSelectedTenant(null)}
        />
      </div>
    );
  }

  return (
    <div className="container mx-auto py-10">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Tenant Management</h1>
          <p className="text-muted-foreground mt-2">
            Manage organizations and their access to the platform.
          </p>
        </div>
        <CreateTenantDialog />
      </div>
      <div className="bg-card rounded-lg border shadow-sm">
        <TenantList onSelect={(id, name) => setSelectedTenant({ id, name })} />
      </div>
    </div>
  );
}
