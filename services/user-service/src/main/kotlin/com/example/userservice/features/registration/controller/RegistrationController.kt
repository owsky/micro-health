package com.example.userservice.features.registration.controller

import com.example.userservice.features.registration.dto.CreateUserRequest
import com.example.userservice.features.registration.dto.CreateUserResponse
import com.example.userservice.features.registration.service.RegistrationService
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.PostMapping
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/signup")
class RegistrationController(
    private val registrationService: RegistrationService
) {
    @PostMapping
    fun registerUser(@RequestBody body: CreateUserRequest): ResponseEntity<CreateUserResponse> {
        return ResponseEntity.ok(registrationService.createNewUser(body))
    }
}