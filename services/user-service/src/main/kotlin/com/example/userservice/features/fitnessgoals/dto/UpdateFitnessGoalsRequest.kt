package com.example.userservice.features.fitnessgoals.dto

import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import com.fasterxml.jackson.annotation.JsonIgnore
import jakarta.validation.constraints.AssertTrue
import jakarta.validation.constraints.Max
import jakarta.validation.constraints.Min

data class UpdateFitnessGoalsRequest(
    @field:Min(30) @field:Max(300) val targetWeight: Float?,

    @field:Min(1000) @field:Max(50000) val dailySteps: Int?,

    @field:Min(100) @field:Max(5000) val burnedCalories: Int?
) {
    @get:JsonIgnore
    @get:AssertTrue(message = "At least one goal field must be provided")
    val hasAtLeastOneField: Boolean
        get() = targetWeight != null || dailySteps != null || burnedCalories != null
}

fun FitnessGoalsEntity.applyUpdate(request: UpdateFitnessGoalsRequest): FitnessGoalsEntity {
    request.targetWeight?.let { this.targetWeight = it }
    request.dailySteps?.let { this.dailySteps = it }
    request.burnedCalories?.let { this.burnedCalories = it }
    return this
}

fun FitnessGoalsEntity.toResponse(): FitnessGoalsResponse = FitnessGoalsResponse(
    targetWeight = this.targetWeight, dailySteps = this.dailySteps, burnedCalories = this.burnedCalories
)