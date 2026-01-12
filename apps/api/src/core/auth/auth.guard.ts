import { CanActivate, ExecutionContext, Injectable, UnauthorizedException } from '@nestjs/common';
import { ContextService } from '../context/context.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private readonly contextService: ContextService) {}

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  canActivate(_context: ExecutionContext): boolean {
    const userContext = this.contextService.getContext();

    if (!userContext || !userContext.userId) {
      throw new UnauthorizedException('User context not found or invalid');
    }

    return true;
  }
}
