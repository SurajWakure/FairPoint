USE FairPointDb;
GO

CREATE TABLE Complaints
(
    ComplaintId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintNumber NVARCHAR(30) NOT NULL,

    CreatedByUserId BIGINT NOT NULL,

    CategoryId INT NOT NULL,

    StatusId INT NOT NULL,

    SeverityId INT NOT NULL,

    Title NVARCHAR(200) NOT NULL,

    Description NVARCHAR(MAX) NOT NULL,

    VisibilityCode NVARCHAR(30) NOT NULL
        CONSTRAINT DF_Complaints_VisibilityCode
        DEFAULT ('PUBLIC'),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Complaints_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    UpdatedAt DATETIME2 NULL,

    ResolvedAt DATETIME2 NULL,

    ClosedAt DATETIME2 NULL,

    CONSTRAINT PK_Complaints
        PRIMARY KEY (ComplaintId),

    CONSTRAINT UQ_Complaints_ComplaintNumber
        UNIQUE (ComplaintNumber),

    CONSTRAINT FK_Complaints_CreatedByUser
        FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Complaints_Category
        FOREIGN KEY (CategoryId)
        REFERENCES ComplaintCategories(CategoryId),

    CONSTRAINT FK_Complaints_Status
        FOREIGN KEY (StatusId)
        REFERENCES ComplaintStatuses(StatusId),

    CONSTRAINT FK_Complaints_Severity
        FOREIGN KEY (SeverityId)
        REFERENCES ComplaintSeverities(SeverityId),

    CONSTRAINT CK_Complaints_VisibilityCode
        CHECK (VisibilityCode IN ('PUBLIC', 'PRIVATE', 'RESTRICTED'))
);
GO