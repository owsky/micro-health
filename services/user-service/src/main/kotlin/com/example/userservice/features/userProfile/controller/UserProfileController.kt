package com.example.userservice.features.userProfile.controller

import com.example.userservice.features.userProfile.dto.UpdateUserProfileRequest
import com.example.userservice.features.userProfile.dto.CreateUserProfileRequest
import com.example.userservice.features.userProfile.dto.UserProfileResponse
import com.example.userservice.features.userProfile.service.UserProfileService
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.DeleteMapping
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PatchMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.PostMapping
import jakarta.validation.Valid
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController
import java.security.Principal

@RestController
@RequestMapping("/profiles/v1")
class UserProfileController(
    private val userProfileService: UserProfileService
) {
    @GetMapping("/me")
    fun getMyProfile(principal: Principal): UserProfileResponse =
        userProfileService.getUserProfile(principal.name)

    @GetMapping("/{id}")
    fun getProfile(@PathVariable id: Long): UserProfileResponse =
        userProfileService.getUserProfile(id)

    @PostMapping
    fun createProfile(
        principal: Principal,
        @Valid @RequestBody body: CreateUserProfileRequest
    ): UserProfileResponse =
        userProfileService.createUserProfile(principal.name, body)

    @PatchMapping
    fun updateMyProfile(
        principal: Principal,
        @RequestBody body: UpdateUserProfileRequest
    ): UserProfileResponse =
        userProfileService.updateUserProfile(principal.name, body)

    @DeleteMapping
    fun deleteMyProfile(principal: Principal): ResponseEntity<Unit> {
        userProfileService.deleteUserProfile(principal.name)
        return ResponseEntity.noContent().build()
    }
}