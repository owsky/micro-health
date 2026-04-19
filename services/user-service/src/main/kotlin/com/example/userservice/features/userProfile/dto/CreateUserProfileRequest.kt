package com.example.userservice.features.userProfile.dto

import com.example.userservice.features.userProfile.enums.GenderEnum
import jakarta.validation.constraints.Max
import jakarta.validation.constraints.Min
import jakarta.validation.constraints.NotNull
import jakarta.validation.constraints.Past
import jakarta.validation.constraints.Positive
import java.time.LocalDate

data class CreateUserProfileRequest(
    @field:Min(50) @field:Max(300) val height: Int,

    @field:Positive val weight: Float,

    @field:NotNull @field:Past val birthday: LocalDate,

    @field:NotNull val gender: GenderEnum
)
