import { NestFactory } from "@nestjs/core";
import { AppModule } from "./app.module";

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  // Jobs worker doesn't necessarily need to listen on a port if it only processes jobs,
  // but for health checks it might be useful.
  await app.listen(3002);
  console.log(`Jobs Worker is running on: ${await app.getUrl()}`);
}
bootstrap();
