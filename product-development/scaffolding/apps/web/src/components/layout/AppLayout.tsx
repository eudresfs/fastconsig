import { type ReactNode } from 'react'
import { Link, useLocation } from '@tanstack/react-router'
import { useAuthStore } from '@/stores/auth'
import {
  LayoutDashboard,
  Users,
  FileText,
  Calculator,
  Building2,
  FileSpreadsheet,
  Upload,
  ClipboardList,
  Settings,
  LogOut,
  Menu,
  X,
} from 'lucide-react'
import { useState } from 'react'

interface AppLayoutProps {
  children: ReactNode
}

const menuItems = [
  { path: '/dashboard', label: 'Dashboard', icon: LayoutDashboard, permission: null },
  { path: '/funcionarios', label: 'Funcionarios', icon: Users, permission: 'FUNCIONARIOS_VISUALIZAR' },
  { path: '/averbacoes', label: 'Averbacoes', icon: FileText, permission: 'AVERBACOES_VISUALIZAR' },
  { path: '/simulacao', label: 'Simulacao', icon: Calculator, permission: null },
  { path: '/consignatarias', label: 'Consignatarias', icon: Building2, permission: 'CONSIGNATARIAS_VISUALIZAR' },
  { path: '/conciliacao', label: 'Conciliacao', icon: FileSpreadsheet, permission: 'CONCILIACAO_VISUALIZAR' },
  { path: '/importacao', label: 'Importacao', icon: Upload, permission: 'IMPORTACAO_VISUALIZAR' },
  { path: '/relatorios', label: 'Relatorios', icon: ClipboardList, permission: 'RELATORIOS_VISUALIZAR' },
  { path: '/auditoria', label: 'Auditoria', icon: ClipboardList, permission: 'AUDITORIA_VISUALIZAR' },
  { path: '/configuracoes', label: 'Configuracoes', icon: Settings, permission: 'CONFIGURACOES_VISUALIZAR' },
]

export function AppLayout({ children }: AppLayoutProps): JSX.Element {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const location = useLocation()
  const { user, logout, hasPermission } = useAuthStore()

  const visibleMenuItems = menuItems.filter(
    (item) => !item.permission || hasPermission(item.permission)
  )

  const handleLogout = (): void => {
    logout()
    window.location.href = '/login'
  }

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar Mobile Overlay */}
      {sidebarOpen && (
        <div
          className="fixed inset-0 z-40 bg-black/50 lg:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`fixed inset-y-0 left-0 z-50 w-64 transform bg-white shadow-lg transition-transform lg:static lg:translate-x-0 ${
          sidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        <div className="flex h-16 items-center justify-between border-b px-4">
          <h1 className="text-xl font-bold text-primary">FastConsig</h1>
          <button
            className="lg:hidden"
            onClick={() => setSidebarOpen(false)}
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        <nav className="flex-1 space-y-1 p-4">
          {visibleMenuItems.map((item) => {
            const isActive = location.pathname.startsWith(item.path)
            const Icon = item.icon

            return (
              <Link
                key={item.path}
                className={`flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-primary text-white'
                    : 'text-gray-700 hover:bg-gray-100'
                }`}
                to={item.path}
                onClick={() => setSidebarOpen(false)}
              >
                <Icon className="h-5 w-5" />
                {item.label}
              </Link>
            )
          })}
        </nav>

        <div className="border-t p-4">
          <div className="mb-2 text-sm">
            <p className="font-medium text-gray-900">{user?.nome}</p>
            <p className="text-gray-500">{user?.perfil.nome}</p>
          </div>
          <button
            className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium text-red-600 hover:bg-red-50"
            onClick={handleLogout}
          >
            <LogOut className="h-5 w-5" />
            Sair
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex flex-1 flex-col overflow-hidden">
        {/* Top Bar */}
        <header className="flex h-16 items-center justify-between border-b bg-white px-4 lg:px-6">
          <button
            className="lg:hidden"
            onClick={() => setSidebarOpen(true)}
          >
            <Menu className="h-6 w-6" />
          </button>

          <div className="flex items-center gap-4">
            {user?.tenant && (
              <span className="rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
                {user.tenant.nome}
              </span>
            )}
          </div>
        </header>

        {/* Page Content */}
        <main className="flex-1 overflow-auto p-4 lg:p-6">
          {children}
        </main>
      </div>
    </div>
  )
}
