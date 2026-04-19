package com.example.userservice.features.userProfile.service

import com.example.userservice.features.userProfile.dto.UpdateUserProfileRequest
import com.example.userservice.features.userProfile.dto.CreateUserProfileRequest
import com.example.userservice.features.userProfile.dto.UserProfileResponse

interface UserProfileService {
    fun createUserProfile(userName: String, request: CreateUserProfileRequest): UserProfileResponse

    fun getUserProfile(id: Long): UserProfileResponse

    fun getUserProfile(userName: String): UserProfileResponse

    fun updateUserProfile(userName: String, request: UpdateUserProfileRequest): UserProfileResponse

    fun deleteUserProfile(userName: String)
}