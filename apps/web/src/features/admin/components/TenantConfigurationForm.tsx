import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { UpdateTenantConfigSchema, UpdateTenantConfigInput } from "@fast-consig/shared";
import { trpc } from "@/lib/trpc";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { toast } from "sonner";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";

import { Skeleton } from "@/components/ui/skeleton";

interface TenantConfigurationFormProps {
  tenantId: string;
}

export function TenantConfigurationForm({ tenantId }: TenantConfigurationFormProps) {
  const { data: config, isLoading, refetch } = trpc.tenants.getConfig.useQuery({ tenantId });

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors, isSubmitting, isDirty },
  } = useForm<UpdateTenantConfigInput>({
    resolver: zodResolver(UpdateTenantConfigSchema),
    defaultValues: {
      standardMarginPercent: 30,
      benefitCardMarginPercent: 5,
      payrollCutoffDay: 20,
      minInstallmentValueCents: 1000,
      maxInstallments: 96,
    },
  });

  useEffect(() => {
    if (config) {
      // Safely convert basis points to percent with validation
      const standardPercent = typeof config.standardMarginBasisPoints === 'number'
        ? config.standardMarginBasisPoints / 100
        : 30; // fallback to default
      const benefitPercent = typeof config.benefitCardMarginBasisPoints === 'number'
        ? config.benefitCardMarginBasisPoints / 100
        : 5; // fallback to default

      reset({
        standardMarginPercent: standardPercent,
        benefitCardMarginPercent: benefitPercent,
        payrollCutoffDay: config.payrollCutoffDay ?? 20,
        minInstallmentValueCents: config.minInstallmentValueCents ?? 1000,
        maxInstallments: config.maxInstallments ?? 96,
      });
    }
  }, [config, reset]);

  const updateConfigMutation = trpc.tenants.updateConfig.useMutation({
    onSuccess: () => {
      toast.success("Configuration updated successfully");
      refetch();
    },
    onError: (error: any) => {
      console.error("Failed to update configuration:", error);

      // Map specific errors to user-friendly messages
      let userMessage = "Failed to update configuration";

      if (error.message.includes("not found")) {
        userMessage = "Tenant not found. Please refresh and try again.";
      } else if (error.message.includes("Sum of")) {
        userMessage = "The sum of margins cannot exceed 100%";
      } else if (error.message.includes("decimal places")) {
        userMessage = "Margin values can have at most 2 decimal places";
      } else if (error.message.includes("Payroll cutoff day")) {
        userMessage = "Payroll cutoff day must be between 1 and 28";
      } else if (error.data?.code === "UNAUTHORIZED") {
        userMessage = "You don't have permission to update this configuration";
      } else if (error.data?.code === "BAD_REQUEST") {
        userMessage = "Invalid configuration values. Please check your input.";
      }

      toast.error(userMessage);
    },
  });

  const onSubmit = (data: UpdateTenantConfigInput) => {
    updateConfigMutation.mutate({ tenantId, config: data });
  };

  if (isLoading) {
    return (
      <Card>
        <CardHeader>
          <Skeleton className="h-8 w-[200px] mb-2" />
          <Skeleton className="h-4 w-[300px]" />
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Skeleton className="h-4 w-[100px]" />
              <Skeleton className="h-10 w-full" />
            </div>
            <div className="space-y-2">
              <Skeleton className="h-4 w-[100px]" />
              <Skeleton className="h-10 w-full" />
            </div>
          </div>
          <div className="grid grid-cols-3 gap-4">
            {[1, 2, 3].map((i) => (
              <div key={i} className="space-y-2">
                <Skeleton className="h-4 w-[100px]" />
                <Skeleton className="h-10 w-full" />
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Business Configuration</CardTitle>
        <CardDescription>
          Configure margin rules, payroll dates, and loan limits for this tenant.
        </CardDescription>
      </CardHeader>
      <form onSubmit={handleSubmit(onSubmit)}>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="standardMarginPercent">Standard Margin (%)</Label>
              <Input
                id="standardMarginPercent"
                type="number"
                step="0.01"
                min="0"
                max="100"
                {...register("standardMarginPercent", { valueAsNumber: true })}
              />
              {errors.standardMarginPercent && (
                <p className="text-sm text-red-500">{errors.standardMarginPercent.message}</p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="benefitCardMarginPercent">Benefit Card Margin (%)</Label>
              <Input
                id="benefitCardMarginPercent"
                type="number"
                step="0.01"
                min="0"
                max="100"
                {...register("benefitCardMarginPercent", { valueAsNumber: true })}
              />
              {errors.benefitCardMarginPercent && (
                <p className="text-sm text-red-500">{errors.benefitCardMarginPercent.message}</p>
              )}
            </div>
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div className="space-y-2">
              <Label htmlFor="payrollCutoffDay">Payroll Cutoff Day</Label>
              <Input
                id="payrollCutoffDay"
                type="number"
                min="1"
                max="31"
                {...register("payrollCutoffDay", { valueAsNumber: true })}
              />
              {errors.payrollCutoffDay && (
                <p className="text-sm text-red-500">{errors.payrollCutoffDay.message}</p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="maxInstallments">Max Installments</Label>
              <Input
                id="maxInstallments"
                type="number"
                min="1"
                max="120"
                {...register("maxInstallments", { valueAsNumber: true })}
              />
              {errors.maxInstallments && (
                <p className="text-sm text-red-500">{errors.maxInstallments.message}</p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="minInstallmentValueCents">Min Installment Value (Cents)</Label>
              <Input
                id="minInstallmentValueCents"
                type="number"
                min="0"
                {...register("minInstallmentValueCents", { valueAsNumber: true })}
              />
              <p className="text-xs text-muted-foreground">
                {(Number(config?.minInstallmentValueCents || 1000) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
              </p>
              {errors.minInstallmentValueCents && (
                <p className="text-sm text-red-500">{errors.minInstallmentValueCents.message}</p>
              )}
            </div>
          </div>
        </CardContent>
        <CardFooter>
          <Button type="submit" disabled={isSubmitting || !isDirty}>
            {isSubmitting ? "Saving..." : "Save Configuration"}
          </Button>
        </CardFooter>
      </form>
    </Card>
  );
}
