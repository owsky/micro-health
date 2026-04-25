package com.example.userservice.features.preferences.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.config.RabbitMQConfig.Companion.PREFERENCES_UPDATED_ROUTING_KEY
import com.example.userservice.features.preferences.dto.PreferencesResponse
import org.slf4j.LoggerFactory
import org.springframework.amqp.rabbit.core.RabbitTemplate
import org.springframework.stereotype.Service

@Service
class PreferencesEventPublisher(private val rabbitTemplate: RabbitTemplate) {
    private val log = LoggerFactory.getLogger(javaClass)

    fun publishPreferencesUpdated(preferences: PreferencesResponse) {
        log.info("Publishing $PREFERENCES_UPDATED_ROUTING_KEY event")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE, RabbitMQConfig.PREFERENCES_UPDATED_ROUTING_KEY, preferences
        )
    }
}