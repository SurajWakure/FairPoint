USE FairPointDb;
GO

CREATE TABLE ComplaintSeverities
(
    SeverityId INT IDENTITY(1,1) NOT NULL,

    SeverityCode NVARCHAR(50) NOT NULL,

    SeverityName NVARCHAR(100) NOT NULL,

    Weight INT NOT NULL,

    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_ComplaintSeverities_IsActive DEFAULT (1),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintSeverities_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ComplaintSeverities
        PRIMARY KEY (SeverityId),

    CONSTRAINT UQ_ComplaintSeverities_Code
        UNIQUE (SeverityCode),

    CONSTRAINT UQ_ComplaintSeverities_Name
        UNIQUE (SeverityName),

    CONSTRAINT CK_ComplaintSeverities_Weight
        CHECK (Weight BETWEEN 1 AND 4)
);
GO