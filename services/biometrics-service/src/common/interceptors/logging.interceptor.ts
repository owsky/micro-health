import { CallHandler, ExecutionContext, Injectable, Logger, NestInterceptor } from "@nestjs/common"
import { Observable } from "rxjs"
import { tap } from "rxjs/operators"
import type { FastifyReply, FastifyRequest } from "fastify"

interface RequestLog {
  method: string
  url: string
  statusCode: number | undefined
  durationMs: number
  traceId: string | undefined
}

@Injectable()
export class LoggingInterceptor implements NestInterceptor {
  private readonly logger = new Logger(LoggingInterceptor.name)

  intercept(context: ExecutionContext, next: CallHandler): Observable<unknown> {
    if (context.getType() !== "http") {
      return next.handle()
    }

    const host = context.switchToHttp()
    const request: FastifyRequest = host.getRequest<FastifyRequest>()
    const response = host.getResponse<FastifyReply>()

    const { method, url, headers } = request
    const startedAt = Date.now()

    return next.handle().pipe(
      tap({
        next: () => {
          const log: RequestLog = {
            method,
            url,
            statusCode: response.statusCode,
            durationMs: Date.now() - startedAt,
            traceId: headers["x-request-id"] as string | undefined
          }

          this.logger.log(this.format(log))
        },
        error: (_err: unknown) => {
          const log: RequestLog = {
            method,
            url,
            statusCode: response.statusCode,
            durationMs: Date.now() - startedAt,
            traceId: headers["X-Trace-Id"] as string | undefined
          }

          this.logger.error(this.format(log))
        }
      })
    )
  }

  private format(log: RequestLog): string {
    const status = log.statusCode ?? "???"
    const reqId = log.traceId ? ` [${log.traceId}]` : ""
    return `${log.method} ${log.url} → ${status}` + ` | ${log.durationMs}ms` + reqId
  }
}
