import { Injectable, OnModuleDestroy } from '@nestjs/common';
import { ThrottlerStorage } from '@nestjs/throttler';
import { ThrottlerStorageRecord } from '@nestjs/throttler/dist/throttler-storage-record.interface';
import { ConfigService } from '@nestjs/config';
import Redis from 'ioredis';

@Injectable()
export class RedisThrottlerStorageService implements ThrottlerStorage, OnModuleDestroy {
  private readonly redis: Redis;

  constructor(private readonly configService: ConfigService) {
    const redisUrl = this.configService.get<string>('REDIS_URL', 'redis://localhost:6379');
    this.redis = new Redis(redisUrl);
  }

  async increment(key: string, ttl: number): Promise<ThrottlerStorageRecord> {
    const totalHits = await this.redis.incr(key);

    if (totalHits === 1) {
      await this.redis.expire(key, Math.ceil(ttl / 1000));
    }

    const timeToExpire = await this.redis.ttl(key);

    return {
      totalHits,
      timeToExpire: timeToExpire > 0 ? timeToExpire : Math.ceil(ttl / 1000),
    };
  }

  onModuleDestroy() {
    this.redis.disconnect();
  }
}
