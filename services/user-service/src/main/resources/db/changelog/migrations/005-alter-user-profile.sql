--liquibase formatted sql

--changeset system:005-alter-user-profile.sql
ALTER TABLE user_profile
    ALTER COLUMN username TYPE VARCHAR(20);

ALTER TABLE user_profile
    ADD CHECK (char_length(username) BETWEEN 5 AND 20 AND username ~ '^[A-Za-z0-9]+$');