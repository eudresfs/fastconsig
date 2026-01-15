import { Module } from "@nestjs/common";
import { ConfigModule, ConfigService } from "@nestjs/config";
import { APP_INTERCEPTOR, APP_GUARD } from "@nestjs/core";
import { ThrottlerModule, ThrottlerGuard } from "@nestjs/throttler";
import { AppController } from "./app.controller";
import { AppService } from "./app.service";
import { ContextModule } from "./core/context/context.module";
import { AuthModule } from "./core/auth/auth.module";
import { TrpcModule } from "./core/trpc/trpc.module";
import { AppRouter } from "./app.router";
import { TenantsModule } from "./modules/tenants/tenants.module";
import { EmployeesModule } from "./modules/employees/employees.module";
import { AuditModule } from "./shared/audit.module";
import { AuditLogInterceptor } from "./shared/interceptors/audit-log.interceptor";
import { RedisThrottlerStorageService } from "./shared/services/redis-throttler-storage.service";
import { SecurityService } from "./shared/services/security.service";
import { LoanOperationsRouter } from "./modules/loans/loan-operations.router";
import { RATE_LIMITS } from "@fast-consig/shared";

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
    }),
    ThrottlerModule.forRootAsync({
      imports: [ConfigModule],
      inject: [ConfigService, RedisThrottlerStorageService],
      useFactory: (config: ConfigService, storage: RedisThrottlerStorageService) => ({
        throttlers: [
          {
            ttl: RATE_LIMITS.STANDARD.TTL,
            limit: RATE_LIMITS.STANDARD.LIMIT,
          },
        ],
        storage: storage,
      }),
    }),
    ContextModule,
    AuthModule,
    TrpcModule,
    TenantsModule,
    EmployeesModule,
    AuditModule, // Global audit infrastructure
  ],
  controllers: [AppController],
  providers: [
    AppService,
    AppRouter,
    LoanOperationsRouter, // Register new router for Epic 4 placeholder
    SecurityService, // Register SecurityService for Story 1.6
    RedisThrottlerStorageService, // Register as provider for DI lifecycle management
    {
      provide: APP_INTERCEPTOR,
      useClass: AuditLogInterceptor, // Register audit interceptor globally
    },
    {
      provide: APP_GUARD,
      useClass: ThrottlerGuard, // Register throttler guard globally
    },
  ],
})
export class AppModule {}
