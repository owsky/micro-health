package com.example.userservice.features.preferences.service.impl

import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.dto.toResponse
import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.preferences.messaging.PreferencesEventPublisher
import com.example.userservice.features.preferences.repository.PreferencesRepository
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import io.mockk.*
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import org.junit.jupiter.api.*
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.data.repository.CrudRepository
import org.springframework.data.repository.findByIdOrNull
import java.time.LocalDate
import kotlin.test.assertEquals

@ExtendWith(MockKExtension::class)
class PreferencesServiceImplTest {

    @MockK
    private lateinit var repository: PreferencesRepository

    @MockK
    private lateinit var eventPublisher: PreferencesEventPublisher

    @MockK
    private lateinit var userProfileRepository: UserProfileRepository

    @InjectMockKs
    private lateinit var sut: PreferencesServiceImpl

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val userProfileEntity = UserProfileEntity(
        username = userInfo.username,
        email = "alice@example.com",
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val preferencesEntity = PreferencesEntity(
        username = userInfo.username,
        units = UnitsEnum.METRIC,
        emailNotificationsEnabled = true,
        pushNotificationsEnabled = true,
        privateProfile = false,
        userProfile = userProfileEntity
    )

    private val updateRequest = UpdatePreferencesRequest(
        units = UnitsEnum.IMPERIAL,
        emailNotificationsEnabled = false,
        pushNotificationsEnabled = false,
        privateProfile = true
    )

    private val preferencesResponse = PreferencesResponse(
        units = UnitsEnum.IMPERIAL,
        emailNotificationsEnabled = false,
        pushNotificationsEnabled = false,
        privateProfile = true
    )

    @BeforeEach
    fun setUp() {
        mockkStatic(PreferencesEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    @AfterEach
    fun tearDown() {
        unmockkStatic(PreferencesEntity::toResponse, CrudRepository<*, *>::findByIdOrNull)
    }

    // -------------------------------------------------------------------------
    // getPreferencesByUsername
    // -------------------------------------------------------------------------
    @Nested
    inner class GetPreferencesByUsername {

        @Test
        fun `returns mapped response when entity exists`() {
            // arrange
            val entity = mockk<PreferencesEntity>()
            every { repository.findByIdOrNull(userInfo.username) } returns entity
            every { entity.toResponse() } returns preferencesResponse

            // act
            val result = sut.getPreferences(userInfo)

            //assert
            Assertions.assertEquals(preferencesResponse, result)
        }

        @Test
        fun `throws ResourceNotFoundException when no entity exists`() {
            // arrange
            every { repository.findByIdOrNull(userInfo.username) } returns null

            // act & assert
            assertThrows<ResourceNotFoundException> {
                sut.getPreferences(userInfo)
            }
        }
    }

    // -------------------------------------------------------------------------
    // updatePreferencesByUsername
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdatedPreferencesByUsername {

        @Test
        fun `returns the updated preferences when they already exist`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            every { repository.findByIdOrNull(userInfo.username) } returns preferencesEntity
            every { repository.save(any()) } answers { firstArg() }
            every { eventPublisher.publishPreferencesUpdated(any()) } just Runs

            // act
            val response = sut.updatePreferences(userInfo, updateRequest)

            // assert
            assertEquals(UnitsEnum.IMPERIAL, response.units)
            assertEquals(false, response.emailNotificationsEnabled)
            assertEquals(false, response.pushNotificationsEnabled)
            assertEquals(true, response.privateProfile)
            verify(exactly = 1) { repository.save(any()) }
        }

        @Test
        fun `publishes preferences updated event after saving`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            every { repository.findByIdOrNull(userInfo.username) } returns preferencesEntity
            every { repository.save(any()) } answers { firstArg() }
            every { eventPublisher.publishPreferencesUpdated(any()) } just Runs

            // act
            sut.updatePreferences(userInfo, updateRequest)

            // assert
            verify { eventPublisher.publishPreferencesUpdated(any()) }
        }

        @Test
        fun `throws ResourceNotFoundException when no profile exists`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns null

            // act & assert
            assertThrows<ResourceNotFoundException> { sut.updatePreferences(userInfo, updateRequest) }
        }

        @Test
        fun `returns the newly created preferences when they don't already exist`() {
            // arrange
            every { userProfileRepository.findByIdOrNull(userInfo.username) } returns userProfileEntity
            every { repository.findByIdOrNull(userInfo.username) } returns null
            every { repository.save(any()) } answers { firstArg() }
            every { eventPublisher.publishPreferencesUpdated(any()) } just Runs

            // act
            val response = sut.updatePreferences(userInfo, updateRequest)

            // assert
            assertEquals(UnitsEnum.IMPERIAL, response.units)
            assertEquals(false, response.emailNotificationsEnabled)
            assertEquals(false, response.pushNotificationsEnabled)
            assertEquals(true, response.privateProfile)
            verify(exactly = 1) { repository.save(any()) }
        }
    }
}