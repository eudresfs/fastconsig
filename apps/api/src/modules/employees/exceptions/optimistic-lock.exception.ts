import { HttpException, HttpStatus } from '@nestjs/common';

export class OptimisticLockException extends HttpException {
  constructor(message: string) {
    super(
      {
        success: false,
        error: {
          code: 'OPTIMISTIC_LOCK_ERROR',
          message,
        },
      },
      HttpStatus.CONFLICT,
    );
  }
}
