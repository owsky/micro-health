package com.example.userservice.shared

import com.fasterxml.jackson.annotation.JsonProperty

data class UserInfo(
    @JsonProperty("preferred_username") val username: String, val sub: String? = null, val email: String
)

