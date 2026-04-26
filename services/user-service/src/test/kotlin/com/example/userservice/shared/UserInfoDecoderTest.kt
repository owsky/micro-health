package com.example.userservice.shared

import io.mockk.every
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows
import org.junit.jupiter.api.extension.ExtendWith
import tools.jackson.databind.ObjectMapper
import java.util.Base64
import kotlin.test.assertEquals

@ExtendWith(MockKExtension::class)
class UserInfoDecoderTest {

    @MockK
    private lateinit var objectMapper: ObjectMapper

    @InjectMockKs
    private lateinit var sut: UserInfoDecoder

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(username = "alice", email = "alice@example.com")

    private fun encode(json: String): String =
        Base64.getEncoder().encodeToString(json.toByteArray())

    // -------------------------------------------------------------------------
    // decode
    // -------------------------------------------------------------------------
    @Nested
    inner class Decode {

        @Test
        fun `returns UserInfo decoded from Base64 header`() {
            // arrange
            val json = """{"preferred_username":"alice","email":"alice@example.com"}"""
            val token = encode(json)
            val bytes = Base64.getDecoder().decode(token)
            every { objectMapper.readValue(bytes, UserInfo::class.java) } returns userInfo

            // act
            val result = sut.decode(token)

            // assert
            assertEquals(userInfo, result)
        }

        @Test
        fun `throws when Base64 input is invalid`() {
            assertThrows<Exception> { sut.decode("not-valid-base64!!!") }
        }
    }
}



