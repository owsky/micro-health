package com.example.userservice.features.profile.entity

import com.example.userservice.features.profile.enums.GenderEnum
import jakarta.persistence.*
import java.time.LocalDate

@Entity
@Table(name = "user_profile")
class UserProfileEntity(
    @Id @Column(nullable = false) var username: String,

    @Column(nullable = false) var email: String,

    @Column(nullable = false) var height: Int,

    @Column(nullable = false) var weight: Float,

    @Column(nullable = false) var birthday: LocalDate,

    @Column(nullable = false) @Enumerated(EnumType.STRING) var gender: GenderEnum,
)
