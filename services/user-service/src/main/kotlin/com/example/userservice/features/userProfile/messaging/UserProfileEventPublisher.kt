package com.example.userservice.features.userProfile.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.userProfile.dto.UserProfileResponse
import org.slf4j.LoggerFactory
import org.springframework.amqp.rabbit.core.RabbitTemplate
import org.springframework.stereotype.Service

@Service
class UserProfileEventPublisher(private val rabbitTemplate: RabbitTemplate) {

    private val log = LoggerFactory.getLogger(javaClass)

    fun publishUserCreated(userProfile: UserProfileResponse) {
        log.info("Publishing user.created event for user: ${userProfile.userName}")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE,
            RabbitMQConfig.USER_CREATED_ROUTING_KEY,
            userProfile
        )
    }
}

