USE FairPointDb;
GO

CREATE TABLE Appeals
(
    AppealId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintId BIGINT NOT NULL,

    SubmittedByUserId BIGINT NOT NULL,

    Reason NVARCHAR(3000) NOT NULL,

    StatusCode NVARCHAR(50) NOT NULL
        CONSTRAINT DF_Appeals_StatusCode
        DEFAULT ('SUBMITTED'),

    ReviewedByUserId BIGINT NULL,

    DecisionReason NVARCHAR(3000) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Appeals_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    ReviewedAt DATETIME2 NULL,

    CONSTRAINT PK_Appeals
        PRIMARY KEY (AppealId),

    CONSTRAINT FK_Appeals_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_Appeals_SubmittedByUser
        FOREIGN KEY (SubmittedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Appeals_ReviewedByUser
        FOREIGN KEY (ReviewedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT CK_Appeals_StatusCode
        CHECK
        (
            StatusCode IN
            (
                'SUBMITTED',
                'UNDER_REVIEW',
                'APPROVED',
                'REJECTED'
            )
        )
);
GO