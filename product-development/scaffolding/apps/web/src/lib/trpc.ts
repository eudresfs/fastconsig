import { createTRPCReact } from '@trpc/react-query'
import { httpBatchLink } from '@trpc/client'
import superjson from 'superjson'
import { type AppRouter } from '@fastconsig/api/trpc/router'
import { useAuthStore } from '@/stores/auth'

export const trpc = createTRPCReact<AppRouter>()

export function createTRPCClient() {
  return trpc.createClient({
    links: [
      httpBatchLink({
        url: '/trpc',
        transformer: superjson,
        headers() {
          const token = useAuthStore.getState().accessToken
          return token
            ? { Authorization: `Bearer ${token}` }
            : {}
        },
      }),
    ],
  })
}
