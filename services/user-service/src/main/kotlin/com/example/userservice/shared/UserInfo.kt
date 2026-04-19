package com.example.userservice.shared

import com.fasterxml.jackson.annotation.JsonProperty

data class UserInfo(
    @JsonProperty("preferred_username")
    val preferredUsername: String,
    val sub: String? = null,
    val email: String? = null
)

