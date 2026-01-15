import { InviteMemberDialog } from '../components/InviteMemberDialog';
import { MemberList } from '../components/MemberList';

export function TeamPage({ tenantId }: { tenantId: string }) {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Team Management</h2>
          <p className="text-muted-foreground">
            Manage your organization members and their permissions.
          </p>
        </div>
        <InviteMemberDialog tenantId={tenantId} />
      </div>

      <MemberList tenantId={tenantId} />
    </div>
  );
}
