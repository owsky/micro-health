package com.example.userservice.features.profile.messaging

import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.enums.GenderEnum
import io.mockk.impl.annotations.InjectMockKs
import io.mockk.impl.annotations.MockK
import io.mockk.junit5.MockKExtension
import io.mockk.justRun
import io.mockk.verify
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.amqp.rabbit.core.RabbitTemplate
import java.time.LocalDate

@ExtendWith(MockKExtension::class)
class UserProfileEventPublisherTest {

    @MockK
    private lateinit var rabbitTemplate: RabbitTemplate

    @InjectMockKs
    private lateinit var sut: UserProfileEventPublisher

    // -------------------------------------------------------------------------
    // Test fixtures
    // -------------------------------------------------------------------------

    private val userProfileResponse = UserProfileResponse(
        username = "alice",
        email = "alice@example.com",
        height = 170,
        weight = 65f,
        birthday = LocalDate.of(1990, 1, 1),
        gender = GenderEnum.FEMALE
    )

    // -------------------------------------------------------------------------
    // publishUserCreated
    // -------------------------------------------------------------------------
    @Nested
    inner class PublishUserCreated {

        @Test
        fun `publishes to the correct exchange and routing key`() {
            // arrange
            justRun {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_CREATED_ROUTING_KEY,
                    userProfileResponse
                )
            }

            // act
            sut.publishUserCreated(userProfileResponse)

            // assert
            verify(exactly = 1) {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_CREATED_ROUTING_KEY,
                    userProfileResponse
                )
            }
        }

        @Test
        fun `passes the user profile payload unchanged`() {
            // arrange
            justRun { rabbitTemplate.convertAndSend(any<String>(), any<String>(), any<Any>()) }

            // act
            sut.publishUserCreated(userProfileResponse)

            // assert
            verify {
                rabbitTemplate.convertAndSend(any<String>(), any<String>(), userProfileResponse)
            }
        }
    }

    // -------------------------------------------------------------------------
    // publishUserUpdated
    // -------------------------------------------------------------------------
    @Nested
    inner class PublishUserUpdated {

        @Test
        fun `publishes to the correct exchange and routing key`() {
            // arrange
            justRun {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_UPDATED_ROUTING_KEY,
                    userProfileResponse
                )
            }

            // act
            sut.publishUserUpdated(userProfileResponse)

            // assert
            verify(exactly = 1) {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_UPDATED_ROUTING_KEY,
                    userProfileResponse
                )
            }
        }

        @Test
        fun `passes the user profile payload unchanged`() {
            // arrange
            justRun { rabbitTemplate.convertAndSend(any<String>(), any<String>(), any<Any>()) }

            // act
            sut.publishUserUpdated(userProfileResponse)

            // assert
            verify {
                rabbitTemplate.convertAndSend(any<String>(), any<String>(), userProfileResponse)
            }
        }
    }

    // -------------------------------------------------------------------------
    // publishUserDeleted
    // -------------------------------------------------------------------------
    @Nested
    inner class PublishUserDeleted {

        @Test
        fun `publishes to the correct exchange and routing key`() {
            // arrange
            justRun {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_DELETED_ROUTING_KEY,
                    userProfileResponse.username
                )
            }

            // act
            sut.publishUserDeleted(userProfileResponse.username)

            // assert
            verify(exactly = 1) {
                rabbitTemplate.convertAndSend(
                    RabbitMQConfig.EXCHANGE,
                    RabbitMQConfig.USER_DELETED_ROUTING_KEY,
                    userProfileResponse.username
                )
            }
        }

        @Test
        fun `passes the username payload unchanged`() {
            // arrange
            justRun { rabbitTemplate.convertAndSend(any<String>(), any<String>(), any<Any>()) }

            // act
            sut.publishUserDeleted(userProfileResponse.username)

            // assert
            verify {
                rabbitTemplate.convertAndSend(any<String>(), any<String>(), userProfileResponse.username)
            }
        }
    }
}

