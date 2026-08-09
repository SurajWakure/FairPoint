USE FairPointDb;
GO

CREATE TABLE ComplaintParties
(
    ComplaintPartyId BIGINT IDENTITY(1,1) NOT NULL,

    ComplaintId BIGINT NOT NULL,

    UserId BIGINT NOT NULL,

    PartyTypeId INT NOT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ComplaintParties_CreatedAt
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_ComplaintParties
        PRIMARY KEY (ComplaintPartyId),

    CONSTRAINT FK_ComplaintParties_Complaint
        FOREIGN KEY (ComplaintId)
        REFERENCES Complaints(ComplaintId),

    CONSTRAINT FK_ComplaintParties_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_ComplaintParties_PartyType
        FOREIGN KEY (PartyTypeId)
        REFERENCES ComplaintPartyTypes(PartyTypeId),

    CONSTRAINT UQ_ComplaintParties_Complaint_User_Type
        UNIQUE (ComplaintId, UserId, PartyTypeId)
);
GO