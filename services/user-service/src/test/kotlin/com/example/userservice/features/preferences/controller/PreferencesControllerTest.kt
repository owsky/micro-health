package com.example.userservice.features.preferences.controller

import com.example.userservice.config.SecurityConfig
import com.example.userservice.config.WebMvcSecurityTestConfig
import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.preferences.service.PreferencesService
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.UserInfoDecoder
import com.example.userservice.shared.asMockUser
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
import org.springframework.test.web.servlet.put
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.setup.DefaultMockMvcBuilder
import org.springframework.test.web.servlet.setup.MockMvcBuilders
import org.springframework.web.context.WebApplicationContext
import tools.jackson.databind.ObjectMapper


@WebMvcTest(PreferencesController::class)
@Import(WebMvcSecurityTestConfig::class, SecurityConfig::class)
@ExtendWith(MockKExtension::class)
class PreferencesControllerTest(
    @Autowired private val wac: WebApplicationContext, @Autowired private val objectMapper: ObjectMapper
) {
    private lateinit var mockMvc: MockMvc

    @MockkBean
    private lateinit var service: PreferencesService

    @MockkBean
    private lateinit var userInfoDecoder: UserInfoDecoder

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userInfo = UserInfo(
        username = "alice", email = "alice@example.com"
    )

    private val updatePreferencesRequest = UpdatePreferencesRequest(
        units = UnitsEnum.METRIC,
        emailNotificationsEnabled = true,
        pushNotificationsEnabled = true,
        privateProfile = true
    )

    private val preferencesResponse = PreferencesResponse(
        units = UnitsEnum.METRIC,
        emailNotificationsEnabled = true,
        pushNotificationsEnabled = true,
        privateProfile = true
    )

    @BeforeEach
    fun setup() {
        mockMvc = MockMvcBuilders.webAppContextSetup(wac).apply<DefaultMockMvcBuilder>(springSecurity())
            .defaultRequest<DefaultMockMvcBuilder>(
                MockMvcRequestBuilders.get("/").with(userInfo.asMockUser())
            ).build()
    }

    // -------------------------------------------------------------------------
    // getMyPreferences
    // -------------------------------------------------------------------------
    @Nested
    inner class GetMyPreferences {

        @Test
        fun `returns 200 with serialised preferences`() {
            // arrange
            every { service.getPreferences(userInfo) } returns preferencesResponse

            // act & assert
            mockMvc.get("/preferences/v1/me").andExpect {
                status { isOk() }
                jsonPath("$.units") { value("METRIC") }
                jsonPath("$.emailNotificationsEnabled") { value("true") }
                jsonPath("$.pushNotificationsEnabled") { value("true") }
                jsonPath("$.privateProfile") { value("true") }
            }
        }

        @Test
        fun `returns 404 when preferences are not found`() {
            // arrange
            every { service.getPreferences(userInfo) } throws ResourceNotFoundException("User preferences not found")

            // act & assert
            mockMvc.get("/preferences/v1/me").andExpect { status { isNotFound() } }
        }
    }

    // -------------------------------------------------------------------------
    // updateMyPreferences
    // -------------------------------------------------------------------------
    @Nested
    inner class UpdateMyPreferences {

        @Test
        fun `returns 201 with serialised updated preferences`() {
            // arrange
            every { service.updatePreferences(userInfo, updatePreferencesRequest) } returns preferencesResponse

            // act & assert
            mockMvc.put("/preferences/v1/me") {
                contentType = org.springframework.http.MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updatePreferencesRequest)
            }.andExpect {
                status { isCreated() }
                jsonPath("$.units") { value("METRIC") }
                jsonPath("$.emailNotificationsEnabled") { value("true") }
                jsonPath("$.pushNotificationsEnabled") { value("true") }
                jsonPath("$.privateProfile") { value("true") }
            }
        }

        @Test
        fun `returns 404 when preferences are not found`() {
            // arrange
            every {
                service.updatePreferences(userInfo, updatePreferencesRequest)
            } throws ResourceNotFoundException("User preferences not found")

            // act & assert
            mockMvc.put("/preferences/v1/me") {
                contentType = org.springframework.http.MediaType.APPLICATION_JSON
                content = objectMapper.writeValueAsString(updatePreferencesRequest)
            }.andExpect { status { isNotFound() } }
        }
    }
}
