package com.example.userservice.features.userProfile.entity

import com.example.userservice.features.userProfile.enums.GenderEnum
import jakarta.persistence.*
import java.time.LocalDate

@Entity
@Table(name = "user_profile")
class UserProfileEntity(
    @Id var userName: String,
    var height: Int = 0,
    var weight: Float = 0f,
    var birthday: LocalDate = LocalDate.EPOCH,
    @Enumerated(EnumType.STRING) var gender: GenderEnum = GenderEnum.MALE
)
