package com.example.userservice.features.fitnessgoals.controller

import com.example.userservice.config.SecurityConfig
import com.example.userservice.config.WebMvcSecurityTestConfig
import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.features.fitnessgoals.service.impl.FitnessGoalsServiceImpl
import com.example.userservice.features.preferences.service.PreferencesService
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.UserInfoDecoder
import com.example.userservice.shared.asMockUser
import com.example.userservice.shared.exceptions.ConflictException
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
import org.springframework.security.test.web.servlet.setup.SecurityMockMvcConfigurers.springSecurity
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.get
import org.springframework.test.web.servlet.patch
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.setup.DefaultMockMvcBuilder
import org.springframework.test.web.servlet.setup.MockMvcBuilders
import org.springframework.web.context.WebApplicationContext
import tools.jackson.databind.ObjectMapper

@WebMvcTest(FitnessGoalsController::class)
@Import(WebMvcSecurityTestConfig::class, SecurityConfig::class)
@ExtendWith(MockKExtension::class)
class FitnessGoalsControllerTest(
    @Autowired private val wac: WebApplicationContext, @Autowired private val objectMapper: ObjectMapper
) {
    private lateinit var mockMvc: MockMvc

    @MockkBean
    private lateinit var service: FitnessGoalsServiceImpl

    @MockkBean
    private lateinit var userInfoDecoder: UserInfoDecoder

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val updateFitnessGoalsRequest = UpdateFitnessGoalsRequest(
        targetWeight = 50f, dailySteps = 10000, burnedCalories = 1200
    )

    private val fitnessGoalsResponse = FitnessGoalsResponse(
        targetWeight = 50f, dailySteps = 10000, burnedCalories = 1200
    )

    @BeforeEach
    fun setup() {
        mockMvc = MockMvcBuilders.webAppContextSetup(wac).apply<DefaultMockMvcBuilder>(springSecurity())
            .defaultRequest<DefaultMockMvcBuilder>(
                MockMvcRequestBuilders.get("/").with(userInfo.asMockUser())
            ).build()
    }

    // -------------------------------------------------------------------------
    // getMyGoals
    // -------------------------------------------------------------------------
    @Nested
    inner class GetMyGoals {

        @Test
        fun `returns 200 with serialised fitness goals`() {
            // arrange
            every { service.getFitnessGoals(userInfo) } returns fitnessGoalsResponse

            // act & assert
            mockMvc.get("/fitness-goals/v1/me").andExpect {
                status { isOk() }
                jsonPath("$.targetWeight") { value("50.0") }
                jsonPath("$.dailySteps") { value("10000") }
                jsonPath("$.burnedCalories") { value("1200") }
            }
        }

        @Test
        fun `returns 404 when fitness goals are not found`() {
            // arrange
            every { service.getFitnessGoals(userInfo) } throws ResourceNotFoundException("User has not set any fitness goals")

            // act & assert
            mockMvc.get("/fitness-goals/v1/me").andExpect { status { isNotFound() } }
        }
    }

    // -------------------------------------------------------------------------
    // updateMyGoals
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdateMyGoals {

        @Test
        fun `returns 201 with serialised updated fitness goals`() {
            // arrange
            every { service.updateFitnessGoals(userInfo, updateFitnessGoalsRequest) } returns fitnessGoalsResponse

            // act & assert
            mockMvc.patch("/fitness-goals/v1/me") {
                contentType = org.springframework.http.MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updateFitnessGoalsRequest)
            }.andExpect {
                status { isCreated() }
                jsonPath("$.targetWeight") { value("50.0") }
                jsonPath("$.dailySteps") { value("10000") }
                jsonPath("$.burnedCalories") { value("1200") }
            }
        }

        @Test
        fun `returns 409 when user profile is not found`() {
            // arrange
            every {
                service.updateFitnessGoals(
                    userInfo, updateFitnessGoalsRequest
                )
            } throws ConflictException("Cannot create fitness goals for a user without a profile")

            // act & assert
            mockMvc.patch("/fitness-goals/v1/me") {
                contentType = org.springframework.http.MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updateFitnessGoalsRequest)
            }.andExpect { status { isConflict() } }
        }
    }
}