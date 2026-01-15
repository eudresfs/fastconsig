import { Module } from '@nestjs/common';
import { BullModule } from '@nestjs/bullmq';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { EmployeesController } from './employees.controller';
import { EmployeesService } from './employees.service';
import { EmployeeImportController } from './employee-import.controller';
import { EmployeeImportService, EMPLOYEE_IMPORT_QUEUE } from './employee-import.service';
import { MarginCalculationService } from '../../shared/services/margin-calculation.service';
import { AuditModule } from '../../shared/audit.module';
import { ContextModule } from '../../core/context/context.module';
import { StorageModule } from '../../infrastructure/storage';

@Module({
  imports: [
    AuditModule,
    ContextModule,
    StorageModule.forRoot(),
    BullModule.forRootAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: (configService: ConfigService) => {
        const password = configService.get<string>('REDIS_PASSWORD');
        return {
          connection: {
            host: configService.get<string>('REDIS_HOST') || 'localhost',
            port: configService.get<number>('REDIS_PORT') || 6379,
            ...(password ? { password } : {}),
          },
        };
      },
    }),
    BullModule.registerQueue({
      name: EMPLOYEE_IMPORT_QUEUE,
    }),
  ],
  controllers: [EmployeesController, EmployeeImportController],
  providers: [EmployeesService, EmployeeImportService, MarginCalculationService],
  exports: [EmployeesService, EmployeeImportService, MarginCalculationService],
})
export class EmployeesModule {}
