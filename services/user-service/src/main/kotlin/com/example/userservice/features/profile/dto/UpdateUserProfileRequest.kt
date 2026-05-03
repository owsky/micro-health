package com.example.userservice.features.profile.dto

import com.example.userservice.features.profile.entity.UserProfileEntity
import com.example.userservice.features.profile.enums.GenderEnum
import com.fasterxml.jackson.annotation.JsonFormat
import com.fasterxml.jackson.annotation.JsonIgnore
import io.swagger.v3.oas.annotations.media.Schema
import jakarta.validation.constraints.AssertTrue
import jakarta.validation.constraints.Max
import jakarta.validation.constraints.Min
import jakarta.validation.constraints.Past
import java.time.LocalDate

@Schema(description = "Payload for updating a user profile")
data class UpdateUserProfileRequest(
    @field:Min(50) @field:Max(300) @field:Schema(
        description = "Height in centimeters",
        example = "175",
        minimum = "50",
        maximum = "300"
    ) val height: Int?,

    @field:Min(30) @field:Max(300) @field:Schema(
        description = "Weight in kilograms",
        example = "70.5",
        minimum = "30",
        maximum = "300"
    ) val weight: Float?,

    @field:Past @field:JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd") @field:Schema(
        description = "Birthday in ISO-8601 date format (yyyy-MM-dd)",
        type = "string",
        format = "date",
        pattern = "yyyy-MM-dd",
        example = "1990-01-31"
    ) val birthday: LocalDate?,

    @field:Schema(
        description = "Gender of the user", example = "MALE", allowableValues = ["MALE", "FEMALE", "OTHER"]
    ) val gender: GenderEnum?
) {
    @get:JsonIgnore
    @get:AssertTrue(message = "At least one profile field must be provided")
    val hasAtLeastOneField: Boolean
        get() = height != null || weight != null || birthday != null || gender != null
}

fun UserProfileEntity.applyUpdate(request: UpdateUserProfileRequest): UserProfileEntity {
    request.height?.let { this.height = it }
    request.weight?.let { this.weight = it }
    request.birthday?.let { this.birthday = it }
    request.gender?.let { this.gender = it }
    return this
}