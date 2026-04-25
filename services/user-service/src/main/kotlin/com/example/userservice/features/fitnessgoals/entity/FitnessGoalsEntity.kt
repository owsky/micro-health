package com.example.userservice.features.fitnessgoals.entity

import com.example.userservice.features.profile.entity.UserProfileEntity
import jakarta.persistence.Column
import jakarta.persistence.Entity
import jakarta.persistence.Id
import jakarta.persistence.JoinColumn
import jakarta.persistence.MapsId
import jakarta.persistence.OneToOne
import jakarta.persistence.Table

@Entity
@Table(name = "fitness_goals")
class FitnessGoalsEntity(
    @Id var username: String,

    var targetWeight: Float?,

    var dailySteps: Int?,

    var burnedCalories: Int?,

    @MapsId @OneToOne @JoinColumn(
        name = "username", referencedColumnName = "username"
    ) var userProfile: UserProfileEntity
)