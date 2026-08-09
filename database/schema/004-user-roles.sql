USE FairPointDb;
GO

CREATE TABLE UserRoles
(
    UserRoleId BIGINT IDENTITY(1,1) NOT NULL,

    UserId BIGINT NOT NULL,

    RoleId INT NOT NULL,

    AssignedAt DATETIME2 NOT NULL
        CONSTRAINT DF_UserRoles_AssignedAt
        DEFAULT (SYSUTCDATETIME()),

    AssignedBy BIGINT NULL,

    CONSTRAINT PK_UserRoles
        PRIMARY KEY (UserRoleId),

    CONSTRAINT FK_UserRoles_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_UserRoles_Role
        FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId),

    CONSTRAINT UQ_UserRoles_User_Role
        UNIQUE (UserId, RoleId)
);
GO