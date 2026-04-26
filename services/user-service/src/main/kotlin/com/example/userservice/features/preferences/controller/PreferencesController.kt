package com.example.userservice.features.preferences.controller

import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.service.PreferencesService
import com.example.userservice.shared.UserInfo
import org.springframework.security.core.annotation.AuthenticationPrincipal
import org.springframework.web.bind.annotation.*

@RestController
@RequestMapping("/preferences/v1")
class PreferencesController(
    val service: PreferencesService
) {
    @GetMapping("/me")
    fun getMyPreferences(@AuthenticationPrincipal userInfo: UserInfo): PreferencesResponse =
        service.getPreferencesByUsername(username = userInfo.preferredUsername)

    @PutMapping("/me")
    fun updateMyPreferences(
        @AuthenticationPrincipal userInfo: UserInfo, @RequestBody request: UpdatePreferencesRequest
    ): PreferencesResponse =
        service.updatePreferencesByUsername(username = userInfo.preferredUsername, request = request)
}