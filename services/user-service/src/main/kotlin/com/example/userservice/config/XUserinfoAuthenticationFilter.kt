package com.example.userservice.config

import com.example.userservice.shared.UserInfoDecoder
import jakarta.servlet.FilterChain
import jakarta.servlet.http.HttpServletRequest
import jakarta.servlet.http.HttpServletResponse
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken
import org.springframework.security.core.authority.SimpleGrantedAuthority
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.stereotype.Component
import org.springframework.web.filter.OncePerRequestFilter

@Component
class XUserinfoAuthenticationFilter(private val userInfoDecoder: UserInfoDecoder) : OncePerRequestFilter() {

    override fun doFilterInternal(
        request: HttpServletRequest,
        response: HttpServletResponse,
        filterChain: FilterChain
    ) {
        val header = request.getHeader("X-Userinfo")
        if (!header.isNullOrBlank()) {
            try {
                val userInfo = userInfoDecoder.decode(header)
                val auth = UsernamePasswordAuthenticationToken(
                    userInfo,
                    null,
                    listOf(SimpleGrantedAuthority("ROLE_USER"))
                )
                SecurityContextHolder.getContext().authentication = auth
            } catch (_: Exception) {
                // invalid header — leave SecurityContext empty; Spring Security will deny protected routes
            }
        }
        filterChain.doFilter(request, response)
    }
}


