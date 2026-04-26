package com.example.userservice.features.preferences.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.preferences.dto.PreferencesResponse
import com.example.userservice.features.preferences.enums.UnitsEnum
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import io.mockk.verify
import io.mockk.justRun
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.amqp.rabbit.core.RabbitTemplate

@ExtendWith(MockKExtension::class)
class PreferencesEventPublisherTest {

    @MockK
    private lateinit var rabbitTemplate: RabbitTemplate

    @InjectMockKs
    private lateinit var sut: PreferencesEventPublisher

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val preferencesResponse = PreferencesResponse(
        units = UnitsEnum.METRIC,
        emailNotificationsEnabled = true,
        pushNotificationsEnabled = true,
        privateProfile = false
    )

    // -------------------------------------------------------------------------
    // publishPreferencesUpdated
    // -------------------------------------------------------------------------

    @Test
    fun `publishes to the correct exchange and routing key`() {
        // arrange
        justRun {
            rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE,
                RabbitMQConfig.PREFERENCES_UPDATED_ROUTING_KEY,
                preferencesResponse
            )
        }

        // act
        sut.publishPreferencesUpdated(preferencesResponse)

        // assert
        verify(exactly = 1) {
            rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE,
                RabbitMQConfig.PREFERENCES_UPDATED_ROUTING_KEY,
                preferencesResponse
            )
        }
    }

    @Test
    fun `passes the preferences payload unchanged`() {
        // arrange
        justRun { rabbitTemplate.convertAndSend(any<String>(), any<String>(), any<Any>()) }

        // act
        sut.publishPreferencesUpdated(preferencesResponse)

        // assert
        verify {
            rabbitTemplate.convertAndSend(any<String>(), any<String>(), preferencesResponse)
        }
    }
}
