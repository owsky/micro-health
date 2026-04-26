package com.example.userservice.features.fitnessgoals.service.impl

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.features.fitnessgoals.dto.applyUpdate
import com.example.userservice.features.fitnessgoals.dto.toResponse
import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import com.example.userservice.features.fitnessgoals.messaging.FitnessGoalsEventPublisher
import com.example.userservice.features.fitnessgoals.repository.FitnessGoalsRepository
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.exceptions.ConflictException
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import io.mockk.every
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import io.mockk.just
import io.mockk.mockk
import io.mockk.mockkStatic
import io.mockk.runs
import io.mockk.unmockkStatic
import io.mockk.verify
import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.Assertions
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertNull
import org.junit.jupiter.api.assertThrows
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.data.repository.CrudRepository
import org.springframework.data.repository.findByIdOrNull
import java.time.LocalDate
import kotlin.test.assertEquals

@ExtendWith(MockKExtension::class)
class FitnessGoalsServiceImplTest {

    @MockK
    private lateinit var repository: FitnessGoalsRepository

    @MockK
    private lateinit var eventPublisher: FitnessGoalsEventPublisher

    @MockK
    private lateinit var userProfileRepository: UserProfileRepository

    @InjectMockKs
    private lateinit var sut: FitnessGoalsServiceImpl

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val userProfileEntity = UserProfileEntity(
        username = userInfo.username,
        email = userInfo.email,
        height = 150,
        weight = 55f,
        birthday = LocalDate.of(1970, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val updateFitnessGoalsRequest = UpdateFitnessGoalsRequest(
        targetWeight = 50f, dailySteps = 10000, burnedCalories = 1500
    )

    private val fitnessGoalsResponse = FitnessGoalsResponse(
        targetWeight = 50f, dailySteps = 10000, burnedCalories = 1500
    )

    @BeforeEach
    fun setUp() {
        mockkStatic(FitnessGoalsEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    @AfterEach
    fun tearDown() {
        unmockkStatic(FitnessGoalsEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    // -------------------------------------------------------------------------
    // getFitnessGoals
    // -------------------------------------------------------------------------
    @Nested
    inner class GetFitnessGoals {

        @Test
        fun `returns mapped response when entity exists`() {
            // arrange
            val entity = mockk<FitnessGoalsEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.toResponse() } returns fitnessGoalsResponse

            // act
            val result = sut.getFitnessGoals(userInfo)

            // assert
            Assertions.assertEquals(fitnessGoalsResponse, result)
        }

        @Test
        fun `throws ResourceNotFoundException when entity does not exist`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } throws ResourceNotFoundException("User has not set any fitness goals")

            // act & assert
            assertThrows<ResourceNotFoundException> { sut.getFitnessGoals(userInfo) }
        }
    }

    // -------------------------------------------------------------------------
    // updateFitnessGoals
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdateFitnessGoals {

        @Test
        fun `returns updated serialised fitness goals when user profile exists`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            val entity = mockk<FitnessGoalsEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.applyUpdate(updateFitnessGoalsRequest) } returns entity
            every { repository.save(entity) } returns entity
            every { entity.toResponse() } returns fitnessGoalsResponse
            every { eventPublisher.publishFitnessGoalsUpdated(fitnessGoalsResponse) } just runs

            // act
            val res = sut.updateFitnessGoals(userInfo, updateFitnessGoalsRequest)

            // assert
            assertEquals(fitnessGoalsResponse, res)
        }

        @Test
        fun `published fitness goals updated event after saving`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            val entity = mockk<FitnessGoalsEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.applyUpdate(updateFitnessGoalsRequest) } returns entity
            every { repository.save(entity) } returns entity
            every { entity.toResponse() } returns fitnessGoalsResponse
            every { eventPublisher.publishFitnessGoalsUpdated(fitnessGoalsResponse) } just runs

            // act
            sut.updateFitnessGoals(userInfo, updateFitnessGoalsRequest)

            // assert
            verify { eventPublisher.publishFitnessGoalsUpdated(any()) }
        }

        @Test
        fun `throws ConflictException when user profile does not exist`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns null

            // act & assert
            assertThrows<ConflictException> { sut.updateFitnessGoals(userInfo, updateFitnessGoalsRequest) }
        }

        @Test
        fun `returns the newly created serialised fitness goals when the entity does not exist`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            every { repository.findByIdOrNull(userInfo.username) } returns null
            val entity = mockk<FitnessGoalsEntity>()
            every { entity.applyUpdate(updateFitnessGoalsRequest) } returns entity
            every { repository.save(any()) } returns entity
            every { entity.toResponse() } returns fitnessGoalsResponse
            every { eventPublisher.publishFitnessGoalsUpdated(fitnessGoalsResponse) } just runs

            // act
            val res = sut.updateFitnessGoals(userInfo, updateFitnessGoalsRequest)

            // assert
            assertEquals(fitnessGoalsResponse, res)
        }
    }
}