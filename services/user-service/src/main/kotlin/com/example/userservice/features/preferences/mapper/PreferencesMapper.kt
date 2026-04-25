package com.example.userservice.features.preferences.mapper

import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.dto.UpdatePreferencesRequest
import com.example.userservice.features.preferences.entity.PreferencesEntity
import com.example.userservice.features.profile.entity.UserProfileEntity
import org.springframework.stereotype.Component

@Component
class PreferencesMapper {

    fun toEntity(request: UpdatePreferencesRequest, userProfile: UserProfileEntity): PreferencesEntity =
        PreferencesEntity(
            username = userProfile.username,
            units = request.units,
            emailNotificationsEnabled = request.emailNotificationsEnabled,
            pushNotificationsEnabled = request.pushNotificationsEnabled,
            privateProfile = request.privateProfile,
            userProfile = userProfile
        )

    fun toResponse(entity: PreferencesEntity): PreferencesResponse = PreferencesResponse(
        units = entity.units,
        emailNotificationsEnabled = entity.emailNotificationsEnabled,
        pushNotificationsEnabled = entity.pushNotificationsEnabled,
        privateProfile = entity.privateProfile
    )
}