using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class AppealRepository : IAppealRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AppealRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAsync(
        Appeal appeal,
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
            INSERT INTO dbo.Appeals
            (
                ComplaintId,
                SubmittedByUserId,
                Reason,
                StatusCode,
                CreatedAt
            )
            OUTPUT INSERTED.AppealId
            VALUES
            (
                @ComplaintId,
                @SubmittedByUserId,
                @Reason,
                'SUBMITTED',
                SYSUTCDATETIME()
            );
            """;

        AddParameter(
            command,
            "@ComplaintId",
            appeal.ComplaintId);

        AddParameter(
            command,
            "@SubmittedByUserId",
            appeal.SubmittedByUserId);

        AddParameter(
            command,
            "@Reason",
            appeal.Reason);

        var result =
            command.ExecuteScalar();

        return Convert.ToInt64(result);
    }

    public async Task<Appeal?> GetByIdAsync(
        long appealId,
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
                AppealId,
                ComplaintId,
                SubmittedByUserId,
                Reason,
                StatusCode,
                ReviewedByUserId,
                DecisionReason,
                CreatedAt,
                ReviewedAt
            FROM dbo.Appeals
            WHERE AppealId = @AppealId;
            """;

        AddParameter(
            command,
            "@AppealId",
            appealId);

        using var reader =
            command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return Map(reader);
    }

    public async Task<IReadOnlyList<Appeal>>
        GetByComplaintIdAsync(
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
                AppealId,
                ComplaintId,
                SubmittedByUserId,
                Reason,
                StatusCode,
                ReviewedByUserId,
                DecisionReason,
                CreatedAt,
                ReviewedAt
            FROM dbo.Appeals
            WHERE ComplaintId = @ComplaintId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(
            command,
            "@ComplaintId",
            complaintId);

        var appeals =
            new List<Appeal>();

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            appeals.Add(Map(reader));
        }

        return appeals;
    }

    public async Task<IReadOnlyList<Appeal>>
        GetPendingAsync(
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
                AppealId,
                ComplaintId,
                SubmittedByUserId,
                Reason,
                StatusCode,
                ReviewedByUserId,
                DecisionReason,
                CreatedAt,
                ReviewedAt
            FROM dbo.Appeals
            WHERE StatusCode = 'SUBMITTED'
            ORDER BY CreatedAt ASC;
            """;

        var appeals =
            new List<Appeal>();

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            appeals.Add(Map(reader));
        }

        return appeals;
    }

    public async Task<bool> ReviewAsync(
        long appealId,
        long reviewedByUserId,
        string statusCode,
        string decisionReason,
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
            UPDATE dbo.Appeals
            SET
                StatusCode = @StatusCode,
                ReviewedByUserId = @ReviewedByUserId,
                DecisionReason = @DecisionReason,
                ReviewedAt = SYSUTCDATETIME()
            WHERE AppealId = @AppealId
              AND StatusCode = 'SUBMITTED';
            """;

        AddParameter(
            command,
            "@AppealId",
            appealId);

        AddParameter(
            command,
            "@ReviewedByUserId",
            reviewedByUserId);

        AddParameter(
            command,
            "@StatusCode",
            statusCode);

        AddParameter(
            command,
            "@DecisionReason",
            decisionReason);

        var affected =
            command.ExecuteNonQuery();

        return affected > 0;
    }

    private static Appeal Map(
        IDataRecord reader)
    {
        return new Appeal
        {
            AppealId =
                Convert.ToInt64(
                    reader["AppealId"]),

            ComplaintId =
                Convert.ToInt64(
                    reader["ComplaintId"]),

            SubmittedByUserId =
                Convert.ToInt64(
                    reader["SubmittedByUserId"]),

            Reason =
                Convert.ToString(
                    reader["Reason"])
                ?? string.Empty,

            StatusCode =
                Convert.ToString(
                    reader["StatusCode"])
                ?? "SUBMITTED",

            ReviewedByUserId =
                reader["ReviewedByUserId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(
                        reader["ReviewedByUserId"]),

            DecisionReason =
                reader["DecisionReason"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["DecisionReason"]),

            CreatedAt =
                Convert.ToDateTime(
                    reader["CreatedAt"]),

            ReviewedAt =
                reader["ReviewedAt"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(
                        reader["ReviewedAt"])
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