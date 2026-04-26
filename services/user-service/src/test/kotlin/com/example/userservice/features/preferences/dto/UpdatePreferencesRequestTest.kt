package com.example.userservice.features.preferences.dto

import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import java.time.LocalDate
import kotlin.test.assertEquals

class UpdatePreferencesRequestTest {

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userProfile = UserProfileEntity(
        username = "alice",
        email = "alice@example.com",
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val request = UpdatePreferencesRequest(
        units = UnitsEnum.IMPERIAL,
        emailNotificationsEnabled = false,
        pushNotificationsEnabled = true,
        privateProfile = true
    )

    private val existingEntity = PreferencesEntity(
        username = userProfile.username,
        units = UnitsEnum.METRIC,
        emailNotificationsEnabled = true,
        pushNotificationsEnabled = false,
        privateProfile = false,
        userProfile = userProfile
    )

    // -------------------------------------------------------------------------
    // PreferencesEntity(request, userProfile) factory
    // -------------------------------------------------------------------------
    @Nested
    inner class PreferencesEntityFactory {

        @Test
        fun `sets username from user profile`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(userProfile.username, entity.username)
        }

        @Test
        fun `sets units from request`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(request.units, entity.units)
        }

        @Test
        fun `sets emailNotificationsEnabled from request`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(request.emailNotificationsEnabled, entity.emailNotificationsEnabled)
        }

        @Test
        fun `sets pushNotificationsEnabled from request`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(request.pushNotificationsEnabled, entity.pushNotificationsEnabled)
        }

        @Test
        fun `sets privateProfile from request`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(request.privateProfile, entity.privateProfile)
        }

        @Test
        fun `sets userProfile reference`() {
            val entity = PreferencesEntity(request, userProfile)
            assertEquals(userProfile, entity.userProfile)
        }
    }

    // -------------------------------------------------------------------------
    // PreferencesEntity.applyUpdate(request)
    // -------------------------------------------------------------------------
    @Nested
    inner class ApplyUpdate {

        @Test
        fun `updates units`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.units, existingEntity.units)
        }

        @Test
        fun `updates emailNotificationsEnabled`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.emailNotificationsEnabled, existingEntity.emailNotificationsEnabled)
        }

        @Test
        fun `updates pushNotificationsEnabled`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.pushNotificationsEnabled, existingEntity.pushNotificationsEnabled)
        }

        @Test
        fun `updates privateProfile`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.privateProfile, existingEntity.privateProfile)
        }

        @Test
        fun `returns the same entity instance`() {
            val result = existingEntity.applyUpdate(request)
            assertEquals(existingEntity, result)
        }
    }

    // -------------------------------------------------------------------------
    // PreferencesEntity.toResponse()
    // -------------------------------------------------------------------------
    @Nested
    inner class ToResponse {

        @Test
        fun `maps units correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.units, response.units)
        }

        @Test
        fun `maps emailNotificationsEnabled correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.emailNotificationsEnabled, response.emailNotificationsEnabled)
        }

        @Test
        fun `maps pushNotificationsEnabled correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.pushNotificationsEnabled, response.pushNotificationsEnabled)
        }

        @Test
        fun `maps privateProfile correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.privateProfile, response.privateProfile)
        }
    }
}

