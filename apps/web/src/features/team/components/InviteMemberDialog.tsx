import { useState } from 'react';
import { trpc } from '../../../lib/trpc';
import { Button } from '../../../components/ui/button';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '../../../components/ui/dialog';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';
import { toast } from 'sonner';

export function InviteMemberDialog({ tenantId }: { tenantId: string }) {
  const [open, setOpen] = useState(false);
  const [email, setEmail] = useState('');
  const [role, setRole] = useState('org:member'); // Default to Operator

  const utils = trpc.useContext();
  const inviteMutation = trpc.tenants.inviteMember.useMutation({
    onSuccess: () => {
      toast.success('Invitation sent successfully');
      setOpen(false);
      setEmail('');
      utils.tenants.listMembers.invalidate({ tenantId });
    },
    onError: (error) => {
      toast.error(error.message);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    inviteMutation.mutate({ tenantId, email, role: role as 'org:admin' | 'org:member' });
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>Invite Member</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Invite New Member</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <Label htmlFor="email">Email Address</Label>
            <Input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="colleague@example.com"
              required
            />
          </div>
          <div>
            <Label htmlFor="role">Role</Label>
            <select
              id="role"
              value={role}
              onChange={(e) => setRole(e.target.value)}
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
            >
              <option value="org:member">Operator (Restricted)</option>
              <option value="org:admin">Manager (Admin)</option>
            </select>
          </div>
          <Button type="submit" disabled={inviteMutation.isLoading}>
            {inviteMutation.isLoading ? 'Sending...' : 'Send Invitation'}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
