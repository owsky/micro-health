package com.example.userservice.shared

import org.springframework.security.authentication.UsernamePasswordAuthenticationToken
import org.springframework.security.core.authority.SimpleGrantedAuthority
import org.springframework.security.test.web.servlet.request.SecurityMockMvcRequestPostProcessors.authentication

fun UserInfo.asMockUser() = authentication(
    UsernamePasswordAuthenticationToken(
        this, null, listOf(SimpleGrantedAuthority("ROLE_USER"))
    )
)


