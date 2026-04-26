package com.example.userservice.features.preferences.service

import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.shared.UserInfo

interface PreferencesService {
    fun getPreferences(userInfo: UserInfo): PreferencesResponse

    fun updatePreferences(userInfo: UserInfo, request: UpdatePreferencesRequest): PreferencesResponse
}