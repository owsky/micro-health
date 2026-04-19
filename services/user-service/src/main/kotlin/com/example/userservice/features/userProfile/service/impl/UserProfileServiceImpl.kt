package com.example.userservice.features.userProfile.service.impl

import com.example.userservice.features.userProfile.dto.UpdateUserProfileRequest
import com.example.userservice.features.userProfile.dto.CreateUserProfileRequest
import com.example.userservice.features.userProfile.dto.UserProfileResponse
import com.example.userservice.features.userProfile.mapper.UserProfileMapper
import com.example.userservice.features.userProfile.repository.UserProfileRepository
import com.example.userservice.features.userProfile.messaging.UserProfileEventPublisher
import com.example.userservice.features.userProfile.service.UserProfileService
import com.example.userservice.shared.exceptions.ConflictException
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service

@Service
class UserProfileServiceImpl(
    private val repository: UserProfileRepository,
    private val mapper: UserProfileMapper,
    private val eventPublisher: UserProfileEventPublisher
) : UserProfileService {
    override fun createUserProfile(userName: String, request: CreateUserProfileRequest): UserProfileResponse {
        repository.findByUserName(userName)?.let { throw ConflictException("User profile already exists") }
        val saved = repository.save(mapper.toEntity(request, userName))
        val response = mapper.toResponse(saved)
        eventPublisher.publishUserCreated(response)
        return response
    }

    override fun getUserProfile(id: Long): UserProfileResponse =
        mapper.toResponse(repository.findByIdOrNull(id) ?: throw ResourceNotFoundException("User profile not found"))


    override fun getUserProfile(userName: String): UserProfileResponse =
        mapper.toResponse(
            repository.findByUserName(userName) ?: throw ResourceNotFoundException("User profile not found")
        )

    override fun updateUserProfile(userName: String, request: UpdateUserProfileRequest): UserProfileResponse {
        val entity = repository.findByUserName(userName) ?: throw ResourceNotFoundException("User profile not found")
        val updated = mapper.applyUpdate(request, entity)
        val saved = repository.save(updated)
        return mapper.toResponse(saved)
    }

    override fun deleteUserProfile(userName: String) = repository.deleteByUserName(userName)

}