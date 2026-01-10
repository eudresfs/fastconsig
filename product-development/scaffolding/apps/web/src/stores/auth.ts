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
  permissoes: string[]
  tenant: { id: number; nome: string } | null
  consignataria: { id: number; razaoSocial: string } | null
}

interface AuthState {
  accessToken: string | null
  refreshToken: string | null
  user: User | null
  isAuthenticated: boolean
  setAuth: (accessToken: string, refreshToken: string, user: User) => void
  setAccessToken: (accessToken: string) => void
  logout: () => void
  hasPermission: (permission: string) => boolean
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      accessToken: null,
      refreshToken: null,
      user: null,
      isAuthenticated: false,

      setAuth: (accessToken, refreshToken, user) => {
        set({
          accessToken,
          refreshToken,
          user,
          isAuthenticated: true,
        })
      },

      setAccessToken: (accessToken) => {
        set({ accessToken })
      },

      logout: () => {
        set({
          accessToken: null,
          refreshToken: null,
          user: null,
          isAuthenticated: false,
        })
      },

      hasPermission: (permission) => {
        const { user } = get()
        if (!user) return false
        return user.permissoes.includes(permission)
      },
    }),
    {
      name: 'fastconsig-auth',
      partialize: (state) => ({
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        user: state.user,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
)
