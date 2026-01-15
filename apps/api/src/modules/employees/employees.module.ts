import { Module } from '@nestjs/common';
import { EmployeesController } from './employees.controller';
import { EmployeesService } from './employees.service';
import { MarginCalculationService } from '../../shared/services/margin-calculation.service';
import { AuditModule } from '../../shared/audit.module';
import { ContextModule } from '../../core/context/context.module';

@Module({
  imports: [AuditModule, ContextModule],
  controllers: [EmployeesController],
  providers: [EmployeesService, MarginCalculationService],
  exports: [EmployeesService, MarginCalculationService],
})
export class EmployeesModule {}
