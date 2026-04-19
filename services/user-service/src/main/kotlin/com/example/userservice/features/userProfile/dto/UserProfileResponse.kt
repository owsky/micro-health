package com.example.userservice.features.userProfile.dto

import com.example.userservice.features.userProfile.enums.GenderEnum
import io.swagger.v3.oas.annotations.media.Schema
import java.time.LocalDate

@Schema(description = "User profile data")
data class UserProfileResponse(
    @Schema(description = "Unique username", example = "john_doe") val userName: String,

    @Schema(description = "Height in centimeters", example = "175") val height: Int,

    @Schema(description = "Weight in kilograms", example = "70.5") val weight: Float,

    @Schema(description = "Birthday in dd-mm-yyyy format", example = "01-01-1970") val birthday: LocalDate,

    @Schema(
        description = "Gender of the user",
        example = "MALE",
        allowableValues = ["MALE", "FEMALE", "OTHER"]
    ) val gender: GenderEnum
)
