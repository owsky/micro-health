// src/types/fastify.d.ts
import { UserInfo } from "../auth/user-info.interface"

declare module "fastify" {
  interface FastifyRequest {
    user: UserInfo
  }
}
