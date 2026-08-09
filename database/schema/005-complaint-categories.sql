USE FairPointDb;
GO

CREATE TABLE ComplaintCategories
(
    CategoryId INT IDENTITY(1,1) NOT NULL,

    CategoryCode NVARCHAR(50) NOT NULL,

    CategoryName NVARCHAR(100) NOT NULL,

    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_ComplaintCategories_IsActive DEFAULT (1),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintCategories_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ComplaintCategories
        PRIMARY KEY (CategoryId),

    CONSTRAINT UQ_ComplaintCategories_Code
        UNIQUE (CategoryCode),

    CONSTRAINT UQ_ComplaintCategories_Name
        UNIQUE (CategoryName)
);
GO