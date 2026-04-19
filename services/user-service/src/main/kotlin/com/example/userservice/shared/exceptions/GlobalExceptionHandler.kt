package com.example.userservice.shared.exceptions

import org.springframework.http.HttpStatus
import org.springframework.web.bind.MethodArgumentNotValidException
import org.springframework.web.bind.annotation.ExceptionHandler
import org.springframework.web.bind.annotation.ResponseStatus
import org.springframework.web.bind.annotation.RestControllerAdvice
import kotlin.time.Clock
import kotlin.time.Instant

@RestControllerAdvice
class GlobalExceptionHandler {

    @ExceptionHandler(ResourceNotFoundException::class)
    @ResponseStatus(HttpStatus.NOT_FOUND)
    fun handleNotFound(ex: ResourceNotFoundException): ErrorResponse =
        ErrorResponse(HttpStatus.NOT_FOUND.value(), ex.message ?: "Not found")

    @ExceptionHandler(ConflictException::class)
    @ResponseStatus(HttpStatus.CONFLICT)
    fun handleNotFound(ex: ConflictException): ErrorResponse =
        ErrorResponse(HttpStatus.CONFLICT.value(), ex.message ?: "Conflict")

    @ExceptionHandler(MethodArgumentNotValidException::class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    fun handleValidationErrors(ex: MethodArgumentNotValidException): ValidationErrorResponse {
        val errors = ex.bindingResult.fieldErrors.associate { it.field to it.defaultMessage }
        return ValidationErrorResponse(errors)
    }

    @ExceptionHandler(Exception::class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    fun handleNotFound(ex: Exception): ErrorResponse = ErrorResponse(500, "An unexpected error occurred")
}

data class ErrorResponse(val status: Int, val message: String, val timestamp: Instant = Clock.System.now())

data class ValidationErrorResponse(val errors: Map<String, String?>)
