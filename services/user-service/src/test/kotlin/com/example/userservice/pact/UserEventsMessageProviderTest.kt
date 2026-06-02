package com.example.userservice.pact

import au.com.dius.pact.provider.PactVerifyProvider
import au.com.dius.pact.provider.junit5.MessageTestTarget
import au.com.dius.pact.provider.junit5.PactVerificationContext
import au.com.dius.pact.provider.junit5.PactVerificationInvocationContextProvider
import au.com.dius.pact.provider.junitsupport.Provider
import au.com.dius.pact.provider.junitsupport.loader.PactFolder
import com.example.userservice.config.RabbitMQConfig
import com.example.userservice.features.profile.dto.UserProfileResponse
import com.example.userservice.features.profile.enums.GenderEnum
import com.example.userservice.features.profile.messaging.UserProfileEventPublisher
import io.github.cdimascio.dotenv.Dotenv
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.TestTemplate
import org.junit.jupiter.api.extension.ExtendWith
import tools.jackson.module.kotlin.jacksonObjectMapper
import org.mockito.ArgumentCaptor
import org.mockito.Mockito.*
import org.springframework.amqp.rabbit.core.RabbitTemplate
import java.time.LocalDate

@Provider("user-service")
@PactFolder("\${CONTRACTS_PATH:../../contracts/events}")
@ExtendWith(PactVerificationInvocationContextProvider::class)
class UserEventsMessageProviderTest {
    private val rabbitTemplate: RabbitTemplate = mock(RabbitTemplate::class.java)
    private val eventPublisher = UserProfileEventPublisher(rabbitTemplate)
    private val objectMapper = jacksonObjectMapper()

    @BeforeEach
    fun before(context: PactVerificationContext) {
        context.target = MessageTestTarget()
        reset(rabbitTemplate)
    }

    @TestTemplate
    fun pactVerificationTestTemplate(context: PactVerificationContext) {
        context.verifyInteraction()
    }

    @PactVerifyProvider("a user created event")
    fun verifyUserCreatedEvent(): String {
        val fakeProfile = UserProfileResponse(
            username = "john_doe",
            email = "john@example.com",
            height = 167,
            weight = 77f,
            birthday = LocalDate.of(1970, 1, 1),
            gender = GenderEnum.MALE
        )

        eventPublisher.publishUserCreated(fakeProfile)

        val payloadCaptor = ArgumentCaptor.forClass(Any::class.java)
        verify(rabbitTemplate).convertAndSend(
            eq(RabbitMQConfig.EXCHANGE), eq(RabbitMQConfig.USER_CREATED_ROUTING_KEY), payloadCaptor.capture()
        )

        return objectMapper.writeValueAsString(payloadCaptor.value)
    }

    @PactVerifyProvider("a user deleted event when a user account is removed")
    fun verifyUserDeletedEvent(): String {
        eventPublisher.publishUserDeleted("john_doe")

        val payloadCaptor = ArgumentCaptor.forClass(Any::class.java)
        verify(rabbitTemplate).convertAndSend(
            eq(RabbitMQConfig.EXCHANGE), eq(RabbitMQConfig.USER_DELETED_ROUTING_KEY), payloadCaptor.capture()
        )

        return objectMapper.writeValueAsString(payloadCaptor.value)
    }
}
