import { Link } from '@tanstack/react-router'
import { ChevronRight, Home } from 'lucide-react'
import { cn } from '@/lib/utils'

export interface BreadcrumbItem {
  label: string
  href?: string
}

interface BreadcrumbProps {
  items: BreadcrumbItem[]
  className?: string
}

export function Breadcrumb({ items, className }: BreadcrumbProps): JSX.Element {
  return (
    <nav
      className={cn('flex items-center text-sm', className)}
      aria-label="Navegacao estrutural"
    >
      <ol className="flex items-center gap-1" role="list">
        {/* Home link */}
        <li className="flex items-center">
          <Link
            to="/dashboard"
            className="flex items-center text-muted-foreground transition-colors hover:text-foreground"
            aria-label="Pagina inicial"
          >
            <Home className="h-4 w-4" />
          </Link>
        </li>

        {items.map((item, index) => {
          const isLast = index === items.length - 1

          return (
            <li key={item.label} className="flex items-center">
              <ChevronRight
                className="mx-1 h-4 w-4 text-muted-foreground"
                aria-hidden="true"
              />
              {isLast || !item.href ? (
                <span
                  className={cn(
                    isLast ? 'font-medium text-foreground' : 'text-muted-foreground'
                  )}
                  aria-current={isLast ? 'page' : undefined}
                >
                  {item.label}
                </span>
              ) : (
                <Link
                  to={item.href}
                  className="text-muted-foreground transition-colors hover:text-foreground"
                >
                  {item.label}
                </Link>
              )}
            </li>
          )
        })}
      </ol>
    </nav>
  )
}

// Helper hook for common breadcrumb patterns
export function useBreadcrumbItems(
  pageName: string,
  parentPages?: Array<{ label: string; href: string }>
): BreadcrumbItem[] {
  const items: BreadcrumbItem[] = []

  if (parentPages) {
    items.push(...parentPages)
  }

  items.push({ label: pageName })

  return items
}
