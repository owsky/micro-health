package com.example.userservice

import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.asMockUser
import org.junit.jupiter.api.Test
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.get
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get

@SpringBootTest
@AutoConfigureMockMvc
class UserServiceApplicationTests {

    @Autowired
    private lateinit var mockMvc: MockMvc

    // -------------------------------------------------------------------------
    // Context
    // -------------------------------------------------------------------------

    @Test
    fun contextLoads() {
    }

    // -------------------------------------------------------------------------
    // Security — unauthenticated access
    // -------------------------------------------------------------------------

    @Test
    fun `unauthenticated request to protected endpoint returns 401`() {
        mockMvc.get("/profile/v1/me").andExpect {
            status { isUnauthorized() }
        }
    }

    // -------------------------------------------------------------------------
    // Security — authenticated access
    // -------------------------------------------------------------------------

    @Test
    fun `authenticated request to protected endpoint does not return 401`() {
        val userInfo = UserInfo(username = "alice", email = "alice@example.com")
        mockMvc.perform(get("/profile/v1/me").with(userInfo.asMockUser()))
            .andExpect { result ->
                val status = result.response.status
                assert(status != 401) { "Expected a non-401 response but got $status" }
            }
    }

    // -------------------------------------------------------------------------
    // Public endpoints
    // -------------------------------------------------------------------------

    @Test
    fun `actuator health endpoint is publicly accessible`() {
        mockMvc.get("/actuator/health").andExpect {
            status { isOk() }
        }
    }
}
