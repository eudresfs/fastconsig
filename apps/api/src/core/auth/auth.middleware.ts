import { Injectable, NestMiddleware, Logger } from '@nestjs/common';
import { FastifyRequest, FastifyReply } from 'fastify';
import { verifyToken } from '@clerk/clerk-sdk-node';
import { ConfigService } from '@nestjs/config';
import { ContextService } from '../context/context.service';

@Injectable()
export class ClerkAuthMiddleware implements NestMiddleware {
  private readonly logger = new Logger(ClerkAuthMiddleware.name);

  constructor(
    private readonly configService: ConfigService,
    private readonly contextService: ContextService,
  ) {}

  async use(req: FastifyRequest['raw'], res: FastifyReply['raw'], next: () => void) {
    const token = this.extractToken(req);

    if (!token) {
      // No token provided, continue without context (public routes might exist)
      // or we could enforce it here. For now, let's allow it and let Guards decide.
      return next();
    }

    try {
      const secretKey = this.configService.get<string>('CLERK_SECRET_KEY');

      const claims = await verifyToken(token, {
        secretKey: secretKey,
      });

      const userContext = {
        userId: claims.sub,
        tenantId: claims.org_id,
        email: undefined, // Clerk JWT might not have email by default without extra config
        roles: claims.org_role ? [claims.org_role] : [],
        permissions: claims.org_permissions ? claims.org_permissions : [],
      };

      this.contextService.run(userContext, () => {
        next();
      });
    } catch (error) {
      this.logger.error('Token verification failed', error);
      // If token is invalid, strictly speaking we should probably fail?
      // Or just not set context. Let's fail for security if a token was attempted.
      // However, NestMiddleware 'next' with error might not be the best for Fastify adapter sometimes.
      // Let's just log and continue empty, let the Guard handle 401.
      next();
    }
  }

  private extractToken(req: FastifyRequest['raw']): string | undefined {
    // Check Authorization header
    const authHeader = req.headers['authorization'];
    if (authHeader && authHeader.startsWith('Bearer ')) {
      return authHeader.substring(7);
    }
    return undefined;
  }
}
