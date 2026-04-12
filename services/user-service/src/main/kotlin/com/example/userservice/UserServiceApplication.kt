package com.example.userservice

import org.springframework.boot.CommandLineRunner
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.context.annotation.Bean
import org.springframework.core.env.Environment

@SpringBootApplication
class UserServiceApplication {
    @Bean
    fun commandLineRunner(environment: Environment) = CommandLineRunner {
        val port = environment.getProperty("server.port") ?: "8080"
        val address = environment.getProperty("server.address") ?: "localhost"
        println("\n========================================")
        println("Application started successfully!")
        println("Listening on: http://$address:$port")
        println("========================================\n")
    }
}

fun main(args: Array<String>) {
    runApplication<UserServiceApplication>(*args)
}
