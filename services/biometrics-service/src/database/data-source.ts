import { DataSourceOptions } from "typeorm"

export const dataSourceOptions: DataSourceOptions = {
  type: "postgres",
  host: process.env.DB_HOST ?? "localhost",
  port: parseInt(process.env.DB_PORT ?? "5431"),
  username: process.env.POSTGRES_USER ?? "postgres",
  password: process.env.POSTGRES_PASSWORD ?? "password",
  database: process.env.POSTGRES_DB ?? "biometrics_db",
  entities: [__dirname + "../../../**/*.entity.{ts,js}"],
  migrations: [__dirname + "/migrations/*.ts"],
  synchronize: false,
  migrationsRun: true
}
