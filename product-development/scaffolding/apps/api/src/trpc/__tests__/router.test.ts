import { describe, it, expect } from 'vitest'
import { appRouter } from '../router'

describe('App Router', () => {
  it('should have all expected sub-routers', () => {
    expect(appRouter).toBeDefined()
    expect(appRouter._def.procedures).toBeDefined()

    // Check if sub-routers are mounted (checking via procedures key prefixes)
    // tRPC v10 structure is different, usually we check keys of the router record if available
    // or check if specific procedures exist

    // Use createCaller to verify structure via the public API
    // We mock the context as it might be used during router creation/initialization if there are middlewares
    const mockCtx = {
      prisma: {},
      userId: null,
      tenantId: null,
      req: {},
      res: {},
    } as any

    const caller = appRouter.createCaller(mockCtx)

    expect(caller.auth).toBeDefined()
    expect(caller.auth.login).toBeDefined()

    expect(caller.funcionarios).toBeDefined()
    expect(caller.funcionarios.list).toBeDefined()

    expect(caller.averbacoes).toBeDefined()
    expect(caller.averbacoes.list).toBeDefined()

    expect(caller.margem).toBeDefined()
    expect(caller.margem.calcular).toBeDefined()

    expect(caller.simulacao).toBeDefined()
    expect(caller.simulacao.simular).toBeDefined()

    expect(caller.consignatarias).toBeDefined()
    expect(caller.consignatarias.list).toBeDefined()

    expect(caller.conciliacao).toBeDefined()
    expect(caller.conciliacao.list).toBeDefined()

    expect(caller.relatorios).toBeDefined()
    expect(caller.relatorios.resumoGeral).toBeDefined()

    expect(caller.importacao).toBeDefined()
    expect(caller.importacao.list).toBeDefined()

    expect(caller.auditoria).toBeDefined()
    expect(caller.auditoria.list).toBeDefined()
  })
})
