USE FairPointDb;
GO

INSERT INTO Roles
(
    RoleName,
    Description
)
SELECT
    'User',
    'Standard FairPoint user'
WHERE NOT EXISTS
(
    SELECT 1
    FROM Roles
    WHERE RoleName = 'User'
);

INSERT INTO Roles
(
    RoleName,
    Description
)
SELECT
    'Moderator',
    'Reviews complaints and moderation cases'
WHERE NOT EXISTS
(
    SELECT 1
    FROM Roles
    WHERE RoleName = 'Moderator'
);

INSERT INTO Roles
(
    RoleName,
    Description
)
SELECT
    'Admin',
    'Manages application configuration and users'
WHERE NOT EXISTS
(
    SELECT 1
    FROM Roles
    WHERE RoleName = 'Admin'
);

INSERT INTO Roles
(
    RoleName,
    Description
)
SELECT
    'SuperAdmin',
    'Highest level system administrator'
WHERE NOT EXISTS
(
    SELECT 1
    FROM Roles
    WHERE RoleName = 'SuperAdmin'
);
GO