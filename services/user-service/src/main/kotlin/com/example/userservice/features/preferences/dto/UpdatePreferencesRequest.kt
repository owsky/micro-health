package com.example.userservice.features.preferences.dto

import com.example.userservice.features.preferences.enums.UnitsEnum

data class UpdatePreferencesRequest(
    val units: UnitsEnum,
    val emailNotificationsEnabled: Boolean,
    val pushNotificationsEnabled: Boolean,
    val privateProfile: Boolean
)
