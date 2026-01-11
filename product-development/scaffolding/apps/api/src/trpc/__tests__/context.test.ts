import { describe, it, expect, vi, beforeEach } from 'vitest'
import { createContext } from '../context'

describe('TRPC Context', () => {
  const mockReq = {
    headers: {},
    jwtVerify: vi.fn(),
  } as any

  const mockRes = {} as any

  beforeEach(() => {
    vi.clearAllMocks()
    mockReq.headers = {}
  })

  it('should create context with empty user if no token', async () => {
    const ctx = await createContext({ req: mockReq, res: mockRes })

    expect(ctx.userId).toBeNull()
    expect(ctx.tenantId).toBeNull()
    expect(ctx.prisma).toBeDefined()
  })

  it('should extract user and tenant from valid token', async () => {
    mockReq.headers.authorization = 'Bearer valid_token'
    mockReq.jwtVerify.mockResolvedValue({ sub: 1, tenantId: 10 })

    const ctx = await createContext({ req: mockReq, res: mockRes })

    expect(ctx.userId).toBe(1)
    expect(ctx.tenantId).toBe(10)
  })

  it('should handle invalid token gracefully', async () => {
    mockReq.headers.authorization = 'Bearer invalid'
    mockReq.jwtVerify.mockRejectedValue(new Error('Invalid token'))

    const ctx = await createContext({ req: mockReq, res: mockRes })

    expect(ctx.userId).toBeNull()
    expect(ctx.tenantId).toBeNull()
  })

  it('should handle missing bearer prefix', async () => {
    // The implementation replaces 'Bearer ', so if missing, it just uses the string?
    // Let's check implementation: req.headers.authorization?.replace('Bearer ', '')
    // If header is "Token 123", it becomes "Token 123"
    // If header is "123", it is "123"
    // verify will fail if format is wrong, but let's test extraction logic if needed.
    // Actually test covers the "graceful failure" which includes format errors caught by verify.

    mockReq.headers.authorization = 'invalid_format'
    mockReq.jwtVerify.mockRejectedValue(new Error('Invalid'))

    const ctx = await createContext({ req: mockReq, res: mockRes })
    expect(ctx.userId).toBeNull()
  })
})
