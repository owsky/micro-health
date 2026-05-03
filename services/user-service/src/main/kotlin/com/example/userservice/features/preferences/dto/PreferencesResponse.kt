package com.example.userservice.features.preferences.dto

import com.example.userservice.features.preferences.enums.UnitsEnum
import io.swagger.v3.oas.annotations.media.Schema

@Schema(description = "User preferences data")
data class PreferencesResponse(
    @field:Schema(
        description = "Measurement units preference",
        example = "METRIC",
        allowableValues = ["METRIC", "IMPERIAL"]
    )
    val units: UnitsEnum,

    @field:Schema(description = "Whether email notifications are enabled", example = "true")
    val emailNotificationsEnabled: Boolean,

    @field:Schema(description = "Whether push notifications are enabled", example = "false")
    val pushNotificationsEnabled: Boolean,

    @field:Schema(description = "Whether the user's profile is private", example = "false")
    val privateProfile: Boolean
)
