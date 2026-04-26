package com.example.userservice.features.profile.controller

import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.service.UserProfileService
import com.example.userservice.shared.UserInfo
import jakarta.validation.Valid
import org.springframework.http.HttpStatus
import org.springframework.security.core.annotation.AuthenticationPrincipal
import org.springframework.web.bind.annotation.*

@RestController
@RequestMapping("/profiles/v1")
class UserProfileController(private val userProfileService: UserProfileService) {
    @GetMapping("/me")
    @ResponseStatus(HttpStatus.OK)
    fun getMyProfile(@AuthenticationPrincipal userInfo: UserInfo): UserProfileResponse =
        userProfileService.getUserProfile(userInfo)

    @GetMapping("/{username}")
    @ResponseStatus(HttpStatus.OK)
    fun getProfile(@AuthenticationPrincipal userInfo: UserInfo, @PathVariable username: String): UserProfileResponse =
        userProfileService.getUserProfileByUsername(userInfo, username)

    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    fun createProfile(
        @AuthenticationPrincipal userInfo: UserInfo, @Valid @RequestBody body: CreateUserProfileRequest
    ): UserProfileResponse = userProfileService.createUserProfile(userInfo, body)

    @PatchMapping("/me")
    @ResponseStatus(HttpStatus.CREATED)
    fun updateMyProfile(
        @AuthenticationPrincipal userInfo: UserInfo, @RequestBody body: UpdateUserProfileRequest
    ): UserProfileResponse = userProfileService.updateUserProfile(userInfo, body)

    @DeleteMapping("/me")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    fun deleteMyProfile(@AuthenticationPrincipal userInfo: UserInfo) = userProfileService.deleteUserProfile(userInfo)
}