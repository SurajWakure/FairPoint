USE FairPointDb;
GO

CREATE TABLE Evidence
(
    EvidenceId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintId BIGINT NOT NULL,

    UploadedByUserId BIGINT NOT NULL,

    FileName NVARCHAR(255) NOT NULL,

    StorageKey NVARCHAR(1000) NOT NULL,

    ContentType NVARCHAR(150) NOT NULL,

    FileSize BIGINT NOT NULL,

    FileHash NVARCHAR(128) NULL,

    Description NVARCHAR(1000) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Evidence_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Evidence_IsDeleted
        DEFAULT (0),

    DeletedAt DATETIME2 NULL,

    DeletedByUserId BIGINT NULL,

    CONSTRAINT PK_Evidence
        PRIMARY KEY (EvidenceId),

    CONSTRAINT FK_Evidence_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_Evidence_UploadedByUser
        FOREIGN KEY (UploadedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Evidence_DeletedByUser
        FOREIGN KEY (DeletedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT CK_Evidence_FileSize
        CHECK (FileSize > 0)
);
GO