package com.example.userservice.features.registration.dto

data class CreateUserRequest (
    val username: String,
    val firstName: String,
    val lastName: String,
    val email: String
)