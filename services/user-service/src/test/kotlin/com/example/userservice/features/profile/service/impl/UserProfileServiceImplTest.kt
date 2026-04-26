package com.example.userservice.features.profile.service.impl

import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.dto.toResponse
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.features.profile.messaging.UserProfileEventPublisher
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.UserInfo
import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.applyUpdate
import com.example.userservice.shared.exceptions.ConflictException
import com.example.userservice.shared.exceptions.ForbiddenException
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
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.data.repository.CrudRepository
import org.springframework.data.repository.findByIdOrNull
import java.time.LocalDate
import kotlin.test.assertEquals

@ExtendWith(MockKExtension::class)
class UserProfileServiceImplTest {

    @MockK
    private lateinit var repository: UserProfileRepository

    @MockK
    private lateinit var eventPublisher: UserProfileEventPublisher

    @InjectMockKs
    private lateinit var sut: UserProfileServiceImpl

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val createUserProfileRequest = CreateUserProfileRequest(
        height = 150, weight = 50f, birthday = LocalDate.of(1970, 1, 1), gender = GenderEnum.FEMALE
    )

    private val userProfileResponse = UserProfileResponse(
        username = userInfo.username,
        email = userInfo.email,
        height = 150,
        weight = 50f,
        birthday = LocalDate.of(1970, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val updateUserProfileRequest = UpdateUserProfileRequest(
        height = null, weight = 49f, birthday = null, gender = null
    )

    private val updatedUserProfileResponse = UserProfileResponse(
        username = userInfo.username,
        email = userInfo.email,
        height = 150,
        weight = 49f,
        birthday = LocalDate.of(1970, 1, 1),
        gender = GenderEnum.FEMALE
    )

    @BeforeEach
    fun setUp() {
        mockkStatic(UserProfileEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    @AfterEach
    fun tearDown() {
        unmockkStatic(UserProfileEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    // -------------------------------------------------------------------------
    // createUserProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class CreateUserProfile {

        @Test
        fun `returns mapped response when entity does not exist`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } returns null
            val entity = mockk<UserProfileEntity>()
            every { repository.save(any<UserProfileEntity>()) } returns entity
            every { entity.toResponse() } returns userProfileResponse
            every { eventPublisher.publishUserCreated(userProfileResponse) } just runs

            // act
            val res = sut.createUserProfile(userInfo, createUserProfileRequest)

            // assert
            assertEquals(userProfileResponse, res)
        }

        @Test
        fun `throws ConflictException when entity already exists`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } throws ConflictException("User profile already exists")

            // act & assert
            assertThrows<ConflictException> { sut.createUserProfile(userInfo, createUserProfileRequest) }
        }
    }

    // -------------------------------------------------------------------------
    // getUserProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class GetUserProfile {

        @Test
        fun `returns mapped response when entity exists`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.toResponse() } returns userProfileResponse

            // act
            val res = sut.getUserProfile(userInfo)

            // assert
            assertEquals(userProfileResponse, res)
        }

        @Test
        fun `throws ResourceNotFoundException when entity does not exist`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } throws ResourceNotFoundException("User profile not found")

            // act & assert
            assertThrows<ResourceNotFoundException> { sut.getUserProfile(userInfo) }
        }
    }

    // -------------------------------------------------------------------------
    // getUserProfileByUsername
    // -------------------------------------------------------------------------
    @Nested
    inner class GetUserProfileByUsername {

        private val targetUsername = "bob"

        @Test
        fun `returns the mapped response when entity exists and is not private`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(targetUsername) } returns entity
            every { entity.email } returns "bob@example.com"
            every { entity.preferences } returns mockk<PreferencesEntity> { every { privateProfile } returns false }
            every { entity.toResponse() } returns userProfileResponse

            // act
            val res = sut.getUserProfileByUsername(userInfo, targetUsername)

            // assert
            assertEquals(userProfileResponse, res)
        }

        @Test
        fun `returns the mapped response when requester is the profile owner even if private`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.email } returns userInfo.email
            every { entity.preferences } returns mockk<PreferencesEntity> { every { privateProfile } returns true }
            every { entity.toResponse() } returns userProfileResponse

            // act
            val res = sut.getUserProfileByUsername(userInfo, userInfo.username)

            // assert
            assertEquals(userProfileResponse, res)
        }

        @Test
        fun `throws ForbiddenException when entity exists but profile is private`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(targetUsername) } returns entity
            every { entity.email } returns "bob@example.com"
            every { entity.preferences } returns mockk<PreferencesEntity> { every { privateProfile } returns true }

            // act & assert
            assertThrows<ForbiddenException> { sut.getUserProfileByUsername(userInfo, targetUsername) }
        }

        @Test
        fun `throws ResourceNotFoundException when entity does not exist`() {
            // arrange
            every { repository.findByIdOrNull(targetUsername) } returns null

            // act & assert
            assertThrows<ResourceNotFoundException> { sut.getUserProfileByUsername(userInfo, targetUsername) }
        }
    }

    // -------------------------------------------------------------------------
    // updateUserProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdateUserProfile {

        @Test
        fun `returns the mapped updated response when the entity exists`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.applyUpdate(updateUserProfileRequest) } returns entity
            every { repository.save(entity) } returns entity
            every { entity.toResponse() } returns updatedUserProfileResponse
            every { eventPublisher.publishUserUpdated(updatedUserProfileResponse) } just runs

            // act
            val res = sut.updateUserProfile(userInfo, updateUserProfileRequest)

            // assert
            assertEquals(updatedUserProfileResponse, res)
        }

        @Test
        fun `publishes user profile updated event after saving`() {
            // arrange
            val entity = mockk<UserProfileEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.applyUpdate(updateUserProfileRequest) } returns entity
            every { repository.save(entity) } returns entity
            every { entity.toResponse() } returns updatedUserProfileResponse
            every { eventPublisher.publishUserUpdated(updatedUserProfileResponse) } just runs

            // act
            sut.updateUserProfile(userInfo, updateUserProfileRequest)

            // assert
            verify(exactly = 1) { eventPublisher.publishUserUpdated(updatedUserProfileResponse) }
        }

        @Test
        fun `throws ResourceNotFoundException when the entity does not exist`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } throws ResourceNotFoundException("User profile not found")

            // act & assert
            assertThrows<ResourceNotFoundException> { sut.updateUserProfile(userInfo, updateUserProfileRequest) }
        }
    }

    // -------------------------------------------------------------------------
    // deleteUserProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class DeleteUserProfile {

        @Test
        fun `returns nothing after deletion`() {
            // arrange
            every { repository.deleteByUsername(userInfo.username) } just runs
            every { eventPublisher.publishUserDeleted(userInfo.username) } just runs

            // act
            val res = sut.deleteUserProfile(userInfo)

            // assert
            assertEquals(Unit, res)
        }

        @Test
        fun `publishes user profile deleted event after deleting`() {
            // arrange
            every { repository.deleteByUsername(userInfo.username) } just runs
            every { eventPublisher.publishUserDeleted(userInfo.username) } just runs

            // act
            sut.deleteUserProfile(userInfo)

            // assert
            verify(exactly = 1) { eventPublisher.publishUserDeleted(userInfo.username) }
        }
    }
}