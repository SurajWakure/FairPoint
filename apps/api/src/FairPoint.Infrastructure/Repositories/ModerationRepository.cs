using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class ModerationRepository : IModerationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ModerationRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> ChangeStatusAsync(
        long complaintId,
        long moderatorUserId,
        string actionType,
        string reason,
        int previousStatusId,
        int newStatusId,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Update complaint status
            using var updateCommand =
                connection.CreateCommand();

            updateCommand.Transaction = transaction;

            updateCommand.CommandText = """
                UPDATE dbo.Complaints
                SET
                    StatusId = @NewStatusId,
                    UpdatedAt = SYSUTCDATETIME()
                WHERE ComplaintId = @ComplaintId;
                """;

            AddParameter(
                updateCommand,
                "@NewStatusId",
                newStatusId);

            AddParameter(
                updateCommand,
                "@ComplaintId",
                complaintId);

            var affected =
                updateCommand.ExecuteNonQuery();

            if (affected == 0)
            {
                throw new KeyNotFoundException(
                    "Complaint was not found.");
            }

            // 2. Record moderation action
            using var insertCommand =
                connection.CreateCommand();

            insertCommand.Transaction = transaction;

            insertCommand.CommandText = """
                INSERT INTO dbo.ModerationActions
                (
                    ComplaintId,
                    ModeratorUserId,
                    ActionType,
                    Reason,
                    PreviousStatusId,
                    NewStatusId,
                    CreatedAt
                )
                OUTPUT INSERTED.ModerationActionId
                VALUES
                (
                    @ComplaintId,
                    @ModeratorUserId,
                    @ActionType,
                    @Reason,
                    @PreviousStatusId,
                    @NewStatusId,
                    SYSUTCDATETIME()
                );
                """;

            AddParameter(
                insertCommand,
                "@ComplaintId",
                complaintId);

            AddParameter(
                insertCommand,
                "@ModeratorUserId",
                moderatorUserId);

            AddParameter(
                insertCommand,
                "@ActionType",
                actionType);

            AddParameter(
                insertCommand,
                "@Reason",
                reason);

            AddParameter(
                insertCommand,
                "@PreviousStatusId",
                previousStatusId);

            AddParameter(
                insertCommand,
                "@NewStatusId",
                newStatusId);

            var result =
                insertCommand.ExecuteScalar();

            var moderationActionId =
                Convert.ToInt64(result);

            transaction.Commit();

            return moderationActionId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IReadOnlyList<ModerationAction>>
        GetActionsAsync(
            long complaintId,
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
                ModerationActionId,
                ComplaintId,
                ModeratorUserId,
                ActionType,
                Reason,
                PreviousStatusId,
                NewStatusId,
                CreatedAt
            FROM dbo.ModerationActions
            WHERE ComplaintId = @ComplaintId
            ORDER BY CreatedAt ASC;
            """;

        AddParameter(
            command,
            "@ComplaintId",
            complaintId);

        var actions =
            new List<ModerationAction>();

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            actions.Add(new ModerationAction
            {
                ModerationActionId =
                    Convert.ToInt64(
                        reader["ModerationActionId"]),

                ComplaintId =
                    Convert.ToInt64(
                        reader["ComplaintId"]),

                ModeratorUserId =
                    Convert.ToInt64(
                        reader["ModeratorUserId"]),

                ActionType =
                    Convert.ToString(
                        reader["ActionType"])
                    ?? string.Empty,

                Reason =
                    Convert.ToString(
                        reader["Reason"])
                    ?? string.Empty,

                PreviousStatusId =
                    reader["PreviousStatusId"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["PreviousStatusId"]),

                NewStatusId =
                    reader["NewStatusId"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            reader["NewStatusId"]),

                CreatedAt =
                    Convert.ToDateTime(
                        reader["CreatedAt"])
            });
        }

        return actions;
    }

    public async Task<ModerationAction?>
        GetLastActionAsync(
            long complaintId,
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
            SELECT TOP 1
                ModerationActionId,
                ComplaintId,
                ModeratorUserId,
                ActionType,
                Reason,
                PreviousStatusId,
                NewStatusId,
                CreatedAt
            FROM dbo.ModerationActions
            WHERE ComplaintId = @ComplaintId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(
            command,
            "@ComplaintId",
            complaintId);

        using var reader =
            command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new ModerationAction
        {
            ModerationActionId =
                Convert.ToInt64(
                    reader["ModerationActionId"]),

            ComplaintId =
                Convert.ToInt64(
                    reader["ComplaintId"]),

            ModeratorUserId =
                Convert.ToInt64(
                    reader["ModeratorUserId"]),

            ActionType =
                Convert.ToString(
                    reader["ActionType"])
                ?? string.Empty,

            Reason =
                Convert.ToString(
                    reader["Reason"])
                ?? string.Empty,

            PreviousStatusId =
                reader["PreviousStatusId"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(
                        reader["PreviousStatusId"]),

            NewStatusId =
                reader["NewStatusId"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(
                        reader["NewStatusId"]),

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