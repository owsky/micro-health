package com.example.userservice.config

import org.aspectj.lang.ProceedingJoinPoint
import org.aspectj.lang.annotation.Around
import org.aspectj.lang.annotation.Aspect
import org.aspectj.lang.reflect.MethodSignature
import org.slf4j.LoggerFactory
import org.springframework.stereotype.Component

@Aspect
@Component
class ControllerLoggingAspect {

    @Around("within(@org.springframework.web.bind.annotation.RestController *)")
    fun logControllerInvocation(pjp: ProceedingJoinPoint): Any? {
        val signature = pjp.signature as MethodSignature
        val className = pjp.target.javaClass.simpleName
        val methodName = signature.name
        val args = pjp.args
            .filterNot { it is org.springframework.security.core.Authentication }
            .filterNot { it?.javaClass?.name?.contains("Principal") == true }
            .joinToString(", ") { it?.toString() ?: "null" }

        val log = LoggerFactory.getLogger(pjp.target.javaClass)

        log.info("→ {}.{}({})", className, methodName, args)
        val start = System.currentTimeMillis()
        return try {
            val result = pjp.proceed()
            val elapsed = System.currentTimeMillis() - start
            log.info("← {}.{} completed in {}ms", className, methodName, elapsed)
            result
        } catch (ex: Throwable) {
            val elapsed = System.currentTimeMillis() - start
            log.error("✗ {}.{} failed after {}ms — {}: {}", className, methodName, elapsed, ex.javaClass.simpleName, ex.message)
            throw ex
        }
    }
}

