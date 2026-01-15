import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { BullModule } from '@nestjs/bullmq';
import { EmployeeImportJob, EMPLOYEE_IMPORT_QUEUE } from './jobs/employee-import.job';
import { StreamProcessor } from './processors/stream-processor';
import { MarginCalculationService } from './services/margin-calculation.service';
import { StorageModule } from './infrastructure/storage';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
    }),
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
          defaultJobOptions: {
            attempts: 3,
            backoff: {
              type: 'exponential',
              delay: 1000,
            },
            removeOnComplete: {
              count: 100, // Keep last 100 completed jobs
              age: 24 * 60 * 60, // Keep for 24 hours
            },
            removeOnFail: {
              count: 50, // Keep last 50 failed jobs
              age: 7 * 24 * 60 * 60, // Keep for 7 days
            },
          },
        };
      },
    }),
    BullModule.registerQueue({
      name: EMPLOYEE_IMPORT_QUEUE,
      defaultJobOptions: {
        attempts: 3,
        backoff: {
          type: 'exponential',
          delay: 2000,
        },
      },
    }),
  ],
  controllers: [],
  providers: [
    EmployeeImportJob,
    StreamProcessor,
    MarginCalculationService,
  ],
})
export class AppModule {}
