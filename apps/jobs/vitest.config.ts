import { defineConfig } from 'vitest/config';
import path from 'path';

export default defineConfig({
  test: {
    globals: true,
    environment: 'node',
    include: ['src/**/*.spec.ts', 'src/**/*.test.ts'],
    coverage: {
      provider: 'v8',
      reporter: ['text', 'json', 'html'],
    },
  },
  resolve: {
    alias: {
      '@fast-consig/shared': path.resolve(__dirname, '../../packages/shared/src/index.ts'),
      '@fast-consig/database': path.resolve(__dirname, '../../packages/database/src/index.ts'),
    },
  },
});
