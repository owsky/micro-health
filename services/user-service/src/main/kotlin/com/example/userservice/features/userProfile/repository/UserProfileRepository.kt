package com.example.userservice.features.userProfile.repository

import com.example.userservice.features.userProfile.entity.UserProfileEntity
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.stereotype.Repository

@Repository
interface UserProfileRepository : JpaRepository<UserProfileEntity, Long> {
    fun findByUserName(userName: String): UserProfileEntity?
    fun deleteByUserName(userName: String)
}