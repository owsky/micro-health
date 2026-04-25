package com.example.userservice.features.profile.mapper

import com.example.userservice.features.profile.dto.UpdateUserProfileRequest
import com.example.userservice.features.profile.dto.CreateUserProfileRequest
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.shared.UserInfo
import org.springframework.stereotype.Component

@Component
class UserProfileMapper {

    fun toEntity(request: CreateUserProfileRequest, userInfo: UserInfo): UserProfileEntity {
        return UserProfileEntity(
            username = userInfo.preferredUsername,
            email = userInfo.email,
            height = request.height,
            weight = request.weight,
            birthday = request.birthday,
            gender = request.gender
        )
    }

    fun applyUpdate(request: UpdateUserProfileRequest, entity: UserProfileEntity): UserProfileEntity {
        request.height?.let { entity.height = it }
        request.weight?.let { entity.weight = it }
        request.birthday?.let { entity.birthday = it }
        request.gender?.let { entity.gender = it }
        return entity
    }

    fun toResponse(entity: UserProfileEntity): UserProfileResponse = UserProfileResponse(
        username = entity.username,
        email = entity.email,
        height = entity.height,
        weight = entity.weight,
        birthday = entity.birthday,
        gender = entity.gender
    )
}