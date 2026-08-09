USE FairPointDb;
GO

------------------------------------------------------------
-- Users
------------------------------------------------------------

CREATE INDEX IX_Users_CreatedAt
ON Users (CreatedAt);

CREATE INDEX IX_Users_IsActive
ON Users (IsActive);


------------------------------------------------------------
-- UserRoles
------------------------------------------------------------

CREATE INDEX IX_UserRoles_UserId
ON UserRoles (UserId);

CREATE INDEX IX_UserRoles_RoleId
ON UserRoles (RoleId);


------------------------------------------------------------
-- Complaints
------------------------------------------------------------

CREATE INDEX IX_Complaints_CreatedByUserId
ON Complaints (CreatedByUserId);

CREATE INDEX IX_Complaints_CategoryId
ON Complaints (CategoryId);

CREATE INDEX IX_Complaints_StatusId
ON Complaints (StatusId);

CREATE INDEX IX_Complaints_SeverityId
ON Complaints (SeverityId);

CREATE INDEX IX_Complaints_CreatedAt
ON Complaints (CreatedAt);


------------------------------------------------------------
-- Complaint Parties
------------------------------------------------------------

CREATE INDEX IX_ComplaintParties_UserId
ON ComplaintParties (UserId);

CREATE INDEX IX_ComplaintParties_ComplaintId
ON ComplaintParties (ComplaintId);


------------------------------------------------------------
-- Complaint Responses
------------------------------------------------------------

CREATE INDEX IX_ComplaintResponses_ComplaintId
ON ComplaintResponses (ComplaintId);

CREATE INDEX IX_ComplaintResponses_UserId
ON ComplaintResponses (UserId);

CREATE INDEX IX_ComplaintResponses_CreatedAt
ON ComplaintResponses (CreatedAt);


------------------------------------------------------------
-- Evidence
------------------------------------------------------------

CREATE INDEX IX_Evidence_ComplaintId
ON Evidence (ComplaintId);

CREATE INDEX IX_Evidence_UploadedByUserId
ON Evidence (UploadedByUserId);


------------------------------------------------------------
-- Moderation
------------------------------------------------------------

CREATE INDEX IX_ModerationActions_ComplaintId
ON ModerationActions (ComplaintId);

CREATE INDEX IX_ModerationActions_ModeratorUserId
ON ModerationActions (ModeratorUserId);

CREATE INDEX IX_ModerationActions_CreatedAt
ON ModerationActions (CreatedAt);


------------------------------------------------------------
-- Appeals
------------------------------------------------------------

CREATE INDEX IX_Appeals_ComplaintId
ON Appeals (ComplaintId);

CREATE INDEX IX_Appeals_SubmittedByUserId
ON Appeals (SubmittedByUserId);

CREATE INDEX IX_Appeals_StatusCode
ON Appeals (StatusCode);


------------------------------------------------------------
-- Reputation
------------------------------------------------------------

CREATE INDEX IX_ReputationHistory_UserId
ON ReputationHistory (UserId);

CREATE INDEX IX_ReputationHistory_ComplaintId
ON ReputationHistory (ComplaintId);

CREATE INDEX IX_ReputationHistory_CreatedAt
ON ReputationHistory (CreatedAt);


------------------------------------------------------------
-- Notifications
------------------------------------------------------------

CREATE INDEX IX_Notifications_UserId_IsRead
ON Notifications (UserId, IsRead);

CREATE INDEX IX_Notifications_CreatedAt
ON Notifications (CreatedAt);


------------------------------------------------------------
-- Audit Logs
------------------------------------------------------------

CREATE INDEX IX_AuditLogs_UserId
ON AuditLogs (UserId);

CREATE INDEX IX_AuditLogs_Entity
ON AuditLogs (EntityType, EntityId);

CREATE INDEX IX_AuditLogs_CreatedAt
ON AuditLogs (CreatedAt);

CREATE INDEX IX_Complaints_Status_CreatedAt
ON Complaints (StatusId, CreatedAt DESC);
GO