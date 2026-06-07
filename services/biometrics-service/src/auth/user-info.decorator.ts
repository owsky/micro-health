import { createParamDecorator, ExecutionContext } from "@nestjs/common"
import { UserInfo } from "./user-info.interface"
import { FastifyRequest } from "fastify"

/**
 * Decorator which extracts the user info object from the request and returns it
 */
export const UserInfoDecorator = createParamDecorator((_data: unknown, ctx: ExecutionContext): UserInfo => {
  const request = ctx.switchToHttp().getRequest<FastifyRequest>()

  return request.user
})
