package com.example.userservice.features.profile.service.impl

import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.mapper.UserProfileMapper
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
    private val repository: UserProfileRepository,
    private val mapper: UserProfileMapper,
    private val eventPublisher: UserProfileEventPublisher
) : UserProfileService {

    @Transactional
    override fun createUserProfile(userInfo: UserInfo, request: CreateUserProfileRequest): UserProfileResponse {
        repository.findByIdOrNull(userInfo.preferredUsername)
            ?.let { throw ConflictException("User profile already exists") }
        val saved = repository.save(mapper.toEntity(request, userInfo))
        val response = mapper.toResponse(saved)
        eventPublisher.publishUserCreated(response)
        return response
    }

    override fun getUserProfile(userInfo: UserInfo): UserProfileResponse = mapper.toResponse(
        repository.findByIdOrNull(userInfo.preferredUsername)
            ?: throw ResourceNotFoundException("User profile not found")
    )

    override fun getUserProfileByUsername(
        userInfo: UserInfo, username: String
    ): UserProfileResponse {
        val entity = repository.findByIdOrNull(username) ?: throw ResourceNotFoundException("User profile not found")
        if (entity.preferences?.privateProfile == true) throw ForbiddenException("User profile is set to private")
        return mapper.toResponse(entity)
    }

    @Transactional
    override fun updateUserProfile(userInfo: UserInfo, request: UpdateUserProfileRequest): UserProfileResponse {
        val entity = repository.findByIdOrNull(userInfo.preferredUsername)
            ?: throw ResourceNotFoundException("User profile not found")
        val updated = mapper.applyUpdate(request, entity)
        val saved = repository.save(updated)
        val response = mapper.toResponse(saved)
        eventPublisher.publishUserUpdated(response)
        return response
    }

    override fun deleteUserProfile(userInfo: UserInfo) {
        repository.deleteByUsername(userInfo.preferredUsername)
        eventPublisher.publishUserDeleted(userInfo.preferredUsername)
    }

}