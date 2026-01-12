import { Module } from '@nestjs/common';
import { TenantsService } from './tenants.service';
import { TenantsRouter } from './tenants.router';
import { ContextModule } from '../../core/context/context.module';

@Module({
  imports: [ContextModule],
  providers: [TenantsService, TenantsRouter],
  exports: [TenantsService, TenantsRouter],
})
export class TenantsModule {}
