--liquibase formatted sql

--changeset system:004-create-fitness-goals.sql
CREATE TABLE fitness_goals
(
    username        VARCHAR(255) NOT NULL,
    target_weight   FLOAT,
    daily_steps     INT,
    burned_calories INT,
    CONSTRAINT pk_fitness_goals PRIMARY KEY (username),
    CONSTRAINT fk_fitness_goals FOREIGN KEY (username) REFERENCES user_profile (username)
)