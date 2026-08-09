USE FairPointDb;
GO

CREATE TABLE ComplaintPartyTypes
(
    PartyTypeId INT IDENTITY(1,1) NOT NULL,

    PartyTypeCode NVARCHAR(50) NOT NULL,

    PartyTypeName NVARCHAR(100) NOT NULL,

    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_ComplaintPartyTypes_IsActive DEFAULT (1),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintPartyTypes_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ComplaintPartyTypes
        PRIMARY KEY (PartyTypeId),

    CONSTRAINT UQ_ComplaintPartyTypes_Code
        UNIQUE (PartyTypeCode),

    CONSTRAINT UQ_ComplaintPartyTypes_Name
        UNIQUE (PartyTypeName)
);
GO