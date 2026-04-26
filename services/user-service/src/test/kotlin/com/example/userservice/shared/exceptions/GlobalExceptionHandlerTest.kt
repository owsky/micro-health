package com.example.userservice.shared.exceptions

import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.springframework.core.MethodParameter
import org.springframework.http.HttpStatus
import org.springframework.validation.BeanPropertyBindingResult
import org.springframework.validation.FieldError
import org.springframework.web.bind.MethodArgumentNotValidException
import kotlin.test.assertEquals
import kotlin.test.assertNotNull
import kotlin.test.assertTrue

class GlobalExceptionHandlerTest {

    private val sut = GlobalExceptionHandler()

    // Creates a MethodParameter stub via a dummy method's first parameter
    private val methodParameter: MethodParameter = MethodParameter(
        String::class.java.getMethod("valueOf", Any::class.java), 0
    )

    // -------------------------------------------------------------------------
    // handleNotFound
    // -------------------------------------------------------------------------
    @Nested
    inner class HandleNotFound {

        @Test
        fun `returns status 404`() {
            val response = sut.handleNotFound(ResourceNotFoundException("not found"))
            assertEquals(HttpStatus.NOT_FOUND.value(), response.status)
        }

        @Test
        fun `returns the exception message`() {
            val response = sut.handleNotFound(ResourceNotFoundException("Resource X not found"))
            assertEquals("Resource X not found", response.message)
        }

        @Test
        fun `includes a traceId`() {
            val response = sut.handleNotFound(ResourceNotFoundException("not found"))
            assertNotNull(response.traceId)
        }
    }

    // -------------------------------------------------------------------------
    // handleConflict
    // -------------------------------------------------------------------------
    @Nested
    inner class HandleConflict {

        @Test
        fun `returns status 409`() {
            val response = sut.handleConflict(ConflictException("conflict"))
            assertEquals(HttpStatus.CONFLICT.value(), response.status)
        }

        @Test
        fun `returns the exception message`() {
            val response = sut.handleConflict(ConflictException("User already exists"))
            assertEquals("User already exists", response.message)
        }

        @Test
        fun `includes a traceId`() {
            val response = sut.handleConflict(ConflictException("conflict"))
            assertNotNull(response.traceId)
        }
    }

    // -------------------------------------------------------------------------
    // handleForbidden
    // -------------------------------------------------------------------------
    @Nested
    inner class HandleForbidden {

        @Test
        fun `returns status 403`() {
            val response = sut.handleForbidden(ForbiddenException("forbidden"))
            assertEquals(HttpStatus.FORBIDDEN.value(), response.status)
        }

        @Test
        fun `returns the exception message`() {
            val response = sut.handleForbidden(ForbiddenException("Profile is private"))
            assertEquals("Profile is private", response.message)
        }

        @Test
        fun `includes a traceId`() {
            val response = sut.handleForbidden(ForbiddenException("forbidden"))
            assertNotNull(response.traceId)
        }
    }

    // -------------------------------------------------------------------------
    // handleValidationErrors
    // -------------------------------------------------------------------------
    @Nested
    inner class HandleValidationErrors {

        @Test
        fun `returns a map containing the failing field`() {
            val bindingResult = BeanPropertyBindingResult(Any(), "target")
            bindingResult.addError(FieldError("target", "height", "must be between 50 and 300"))
            val ex = MethodArgumentNotValidException(methodParameter, bindingResult)

            val response = sut.handleValidationErrors(ex)

            assertTrue(response.errors.containsKey("height"))
            assertEquals("must be between 50 and 300", response.errors["height"])
        }

        @Test
        fun `collects all failing fields`() {
            val bindingResult = BeanPropertyBindingResult(Any(), "target")
            bindingResult.addError(FieldError("target", "height", "too small"))
            bindingResult.addError(FieldError("target", "weight", "must be positive"))
            val ex = MethodArgumentNotValidException(methodParameter, bindingResult)

            val response = sut.handleValidationErrors(ex)

            assertEquals(2, response.errors.size)
            assertTrue(response.errors.containsKey("height"))
            assertTrue(response.errors.containsKey("weight"))
        }

        @Test
        fun `includes a traceId`() {
            val bindingResult = BeanPropertyBindingResult(Any(), "target")
            bindingResult.addError(FieldError("target", "field", "invalid"))
            val ex = MethodArgumentNotValidException(methodParameter, bindingResult)

            val response = sut.handleValidationErrors(ex)

            assertNotNull(response.traceId)
        }
    }

    // -------------------------------------------------------------------------
    // handleGeneric
    // -------------------------------------------------------------------------
    @Nested
    inner class HandleGeneric {

        @Test
        fun `returns status 500`() {
            val response = sut.handleGeneric(RuntimeException("unexpected"))
            assertEquals(500, response.status)
        }

        @Test
        fun `returns a generic message`() {
            val response = sut.handleGeneric(RuntimeException("unexpected"))
            assertEquals("An unexpected error occurred", response.message)
        }

        @Test
        fun `includes a traceId`() {
            val response = sut.handleGeneric(RuntimeException("unexpected"))
            assertNotNull(response.traceId)
        }
    }
}
