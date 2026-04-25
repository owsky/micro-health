package com.example.userservice.features.profile.repository

import com.example.userservice.features.profile.entity.UserProfileEntity
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.stereotype.Repository

@Repository
interface UserProfileRepository : JpaRepository<UserProfileEntity, String> {
    fun deleteByUsername(userName: String)
}