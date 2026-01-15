import { Injectable, CanActivate, ExecutionContext, UnauthorizedException } from '@nestjs/common';
import { clerkClient } from '@clerk/clerk-sdk-node';

@Injectable()
export class ClerkAuthGuard implements CanActivate {
  async canActivate(context: ExecutionContext): Promise<boolean> {
    const request = context.switchToHttp().getRequest();
    const authHeader = request.headers.authorization;

    if (!authHeader || !authHeader.startsWith('Bearer ')) {
      throw new UnauthorizedException('Missing or invalid authorization header');
    }

    const token = authHeader.substring(7);

    try {
      // Verify the session token with Clerk
      const session = await clerkClient.sessions.verifySession(token, token);

      if (!session) {
        throw new UnauthorizedException('Invalid session');
      }

      // Attach user info to request for downstream use
      request.user = {
        userId: session.userId,
        sessionId: session.id,
      };

      return true;
    } catch (error) {
      throw new UnauthorizedException('Invalid or expired token');
    }
  }
}
