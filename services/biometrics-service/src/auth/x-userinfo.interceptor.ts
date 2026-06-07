import { Injectable, NestInterceptor, ExecutionContext, CallHandler, UnauthorizedException } from "@nestjs/common"
import { Observable } from "rxjs"
import { UserInfoSchema } from "./user-info.interface"
import { FastifyRequest } from "fastify"

@Injectable()
export class XUserinfoInterceptor implements NestInterceptor {
  intercept(context: ExecutionContext, next: CallHandler): Observable<any> {
    const request = context.switchToHttp().getRequest<FastifyRequest>()
    if (!request.url.startsWith("/api")) {
      return next.handle()
    }

    const base64UserInfo = request.headers["x-userinfo"] as string

    if (!base64UserInfo) {
      throw new UnauthorizedException("Missing X-Userinfo header")
    }

    try {
      const decodedString = Buffer.from(base64UserInfo, "base64").toString("utf-8")
      const parsedObject: unknown = JSON.parse(decodedString)
      if (!UserInfoSchema.Check(parsedObject)) {
        const validationErrors = UserInfoSchema.Errors(parsedObject)
        console.log(validationErrors)
        throw new UnauthorizedException("Invalid X-Userinfo format")
      }
      const userInfo = UserInfoSchema.Parse(parsedObject)

      request.user = userInfo
    } catch (_error: unknown) {
      throw new UnauthorizedException("Invalid X-Userinfo format")
    }

    return next.handle()
  }
}
