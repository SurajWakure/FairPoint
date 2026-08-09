USE FairPointDb;
GO

------------------------------------------------------------
-- Complaint Categories
------------------------------------------------------------

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'HARASSMENT',
    'Harassment',
    'Complaints involving harassment or inappropriate conduct'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'HARASSMENT'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'PROPERTY_DAMAGE',
    'Property Damage',
    'Complaints involving damage to property'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'PROPERTY_DAMAGE'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'ONLINE_ABUSE',
    'Online Abuse',
    'Complaints involving online abuse or harmful communication'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'ONLINE_ABUSE'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'SERVICE_ISSUE',
    'Service Issue',
    'Complaints involving poor or improper service'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'SERVICE_ISSUE'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'WORKPLACE_ISSUE',
    'Workplace Issue',
    'Complaints involving workplace-related issues'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'WORKPLACE_ISSUE'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'COMMUNITY_ISSUE',
    'Community Issue',
    'Complaints involving community-related issues'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'COMMUNITY_ISSUE'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'FRAUD_SCAM',
    'Fraud / Scam',
    'Complaints involving suspected fraud or scams'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'FRAUD_SCAM'
);

INSERT INTO ComplaintCategories
(
    CategoryCode,
    CategoryName,
    Description
)
SELECT
    'OTHER',
    'Other',
    'Other complaint categories'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintCategories
    WHERE CategoryCode = 'OTHER'
);


------------------------------------------------------------
-- Complaint Statuses
------------------------------------------------------------

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'DRAFT',
    'Draft',
    'Complaint is being prepared and has not been submitted'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'DRAFT'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'SUBMITTED',
    'Submitted',
    'Complaint has been submitted'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'SUBMITTED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'UNDER_REVIEW',
    'Under Review',
    'Complaint is currently being reviewed'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'UNDER_REVIEW'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'NEEDS_INFORMATION',
    'Needs Information',
    'Additional information has been requested'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'NEEDS_INFORMATION'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'RESPONDED',
    'Responded',
    'The involved party has provided a response'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'RESPONDED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'VERIFIED',
    'Verified',
    'Complaint has been verified through the moderation process'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'VERIFIED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'REJECTED',
    'Rejected',
    'Complaint was rejected after review'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'REJECTED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'RESOLVED',
    'Resolved',
    'Complaint has been resolved'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'RESOLVED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'CLOSED',
    'Closed',
    'Complaint workflow is complete'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'CLOSED'
);

INSERT INTO ComplaintStatuses
(
    StatusCode,
    StatusName,
    Description
)
SELECT
    'APPEALED',
    'Appealed',
    'A complaint decision is under appeal'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintStatuses
    WHERE StatusCode = 'APPEALED'
);


------------------------------------------------------------
-- Complaint Severities
------------------------------------------------------------

INSERT INTO ComplaintSeverities
(
    SeverityCode,
    SeverityName,
    Weight,
    Description
)
SELECT
    'LOW',
    'Low',
    1,
    'Low severity complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintSeverities
    WHERE SeverityCode = 'LOW'
);

INSERT INTO ComplaintSeverities
(
    SeverityCode,
    SeverityName,
    Weight,
    Description
)
SELECT
    'MEDIUM',
    'Medium',
    2,
    'Medium severity complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintSeverities
    WHERE SeverityCode = 'MEDIUM'
);

INSERT INTO ComplaintSeverities
(
    SeverityCode,
    SeverityName,
    Weight,
    Description
)
SELECT
    'HIGH',
    'High',
    3,
    'High severity complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintSeverities
    WHERE SeverityCode = 'HIGH'
);

INSERT INTO ComplaintSeverities
(
    SeverityCode,
    SeverityName,
    Weight,
    Description
)
SELECT
    'CRITICAL',
    'Critical',
    4,
    'Critical severity complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintSeverities
    WHERE SeverityCode = 'CRITICAL'
);


------------------------------------------------------------
-- Complaint Party Types
------------------------------------------------------------

INSERT INTO ComplaintPartyTypes
(
    PartyTypeCode,
    PartyTypeName,
    Description
)
SELECT
    'COMPLAINANT',
    'Complainant',
    'Person who submitted the complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintPartyTypes
    WHERE PartyTypeCode = 'COMPLAINANT'
);

INSERT INTO ComplaintPartyTypes
(
    PartyTypeCode,
    PartyTypeName,
    Description
)
SELECT
    'RESPONDENT',
    'Respondent',
    'Person against whom the complaint was submitted'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintPartyTypes
    WHERE PartyTypeCode = 'RESPONDENT'
);

INSERT INTO ComplaintPartyTypes
(
    PartyTypeCode,
    PartyTypeName,
    Description
)
SELECT
    'WITNESS',
    'Witness',
    'Person who may provide relevant information'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintPartyTypes
    WHERE PartyTypeCode = 'WITNESS'
);

INSERT INTO ComplaintPartyTypes
(
    PartyTypeCode,
    PartyTypeName,
    Description
)
SELECT
    'OTHER',
    'Other',
    'Other person associated with the complaint'
WHERE NOT EXISTS
(
    SELECT 1
    FROM ComplaintPartyTypes
    WHERE PartyTypeCode = 'OTHER'
);
GO