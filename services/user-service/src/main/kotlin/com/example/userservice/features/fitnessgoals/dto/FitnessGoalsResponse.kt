package com.example.userservice.features.fitnessgoals.dto

data class FitnessGoalsResponse(
    val targetWeight: Float?,

    val dailySteps: Int?,

    val burnedCalories: Int?
)
