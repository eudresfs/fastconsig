import { createApp } from './app'
import { env } from './config/env'

async function main(): Promise<void> {
  const app = await createApp()

  try {
    await app.listen({ port: env.PORT, host: '0.0.0.0' })
    app.log.info(`Server running on http://localhost:${env.PORT}`)
  } catch (err) {
    app.log.error(err)
    process.exit(1)
  }
}

if (require.main === module) {
  main()
}
