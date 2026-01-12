import { Module, MiddlewareConsumer, RequestMethod } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { ClerkAuthMiddleware } from './auth.middleware';
import { AuthGuard } from './auth.guard';
import { ContextModule } from '../context/context.module';

@Module({
  imports: [ConfigModule, ContextModule],
  providers: [ClerkAuthMiddleware, AuthGuard],
  exports: [AuthGuard],
})
export class AuthModule {
  configure(consumer: MiddlewareConsumer) {
    consumer
      .apply(ClerkAuthMiddleware)
      .forRoutes({ path: '*', method: RequestMethod.ALL });
  }
}
