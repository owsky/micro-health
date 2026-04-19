package com.example.userservice.config

import org.springframework.amqp.core.*
import org.springframework.amqp.rabbit.connection.ConnectionFactory
import org.springframework.amqp.rabbit.core.RabbitTemplate
import org.springframework.amqp.support.converter.JacksonJsonMessageConverter
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration

@Configuration
class RabbitMQConfig {

    companion object {
        const val EXCHANGE = "user.exchange"
        const val USER_CREATED_QUEUE = "user.created.queue"
        const val USER_CREATED_ROUTING_KEY = "user.created"
    }

    @Bean
    fun userExchange(): TopicExchange = TopicExchange(EXCHANGE, true, false)

    @Bean
    fun userCreatedQueue(): Queue = QueueBuilder.durable(USER_CREATED_QUEUE).ttl(3600000).build()

    @Bean
    fun userCreatedBinding(userCreatedQueue: Queue, userExchange: TopicExchange): Binding =
        BindingBuilder.bind(userCreatedQueue).to(userExchange).with(USER_CREATED_ROUTING_KEY)

    @Bean
    fun messageConverter(): JacksonJsonMessageConverter = JacksonJsonMessageConverter()

    @Bean
    fun rabbitTemplate(
        connectionFactory: ConnectionFactory,
        messageConverter: JacksonJsonMessageConverter
    ): RabbitTemplate =
        RabbitTemplate(connectionFactory).apply {
            this.messageConverter = messageConverter
            setBeforePublishPostProcessors(
                { message: Message ->
                    message.messageProperties.deliveryMode = MessageDeliveryMode.PERSISTENT
                    message
                }
            )
        }
}
