package com.example.userservice.features.profile.dto

import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.shared.UserInfo
import jakarta.validation.constraints.*
import java.time.LocalDate

data class CreateUserProfileRequest(
    @field:Min(50) @field:Max(300) val height: Int,

    @field:Positive val weight: Float,

    @field:NotNull @field:Past val birthday: LocalDate,

    @field:NotNull val gender: GenderEnum
)

fun UserProfileEntity.toResponse(): UserProfileResponse = UserProfileResponse(
    username = this.username,
    email = this.email,
    height = this.height,
    weight = this.weight,
    birthday = this.birthday,
    gender = this.gender
)

fun UserProfileEntity(request: CreateUserProfileRequest, userInfo: UserInfo): UserProfileEntity = UserProfileEntity(
    username = userInfo.preferredUsername,
    email = userInfo.email,
    height = request.height,
    weight = request.weight,
    birthday = request.birthday,
    gender = request.gender
)