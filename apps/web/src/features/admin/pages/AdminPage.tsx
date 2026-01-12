import { CreateTenantDialog } from "../components/CreateTenantDialog";
import { TenantList } from "../components/TenantList";

export function AdminPage() {
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
        <TenantList />
      </div>
    </div>
  );
}
