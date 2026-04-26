package com.example.userservice.features.fitnessgoals.service

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.shared.UserInfo

interface FitnessGoalsService {
    fun getFitnessGoals(userInfo: UserInfo): FitnessGoalsResponse

    fun updateFitnessGoals(userInfo: UserInfo, request: UpdateFitnessGoalsRequest): FitnessGoalsResponse
}