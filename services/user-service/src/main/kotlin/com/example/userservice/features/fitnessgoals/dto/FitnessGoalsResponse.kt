package com.example.userservice.features.fitnessgoals.dto

import io.swagger.v3.oas.annotations.media.Schema

@Schema(description = "Fitness goals data")
data class FitnessGoalsResponse(
    @field:Schema(description = "Target weight in kilograms", example = "75.0")
    val targetWeight: Float?,

    @field:Schema(description = "Daily steps goal", example = "10000")
    val dailySteps: Int?,

    @field:Schema(description = "Daily burned calories goal", example = "500")
    val burnedCalories: Int?
)
