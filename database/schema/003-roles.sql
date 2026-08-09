USE FairPointDb;
GO

CREATE TABLE Roles
(
    RoleId INT IDENTITY(1,1) NOT NULL,

    RoleName NVARCHAR(50) NOT NULL,

    Description NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Roles_IsActive DEFAULT (1),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Roles_CreatedAt DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Roles
        PRIMARY KEY (RoleId),

    CONSTRAINT UQ_Roles_RoleName
        UNIQUE (RoleName)
);
GO