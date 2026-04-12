package com.example.userservice.features.registration.service.impl

import com.example.userservice.features.registration.dto.CreateUserRequest
import com.example.userservice.features.registration.dto.CreateUserResponse
import com.example.userservice.features.registration.service.RegistrationService
import org.springframework.stereotype.Service

@Service
class RegistrationServiceImpl : RegistrationService {
    override fun createNewUser(createUserRequest: CreateUserRequest): CreateUserResponse {
        return CreateUserResponse(
            0L,
            createUserRequest.username,
            createUserRequest.firstName,
            createUserRequest.lastName,
            createUserRequest.email
        )
    }
}