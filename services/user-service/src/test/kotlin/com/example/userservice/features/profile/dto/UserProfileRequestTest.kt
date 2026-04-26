package com.example.userservice.features.profile.dto

import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.shared.UserInfo
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import java.time.LocalDate
import kotlin.test.assertEquals
import kotlin.test.assertTrue

class UserProfileRequestTest {

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val createRequest = CreateUserProfileRequest(
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val existingEntity = UserProfileEntity(
        username = "alice",
        email = "alice@example.com",
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    // -------------------------------------------------------------------------
    // UserProfileEntity(request, userInfo) factory
    // -------------------------------------------------------------------------
    @Nested
    inner class UserProfileEntityFactory {

        @Test
        fun `sets username from userInfo`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(userInfo.username, entity.username)
        }

        @Test
        fun `sets email from userInfo`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(userInfo.email, entity.email)
        }

        @Test
        fun `sets height from request`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(createRequest.height, entity.height)
        }

        @Test
        fun `sets weight from request`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(createRequest.weight, entity.weight)
        }

        @Test
        fun `sets birthday from request`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(createRequest.birthday, entity.birthday)
        }

        @Test
        fun `sets gender from request`() {
            val entity = UserProfileEntity(createRequest, userInfo)
            assertEquals(createRequest.gender, entity.gender)
        }
    }

    // -------------------------------------------------------------------------
    // UserProfileEntity.applyUpdate(request)
    // -------------------------------------------------------------------------
    @Nested
    inner class ApplyUpdate {

        @Test
        fun `updates height when provided`() {
            val request = UpdateUserProfileRequest(height = 180, weight = null, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(180, existingEntity.height)
        }

        @Test
        fun `does not update height when null`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(170, existingEntity.height)
        }

        @Test
        fun `updates weight when provided`() {
            val request = UpdateUserProfileRequest(height = null, weight = 70f, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(70f, existingEntity.weight)
        }

        @Test
        fun `does not update weight when null`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(65f, existingEntity.weight)
        }

        @Test
        fun `updates birthday when provided`() {
            val newBirthday = LocalDate.of(1995, 6, 15)
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = newBirthday, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(newBirthday, existingEntity.birthday)
        }

        @Test
        fun `does not update birthday when null`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(LocalDate.of(1990, 1, 1), existingEntity.birthday)
        }

        @Test
        fun `updates gender when provided`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = GenderEnum.MALE)
            existingEntity.applyUpdate(request)
            assertEquals(GenderEnum.MALE, existingEntity.gender)
        }

        @Test
        fun `does not update gender when null`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = null)
            existingEntity.applyUpdate(request)
            assertEquals(GenderEnum.FEMALE, existingEntity.gender)
        }

        @Test
        fun `returns the same entity instance`() {
            val request = UpdateUserProfileRequest(height = null, weight = 70f, birthday = null, gender = null)
            val result = existingEntity.applyUpdate(request)
            assertEquals(existingEntity, result)
        }
    }

    // -------------------------------------------------------------------------
    // UpdateUserProfileRequest.hasAtLeastOneField validation
    // -------------------------------------------------------------------------
    @Nested
    inner class HasAtLeastOneField {

        @Test
        fun `returns true when height is provided`() {
            val request = UpdateUserProfileRequest(height = 180, weight = null, birthday = null, gender = null)
            assertTrue(request.hasAtLeastOneField)
        }

        @Test
        fun `returns true when weight is provided`() {
            val request = UpdateUserProfileRequest(height = null, weight = 70f, birthday = null, gender = null)
            assertTrue(request.hasAtLeastOneField)
        }

        @Test
        fun `returns true when birthday is provided`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = LocalDate.of(1995, 1, 1), gender = null)
            assertTrue(request.hasAtLeastOneField)
        }

        @Test
        fun `returns true when gender is provided`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = GenderEnum.MALE)
            assertTrue(request.hasAtLeastOneField)
        }

        @Test
        fun `returns false when all fields are null`() {
            val request = UpdateUserProfileRequest(height = null, weight = null, birthday = null, gender = null)
            assertEquals(false, request.hasAtLeastOneField)
        }
    }

    // -------------------------------------------------------------------------
    // UserProfileEntity.toResponse()
    // -------------------------------------------------------------------------
    @Nested
    inner class ToResponse {

        @Test
        fun `maps username correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.username, response.username)
        }

        @Test
        fun `maps email correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.email, response.email)
        }

        @Test
        fun `maps height correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.height, response.height)
        }

        @Test
        fun `maps weight correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.weight, response.weight)
        }

        @Test
        fun `maps birthday correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.birthday, response.birthday)
        }

        @Test
        fun `maps gender correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.gender, response.gender)
        }
    }
}

