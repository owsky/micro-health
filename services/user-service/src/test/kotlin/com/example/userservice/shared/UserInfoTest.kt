package com.example.userservice.shared

import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import tools.jackson.module.kotlin.jacksonObjectMapper
import tools.jackson.module.kotlin.readValue
import kotlin.test.assertEquals
import kotlin.test.assertNull

class UserInfoTest {

    private val mapper = jacksonObjectMapper()

    // -------------------------------------------------------------------------
    // JSON Deserialization
    // -------------------------------------------------------------------------
    @Nested
    inner class Deserialization {

        @Test
        fun `maps preferred_username to username`() {
            val json = """{"preferred_username":"alice","email":"alice@example.com"}"""
            val userInfo = mapper.readValue<UserInfo>(json)
            assertEquals("alice", userInfo.username)
        }

        @Test
        fun `maps email field`() {
            val json = """{"preferred_username":"alice","email":"alice@example.com"}"""
            val userInfo = mapper.readValue<UserInfo>(json)
            assertEquals("alice@example.com", userInfo.email)
        }

        @Test
        fun `maps sub field when present`() {
            val json = """{"preferred_username":"alice","email":"alice@example.com","sub":"sub-123"}"""
            val userInfo = mapper.readValue<UserInfo>(json)
            assertEquals("sub-123", userInfo.sub)
        }

        @Test
        fun `sub is null when absent from JSON`() {
            val json = """{"preferred_username":"alice","email":"alice@example.com"}"""
            val userInfo = mapper.readValue<UserInfo>(json)
            assertNull(userInfo.sub)
        }
    }
}

