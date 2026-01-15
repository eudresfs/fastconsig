import { trpc } from '../../../lib/trpc';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '../../../components/ui/table';
import { Button } from '../../../components/ui/button';
import { toast } from 'sonner';
import { Skeleton } from '../../../components/ui/skeleton';

export function MemberList({ tenantId }: { tenantId: string }) {
  const utils = trpc.useContext();
  const { data: members, isLoading } = trpc.tenants.listMembers.useQuery({ tenantId });

  const removeMutation = trpc.tenants.removeMember.useMutation({
    onSuccess: () => {
      toast.success('Member removed successfully');
      utils.tenants.listMembers.invalidate({ tenantId });
    },
    onError: (error) => {
      toast.error(error.message);
    },
  });

  const handleRemove = (userId: string) => {
    if (confirm('Are you sure you want to remove this member?')) {
      removeMutation.mutate({ tenantId, userId });
    }
  };

  if (isLoading) {
    return <Skeleton className="w-full h-48" />;
  }

  return (
    <div className="border rounded-md">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Email</TableHead>
            <TableHead>Role</TableHead>
            <TableHead>Joined At</TableHead>
            <TableHead className="text-right">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {members?.map((member) => (
            <TableRow key={member.userId}>
              <TableCell>{member.firstName} {member.lastName}</TableCell>
              <TableCell>{member.email}</TableCell>
              <TableCell>
                <span className={`px-2 py-1 rounded-full text-xs ${member.role === 'org:admin' ? 'bg-blue-100 text-blue-800' : 'bg-gray-100 text-gray-800'}`}>
                  {member.role === 'org:admin' ? 'Manager' : 'Operator'}
                </span>
              </TableCell>
              <TableCell>{new Date(member.joinedAt).toLocaleDateString()}</TableCell>
              <TableCell className="text-right">
                <Button
                  variant="destructive"
                  size="sm"
                  onClick={() => handleRemove(member.userId)}
                  disabled={removeMutation.isLoading}
                >
                  Remove
                </Button>
              </TableCell>
            </TableRow>
          ))}
          {members?.length === 0 && (
            <TableRow>
              <TableCell colSpan={5} className="text-center py-4">
                No members found
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
}
