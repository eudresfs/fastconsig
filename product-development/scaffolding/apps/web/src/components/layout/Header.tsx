import { useState } from 'react'
import { useAuthStore } from '@/stores/auth'
import { Menu, Bell, User, Settings, LogOut, ChevronDown } from 'lucide-react'
import { Button } from '@fastconsig/ui'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@fastconsig/ui'
import { Badge } from '@fastconsig/ui'

interface HeaderProps {
  onMenuClick: () => void
}

export function Header({ onMenuClick }: HeaderProps): JSX.Element {
  const { user, logout } = useAuthStore()
  const [notificationCount] = useState(3) // Placeholder for notifications

  const handleLogout = (): void => {
    logout()
    window.location.href = '/login'
  }

  return (
    <header
      className="flex h-16 items-center justify-between border-b bg-white px-4 lg:px-6"
      role="banner"
    >
      {/* Left section */}
      <div className="flex items-center gap-4">
        <Button
          variant="ghost"
          size="icon"
          className="lg:hidden"
          onClick={onMenuClick}
          aria-label="Abrir menu de navegacao"
        >
          <Menu className="h-6 w-6" />
        </Button>

        {user?.tenant && (
          <Badge variant="secondary" className="hidden sm:inline-flex">
            {user.tenant.nome}
          </Badge>
        )}
      </div>

      {/* Right section */}
      <div className="flex items-center gap-2">
        {/* Notifications */}
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              size="icon"
              className="relative"
              aria-label={`Notificacoes - ${notificationCount} novas`}
            >
              <Bell className="h-5 w-5" />
              {notificationCount > 0 && (
                <span className="absolute -right-1 -top-1 flex h-5 w-5 items-center justify-center rounded-full bg-destructive text-xs text-destructive-foreground">
                  {notificationCount > 9 ? '9+' : notificationCount}
                </span>
              )}
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-80">
            <DropdownMenuLabel>Notificacoes</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <div className="max-h-80 overflow-y-auto">
              <DropdownMenuItem className="flex flex-col items-start gap-1 py-3">
                <span className="font-medium">Nova averbacao pendente</span>
                <span className="text-xs text-muted-foreground">
                  Averbacao #12345 aguardando aprovacao
                </span>
                <span className="text-xs text-muted-foreground">Ha 5 minutos</span>
              </DropdownMenuItem>
              <DropdownMenuItem className="flex flex-col items-start gap-1 py-3">
                <span className="font-medium">Importacao concluida</span>
                <span className="text-xs text-muted-foreground">
                  Arquivo folha_janeiro.csv processado com sucesso
                </span>
                <span className="text-xs text-muted-foreground">Ha 1 hora</span>
              </DropdownMenuItem>
              <DropdownMenuItem className="flex flex-col items-start gap-1 py-3">
                <span className="font-medium">Relatorio disponivel</span>
                <span className="text-xs text-muted-foreground">
                  Relatorio mensal de averbacoes gerado
                </span>
                <span className="text-xs text-muted-foreground">Ha 2 horas</span>
              </DropdownMenuItem>
            </div>
            <DropdownMenuSeparator />
            <DropdownMenuItem className="justify-center text-primary">
              Ver todas as notificacoes
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>

        {/* User Menu */}
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              className="flex items-center gap-2 px-2"
              aria-label="Menu do usuario"
            >
              <div className="flex h-8 w-8 items-center justify-center rounded-full bg-primary text-primary-foreground">
                <User className="h-4 w-4" />
              </div>
              <div className="hidden flex-col items-start md:flex">
                <span className="text-sm font-medium">{user?.nome}</span>
                <span className="text-xs text-muted-foreground">{user?.perfil.nome}</span>
              </div>
              <ChevronDown className="hidden h-4 w-4 md:block" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-56">
            <DropdownMenuLabel>Minha conta</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem>
              <User className="mr-2 h-4 w-4" />
              Perfil
            </DropdownMenuItem>
            <DropdownMenuItem>
              <Settings className="mr-2 h-4 w-4" />
              Configuracoes
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem
              className="text-destructive focus:text-destructive"
              onClick={handleLogout}
            >
              <LogOut className="mr-2 h-4 w-4" />
              Sair
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  )
}
