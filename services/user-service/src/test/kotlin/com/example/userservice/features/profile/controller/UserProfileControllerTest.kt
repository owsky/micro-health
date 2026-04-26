package com.example.userservice.features.profile.controller

import com.example.userservice.config.SecurityConfig
import com.example.userservice.config.WebMvcSecurityTestConfig
import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.features.profile.service.UserProfileService
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.UserInfoDecoder
import com.example.userservice.shared.asMockUser
import com.example.userservice.shared.exceptions.ConflictException
import com.example.userservice.shared.exceptions.ForbiddenException
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import com.ninjasquad.springmockk.MockkBean
import io.mockk.every
import io.mockk.junit5.MockKExtension
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.webmvc.test.autoconfigure.WebMvcTest
import org.springframework.context.annotation.Import
import org.springframework.http.MediaType
import org.springframework.security.test.web.servlet.setup.SecurityMockMvcConfigurers.springSecurity
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.delete
import org.springframework.test.web.servlet.get
import org.springframework.test.web.servlet.patch
import org.springframework.test.web.servlet.post
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.setup.DefaultMockMvcBuilder
import org.springframework.test.web.servlet.setup.MockMvcBuilders
import org.springframework.web.context.WebApplicationContext
import tools.jackson.databind.ObjectMapper
import java.time.LocalDate

@WebMvcTest(UserProfileController::class)
@Import(WebMvcSecurityTestConfig::class, SecurityConfig::class)
@ExtendWith(MockKExtension::class)
class UserProfileControllerTest(
    @Autowired private val wac: WebApplicationContext, @Autowired private val objectMapper: ObjectMapper
) {
    private lateinit var mockMvc: MockMvc

    @MockkBean
    private lateinit var userProfileService: UserProfileService

    @MockkBean
    private lateinit var userInfoDecoder: UserInfoDecoder

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val userProfileResponse = UserProfileResponse(
        username = "alice",
        email = "alice@example.com",
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    private val createUserProfileRequest = CreateUserProfileRequest(
        height = 170, weight = 65f, birthday = LocalDate.of(1990, 1, 1), gender = GenderEnum.FEMALE
    )

    private val updateUserProfileRequest = UpdateUserProfileRequest(
        height = null, weight = 70f, birthday = null, gender = null
    )

    @BeforeEach
    fun setup() {
        mockMvc = MockMvcBuilders.webAppContextSetup(wac).apply<DefaultMockMvcBuilder>(springSecurity())
            .defaultRequest<DefaultMockMvcBuilder>(
                MockMvcRequestBuilders.get("/").with(userInfo.asMockUser())
            ).build()
    }

    // -------------------------------------------------------------------------
    // getMyProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class GetMyProfile {

        @Test
        fun `returns 200 with serialised profile`() {
            // arrange
            every { userProfileService.getUserProfile(userInfo) } returns userProfileResponse

            // act & assert
            mockMvc.get("/profiles/v1/me").andExpect {
                status { isOk() }
                jsonPath("$.username") { value("alice") }
                jsonPath("$.email") { value("alice@example.com") }
                jsonPath("$.height") { value(170) }
                jsonPath("$.weight") { value(65.0) }
                jsonPath("$.gender") { value("FEMALE") }
            }
        }

        @Test
        fun `returns 404 when profile is not found`() {
            // arrange
            every { userProfileService.getUserProfile(userInfo) } throws ResourceNotFoundException("User profile not found")

            // act & assert
            mockMvc.get("/profiles/v1/me").andExpect { status { isNotFound() } }
        }
    }

    // -------------------------------------------------------------------------
    // getProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class GetProfile {

        @Test
        fun `returns 200 with serialised profile for given username`() {
            // arrange
            every { userProfileService.getUserProfileByUsername(userInfo, "bob") } returns userProfileResponse

            // act & assert
            mockMvc.get("/profiles/v1/bob").andExpect {
                status { isOk() }
                jsonPath("$.username") { value("alice") }
            }
        }

        @Test
        fun `returns 404 when profile is not found`() {
            // arrange
            every {
                userProfileService.getUserProfileByUsername(
                    userInfo, "bob"
                )
            } throws ResourceNotFoundException("User profile not found")

            // act & assert
            mockMvc.get("/profiles/v1/bob").andExpect { status { isNotFound() } }
        }

        @Test
        fun `returns 403 when profile is private`() {
            // arrange
            every {
                userProfileService.getUserProfileByUsername(
                    userInfo, "bob"
                )
            } throws ForbiddenException("Profile is private")

            // act & assert
            mockMvc.get("/profiles/v1/bob").andExpect { status { isForbidden() } }
        }
    }

    // -------------------------------------------------------------------------
    // createProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class CreateProfile {

        @Test
        fun `returns 201 with serialised created profile`() {
            // arrange
            every {
                userProfileService.createUserProfile(
                    userInfo, createUserProfileRequest
                )
            } returns userProfileResponse

            // act & assert
            mockMvc.post("/profiles/v1") {
                contentType = MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(createUserProfileRequest)
            }.andExpect {
                status { isCreated() }
                jsonPath("$.username") { value("alice") }
                jsonPath("$.email") { value("alice@example.com") }
                jsonPath("$.height") { value(170) }
                jsonPath("$.gender") { value("FEMALE") }
            }
        }

        @Test
        fun `returns 409 when profile already exists`() {
            // arrange
            every { userProfileService.createUserProfile(userInfo, createUserProfileRequest) } throws ConflictException(
                "User profile already exists"
            )

            // act & assert
            mockMvc.post("/profiles/v1") {
                contentType = MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(createUserProfileRequest)
            }.andExpect { status { isConflict() } }
        }
    }

    // -------------------------------------------------------------------------
    // updateMyProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdateMyProfile {

        @Test
        fun `returns 201 with serialised updated profile`() {
            // arrange
            val updatedResponse = userProfileResponse.copy(weight = 70f)
            every { userProfileService.updateUserProfile(userInfo, updateUserProfileRequest) } returns updatedResponse

            // act & assert
            mockMvc.patch("/profiles/v1/me") {
                contentType = MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updateUserProfileRequest)
            }.andExpect {
                status { isCreated() }
                jsonPath("$.weight") { value(70.0) }
            }
        }

        @Test
        fun `returns 404 when profile is not found`() {
            // arrange
            every {
                userProfileService.updateUserProfile(
                    userInfo, updateUserProfileRequest
                )
            } throws ResourceNotFoundException("User profile not found")

            // act & assert
            mockMvc.patch("/profiles/v1/me") {
                contentType = MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updateUserProfileRequest)
            }.andExpect { status { isNotFound() } }
        }
    }

    // -------------------------------------------------------------------------
    // deleteMyProfile
    // -------------------------------------------------------------------------
    @Nested
    inner class DeleteMyProfile {

        @Test
        fun `returns 204 on successful deletion`() {
            // arrange
            every { userProfileService.deleteUserProfile(userInfo) } returns Unit

            // act & assert
            mockMvc.delete("/profiles/v1/me").andExpect { status { isNoContent() } }
        }

        @Test
        fun `returns 404 when profile is not found`() {
            // arrange
            every { userProfileService.deleteUserProfile(userInfo) } throws ResourceNotFoundException("User profile not found")

            // act & assert
            mockMvc.delete("/profiles/v1/me").andExpect { status { isNotFound() } }
        }
    }
}
