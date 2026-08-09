USE FairPointDb;
GO

CREATE TABLE ReputationHistory
(
    ReputationHistoryId BIGINT IDENTITY(1,1) NOT NULL,

    UserId BIGINT NOT NULL,

    ComplaintId BIGINT NULL,

    PointsChange INT NOT NULL,

    ReasonCode NVARCHAR(100) NOT NULL,

    Description NVARCHAR(1000) NULL,

    CreatedByUserId BIGINT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ReputationHistory_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ReputationHistory
        PRIMARY KEY (ReputationHistoryId),

    CONSTRAINT FK_ReputationHistory_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_ReputationHistory_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_ReputationHistory_CreatedByUser
        FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(UserId)
);
GO