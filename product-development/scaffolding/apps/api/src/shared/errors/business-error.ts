import { AppError } from './app-error'

/**
 * Business rule violation error
 * Use when a business rule is violated (e.g., insufficient margin)
 */
export class BusinessError extends AppError {
  constructor(
    message: string,
    options?: {
      code?: string
      details?: Record<string, unknown>
    }
  ) {
    super(message, {
      code: options?.code ?? 'BUSINESS_ERROR',
      statusCode: 422,
      isOperational: true,
      details: options?.details,
    })
  }
}

/**
 * Validation error for input data
 * Use when request data fails validation
 */
export class ValidationError extends AppError {
  public readonly errors: Array<{ field: string; message: string }>

  constructor(
    message: string,
    errors: Array<{ field: string; message: string }> = []
  ) {
    super(message, {
      code: 'VALIDATION_ERROR',
      statusCode: 400,
      isOperational: true,
      details: { errors },
    })
    this.errors = errors
  }
}

/**
 * Resource not found error
 * Use when a requested resource does not exist
 */
export class NotFoundError extends AppError {
  constructor(
    resource: string,
    identifier?: string | number,
    options?: {
      details?: Record<string, unknown>
    }
  ) {
    const message = identifier
      ? `${resource} com identificador '${identifier}' nao encontrado`
      : `${resource} nao encontrado`

    super(message, {
      code: 'NOT_FOUND',
      statusCode: 404,
      isOperational: true,
      details: {
        resource,
        identifier,
        ...options?.details,
      },
    })
  }
}

/**
 * Unauthorized error
 * Use when authentication is required but not provided or invalid
 */
export class UnauthorizedError extends AppError {
  constructor(message = 'Voce precisa estar autenticado para acessar este recurso') {
    super(message, {
      code: 'UNAUTHORIZED',
      statusCode: 401,
      isOperational: true,
    })
  }
}

/**
 * Forbidden error
 * Use when user is authenticated but lacks permission
 */
export class ForbiddenError extends AppError {
  constructor(
    message = 'Voce nao tem permissao para acessar este recurso',
    options?: {
      requiredPermission?: string
      details?: Record<string, unknown>
    }
  ) {
    super(message, {
      code: 'FORBIDDEN',
      statusCode: 403,
      isOperational: true,
      details: {
        requiredPermission: options?.requiredPermission,
        ...options?.details,
      },
    })
  }
}

/**
 * Conflict error
 * Use when there is a conflict with current state (e.g., duplicate entry)
 */
export class ConflictError extends AppError {
  constructor(
    message: string,
    options?: {
      details?: Record<string, unknown>
    }
  ) {
    super(message, {
      code: 'CONFLICT',
      statusCode: 409,
      isOperational: true,
      details: options?.details,
    })
  }
}

/**
 * State transition error
 * Use when an invalid state transition is attempted
 */
export class StateTransitionError extends AppError {
  constructor(
    entity: string,
    currentState: string,
    targetState: string,
    options?: {
      allowedTransitions?: string[]
      details?: Record<string, unknown>
    }
  ) {
    super(
      `Transicao invalida para ${entity}: de '${currentState}' para '${targetState}' nao e permitida`,
      {
        code: 'INVALID_STATE_TRANSITION',
        statusCode: 422,
        isOperational: true,
        details: {
          entity,
          currentState,
          targetState,
          allowedTransitions: options?.allowedTransitions,
          ...options?.details,
        },
      }
    )
  }
}

/**
 * Margin error
 * Use when there is insufficient margin for an operation
 */
export class MargemInsuficienteError extends AppError {
  constructor(
    margemDisponivel: number,
    margemRequerida: number,
    options?: {
      funcionarioId?: number
      details?: Record<string, unknown>
    }
  ) {
    super(
      `Margem insuficiente: disponivel R$ ${margemDisponivel.toFixed(2)}, requerida R$ ${margemRequerida.toFixed(2)}`,
      {
        code: 'MARGEM_INSUFICIENTE',
        statusCode: 422,
        isOperational: true,
        details: {
          margemDisponivel,
          margemRequerida,
          diferenca: margemRequerida - margemDisponivel,
          funcionarioId: options?.funcionarioId,
          ...options?.details,
        },
      }
    )
  }
}
