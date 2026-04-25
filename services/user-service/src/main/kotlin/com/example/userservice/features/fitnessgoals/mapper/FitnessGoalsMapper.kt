package com.example.userservice.features.fitnessgoals.mapper

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import org.springframework.stereotype.Component

@Component
class FitnessGoalsMapper {
    fun toResponse(entity: FitnessGoalsEntity): FitnessGoalsResponse = FitnessGoalsResponse(
        targetWeight = entity.targetWeight, dailySteps = entity.dailySteps, burnedCalories = entity.burnedCalories
    )

    fun applyUpdate(request: UpdateFitnessGoalsRequest, entity: FitnessGoalsEntity): FitnessGoalsEntity {
        request.targetWeight?.let { entity.targetWeight = it }
        request.dailySteps?.let { entity.dailySteps = it }
        request.burnedCalories?.let { entity.burnedCalories = it }
        return entity
    }
}