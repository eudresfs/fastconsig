import { trpc } from "@/lib/trpc";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { format } from "date-fns";

export function TenantList() {
  const { data: tenants, isLoading, error } = trpc.tenants.list.useQuery();

  if (isLoading) {
    return <div>Loading tenants...</div>;
  }

  if (error) {
    return <div className="text-red-500">Error loading tenants: {error.message}</div>;
  }

  return (
    <Table>
      <TableCaption>A list of registered tenants.</TableCaption>
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>CNPJ</TableHead>
          <TableHead>Slug</TableHead>
          <TableHead>Status</TableHead>
          <TableHead>Created At</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {tenants?.map((tenant) => (
          <TableRow key={tenant.id}>
            <TableCell className="font-medium">{tenant.name}</TableCell>
            <TableCell>{tenant.cnpj}</TableCell>
            <TableCell>{tenant.slug}</TableCell>
            <TableCell>
              {tenant.active ? (
                <span className="text-green-600">Active</span>
              ) : (
                <span className="text-red-600">Inactive</span>
              )}
            </TableCell>
            <TableCell>
              {format(new Date(tenant.createdAt), "dd/MM/yyyy HH:mm")}
            </TableCell>
          </TableRow>
        ))}
        {tenants?.length === 0 && (
          <TableRow>
            <TableCell colSpan={5} className="text-center">
              No tenants found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
}
