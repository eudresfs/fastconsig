import { Module } from '@nestjs/common';
import { TrpcContext, createContext } from './trpc.context';
import { AppRouter } from '../../app.router';
import { TenantsModule } from '../../modules/tenants/tenants.module';
import { ContextModule } from '../context/context.module';
import { ContextService } from '../context/context.service';

@Module({
  imports: [TenantsModule, ContextModule],
  providers: [AppRouter, ContextService],
  exports: [AppRouter, ContextService],
})
export class TrpcModule {}
