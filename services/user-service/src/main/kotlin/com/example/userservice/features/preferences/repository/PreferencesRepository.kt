package com.example.userservice.features.preferences.repository

import com.example.userservice.features.preferences.entity.PreferencesEntity
import org.springframework.data.jpa.repository.JpaRepository

interface PreferencesRepository : JpaRepository<PreferencesEntity, String> {

}