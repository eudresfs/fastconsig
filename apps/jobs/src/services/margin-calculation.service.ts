import { Injectable, Logger, OnModuleDestroy } from '@nestjs/common';
import { db } from '@fast-consig/database';
import { tenantConfigurations } from '@fast-consig/database';
import { eq, sql } from 'drizzle-orm';

export interface MarginRules {
  standardMarginPercentage: number; // e.g., 0.30 (30%)
  benefitCardMarginPercentage: number; // e.g., 0.05 (5%)
}

export interface MarginCalculationResult {
  availableMargin: number; // em centavos
  standardMargin: number; // em centavos
  benefitCardMargin: number; // em centavos (opcional)
}

/**
 * MarginCalculationService for Jobs App
 * NOTE: This is a copy of the service in apps/api/src/shared/services/margin-calculation.service.ts
 * Future improvement: Move to packages/shared to avoid duplication (H4 tech debt)
 */
@Injectable()
export class MarginCalculationService implements OnModuleDestroy {
  private readonly logger = new Logger(MarginCalculationService.name);
  private marginRulesCache: Map<string, MarginRules> = new Map();
  private cacheTimers: Map<string, NodeJS.Timeout> = new Map();

  /**
   * Calculate available margin based on salary and tenant rules
   * @param grossSalary - Gross salary in centavos
   * @param mandatoryDiscounts - Mandatory discounts in centavos
   * @param tenantId - Tenant ID for margin rules lookup
   * @returns Calculated margin in centavos
   */
  async calculateAvailableMargin(
    grossSalary: number,
    mandatoryDiscounts: number,
    tenantId: string,
  ): Promise<MarginCalculationResult> {
    // Input validation
    if (grossSalary < 0 || mandatoryDiscounts < 0) {
      this.logger.warn('Invalid salary or discounts (negative values)', {
        grossSalary,
        mandatoryDiscounts,
      });
      return { availableMargin: 0, standardMargin: 0, benefitCardMargin: 0 };
    }

    // Calculate net salary
    const netSalary = grossSalary - mandatoryDiscounts;
    if (netSalary <= 0) {
      this.logger.warn('Net salary is zero or negative', { grossSalary, mandatoryDiscounts });
      return { availableMargin: 0, standardMargin: 0, benefitCardMargin: 0 };
    }

    // Get tenant margin rules (with caching)
    const marginRules = await this.getTenantMarginRules(tenantId);

    // Calculate standard margin
    const standardMargin = Math.floor(netSalary * marginRules.standardMarginPercentage);

    // Calculate benefit card margin (optional, depends on tenant config)
    const benefitCardMargin = Math.floor(netSalary * marginRules.benefitCardMarginPercentage);

    // Total available margin (for now, just standard margin)
    // Future: Add benefit card margin if tenant supports it
    const availableMargin = standardMargin;

    this.logger.debug('Margin calculated', {
      tenantId,
      grossSalary,
      mandatoryDiscounts,
      netSalary,
      standardMargin,
      availableMargin,
    });

    return { availableMargin, standardMargin, benefitCardMargin };
  }

  /**
   * Get tenant margin rules with caching
   * @param tenantId - Tenant ID
   * @returns Margin rules configuration
   */
  private async getTenantMarginRules(tenantId: string): Promise<MarginRules> {
    // Check cache first
    const cached = this.marginRulesCache.get(tenantId);
    if (cached) {
      return cached;
    }

    // Set RLS context for multi-tenancy isolation
    await db.execute(sql`SELECT set_config('app.current_tenant_id', ${tenantId}, TRUE)`);

    // Load from database
    const config = await db.query.tenantConfigurations.findFirst({
      where: (configs, { eq }) => eq(configs.tenantId, tenantId),
    });

    // Convert basis points to decimal percentages
    // Example: 3000 basis points = 30.00% = 0.30 decimal
    const marginRules: MarginRules = {
      standardMarginPercentage: (config?.standardMarginBasisPoints ?? 3000) / 10000, // Default 30%
      benefitCardMarginPercentage: (config?.benefitCardMarginBasisPoints ?? 500) / 10000, // Default 5%
    };

    // Cache for 5 minutes (300000ms)
    this.marginRulesCache.set(tenantId, marginRules);

    // Clear any existing timer for this tenant
    const existingTimer = this.cacheTimers.get(tenantId);
    if (existingTimer) {
      clearTimeout(existingTimer);
    }

    // Set new timer and track it for cleanup
    const timer = setTimeout(() => {
      this.marginRulesCache.delete(tenantId);
      this.cacheTimers.delete(tenantId);
    }, 300000);
    this.cacheTimers.set(tenantId, timer);

    return marginRules;
  }

  /**
   * Clear cache for a specific tenant (call after config update)
   * @param tenantId - Tenant ID
   */
  clearCacheForTenant(tenantId: string): void {
    this.marginRulesCache.delete(tenantId);
    const timer = this.cacheTimers.get(tenantId);
    if (timer) {
      clearTimeout(timer);
      this.cacheTimers.delete(tenantId);
    }
  }

  /**
   * Lifecycle hook to clean up all timers when module is destroyed
   * Prevents memory leaks from dangling setTimeout references
   */
  onModuleDestroy(): void {
    this.logger.debug('Cleaning up margin calculation service timers');
    for (const timer of this.cacheTimers.values()) {
      clearTimeout(timer);
    }
    this.cacheTimers.clear();
    this.marginRulesCache.clear();
  }
}
