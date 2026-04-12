package com.example.userservice.features.registration.dto

data class CreateUserResponse(
    val id: Long,
    val username: String,
    val firstName: String,
    val lastName: String,
    val email: String
)