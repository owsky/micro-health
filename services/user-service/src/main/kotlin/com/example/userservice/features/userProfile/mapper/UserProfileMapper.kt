package com.example.userservice.features.userProfile.mapper

import com.example.userservice.features.userProfile.dto.UpdateUserProfileRequest
import com.example.userservice.features.userProfile.dto.CreateUserProfileRequest
import com.example.userservice.features.userProfile.dto.UserProfileResponse
import com.example.userservice.features.userProfile.entity.UserProfileEntity
import org.springframework.stereotype.Component

@Component
class UserProfileMapper {

    fun toEntity(request: CreateUserProfileRequest, userName: String): UserProfileEntity {
        return UserProfileEntity(
            userName = userName,
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

    fun toResponse(entity: UserProfileEntity): UserProfileResponse =
        UserProfileResponse(
            userName = entity.userName,
            height = entity.height,
            weight = entity.weight,
            birthday = entity.birthday,
            gender = entity.gender
        )
}