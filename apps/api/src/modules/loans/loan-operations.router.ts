import { Injectable } from '@nestjs/common';
import { z } from 'zod';
import { router, protectedProcedure } from '../../core/trpc/trpc.init';
import { SecurityService } from '../../shared/services/security.service';
import { ContextService } from '../../core/context/context.service';
import { TRPCError } from '@trpc/server';

@Injectable()
export class LoanOperationsRouter {
  constructor(
    private readonly securityService: SecurityService,
    private readonly contextService: ContextService,
  ) {}

  get router() {
    return router({
      checkMargin: protectedProcedure
        .input(z.object({ cpf: z.string().length(11) }))
        .query(async ({ input }) => {
          const ip = this.contextService.getIp() || 'unknown';

          // 1. Security Check (Anomaly Detection)
          try {
            await this.securityService.checkMarginQueryAnomaly(input.cpf, ip);
          } catch (error: any) {
            // Convert security error to TRPC error
            if (error.message.includes('Security Anomaly Detected')) {
              throw new TRPCError({
                code: 'TOO_MANY_REQUESTS',
                message: 'Request blocked due to suspicious activity. Please try again later.',
              });
            }
            throw error;
          }

          // 2. Mock Business Logic (Placeholder for Epic 4)
          return {
            cpf: input.cpf,
            availableMargin: 1500.00, // Mock value
            timestamp: new Date(),
          };
        }),
    });
  }
}
