import { Module } from '@nestjs/common';
import { TenantsService } from './tenants.service';
import { TenantConfigurationService } from './tenant-configuration.service';
import { TenantsRouter } from './tenants.router';
import { ContextModule } from '../../core/context/context.module';

@Module({
  imports: [ContextModule],
  providers: [TenantsService, TenantConfigurationService, TenantsRouter],
  exports: [TenantsService, TenantConfigurationService, TenantsRouter],
})
export class TenantsModule {}
