import { Module } from "@nestjs/common";
import { ConfigModule } from "@nestjs/config";
import { AppController } from "./app.controller";
import { AppService } from "./app.service";
import { ContextModule } from "./core/context/context.module";
import { AuthModule } from "./core/auth/auth.module";
import { TrpcModule } from "./core/trpc/trpc.module";
import { AppRouter } from "./app.router";
import { TenantsModule } from "./modules/tenants/tenants.module";

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
    }),
    ContextModule,
    AuthModule,
    TrpcModule,
    TenantsModule,
  ],
  controllers: [AppController],
  providers: [AppService, AppRouter],
})
export class AppModule {}
