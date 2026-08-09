USE FairPointDb;
GO

CREATE TABLE Notifications
(
    NotificationId BIGINT IDENTITY(1,1) NOT NULL,

    UserId BIGINT NOT NULL,

    NotificationType NVARCHAR(100) NOT NULL,

    Title NVARCHAR(200) NOT NULL,

    Message NVARCHAR(2000) NOT NULL,

    RelatedEntityType NVARCHAR(100) NULL,

    RelatedEntityId BIGINT NULL,

    IsRead BIT NOT NULL
        CONSTRAINT DF_Notifications_IsRead
        DEFAULT (0),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Notifications_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    ReadAt DATETIME2 NULL,

    CONSTRAINT PK_Notifications
        PRIMARY KEY (NotificationId),

    CONSTRAINT FK_Notifications_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);
GO