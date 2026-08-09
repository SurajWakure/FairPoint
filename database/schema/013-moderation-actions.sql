USE FairPointDb;
GO

CREATE TABLE ModerationActions
(
    ModerationActionId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintId BIGINT NOT NULL,

    ModeratorUserId BIGINT NOT NULL,

    ActionType NVARCHAR(50) NOT NULL,

    Reason NVARCHAR(2000) NOT NULL,

    PreviousStatusId INT NULL,

    NewStatusId INT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ModerationActions_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ModerationActions
        PRIMARY KEY (ModerationActionId),

    CONSTRAINT FK_ModerationActions_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_ModerationActions_Moderator
        FOREIGN KEY (ModeratorUserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_ModerationActions_PreviousStatus
        FOREIGN KEY (PreviousStatusId)
        REFERENCES ComplaintStatuses(StatusId),

    CONSTRAINT FK_ModerationActions_NewStatus
        FOREIGN KEY (NewStatusId)
        REFERENCES ComplaintStatuses(StatusId)
);
GO