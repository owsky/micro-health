package com.example.userservice.features.fitnessgoals.repository

import com.example.userservice.features.fitnessgoals.entity.FitnessGoalsEntity
import org.springframework.data.jpa.repository.JpaRepository

interface FitnessGoalsRepository : JpaRepository<FitnessGoalsEntity, String> {}