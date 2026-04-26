package com.example.userservice.features.fitnessgoals.service.impl

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.features.fitnessgoals.dto.applyUpdate
import com.example.userservice.features.fitnessgoals.dto.toResponse
import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import com.example.userservice.features.fitnessgoals.messaging.FitnessGoalsEventPublisher
import com.example.userservice.features.fitnessgoals.repository.FitnessGoalsRepository
import com.example.userservice.features.fitnessgoals.service.FitnessGoalsService
import com.example.userservice.features.profile.repository.UserProfileRepository
import com.example.userservice.shared.exceptions.ResourceNotFoundException
import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service

@Service
class FitnessGoalsServiceImpl(
    private val repository: FitnessGoalsRepository,
    private val userProfileRepository: UserProfileRepository,
    private val fitnessGoalsEventPublisher: FitnessGoalsEventPublisher
) : FitnessGoalsService {
    override fun getGoalsByUsername(username: String): FitnessGoalsResponse? =
        repository.findByIdOrNull(username)?.toResponse()

    override fun updateGoalsByUsername(
        username: String, request: UpdateFitnessGoalsRequest
    ): FitnessGoalsResponse {
        val userProfileEntity = userProfileRepository.findByIdOrNull(username)
            ?: throw ResourceNotFoundException("Cannot create fitness goals for a user without a profile")
        val entity = repository.findByIdOrNull(username) ?: FitnessGoalsEntity(
            username = username,
            targetWeight = null,
            dailySteps = null,
            burnedCalories = null,
            userProfile = userProfileEntity
        )
        val updated = entity.applyUpdate(request)
        val saved = repository.save(updated)
        val response = saved.toResponse()
        fitnessGoalsEventPublisher.publishFitnessGoalsUpdated(response)
        return response
    }
}