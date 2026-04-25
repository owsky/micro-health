--liquibase formatted sql

--changeset system:003-create-user-preferences.sql
CREATE TABLE user_preferences
(
    username                    VARCHAR(255) NOT NULL,
    units                       VARCHAR(10)  NOT NULL,
    email_notifications_enabled BOOLEAN      NOT NULL DEFAULT FALSE,
    push_notifications_enabled  BOOLEAN      NOT NULL DEFAULT FALSE,
    private_profile             BOOLEAN      NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_user_preferences PRIMARY KEY (username),
    CONSTRAINT fk_user_preferences FOREIGN KEY (username) REFERENCES user_profile (username)
);
