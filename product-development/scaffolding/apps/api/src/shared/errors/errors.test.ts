import { describe, it, expect } from 'vitest'
import {
  AppError,
  BusinessError,
  ValidationError,
  NotFoundError,
  UnauthorizedError,
  ForbiddenError,
  ConflictError,
  StateTransitionError,
  MargemInsuficienteError,
} from './index'

describe('Errors', () => {
  describe('AppError', () => {
    it('should create an instance with correct properties', () => {
      const error = new AppError('Something went wrong', {
        code: 'TEST_ERROR',
        statusCode: 500,
        isOperational: true,
        details: { foo: 'bar' },
      })

      expect(error).toBeInstanceOf(Error)
      expect(error).toBeInstanceOf(AppError)
      expect(error.message).toBe('Something went wrong')
      expect(error.code).toBe('TEST_ERROR')
      expect(error.statusCode).toBe(500)
      expect(error.isOperational).toBe(true)
      expect(error.details).toEqual({ foo: 'bar' })
    })

    it('should default isOperational to true if not provided', () => {
      const error = new AppError('Error', {
        code: 'TEST',
        statusCode: 500,
      })
      expect(error.isOperational).toBe(true)
    })

    it('should serialize to JSON correctly', () => {
      const error = new AppError('Error message', {
        code: 'TEST_CODE',
        statusCode: 400,
        details: { key: 'value' },
      })

      const json = error.toJSON()
      expect(json).toEqual({
        name: 'AppError',
        message: 'Error message',
        code: 'TEST_CODE',
        statusCode: 400,
        details: { key: 'value' },
      })
    })
  })

  describe('BusinessError', () => {
    it('should create with default values', () => {
      const error = new BusinessError('Business rule violation')
      expect(error.code).toBe('BUSINESS_ERROR')
      expect(error.statusCode).toBe(422)
      expect(error.isOperational).toBe(true)
    })

    it('should create with custom options', () => {
      const error = new BusinessError('Custom error', {
        code: 'CUSTOM_BIZ',
        details: { reason: 'logic' },
      })
      expect(error.code).toBe('CUSTOM_BIZ')
      expect(error.details).toEqual({ reason: 'logic' })
    })
  })

  describe('ValidationError', () => {
    it('should create with validation errors', () => {
      const fieldErrors = [{ field: 'email', message: 'Invalid email' }]
      const error = new ValidationError('Validation failed', fieldErrors)

      expect(error.code).toBe('VALIDATION_ERROR')
      expect(error.statusCode).toBe(400)
      expect(error.errors).toEqual(fieldErrors)
      expect(error.details).toEqual({ errors: fieldErrors })
    })

    it('should create with empty errors array if not provided', () => {
      const error = new ValidationError('Validation failed')
      expect(error.errors).toEqual([])
    })
  })

  describe('NotFoundError', () => {
    it('should create simple not found error', () => {
      const error = new NotFoundError('User')
      expect(error.message).toBe('User nao encontrado')
      expect(error.code).toBe('NOT_FOUND')
      expect(error.statusCode).toBe(404)
    })

    it('should create not found error with identifier', () => {
      const error = new NotFoundError('User', '123')
      expect(error.message).toBe("User com identificador '123' nao encontrado")
      expect(error.details).toEqual(expect.objectContaining({
        resource: 'User',
        identifier: '123',
      }))
    })
  })

  describe('UnauthorizedError', () => {
    it('should create with default message', () => {
      const error = new UnauthorizedError()
      expect(error.message).toBe('Voce precisa estar autenticado para acessar este recurso')
      expect(error.code).toBe('UNAUTHORIZED')
      expect(error.statusCode).toBe(401)
    })

    it('should create with custom message', () => {
      const error = new UnauthorizedError('Custom message')
      expect(error.message).toBe('Custom message')
    })
  })

  describe('ForbiddenError', () => {
    it('should create with default message', () => {
      const error = new ForbiddenError()
      expect(error.message).toBe('Voce nao tem permissao para acessar este recurso')
      expect(error.code).toBe('FORBIDDEN')
      expect(error.statusCode).toBe(403)
    })

    it('should create with required permission details', () => {
      const error = new ForbiddenError('Access denied', {
        requiredPermission: 'admin:read',
      })
      expect(error.details).toEqual({ requiredPermission: 'admin:read' })
    })
  })

  describe('ConflictError', () => {
    it('should create with message', () => {
      const error = new ConflictError('User already exists')
      expect(error.message).toBe('User already exists')
      expect(error.code).toBe('CONFLICT')
      expect(error.statusCode).toBe(409)
    })
  })

  describe('StateTransitionError', () => {
    it('should format message correctly', () => {
      const error = new StateTransitionError('Order', 'PENDING', 'SHIPPED')
      expect(error.message).toBe("Transicao invalida para Order: de 'PENDING' para 'SHIPPED' nao e permitida")
      expect(error.code).toBe('INVALID_STATE_TRANSITION')
      expect(error.statusCode).toBe(422)
      expect(error.details).toEqual({
        entity: 'Order',
        currentState: 'PENDING',
        targetState: 'SHIPPED',
        allowedTransitions: undefined,
      })
    })
  })

  describe('MargemInsuficienteError', () => {
    it('should format message and details correctly', () => {
      const error = new MargemInsuficienteError(100, 150, {
        funcionarioId: 1,
      })

      expect(error.message).toBe('Margem insuficiente: disponivel R$ 100.00, requerida R$ 150.00')
      expect(error.code).toBe('MARGEM_INSUFICIENTE')
      expect(error.statusCode).toBe(422)
      expect(error.details).toEqual({
        margemDisponivel: 100,
        margemRequerida: 150,
        diferenca: 50,
        funcionarioId: 1,
      })
    })
  })
})
