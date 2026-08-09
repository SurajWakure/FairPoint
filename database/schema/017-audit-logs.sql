USE FairPointDb;
GO

CREATE TABLE AuditLogs
(
    AuditLogId BIGINT IDENTITY(1,1) NOT NULL,

    UserId BIGINT NULL,

    ActionType NVARCHAR(100) NOT NULL,

    EntityType NVARCHAR(100) NULL,

    EntityId BIGINT NULL,

    OldValues NVARCHAR(MAX) NULL,

    NewValues NVARCHAR(MAX) NULL,

    IpAddress NVARCHAR(64) NULL,

    UserAgent NVARCHAR(1000) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_AuditLogs_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_AuditLogs
        PRIMARY KEY (AuditLogId),

    CONSTRAINT FK_AuditLogs_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);
GO