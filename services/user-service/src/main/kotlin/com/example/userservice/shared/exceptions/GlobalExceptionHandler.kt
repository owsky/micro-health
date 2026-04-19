package com.example.userservice.shared.exceptions

import com.example.userservice.config.TraceIdFilter
import org.slf4j.LoggerFactory
import org.slf4j.MDC
import org.springframework.http.HttpStatus
import org.springframework.web.bind.MethodArgumentNotValidException
import org.springframework.web.bind.annotation.ExceptionHandler
import org.springframework.web.bind.annotation.ResponseStatus
import org.springframework.web.bind.annotation.RestControllerAdvice
import kotlin.time.Clock
import kotlin.time.Instant

@RestControllerAdvice
class GlobalExceptionHandler {

    private val log = LoggerFactory.getLogger(GlobalExceptionHandler::class.java)

    private fun traceId(): String = MDC.get(TraceIdFilter.TRACE_ID_MDC_KEY) ?: "no-trace-id"

    @ExceptionHandler(ResourceNotFoundException::class)
    @ResponseStatus(HttpStatus.NOT_FOUND)
    fun handleNotFound(ex: ResourceNotFoundException): ErrorResponse {
        log.warn("Not found: {}", ex.message)
        return ErrorResponse(HttpStatus.NOT_FOUND.value(), ex.message ?: "Not found", traceId = traceId())
    }

    @ExceptionHandler(ConflictException::class)
    @ResponseStatus(HttpStatus.CONFLICT)
    fun handleConflict(ex: ConflictException): ErrorResponse {
        log.warn("Conflict: {}", ex.message)
        return ErrorResponse(HttpStatus.CONFLICT.value(), ex.message ?: "Conflict", traceId = traceId())
    }

    @ExceptionHandler(MethodArgumentNotValidException::class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    fun handleValidationErrors(ex: MethodArgumentNotValidException): ValidationErrorResponse {
        val errors = ex.bindingResult.fieldErrors.associate { it.field to it.defaultMessage }
        log.warn("Validation failed: {}", errors)
        return ValidationErrorResponse(errors, traceId = traceId())
    }

    @ExceptionHandler(Exception::class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    fun handleGeneric(ex: Exception): ErrorResponse {
        log.error("Unexpected error", ex)
        return ErrorResponse(500, "An unexpected error occurred", traceId = traceId())
    }
}

data class ErrorResponse(
    val status: Int,
    val message: String,
    val traceId: String,
    val timestamp: Instant = Clock.System.now()
)

data class ValidationErrorResponse(val errors: Map<String, String?>, val traceId: String)
