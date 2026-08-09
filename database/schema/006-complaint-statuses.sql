USE FairPointDb;
GO

CREATE TABLE ComplaintStatuses
(
    StatusId INT IDENTITY(1,1) NOT NULL,

    StatusCode NVARCHAR(50) NOT NULL,

    StatusName NVARCHAR(100) NOT NULL,

    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_ComplaintStatuses_IsActive DEFAULT (1),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintStatuses_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ComplaintStatuses
        PRIMARY KEY (StatusId),

    CONSTRAINT UQ_ComplaintStatuses_Code
        UNIQUE (StatusCode),

    CONSTRAINT UQ_ComplaintStatuses_Name
        UNIQUE (StatusName)
);
GO