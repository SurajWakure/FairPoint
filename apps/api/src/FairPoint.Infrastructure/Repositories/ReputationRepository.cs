using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class ReputationRepository : IReputationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ReputationRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> AddHistoryAsync(
        ReputationHistory history,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO dbo.ReputationHistory
            (
                UserId,
                ComplaintId,
                PointsChange,
                ReasonCode,
                Description,
                CreatedByUserId,
                CreatedAt
            )
            OUTPUT INSERTED.ReputationHistoryId
            VALUES
            (
                @UserId,
                @ComplaintId,
                @PointsChange,
                @ReasonCode,
                @Description,
                @CreatedByUserId,
                SYSUTCDATETIME()
            );
            """;

        AddParameter(command, "@UserId", history.UserId);
        AddParameter(command, "@ComplaintId", history.ComplaintId);
        AddParameter(command, "@PointsChange", history.PointsChange);
        AddParameter(command, "@ReasonCode", history.ReasonCode);
        AddParameter(command, "@Description", history.Description);
        AddParameter(command, "@CreatedByUserId", history.CreatedByUserId);

        var result = command.ExecuteScalar();

        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<ReputationHistory>>
        GetHistoryAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                ReputationHistoryId,
                UserId,
                ComplaintId,
                PointsChange,
                ReasonCode,
                Description,
                CreatedByUserId,
                CreatedAt
            FROM dbo.ReputationHistory
            WHERE UserId = @UserId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(command, "@UserId", userId);

        var result = new List<ReputationHistory>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new ReputationHistory
            {
                ReputationHistoryId =
                    Convert.ToInt64(reader["ReputationHistoryId"]),

                UserId =
                    Convert.ToInt64(reader["UserId"]),

                ComplaintId =
                    reader["ComplaintId"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["ComplaintId"]),

                PointsChange =
                    Convert.ToInt32(reader["PointsChange"]),

                ReasonCode =
                    Convert.ToString(reader["ReasonCode"])
                    ?? string.Empty,

                Description =
                    reader["Description"] == DBNull.Value
                        ? null
                        : Convert.ToString(reader["Description"]),

                CreatedByUserId =
                    reader["CreatedByUserId"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(
                            reader["CreatedByUserId"]),

                CreatedAt =
                    Convert.ToDateTime(reader["CreatedAt"])
            });
        }

        return result;
    }

    public async Task<int> GetCurrentScoreAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT ReputationScore
            FROM dbo.Users
            WHERE UserId = @UserId;
            """;

        AddParameter(command, "@UserId", userId);

        var result = command.ExecuteScalar();

        if (result == null || result == DBNull.Value)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        return Convert.ToInt32(result);
    }

    public async Task UpdateUserScoreAsync(
        long userId,
        int pointsChange,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            UPDATE dbo.Users
            SET
                ReputationScore =
                    CASE
                        WHEN ReputationScore + @PointsChange < 0
                            THEN 0
                        ELSE ReputationScore + @PointsChange
                    END,
                UpdatedAt = SYSUTCDATETIME()
            WHERE UserId = @UserId;
            """;

        AddParameter(command, "@UserId", userId);
        AddParameter(command, "@PointsChange", pointsChange);

        var affected = command.ExecuteNonQuery();

        if (affected == 0)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }
    }

    private static void AddParameter(
        IDbCommand command,
        string name,
        object? value)
    {
        var parameter = command.CreateParameter();

        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;

        command.Parameters.Add(parameter);
    }
}