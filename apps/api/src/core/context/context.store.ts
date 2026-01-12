import { AsyncLocalStorage } from 'async_hooks';
import { UserContext } from '@fast-consig/shared';

export const contextStore = new AsyncLocalStorage<UserContext>();
