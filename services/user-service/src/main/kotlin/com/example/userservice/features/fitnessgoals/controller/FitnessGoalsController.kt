package com.example.userservice.features.fitnessgoals.controller

import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import com.example.userservice.features.fitnessgoals.dto.UpdateFitnessGoalsRequest
import com.example.userservice.features.fitnessgoals.service.FitnessGoalsService
import com.example.userservice.shared.UserInfo
import jakarta.validation.Valid
import org.springframework.security.core.annotation.AuthenticationPrincipal
import org.springframework.web.bind.annotation.*

@RestController
@RequestMapping("/fitness-goals/v1")
class FitnessGoalsController(private val service: FitnessGoalsService) {

    @GetMapping("/me")
    fun getMyGoals(@AuthenticationPrincipal userInfo: UserInfo): FitnessGoalsResponse? =
        service.getGoalsByUsername(username = userInfo.preferredUsername)

    @PatchMapping("/me")
    fun updateMyGoals(
        @AuthenticationPrincipal userInfo: UserInfo, @Valid @RequestBody request: UpdateFitnessGoalsRequest
    ): FitnessGoalsResponse = service.updateGoalsByUsername(username = userInfo.preferredUsername, request = request)
}