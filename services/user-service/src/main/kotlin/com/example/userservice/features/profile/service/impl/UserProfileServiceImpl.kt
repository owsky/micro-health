package com.example.userservice.features.profile.service.impl

import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileEntity
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.dto.applyUpdate
import com.example.userservice.features.profile.dto.toResponse
import com.example.userservice.features.profile.messaging.UserProfileEventPublisher
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.features.profile.service.UserProfileService
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.exceptions.ConflictException
import com.example.userservice.shared.exceptions.ForbiddenException
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import jakarta.transaction.Transactional
import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service

@Service
class UserProfileServiceImpl(
    private val repository: UserProfileRepository, private val eventPublisher: UserProfileEventPublisher
) : UserProfileService {

    @Transactional
    override fun createUserProfile(userInfo: UserInfo, request: CreateUserProfileRequest): UserProfileResponse {
        repository.findByIdOrNull(userInfo.username)?.let { throw ConflictException("User profile already exists") }
        val saved = repository.saveAndFlush(UserProfileEntity(request, userInfo))
        val response = saved.toResponse()
        eventPublisher.publishUserCreated(response)
        return response
    }

    override fun getUserProfile(userInfo: UserInfo): UserProfileResponse =
        repository.findByIdOrNull(userInfo.username)?.toResponse()
            ?: throw ResourceNotFoundException("User profile not found")


    override fun getUserProfileByUsername(
        userInfo: UserInfo, username: String
    ): UserProfileResponse {
        val entity = repository.findByIdOrNull(username) ?: throw ResourceNotFoundException("User profile not found")
        if (entity.email != userInfo.email && entity.preferences?.privateProfile == true) throw ForbiddenException(
            "User profile is set to private"
        )
        return entity.toResponse()
    }

    @Transactional
    override fun updateUserProfile(userInfo: UserInfo, request: UpdateUserProfileRequest): UserProfileResponse {
        val entity =
            repository.findByIdOrNull(userInfo.username) ?: throw ResourceNotFoundException("User profile not found")
        val updated = entity.applyUpdate(request)
        val saved = repository.saveAndFlush(updated)
        val response = saved.toResponse()
        eventPublisher.publishUserUpdated(response)
        return response
    }

    override fun deleteUserProfile(userInfo: UserInfo) {
        repository.deleteByUsername(userInfo.username)
        eventPublisher.publishUserDeleted(userInfo.username)
    }

}