import { describe, it, expect, beforeEach, vi } from 'vitest';
import { MarginCalculationService } from '../margin-calculation.service';
import * as database from '@fast-consig/database';

// Mock the database module
vi.mock('@fast-consig/database', () => ({
  db: {
    query: {
      tenantConfigurations: {
        findFirst: vi.fn(),
      },
    },
  },
  tenantConfigurations: {},
  eq: vi.fn(),
}));

describe('MarginCalculationService', () => {
  let service: MarginCalculationService;
  let mockFindFirst: any;

  beforeEach(() => {
    service = new MarginCalculationService();
    mockFindFirst = database.db.query.tenantConfigurations.findFirst;

    // Reset mock call count
    vi.clearAllMocks();

    // Default mock: return 30% standard margin
    mockFindFirst.mockResolvedValue({
      standardMarginPercentage: 0.30,
      benefitCardMarginPercentage: 0.05,
    });
  });

  describe('calculateAvailableMargin', () => {
    it('should calculate margin correctly with standard scenario', async () => {
      // AC1: 5000 gross, 1000 discounts, 30% = 1200 margin
      const result = await service.calculateAvailableMargin(
        500000, // R$ 5.000,00 em centavos
        100000, // R$ 1.000,00 em centavos
        'tenant-123',
      );

      expect(result.availableMargin).toBe(120000); // R$ 1.200,00 em centavos
      expect(result.standardMargin).toBe(120000);
    });

    it('should return zero margin for negative gross salary', async () => {
      const result = await service.calculateAvailableMargin(
        -100000, // negative salary
        50000,
        'tenant-123',
      );

      expect(result.availableMargin).toBe(0);
      expect(result.standardMargin).toBe(0);
    });

    it('should return zero margin for negative discounts', async () => {
      const result = await service.calculateAvailableMargin(
        500000,
        -50000, // negative discounts
        'tenant-123',
      );

      expect(result.availableMargin).toBe(0);
      expect(result.standardMargin).toBe(0);
    });

    it('should return zero margin when discounts exceed salary', async () => {
      const result = await service.calculateAvailableMargin(
        500000, // R$ 5.000,00
        600000, // R$ 6.000,00 (maior que salário)
        'tenant-123',
      );

      expect(result.availableMargin).toBe(0);
      expect(result.standardMargin).toBe(0);
    });

    it('should return zero margin for zero gross salary', async () => {
      const result = await service.calculateAvailableMargin(
        0,
        0,
        'tenant-123',
      );

      expect(result.availableMargin).toBe(0);
      expect(result.standardMargin).toBe(0);
    });

    it('should cache tenant margin rules', async () => {
      // First call - should fetch from database
      await service.calculateAvailableMargin(
        500000,
        100000,
        'tenant-cache-test',
      );

      expect(mockFindFirst).toHaveBeenCalledTimes(1);

      // Second call - should use cache
      await service.calculateAvailableMargin(
        500000,
        100000,
        'tenant-cache-test',
      );

      // Should still be 1 call (cached)
      expect(mockFindFirst).toHaveBeenCalledTimes(1);
    });

    it('should clear cache for specific tenant', async () => {
      await service.calculateAvailableMargin(500000, 100000, 'tenant-clear');

      // Clear cache
      service.clearCacheForTenant('tenant-clear');

      // Next call should fetch from database again
      await service.calculateAvailableMargin(500000, 100000, 'tenant-clear');

      // Should have 2 calls (cache was cleared)
      expect(mockFindFirst).toHaveBeenCalledTimes(2);
    });

    it('should use default 30% margin percentage when tenant config missing', async () => {
      // Mock missing config
      mockFindFirst.mockResolvedValueOnce(null);

      // Default: 30% of net salary
      // Net: 5000 - 1000 = 4000
      // Margin: 4000 * 0.30 = 1200
      const result = await service.calculateAvailableMargin(
        500000,
        100000,
        'tenant-no-config',
      );

      expect(result.availableMargin).toBe(120000);
    });
  });
});
