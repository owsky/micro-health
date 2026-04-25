package com.example.userservice.features.profile.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.profile.dto.UserProfileResponse
import org.slf4j.LoggerFactory
import org.springframework.amqp.rabbit.core.RabbitTemplate
import org.springframework.stereotype.Service

@Service
class UserProfileEventPublisher(private val rabbitTemplate: RabbitTemplate) {

    private val log = LoggerFactory.getLogger(javaClass)

    fun publishUserCreated(userProfile: UserProfileResponse) {
        log.info("Publishing user.created event for user: ${userProfile.username}")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE, RabbitMQConfig.USER_CREATED_ROUTING_KEY, userProfile
        )
    }

    fun publishUserUpdated(userProfile: UserProfileResponse) {
        log.info("Publishing user.updated event for user: ${userProfile.username}")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE, RabbitMQConfig.USER_UPDATED_ROUTING_KEY, userProfile
        )
    }

    fun publishUserDeleted(userName: String) {
        log.info("Publishing user.deleted event for user: $userName")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE, RabbitMQConfig.USER_DELETED_ROUTING_KEY, userName
        )
    }
}
