package com.example.userservice.features.preferences.service.impl

import com.example.userservice.features.preferences.dto.PreferencesEntity
import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.dto.toResponse
import com.example.userservice.features.preferences.messaging.PreferencesEventPublisher
import com.example.userservice.features.preferences.repository.PreferencesRepository
import com.example.userservice.features.preferences.service.PreferencesService
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service

@Service
class PreferencesServiceImpl(
    private val repository: PreferencesRepository,
    private val eventPublisher: PreferencesEventPublisher,
    private val userProfileRepository: UserProfileRepository
) : PreferencesService {
    override fun getPreferencesByUsername(username: String): PreferencesResponse =
        repository.findByIdOrNull(username)?.toResponse()
            ?: throw ResourceNotFoundException("User preferences not found")


    override fun updatePreferencesByUsername(
        username: String, request: UpdatePreferencesRequest
    ): PreferencesResponse {
        val userProfileEntity = userProfileRepository.findByIdOrNull(username)
            ?: throw ResourceNotFoundException("Cannot create preferences for a user without a profile")
        val entity =
            repository.findByIdOrNull(username) ?: PreferencesEntity(request = request, userProfile = userProfileEntity)
        val saved = repository.save(entity)
        val response = saved.toResponse()
        eventPublisher.publishPreferencesUpdated(preferences = response)
        return response
    }
}