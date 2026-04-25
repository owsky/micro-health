--liquibase formatted sql

--changeset system:002-alter-user-profile.sql
ALTER TABLE user_profile
    ADD COLUMN email VARCHAR(255) NOT NULL;

ALTER TABLE user_profile
    RENAME COLUMN user_name TO username;
