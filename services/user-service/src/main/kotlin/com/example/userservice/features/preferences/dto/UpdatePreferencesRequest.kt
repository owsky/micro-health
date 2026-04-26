package com.example.userservice.features.preferences.dto

import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.profile.entity.UserProfileEntity

data class UpdatePreferencesRequest(
    val units: UnitsEnum,
    val emailNotificationsEnabled: Boolean,
    val pushNotificationsEnabled: Boolean,
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