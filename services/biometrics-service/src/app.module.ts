import { Module } from "@nestjs/common"
import { ConfigModule } from "@nestjs/config"
import { TypeOrmModule } from "@nestjs/typeorm"
import { dataSourceOptions } from "./database/data-source"
import { HealthModule } from "./health/health.module"
import { APP_INTERCEPTOR } from "@nestjs/core"
import { XUserinfoInterceptor } from "./auth/x-userinfo.interceptor"

@Module({
  imports: [
    HealthModule,
    ConfigModule.forRoot({
      envFilePath: `.env.${process.env.NODE_ENV || "development"}`,
      isGlobal: true
    }),
    TypeOrmModule.forRoot({
      ...dataSourceOptions,
      autoLoadEntities: true
  providers: [{ provide: APP_INTERCEPTOR, useClass: XUserinfoInterceptor }]
})
export class AppModule {}
