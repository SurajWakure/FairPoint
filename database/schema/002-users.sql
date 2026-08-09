USE FairPointDb;
GO

CREATE TABLE Users
(
    UserId BIGINT IDENTITY(1,1) NOT NULL,

    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NULL,

    Email NVARCHAR(320) NOT NULL,
    PhoneNumber NVARCHAR(30) NULL,

    PasswordHash NVARCHAR(500) NOT NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT (1),

    IsVerified BIT NOT NULL
        CONSTRAINT DF_Users_IsVerified DEFAULT (0),

    ReputationScore INT NOT NULL
        CONSTRAINT DF_Users_ReputationScore DEFAULT (0),

    LastLoginAt DATETIME2 NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),

    UpdatedAt DATETIME2 NULL,

    CONSTRAINT PK_Users
        PRIMARY KEY (UserId),

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email)
);
GO