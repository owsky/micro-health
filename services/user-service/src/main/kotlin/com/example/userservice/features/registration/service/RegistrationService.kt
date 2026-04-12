package com.example.userservice.features.registration.service

import com.example.userservice.features.registration.dto.CreateUserRequest
import com.example.userservice.features.registration.dto.CreateUserResponse

interface RegistrationService {
    fun createNewUser(createUserRequest: CreateUserRequest): CreateUserResponse
}