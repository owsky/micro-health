package com.example.userservice.features.profile.dto

import com.fasterxml.jackson.annotation.JsonFormat
import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.shared.UserInfo
import io.swagger.v3.oas.annotations.media.Schema
import jakarta.validation.constraints.*
import java.time.LocalDate

@Schema(description = "Payload for creating a user profile")
data class CreateUserProfileRequest(
    @field:Min(50)
    @field:Max(300)
    @field:Schema(description = "Height in centimeters", example = "175", minimum = "50", maximum = "300")
    val height: Int,

    @field:Positive
    @field:Schema(description = "Weight in kilograms", example = "70.5", minimum = "0")
    val weight: Float,

    @field:NotNull
    @field:Past
    @field:JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    @field:Schema(
        description = "Birthday in ISO-8601 date format (yyyy-MM-dd)",
        type = "string",
        format = "date",
        pattern = "yyyy-MM-dd",
        example = "1990-01-31"
    )
    val birthday: LocalDate,

    @field:NotNull
    @field:Schema(
        description = "Gender of the user",
        example = "MALE",
        allowableValues = ["MALE", "FEMALE", "OTHER"]
    )
    val gender: GenderEnum
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
    username = userInfo.username,
    email = userInfo.email,
    height = request.height,
    weight = request.weight,
    birthday = request.birthday,
    gender = request.gender
)