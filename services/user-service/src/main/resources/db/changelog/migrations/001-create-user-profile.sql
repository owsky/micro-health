--liquibase formatted sql

--changeset system:001-create-user-profile
CREATE TABLE user_profile
(
    user_name VARCHAR(255) NOT NULL,
    height    INT          NOT NULL,
    weight    FLOAT        NOT NULL,
    birthday  DATE         NOT NULL,
    gender    VARCHAR(10)  NOT NULL,
    CONSTRAINT pk_user_profile PRIMARY KEY (user_name)
);
