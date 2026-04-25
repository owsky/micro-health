package com.example.userservice.features.profile.dto

import com.example.userservice.features.profile.enums.GenderEnum
import jakarta.validation.constraints.*
import java.time.LocalDate

data class CreateUserProfileRequest(
    @field:Min(50) @field:Max(300) val height: Int,

    @field:Positive val weight: Float,

    @field:NotNull @field:Past val birthday: LocalDate,

    @field:NotNull val gender: GenderEnum
)
