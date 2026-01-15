export const ROLES = {
  SUPER_ADMIN: 'super_admin',
  ORG_ADMIN: 'org:admin',
  ORG_MEMBER: 'org:member',
} as const;

export const RATE_LIMITS = {
  STANDARD: {
    TTL: 60000, // 1 minute
    LIMIT: 100, // 100 requests per minute
  },
  BUSINESS: {
    TTL: 60000,
    LIMIT: 5, // 5 margin queries per minute per user
  },
  ANOMALY: {
    TTL: 60000,
    LIMIT: 3, // 3 margin queries per minute per CPF
  },
} as const;
