package com.example.userservice.shared

import org.springframework.stereotype.Component
import tools.jackson.databind.ObjectMapper
import java.util.Base64

@Component
class UserInfoDecoder(private val objectMapper: ObjectMapper) {

    fun decode(xUserinfoHeader: String): UserInfo {
        val bytes = Base64.getDecoder().decode(xUserinfoHeader)
        return objectMapper.readValue(bytes, UserInfo::class.java)
    }
}
