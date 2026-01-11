import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'

// Mock dotenv to prevent loading .env file
vi.mock('dotenv', () => ({
  default: {
    config: vi.fn(),
  },
}))

describe('Config', () => {
  const originalEnv = process.env

  beforeEach(() => {
    vi.resetModules()
    process.env = { ...originalEnv }
    // Ensure critical env vars are set for happy path
    process.env.DATABASE_URL = 'postgresql://user:pass@localhost:5432/db'
    process.env.JWT_ACCESS_SECRET = 'super-secret-access-token-key-32-chars'
    process.env.JWT_REFRESH_SECRET = 'super-secret-refresh-token-key-32-chars'
    // Clear others to test defaults
    delete process.env.PORT
    delete process.env.REDIS_URL
    delete process.env.CORS_ORIGIN
    delete process.env.NODE_ENV
  })

  afterEach(() => {
    process.env = originalEnv
    vi.unstubAllEnvs()
  })

  describe('Env', () => {
    it('should parse valid environment variables', async () => {
      process.env.NODE_ENV = 'test'
      const { env } = await import('./env')
      expect(env.DATABASE_URL).toBeDefined()
      expect(env.PORT).toBe(3001) // Default
      expect(env.NODE_ENV).toBe('test')
    })

    it('should throw error if required variable is missing', async () => {
      delete process.env.DATABASE_URL
      await expect(import('./env')).rejects.toThrow()
    })

    it('should use default values', async () => {
      // Ensure defaults are used by removing specific vars
      delete process.env.PORT
      delete process.env.REDIS_URL
      delete process.env.CORS_ORIGIN

      // NODE_ENV defaults to 'development' in schema
      delete process.env.NODE_ENV

      const { env } = await import('./env')

      expect(env.PORT).toBe(3001)
      expect(env.REDIS_URL).toBe('redis://localhost:6379')
      expect(env.CORS_ORIGIN).toBe('http://localhost:3000')
    })
  })

  describe('Logger', () => {
    it('should configure logger for development', async () => {
      vi.doMock('./env', () => ({
        env: {
          NODE_ENV: 'development',
        }
      }))

      const { logger } = await import('./logger')
      expect(logger.level).toBe('debug')
    })

    it('should configure logger for production', async () => {
      vi.doMock('./env', () => ({
        env: {
          NODE_ENV: 'production',
        }
      }))

      const { logger } = await import('./logger')
      expect(logger.level).toBe('info')
    })
  })
})
