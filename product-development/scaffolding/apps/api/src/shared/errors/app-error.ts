/**
 * Base application error class
 * All custom errors should extend this class
 */
export class AppError extends Error {
  public readonly code: string
  public readonly statusCode: number
  public readonly isOperational: boolean
  public readonly details?: Record<string, unknown>

  constructor(
    message: string,
    options: {
      code: string
      statusCode: number
      isOperational?: boolean
      details?: Record<string, unknown>
    }
  ) {
    super(message)
    this.name = this.constructor.name
    this.code = options.code
    this.statusCode = options.statusCode
    this.isOperational = options.isOperational ?? true
    this.details = options.details

    // Maintains proper stack trace for where error was thrown
    Error.captureStackTrace(this, this.constructor)
  }

  toJSON(): Record<string, unknown> {
    return {
      name: this.name,
      message: this.message,
      code: this.code,
      statusCode: this.statusCode,
      details: this.details,
    }
  }
}
