package com.example.userservice.features.preferences.dto

import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.profile.entity.UserProfileEntity
import io.swagger.v3.oas.annotations.media.Schema

@Schema(description = "Payload for updating user preferences")
data class UpdatePreferencesRequest(
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

fun PreferencesEntity(request: UpdatePreferencesRequest, userProfile: UserProfileEntity): PreferencesEntity =
    PreferencesEntity(
        username = userProfile.username,
        units = request.units,
        emailNotificationsEnabled = request.emailNotificationsEnabled,
        pushNotificationsEnabled = request.pushNotificationsEnabled,
        privateProfile = request.privateProfile,
        userProfile = userProfile
    )

fun PreferencesEntity.applyUpdate(request: UpdatePreferencesRequest): PreferencesEntity {
    this.units = request.units
    this.emailNotificationsEnabled = request.emailNotificationsEnabled
    this.pushNotificationsEnabled = request.pushNotificationsEnabled
    this.privateProfile = request.privateProfile
    return this
}

fun PreferencesEntity.toResponse(): PreferencesResponse = PreferencesResponse(
    units = this.units,
    emailNotificationsEnabled = this.emailNotificationsEnabled,
    pushNotificationsEnabled = this.pushNotificationsEnabled,
    privateProfile = this.privateProfile
)