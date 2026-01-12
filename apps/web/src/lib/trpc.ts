import { createTRPCReact } from '@trpc/react-query';
import type { AppRouterType } from '../../../../api/src/app.router';

export const trpc = createTRPCReact<AppRouterType>();
