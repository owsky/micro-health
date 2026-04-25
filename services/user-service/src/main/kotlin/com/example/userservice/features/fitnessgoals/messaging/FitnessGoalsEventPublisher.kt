package com.example.userservice.features.fitnessgoals.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.config.RabbitMQConfig.Companion.FITNESS_GOALS_UPDATED_ROUTING_KEY
import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import org.slf4j.LoggerFactory
import org.springframework.amqp.rabbit.core.RabbitTemplate
import org.springframework.stereotype.Service

@Service
class FitnessGoalsEventPublisher(private val rabbitTemplate: RabbitTemplate) {
    private val log = LoggerFactory.getLogger(javaClass)

    fun publishFitnessGoalsUpdated(fitnessGoals: FitnessGoalsResponse) {
        log.info("Publishing $FITNESS_GOALS_UPDATED_ROUTING_KEY event")
        rabbitTemplate.convertAndSend(
            RabbitMQConfig.EXCHANGE, FITNESS_GOALS_UPDATED_ROUTING_KEY, fitnessGoals
        )
    }
}