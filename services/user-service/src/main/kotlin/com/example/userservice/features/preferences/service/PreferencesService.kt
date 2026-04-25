package com.example.userservice.features.preferences.service

import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest

interface PreferencesService {
    fun getPreferencesByUsername(username: String): PreferencesResponse

    fun updatePreferencesByUsername(username: String, request: UpdatePreferencesRequest): PreferencesResponse
}