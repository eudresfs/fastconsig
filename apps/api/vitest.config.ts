import { defineConfig } from 'vitest/config';
import swc from 'unplugin-swc';

export default defineConfig({
  test: {
    include: ['src/**/*.spec.ts'],
    exclude: ['dist/**', 'node_modules/**'],
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
      '@fast-consig/shared': '../../packages/shared/src',
    },
  },
});
