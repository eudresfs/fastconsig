export const authConfig = {
  accessToken: {
    expiresIn: '15m',
  },
  refreshToken: {
    expiresIn: '7d',
  },
  password: {
    saltRounds: 12,
    minLength: 8,
    requireNumbers: true,
    requireLetters: true,
    historyCount: 5,
  },
  lockout: {
    maxAttempts: 5,
    durationMinutes: 30,
  },
}
