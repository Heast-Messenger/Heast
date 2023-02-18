CREATE SCHEMA IF NOT EXISTS heast_auth;

CREATE TABLE IF NOT EXISTS heast_auth.accounts
(
    Id       INT          NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Name     VARCHAR(255) NOT NULL,
    Email    VARCHAR(255) NOT NULL,
    Password VARCHAR(128) NOT NULL,
    UNIQUE (Email, Name)
);

CREATE TABLE IF NOT EXISTS heast_auth.sessions
(
    Id        INT          NOT NULL PRIMARY KEY AUTO_INCREMENT,
    AccountId INT          NOT NULL,
    Token     VARCHAR(255) NOT NULL,
    CreatedAt DATETIME     NOT NULL,
    ExpiresAt DATETIME     NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES heast_auth.accounts (Id)
);

CREATE TABLE IF NOT EXISTS heast_auth.servers
(
    Id     INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Ip     INT UNSIGNED NOT NULL,
    Status ENUM('default', 'warn', 'banned') NOT NULL DEFAULT 'default',
    UNIQUE (Ip)
);