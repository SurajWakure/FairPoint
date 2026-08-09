USE FairPointDb;
GO

/* ============================================================
   1. Make ComplaintParties.UserId nullable
   ============================================================ */

ALTER TABLE dbo.ComplaintParties
ALTER COLUMN UserId BIGINT NULL;
GO


/* ============================================================
   2. Add IsIdentified
   ============================================================ */

ALTER TABLE dbo.ComplaintParties
ADD IsIdentified BIT NOT NULL
    CONSTRAINT DF_ComplaintParties_IsIdentified DEFAULT 0;
GO


/* ============================================================
   3. Add UpdatedAt
   ============================================================ */

ALTER TABLE dbo.ComplaintParties
ADD UpdatedAt DATETIME2 NULL;
GO


/* ============================================================
   4. Create ComplaintPartyDetails
   ============================================================ */

CREATE TABLE dbo.ComplaintPartyDetails
(
    ComplaintPartyDetailId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintPartyId BIGINT NOT NULL,

    VehicleNumber NVARCHAR(50) NULL,

    VehicleType NVARCHAR(50) NULL,

    VehicleColor NVARCHAR(50) NULL,

    Description NVARCHAR(1000) NULL,

    IdentificationNotes NVARCHAR(2000) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintPartyDetails_CreatedAt
        DEFAULT SYSUTCDATETIME(),

    UpdatedAt DATETIME2 NULL,

    CONSTRAINT PK_ComplaintPartyDetails
        PRIMARY KEY (ComplaintPartyDetailId),

    CONSTRAINT FK_ComplaintPartyDetails_ComplaintParty
        FOREIGN KEY (ComplaintPartyId)
        REFERENCES dbo.ComplaintParties(ComplaintPartyId)
);
GO