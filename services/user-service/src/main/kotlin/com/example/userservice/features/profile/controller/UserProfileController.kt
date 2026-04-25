package com.example.userservice.features.profile.controller

import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.service.UserProfileService
import com.example.userservice.shared.UserInfo
import org.springframework.http.ResponseEntity
import org.springframework.security.core.annotation.AuthenticationPrincipal
import org.springframework.web.bind.annotation.DeleteMapping
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PatchMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.PostMapping
import jakarta.validation.Valid
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/profiles/v1")
class UserProfileController(
    private val userProfileService: UserProfileService
) {
    @GetMapping("/me")
    fun getMyProfile(@AuthenticationPrincipal userInfo: UserInfo): UserProfileResponse =
        userProfileService.getUserProfile(userInfo.preferredUsername)

    @GetMapping("/{username}")
    fun getProfile(@PathVariable username: String): UserProfileResponse = userProfileService.getUserProfile(username)

    @PostMapping
    fun createProfile(
        @AuthenticationPrincipal userInfo: UserInfo, @Valid @RequestBody body: CreateUserProfileRequest
    ): UserProfileResponse = userProfileService.createUserProfile(userInfo, body)

    @PatchMapping
    fun updateMyProfile(
        @AuthenticationPrincipal userInfo: UserInfo, @RequestBody body: UpdateUserProfileRequest
    ): UserProfileResponse = userProfileService.updateUserProfile(userInfo.preferredUsername, body)

    @DeleteMapping
    fun deleteMyProfile(@AuthenticationPrincipal userInfo: UserInfo): ResponseEntity<Unit> {
        userProfileService.deleteUserProfile(userInfo.preferredUsername)
        return ResponseEntity.noContent().build()
    }
}