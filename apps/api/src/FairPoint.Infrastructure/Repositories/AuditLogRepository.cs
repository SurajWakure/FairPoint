using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuditLogRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            INSERT INTO dbo.AuditLogs
            (
                UserId,
                ActionType,
                EntityType,
                EntityId,
                OldValues,
                NewValues,
                IpAddress,
                UserAgent,
                CreatedAt
            )
            OUTPUT INSERTED.AuditLogId
            VALUES
            (
                @UserId,
                @ActionType,
                @EntityType,
                @EntityId,
                @OldValues,
                @NewValues,
                @IpAddress,
                @UserAgent,
                SYSUTCDATETIME()
            );
            """;

        AddParameter(
            command,
            "@UserId",
            auditLog.UserId);

        AddParameter(
            command,
            "@ActionType",
            auditLog.ActionType);

        AddParameter(
            command,
            "@EntityType",
            auditLog.EntityType);

        AddParameter(
            command,
            "@EntityId",
            auditLog.EntityId);

        AddParameter(
            command,
            "@OldValues",
            auditLog.OldValues);

        AddParameter(
            command,
            "@NewValues",
            auditLog.NewValues);

        AddParameter(
            command,
            "@IpAddress",
            auditLog.IpAddress);

        AddParameter(
            command,
            "@UserAgent",
            auditLog.UserAgent);

        var result =
            command.ExecuteScalar();

        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetByEntityAsync(
            string entityType,
            long entityId,
            CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            SELECT
                AuditLogId,
                UserId,
                ActionType,
                EntityType,
                EntityId,
                OldValues,
                NewValues,
                IpAddress,
                UserAgent,
                CreatedAt
            FROM dbo.AuditLogs
            WHERE EntityType = @EntityType
              AND EntityId = @EntityId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(
            command,
            "@EntityType",
            entityType);

        AddParameter(
            command,
            "@EntityId",
            entityId);

        return ReadLogs(command);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetByUserAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            SELECT
                AuditLogId,
                UserId,
                ActionType,
                EntityType,
                EntityId,
                OldValues,
                NewValues,
                IpAddress,
                UserAgent,
                CreatedAt
            FROM dbo.AuditLogs
            WHERE UserId = @UserId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(
            command,
            "@UserId",
            userId);

        return ReadLogs(command);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetRecentAsync(
            int take = 100,
            CancellationToken cancellationToken = default)
    {
        if (take <= 0)
        {
            take = 100;
        }

        if (take > 500)
        {
            take = 500;
        }

        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            SELECT TOP (@Take)
                AuditLogId,
                UserId,
                ActionType,
                EntityType,
                EntityId,
                OldValues,
                NewValues,
                IpAddress,
                UserAgent,
                CreatedAt
            FROM dbo.AuditLogs
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(
            command,
            "@Take",
            take);

        return ReadLogs(command);
    }

    private static List<AuditLog> ReadLogs(
        IDbCommand command)
    {
        var logs =
            new List<AuditLog>();

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            logs.Add(Map(reader));
        }

        return logs;
    }

    private static AuditLog Map(
        IDataRecord reader)
    {
        return new AuditLog
        {
            AuditLogId =
                Convert.ToInt64(
                    reader["AuditLogId"]),

            UserId =
                reader["UserId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(
                        reader["UserId"]),

            ActionType =
                Convert.ToString(
                    reader["ActionType"])
                ?? string.Empty,

            EntityType =
                reader["EntityType"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["EntityType"]),

            EntityId =
                reader["EntityId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(
                        reader["EntityId"]),

            OldValues =
                reader["OldValues"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["OldValues"]),

            NewValues =
                reader["NewValues"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["NewValues"]),

            IpAddress =
                reader["IpAddress"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["IpAddress"]),

            UserAgent =
                reader["UserAgent"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["UserAgent"]),

            CreatedAt =
                Convert.ToDateTime(
                    reader["CreatedAt"])
        };
    }

    private static void AddParameter(
        IDbCommand command,
        string name,
        object? value)
    {
        var parameter =
            command.CreateParameter();

        parameter.ParameterName = name;

        parameter.Value =
            value ?? DBNull.Value;

        command.Parameters.Add(parameter);
    }
}