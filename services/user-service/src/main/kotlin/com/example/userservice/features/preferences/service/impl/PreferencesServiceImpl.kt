package com.example.userservice.features.preferences.service.impl

import com.example.userservice.features.preferences.dto.PreferencesEntity
import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.dto.applyUpdate
import com.example.userservice.features.preferences.dto.toResponse
import com.example.userservice.features.preferences.messaging.PreferencesEventPublisher
import com.example.userservice.features.preferences.repository.PreferencesRepository
import com.example.userservice.features.preferences.service.PreferencesService
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.UserInfo
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service

@Service
class PreferencesServiceImpl(
    private val repository: PreferencesRepository,
    private val eventPublisher: PreferencesEventPublisher,
    private val userProfileRepository: UserProfileRepository
) : PreferencesService {
    override fun getPreferences(userInfo: UserInfo): PreferencesResponse =
        repository.findByIdOrNull(userInfo.username)?.toResponse()
            ?: throw ResourceNotFoundException("User preferences not found")


    override fun updatePreferences(
        userInfo: UserInfo, request: UpdatePreferencesRequest
    ): PreferencesResponse {
        val userProfileEntity = userProfileRepository.findByIdOrNull(userInfo.username)
            ?: throw ResourceNotFoundException("Cannot create preferences for a user without a profile")
        val entity = repository.findByIdOrNull(userInfo.username)?.applyUpdate(request) ?: PreferencesEntity(
            request, userProfileEntity
        )
        val saved = repository.save(entity)
        val response = saved.toResponse()
        eventPublisher.publishPreferencesUpdated(response)
        return response
    }
}