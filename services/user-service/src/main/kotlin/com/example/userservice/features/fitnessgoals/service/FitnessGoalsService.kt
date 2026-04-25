package com.example.userservice.features.fitnessgoals.service

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest

interface FitnessGoalsService {
    fun getGoalsByUsername(username: String): FitnessGoalsResponse?

    fun updateGoalsByUsername(username: String, request: UpdateFitnessGoalsRequest): FitnessGoalsResponse
}