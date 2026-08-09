USE FairPointDb;
GO

CREATE TABLE ComplaintResponses
(
    ResponseId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintId BIGINT NOT NULL,

    UserId BIGINT NOT NULL,

    ResponseText NVARCHAR(MAX) NOT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintResponses_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    UpdatedAt DATETIME2 NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_ComplaintResponses_IsDeleted
        DEFAULT (0),

    DeletedAt DATETIME2 NULL,

    DeletedByUserId BIGINT NULL,

    CONSTRAINT PK_ComplaintResponses
        PRIMARY KEY (ResponseId),

    CONSTRAINT FK_ComplaintResponses_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_ComplaintResponses_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_ComplaintResponses_DeletedByUser
        FOREIGN KEY (DeletedByUserId)
        REFERENCES Users(UserId)
);
GO