# FairPoint Database

FairPoint uses Microsoft SQL Server as the relational database.

## Database

FairPointDb

## Structure

### schema

Contains database and table creation scripts.

### indexes

Contains performance-related indexes.

### seed

Contains initial reference/master data.

### procedures

Contains stored procedures when required.

### views

Contains database views when required.

## Naming Convention

Tables:

PascalCase

Examples:

- Users
- Complaints
- AuditLogs

Primary Keys:

PK_TableName

Foreign Keys:

FK_TableName_ReferencedTable

Unique Constraints:

UQ_TableName_ColumnName

Default Constraints:

DF_TableName_ColumnName

## Important Design Principles

- Use UTC timestamps.
- Use BIGINT for high-volume entity IDs.
- Do not store large files directly in SQL Server.
- Store evidence metadata in SQL Server.
- Store files in object storage.
- Use indexes based on query patterns.
- Keep audit history for important business actions.
- Do not store passwords in plain text.