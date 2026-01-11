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
  X,
  ChevronLeft,
  ChevronRight,
} from 'lucide-react'
import { cn } from '@/lib/utils'
import { Button } from '@fastconsig/ui'
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from '@fastconsig/ui'

interface MenuItem {
  path: string
  label: string
  icon: React.ElementType
  permission: string | null
}

const menuItems: MenuItem[] = [
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

interface SidebarProps {
  isOpen: boolean
  isCollapsed: boolean
  onClose: () => void
  onToggleCollapse: () => void
}

export function Sidebar({
  isOpen,
  isCollapsed,
  onClose,
  onToggleCollapse,
}: SidebarProps): JSX.Element {
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
    <TooltipProvider delayDuration={0}>
      <aside
        className={cn(
          'fixed inset-y-0 left-0 z-50 flex flex-col bg-white shadow-lg transition-all duration-300 lg:static',
          isOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0',
          isCollapsed ? 'w-16' : 'w-64'
        )}
        aria-label="Menu principal"
      >
        {/* Header */}
        <div className="flex h-16 items-center justify-between border-b px-4">
          {!isCollapsed && (
            <h1 className="text-xl font-bold text-primary">FastConsig</h1>
          )}
          <div className="flex items-center gap-2">
            <button
              className="hidden lg:block"
              onClick={onToggleCollapse}
              aria-label={isCollapsed ? 'Expandir menu' : 'Recolher menu'}
            >
              {isCollapsed ? (
                <ChevronRight className="h-5 w-5 text-gray-500" />
              ) : (
                <ChevronLeft className="h-5 w-5 text-gray-500" />
              )}
            </button>
            <button
              className="lg:hidden"
              onClick={onClose}
              aria-label="Fechar menu"
            >
              <X className="h-6 w-6" />
            </button>
          </div>
        </div>

        {/* Navigation */}
        <nav className="flex-1 space-y-1 overflow-y-auto p-2" aria-label="Navegacao principal">
          {visibleMenuItems.map((item) => {
            const isActive = location.pathname.startsWith(item.path)
            const Icon = item.icon

            const linkContent = (
              <Link
                key={item.path}
                className={cn(
                  'flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                  isActive
                    ? 'bg-primary text-white'
                    : 'text-gray-700 hover:bg-gray-100',
                  isCollapsed && 'justify-center px-2'
                )}
                to={item.path}
                onClick={onClose}
                aria-current={isActive ? 'page' : undefined}
              >
                <Icon className="h-5 w-5 flex-shrink-0" aria-hidden="true" />
                {!isCollapsed && <span>{item.label}</span>}
              </Link>
            )

            if (isCollapsed) {
              return (
                <Tooltip key={item.path}>
                  <TooltipTrigger asChild>{linkContent}</TooltipTrigger>
                  <TooltipContent side="right">{item.label}</TooltipContent>
                </Tooltip>
              )
            }

            return linkContent
          })}
        </nav>

        {/* User Section */}
        <div className="border-t p-4">
          {!isCollapsed && (
            <div className="mb-2 text-sm">
              <p className="font-medium text-gray-900 truncate">{user?.nome}</p>
              <p className="text-gray-500 truncate">{user?.perfil.nome}</p>
            </div>
          )}
          {isCollapsed ? (
            <Tooltip>
              <TooltipTrigger asChild>
                <Button
                  variant="ghost"
                  size="icon"
                  className="w-full text-red-600 hover:bg-red-50 hover:text-red-700"
                  onClick={handleLogout}
                  aria-label="Sair do sistema"
                >
                  <LogOut className="h-5 w-5" />
                </Button>
              </TooltipTrigger>
              <TooltipContent side="right">Sair</TooltipContent>
            </Tooltip>
          ) : (
            <Button
              variant="ghost"
              className="w-full justify-start gap-2 text-red-600 hover:bg-red-50 hover:text-red-700"
              onClick={handleLogout}
            >
              <LogOut className="h-5 w-5" />
              Sair
            </Button>
          )}
        </div>
      </aside>
    </TooltipProvider>
  )
}
