import { NestFactory } from "@nestjs/core"
import { AppModule } from "./app.module"
import { FastifyAdapter, NestFastifyApplication } from "@nestjs/platform-fastify"
import { DocumentBuilder, SwaggerModule } from "@nestjs/swagger"
import { LoggingInterceptor } from "./common/interceptors/logging.interceptor"

async function bootstrap() {
  const app = await NestFactory.create<NestFastifyApplication>(AppModule, new FastifyAdapter())

  const config = new DocumentBuilder()
    .setTitle("Biometrics service")
    .setDescription("Microservice used to store and read user-generated biometrics data")
    .setVersion("0.0.1")
    .addTag("biometrics")
    .build()
  const documentFactory = () => SwaggerModule.createDocument(app, config)
  SwaggerModule.setup("api", app, documentFactory)

  app.enableShutdownHooks()
  app.useGlobalInterceptors(new LoggingInterceptor())

  await app.listen(process.env.PORT ?? 3000)
}
void bootstrap()
