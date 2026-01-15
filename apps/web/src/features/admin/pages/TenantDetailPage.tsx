import { TenantConfigurationForm } from "../components/TenantConfigurationForm";
import { TeamPage } from "../../team/pages/TeamPage";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

interface TenantDetailPageProps {
  tenantId: string;
  tenantName: string;
  onBack: () => void;
}

export function TenantDetailPage({ tenantId, tenantName, onBack }: TenantDetailPageProps) {
  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-4">
        <Button variant="outline" onClick={onBack}>
          &larr; Back
        </Button>
        <h2 className="text-2xl font-bold tracking-tight">{tenantName}</h2>
      </div>

      <Tabs defaultValue="config" className="w-full">
        <TabsList>
          <TabsTrigger value="config">Configuration</TabsTrigger>
          <TabsTrigger value="team">Team Management</TabsTrigger>
        </TabsList>

        <TabsContent value="config">
          <div className="grid gap-6 md:grid-cols-2">
            <Card>
              <CardHeader>
                <CardTitle>Tenant Details</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">ID: {tenantId}</p>
                {/* Add more readonly details here if needed */}
              </CardContent>
            </Card>

            <TenantConfigurationForm tenantId={tenantId} />
          </div>
        </TabsContent>

        <TabsContent value="team">
          <Card>
            <CardHeader>
              <CardTitle>Team Members</CardTitle>
            </CardHeader>
            <CardContent>
              <TeamPage tenantId={tenantId} />
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
