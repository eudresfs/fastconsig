import { createRootRoute, Outlet } from '@tanstack/react-router'
import { Toaster } from 'sonner'

export const Route = createRootRoute({
  component: RootLayout,
})

function RootLayout(): JSX.Element {
  return (
    <>
      <Outlet />
      <Toaster position="top-right" richColors />
    </>
  )
}
