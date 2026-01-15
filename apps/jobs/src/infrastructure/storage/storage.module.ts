import { Module, DynamicModule, Logger } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { STORAGE_SERVICE } from './storage.interface';
import { LocalStorageService } from './local-storage.service';
import { OciStorageService } from './oci-storage.service';

export type StorageProvider = 'local' | 'oci';

@Module({})
export class StorageModule {
  private static readonly logger = new Logger(StorageModule.name);

  /**
   * Register StorageModule with provider selection based on environment
   */
  static forRoot(): DynamicModule {
    return {
      module: StorageModule,
      imports: [ConfigModule],
      providers: [
        {
          provide: STORAGE_SERVICE,
          useFactory: (configService: ConfigService) => {
            const provider = configService.get<StorageProvider>('STORAGE_PROVIDER', 'local');

            StorageModule.logger.log(`Initializing storage provider: ${provider}`);

            if (provider === 'oci') {
              // Check if OCI credentials are configured
              const tenancyId = configService.get<string>('OCI_TENANCY_OCID');
              if (!tenancyId) {
                StorageModule.logger.warn(
                  'OCI credentials not configured, falling back to local storage',
                );
                return new LocalStorageService(configService);
              }
              return new OciStorageService(configService);
            }

            return new LocalStorageService(configService);
          },
          inject: [ConfigService],
        },
      ],
      exports: [STORAGE_SERVICE],
    };
  }

  /**
   * Register StorageModule with a specific provider (for testing)
   */
  static forProvider(provider: StorageProvider): DynamicModule {
    return {
      module: StorageModule,
      imports: [ConfigModule],
      providers: [
        {
          provide: STORAGE_SERVICE,
          useFactory: (configService: ConfigService) => {
            if (provider === 'oci') {
              return new OciStorageService(configService);
            }
            return new LocalStorageService(configService);
          },
          inject: [ConfigService],
        },
      ],
      exports: [STORAGE_SERVICE],
    };
  }
}
