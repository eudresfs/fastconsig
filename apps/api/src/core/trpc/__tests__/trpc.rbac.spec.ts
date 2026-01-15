import { describe, it, expect, vi } from 'vitest';
import { TRPCError } from '@trpc/server';
import { adminProcedure, operatorProcedure, router } from '../trpc.init';
import { z } from 'zod';

// Mock context helper
const createMockContext = (roles: string[]) => ({
  user: {
    userId: 'user_1',
    tenantId: 'tenant_1',
    roles,
  },
  req: {} as any,
  res: {} as any,
  contextService: {} as any,
});

describe('RBAC Procedures', () => {
  // Define a router using the procedures we want to test
  const testRouter = router({
    adminOnly: adminProcedure.query(() => 'success_admin'),
    operatorOnly: operatorProcedure.query(() => 'success_operator'),
  });

  const callerFactory = (roles: string[]) => testRouter.createCaller(createMockContext(roles));

  describe('adminProcedure', () => {
    it('should allow access for org:admin', async () => {
      const caller = callerFactory(['org:admin']);
      const result = await caller.adminOnly();
      expect(result).toBe('success_admin');
    });

    it('should deny access for org:member', async () => {
      const caller = callerFactory(['org:member']);
      await expect(caller.adminOnly()).rejects.toThrow(TRPCError);
      await expect(caller.adminOnly()).rejects.toHaveProperty('code', 'FORBIDDEN');
    });

    it('should deny access for users with no roles', async () => {
      const caller = callerFactory([]);
      await expect(caller.adminOnly()).rejects.toThrow(TRPCError);
    });
  });

  describe('operatorProcedure', () => {
    it('should allow access for org:member', async () => {
      const caller = callerFactory(['org:member']);
      const result = await caller.operatorOnly();
      expect(result).toBe('success_operator');
    });

    it('should allow access for org:admin (implicit operator access)', async () => {
      const caller = callerFactory(['org:admin']);
      const result = await caller.operatorOnly();
      expect(result).toBe('success_operator');
    });

    it('should deny access for users with no roles', async () => {
      const caller = callerFactory([]);
      await expect(caller.operatorOnly()).rejects.toThrow(TRPCError);
    });
  });
});
