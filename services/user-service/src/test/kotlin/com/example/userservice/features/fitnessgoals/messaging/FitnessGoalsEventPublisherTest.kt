package com.example.userservice.features.fitnessgoals.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.fitnessgoals.dto.FitnessGoalsResponse
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import io.mockk.justRun
import io.mockk.verify
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.amqp.rabbit.core.RabbitTemplate
import kotlin.test.Test

@ExtendWith(MockKExtension::class)
class FitnessGoalsEventPublisherTest {

    @MockK
    private lateinit var rabbitTemplate: RabbitTemplate

    @InjectMockKs
    private lateinit var sut: FitnessGoalsEventPublisher

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val fitnessGoalsResponse = FitnessGoalsResponse(
        targetWeight = 50f, dailySteps = 10000, burnedCalories = 1200
    )

    // -------------------------------------------------------------------------
    // publishFitnessGoalsUpdated
    // -------------------------------------------------------------------------

    @Test
    fun `publishe to the correct exchange and routing key`() {
        // arrange
        justRun {
            rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE, RabbitMQConfig.FITNESS_GOALS_UPDATED_ROUTING_KEY, fitnessGoalsResponse
            )
        }

        // act
        sut.publishFitnessGoalsUpdated(fitnessGoalsResponse)

        // assert
        verify(exactly = 1) {
            rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE, RabbitMQConfig.FITNESS_GOALS_UPDATED_ROUTING_KEY, fitnessGoalsResponse
            )
        }
    }

    @Test
    fun `passes the fitness goals payload unchanged`() {
        // arrange
        justRun { rabbitTemplate.convertAndSend(any<String>(), any<String>(), any<Any>()) }

        // act
        sut.publishFitnessGoalsUpdated(fitnessGoalsResponse)

        // assert
        verify {
            rabbitTemplate.convertAndSend(any<String>(), any<String>(), fitnessGoalsResponse)
        }
    }
}