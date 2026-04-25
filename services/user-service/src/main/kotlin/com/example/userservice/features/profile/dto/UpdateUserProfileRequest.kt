package com.example.userservice.features.profile.dto

import com.example.userservice.features.profile.enums.GenderEnum
import com.fasterxml.jackson.annotation.JsonIgnore
import jakarta.validation.constraints.AssertTrue
import jakarta.validation.constraints.Max
import jakarta.validation.constraints.Min
import jakarta.validation.constraints.Past
import jakarta.validation.constraints.Positive
import java.time.LocalDate

data class UpdateUserProfileRequest(
    @field:Min(50) @field:Max(300) val height: Int?,

    @field:Positive val weight: Float?,

    @field:Past val birthday: LocalDate?,

    val gender: GenderEnum?
) {
    @get:JsonIgnore
    @get:AssertTrue(message = "At least one profile field must be provided")
    val hasAtLeastOneField: Boolean
        get() = height != null || weight != null || birthday != null || gender != null
}
