import { defineConfig } from 'vitest/config';
import swc from 'unplugin-swc';

export default defineConfig({
  test: {
    include: ['test/**/*.e2e-spec.ts'],
    globals: true,
    root: './',
    environment: 'node',
  },
  plugins: [
    (swc.vite({
      module: { type: 'es6' },
    }) as any),
  ],
  resolve: {
    alias: {
      '@fast-consig/shared': '../../../packages/shared/src',
    },
  },
});
