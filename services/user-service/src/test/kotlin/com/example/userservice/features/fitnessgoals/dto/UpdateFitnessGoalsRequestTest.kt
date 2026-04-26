package com.example.userservice.features.fitnessgoals.dto

import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import java.time.LocalDate
import kotlin.test.assertEquals
import kotlin.test.assertFalse
import kotlin.test.assertTrue

class UpdateFitnessGoalsRequestTest {

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

    private val request = UpdateFitnessGoalsRequest(
        targetWeight = 60f,
        dailySteps = 8000,
        burnedCalories = 500
    )

    private val existingEntity = FitnessGoalsEntity(
        username = userProfile.username,
        targetWeight = 80f,
        dailySteps = 5000,
        burnedCalories = 300,
        userProfile = userProfile
    )

    // -------------------------------------------------------------------------
    // hasAtLeastOneField
    // -------------------------------------------------------------------------
    @Nested
    inner class HasAtLeastOneField {

        @Test
        fun `returns true when only targetWeight is set`() {
            assertTrue(UpdateFitnessGoalsRequest(targetWeight = 60f, dailySteps = null, burnedCalories = null).hasAtLeastOneField)
        }

        @Test
        fun `returns true when only dailySteps is set`() {
            assertTrue(UpdateFitnessGoalsRequest(targetWeight = null, dailySteps = 8000, burnedCalories = null).hasAtLeastOneField)
        }

        @Test
        fun `returns true when only burnedCalories is set`() {
            assertTrue(UpdateFitnessGoalsRequest(targetWeight = null, dailySteps = null, burnedCalories = 500).hasAtLeastOneField)
        }

        @Test
        fun `returns false when all fields are null`() {
            assertFalse(UpdateFitnessGoalsRequest(targetWeight = null, dailySteps = null, burnedCalories = null).hasAtLeastOneField)
        }
    }

    // -------------------------------------------------------------------------
    // FitnessGoalsEntity.applyUpdate(request)
    // -------------------------------------------------------------------------
    @Nested
    inner class ApplyUpdate {

        @Test
        fun `updates targetWeight when provided`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.targetWeight, existingEntity.targetWeight)
        }

        @Test
        fun `updates dailySteps when provided`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.dailySteps, existingEntity.dailySteps)
        }

        @Test
        fun `updates burnedCalories when provided`() {
            existingEntity.applyUpdate(request)
            assertEquals(request.burnedCalories, existingEntity.burnedCalories)
        }

        @Test
        fun `does not update targetWeight when null`() {
            val original = existingEntity.targetWeight
            existingEntity.applyUpdate(UpdateFitnessGoalsRequest(targetWeight = null, dailySteps = 8000, burnedCalories = 500))
            assertEquals(original, existingEntity.targetWeight)
        }

        @Test
        fun `does not update dailySteps when null`() {
            val original = existingEntity.dailySteps
            existingEntity.applyUpdate(UpdateFitnessGoalsRequest(targetWeight = 60f, dailySteps = null, burnedCalories = 500))
            assertEquals(original, existingEntity.dailySteps)
        }

        @Test
        fun `does not update burnedCalories when null`() {
            val original = existingEntity.burnedCalories
            existingEntity.applyUpdate(UpdateFitnessGoalsRequest(targetWeight = 60f, dailySteps = 8000, burnedCalories = null))
            assertEquals(original, existingEntity.burnedCalories)
        }

        @Test
        fun `returns the same entity instance`() {
            val result = existingEntity.applyUpdate(request)
            assertEquals(existingEntity, result)
        }
    }

    // -------------------------------------------------------------------------
    // FitnessGoalsEntity.toResponse()
    // -------------------------------------------------------------------------
    @Nested
    inner class ToResponse {

        @Test
        fun `maps targetWeight correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.targetWeight, response.targetWeight)
        }

        @Test
        fun `maps dailySteps correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.dailySteps, response.dailySteps)
        }

        @Test
        fun `maps burnedCalories correctly`() {
            val response = existingEntity.toResponse()
            assertEquals(existingEntity.burnedCalories, response.burnedCalories)
        }

        @Test
        fun `maps null fields correctly`() {
            val entity = FitnessGoalsEntity(
                username = userProfile.username,
                targetWeight = null,
                dailySteps = null,
                burnedCalories = null,
                userProfile = userProfile
            )
            val response = entity.toResponse()
            assertEquals(null, response.targetWeight)
            assertEquals(null, response.dailySteps)
            assertEquals(null, response.burnedCalories)
        }
    }
}
