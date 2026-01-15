import { describe, it, expect, beforeAll, afterAll } from 'vitest';
import { exec } from 'child_process';
import { promisify } from 'util';
import path from 'path';

const execAsync = promisify(exec);

/**
 * Foundation E2E Tests - Story 1.0: Project Foundation Setup
 *
 * These tests verify the Turborepo monorepo infrastructure is correctly configured.
 * All tests should FAIL initially (RED phase) until infrastructure is implemented.
 *
 * Test execution order:
 * 1. Turborepo build pipeline
 * 2. Shared package imports
 * 3. Docker Compose services
 * 4. GitHub Actions CI configuration
 */

describe('Foundation Setup (e2e)', () => {
  const projectRoot = path.resolve(__dirname, '../../..');

  describe('Turborepo Configuration', () => {
    it('should have turbo.json configured with build pipeline', async () => {
      // GIVEN: Project is initialized
      const turboConfigPath = path.join(projectRoot, 'turbo.json');

      // WHEN: Reading turbo.json configuration
      const fs = await import('fs/promises');
      let turboConfig;

      try {
        const configContent = await fs.readFile(turboConfigPath, 'utf-8');
        turboConfig = JSON.parse(configContent);
      } catch (error) {
        throw new Error(`turbo.json not found at ${turboConfigPath}. Run 'npx create-turbo@latest' to initialize.`);
      }

      // THEN: Configuration should include build pipeline (supports Turbo v1 pipeline or v2 tasks)
      expect(turboConfig).toBeDefined();

      // Support both v1 (pipeline) and v2 (tasks) formats
      const hasBuildTask = (turboConfig.pipeline && turboConfig.pipeline.build) ||
                          (turboConfig.tasks && turboConfig.tasks.build);

      expect(hasBuildTask).toBeDefined();
    });

    it('should compile all apps successfully with turbo build', async () => {
      // GIVEN: Turborepo is configured
      // WHEN: Running turbo build command
      let buildResult;

      try {
        buildResult = await execAsync('pnpm turbo build', {
          cwd: projectRoot,
          timeout: 120000 // 2 minutes timeout
        });
      } catch (error) {
        throw new Error(`Turbo build failed: ${error.message}`);
      }

      // THEN: All apps should build without errors
      expect(buildResult.stdout).toContain('api:build');
      expect(buildResult.stdout).toContain('web:build');
      expect(buildResult.stdout).toContain('jobs:build');
      expect(buildResult.stderr).not.toContain('ERROR');
    }, 120000); // 2 minute timeout for build
  });

  describe('Shared Packages Configuration', () => {
    it('should import @fast-consig/database in api app', async () => {
      // GIVEN: Shared database package exists
      // WHEN: Importing database package
      let importResult;

      try {
        // Dynamic import to test if package is accessible
        importResult = await import('@fast-consig/database');
      } catch (error) {
        throw new Error(`Cannot import @fast-consig/database: ${error.message}. Ensure packages/database is configured.`);
      }

      // THEN: Database exports should be available
      expect(importResult).toBeDefined();
      expect(importResult.db).toBeDefined();
    });

    it('should import @fast-consig/shared in api app', async () => {
      // GIVEN: Shared schemas package exists
      // WHEN: Importing shared package
      let importResult;

      try {
        importResult = await import('@fast-consig/shared');
      } catch (error) {
        throw new Error(`Cannot import @fast-consig/shared: ${error.message}. Ensure packages/shared is configured.`);
      }

      // THEN: Shared exports should be available
      expect(importResult).toBeDefined();
    });

    it('should have consistent TypeScript configuration across apps', async () => {
      // GIVEN: Multiple apps exist
      const fs = await import('fs/promises');

      // WHEN: Reading tsconfig.json from each app
      const apiTsConfig = JSON.parse(
        await fs.readFile(path.join(projectRoot, 'apps/api/tsconfig.json'), 'utf-8')
      );
      const webTsConfig = JSON.parse(
        await fs.readFile(path.join(projectRoot, 'apps/web/tsconfig.json'), 'utf-8')
      );

      // THEN: Both should extend from base config and have path aliases
      expect(apiTsConfig.extends).toBeDefined();
      expect(webTsConfig.extends).toBeDefined();
      expect(apiTsConfig.compilerOptions?.paths).toBeDefined();
      expect(webTsConfig.compilerOptions?.paths).toBeDefined();
    });
  });

  describe('Docker Compose Services', () => {
    let dockerServices: any;

    beforeAll(async () => {
      // Start Docker Compose services
      try {
        await execAsync('docker-compose up -d postgres redis', { cwd: projectRoot });
        // Wait for services to be healthy
        await new Promise(resolve => setTimeout(resolve, 5000));
      } catch (error) {
        console.warn('Docker Compose startup failed:', error.message);
      }
    });

    afterAll(async () => {
      // Stop Docker Compose services
      try {
        await execAsync('docker-compose down', { cwd: projectRoot });
      } catch (error) {
        console.warn('Docker Compose cleanup failed:', error.message);
      }
    });

    it('should have docker-compose.yml with PostgreSQL and Redis services', async () => {
      // GIVEN: Project root directory
      const fs = await import('fs/promises');
      const dockerComposePath = path.join(projectRoot, 'docker-compose.yml');

      // WHEN: Reading docker-compose.yml
      let dockerComposeContent;

      try {
        dockerComposeContent = await fs.readFile(dockerComposePath, 'utf-8');
      } catch (error) {
        throw new Error(`docker-compose.yml not found at ${dockerComposePath}`);
      }

      // THEN: Should define PostgreSQL and Redis services
      expect(dockerComposeContent).toContain('postgres:');
      // Accept newer postgres versions (16 or 18)
      const hasPostgresVersion = dockerComposeContent.includes('image: postgres:16') ||
                                dockerComposeContent.includes('image: postgres:18');
      expect(hasPostgresVersion).toBe(true);

      expect(dockerComposeContent).toContain('redis:');
      expect(dockerComposeContent).toContain('image: redis:7');
    });

    it('should connect to PostgreSQL successfully', async () => {
      // GIVEN: PostgreSQL service is running
      const { Client } = await import('pg');
      const client = new Client({
        host: 'localhost',
        port: 5432,
        user: process.env.POSTGRES_USER || 'postgres',
        password: process.env.POSTGRES_PASSWORD || 'postgres',
        database: process.env.POSTGRES_DB || 'fast_consig_dev',
      });

      // WHEN: Attempting to connect
      let connected = false;
      try {
        await client.connect();
        connected = true;
        await client.end();
      } catch (error) {
        throw new Error(`PostgreSQL connection failed: ${error.message}. Ensure docker-compose is running.`);
      }

      // THEN: Connection should succeed
      expect(connected).toBe(true);
    });

    it('should connect to Redis successfully', async () => {
      // GIVEN: Redis service is running
      const { createClient } = await import('redis');
      const client = createClient({
        url: process.env.REDIS_URL || 'redis://localhost:6379',
      });

      // WHEN: Attempting to connect
      let pingResponse;
      try {
        await client.connect();
        pingResponse = await client.ping();
        await client.quit();
      } catch (error) {
        throw new Error(`Redis connection failed: ${error.message}. Ensure docker-compose is running.`);
      }

      // THEN: PING should return PONG
      expect(pingResponse).toBe('PONG');
    });
  });

  describe('GitHub Actions CI Configuration', () => {
    it('should have CI workflow file configured', async () => {
      // GIVEN: GitHub Actions directory exists
      const fs = await import('fs/promises');
      const ciWorkflowPath = path.join(projectRoot, '.github/workflows/ci.yml');

      // WHEN: Reading CI workflow configuration
      let workflowContent;

      try {
        workflowContent = await fs.readFile(ciWorkflowPath, 'utf-8');
      } catch (error) {
        throw new Error(`.github/workflows/ci.yml not found. Create CI workflow with Turborepo caching.`);
      }

      // THEN: Should include Turborepo and pnpm configuration
      expect(workflowContent).toContain('pnpm');
      expect(workflowContent).toContain('turbo');
      expect(workflowContent).toContain('cache');
    });

    it('should configure Turborepo remote caching in CI', async () => {
      // GIVEN: CI workflow exists
      const fs = await import('fs/promises');
      const ciWorkflowPath = path.join(projectRoot, '.github/workflows/ci.yml');
      const workflowContent = await fs.readFile(ciWorkflowPath, 'utf-8');

      // WHEN: Checking for Turborepo cache configuration
      const hasTurboCache =
        workflowContent.includes('TURBO_TOKEN') ||
        workflowContent.includes('TURBO_TEAM') ||
        workflowContent.includes('--cache-dir=');

      // THEN: Should have Turborepo caching configured
      expect(hasTurboCache).toBe(true);
    });

    it('should run tests in CI pipeline', async () => {
      // GIVEN: CI workflow exists
      const fs = await import('fs/promises');
      const ciWorkflowPath = path.join(projectRoot, '.github/workflows/ci.yml');
      const workflowContent = await fs.readFile(ciWorkflowPath, 'utf-8');

      // WHEN: Checking for test execution
      const hasTestStep =
        workflowContent.includes('pnpm test') ||
        workflowContent.includes('turbo test') ||
        workflowContent.includes('pnpm turbo run test');

      // THEN: Should execute tests
      expect(hasTestStep).toBe(true);
    });
  });
});
