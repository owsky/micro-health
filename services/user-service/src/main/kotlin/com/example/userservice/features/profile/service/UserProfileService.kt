package com.example.userservice.features.profile.service

import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.shared.UserInfo

interface UserProfileService {
    fun createUserProfile(userInfo: UserInfo, request: CreateUserProfileRequest): UserProfileResponse

    fun getUserProfile(userName: String): UserProfileResponse

    fun updateUserProfile(userName: String, request: UpdateUserProfileRequest): UserProfileResponse

    fun deleteUserProfile(userName: String)
}