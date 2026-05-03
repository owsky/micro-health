--liquibase formatted sql

--changeset system:006-alter-user-profile.sql
ALTER TABLE user_profile
    ALTER COLUMN username TYPE TEXT;

ALTER TABLE user_profile
    DROP CONSTRAINT user_profile_username_check;

ALTER TABLE user_preferences
    ALTER COLUMN username TYPE TEXT,
    ALTER COLUMN units TYPE TEXT;

ALTER TABLE fitness_goals
    ALTER COLUMN username TYPE TEXT;