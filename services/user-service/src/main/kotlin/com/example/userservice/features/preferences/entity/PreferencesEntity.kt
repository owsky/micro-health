package com.example.userservice.features.preferences.entity

import com.example.userservice.features.preferences.enums.UnitsEnum
import com.example.userservice.features.profile.entity.UserProfileEntity
import jakarta.persistence.*

@Entity
@Table(name = "user_preferences")
class PreferencesEntity(
    @Id var username: String,

    @Column(nullable = false) @Enumerated(EnumType.STRING) var units: UnitsEnum,

    @Column(nullable = false) var emailNotificationsEnabled: Boolean,

    @Column(nullable = false) var pushNotificationsEnabled: Boolean,

    @Column(nullable = false) var privateProfile: Boolean,

    @MapsId @OneToOne @JoinColumn(
        name = "username",
        referencedColumnName = "username"
    ) var userProfile: UserProfileEntity
)