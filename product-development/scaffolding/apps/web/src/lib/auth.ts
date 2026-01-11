import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface User {
  id: number
  nome: string
  email: string
  primeiroAcesso: boolean
  perfil: {
    id: number
    nome: string
    tipo: string
  }
  tenantId: number | null
  consignatariaId: number | null
}

interface AuthState {
  accessToken: string | null
  refreshToken: string | null
  user: User | null
  isAuthenticated: boolean

  setAuth: (data: { accessToken: string; refreshToken: string; usuario: User }) => void
  setAccessToken: (token: string) => void
  logout: () => void
  updateUser: (user: Partial<User>) => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      refreshToken: null,
      user: null,
      isAuthenticated: false,

      setAuth: (data) =>
        set({
          accessToken: data.accessToken,
          refreshToken: data.refreshToken,
          user: data.usuario,
          isAuthenticated: true,
        }),

      setAccessToken: (token) =>
        set({
          accessToken: token,
        }),

      logout: () =>
        set({
          accessToken: null,
          refreshToken: null,
          user: null,
          isAuthenticated: false,
        }),

      updateUser: (userData) =>
        set((state) => ({
          user: state.user ? { ...state.user, ...userData } : null,
        })),
    }),
    {
      name: 'fastconsig-auth-storage',
    }
  )
)
