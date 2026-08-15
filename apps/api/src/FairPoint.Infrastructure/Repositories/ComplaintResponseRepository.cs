using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class ComplaintResponseRepository : IComplaintResponseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ComplaintResponseRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> AddAsync(
        ComplaintResponse response,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO dbo.ComplaintResponses
            (
                ComplaintId,
                UserId,
                ResponseText,
                CreatedAt,
                IsDeleted
            )
            OUTPUT INSERTED.ResponseId
            VALUES
            (
                @ComplaintId,
                @UserId,
                @ResponseText,
                SYSUTCDATETIME(),
                0
            );
            """;

        AddParameter(
            command,
            "@ComplaintId",
            response.ComplaintId);

        AddParameter(
            command,
            "@UserId",
            response.UserId);

        AddParameter(
            command,
            "@ResponseText",
            response.ResponseText);

        var result = command.ExecuteScalar();

        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<ComplaintResponse>>
        GetByComplaintIdAsync(
            long complaintId,
            CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                ResponseId,
                ComplaintId,
                UserId,
                ResponseText,
                CreatedAt,
                UpdatedAt,
                IsDeleted,
                DeletedAt,
                DeletedByUserId
            FROM dbo.ComplaintResponses
            WHERE ComplaintId = @ComplaintId
              AND IsDeleted = 0
            ORDER BY CreatedAt ASC;
            """;

        AddParameter(
            command,
            "@ComplaintId",
            complaintId);

        var responses = new List<ComplaintResponse>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            responses.Add(MapResponse(reader));
        }

        return responses;
    }

    public async Task<ComplaintResponse?> GetByIdAsync(
        long responseId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                ResponseId,
                ComplaintId,
                UserId,
                ResponseText,
                CreatedAt,
                UpdatedAt,
                IsDeleted,
                DeletedAt,
                DeletedByUserId
            FROM dbo.ComplaintResponses
            WHERE ResponseId = @ResponseId;
            """;

        AddParameter(
            command,
            "@ResponseId",
            responseId);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return MapResponse(reader);
    }

    public async Task<bool> SoftDeleteAsync(
        long responseId,
        long deletedByUserId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();

        command.CommandText = """
            UPDATE dbo.ComplaintResponses
            SET
                IsDeleted = 1,
                DeletedAt = SYSUTCDATETIME(),
                DeletedByUserId = @DeletedByUserId,
                UpdatedAt = SYSUTCDATETIME()
            WHERE ResponseId = @ResponseId
              AND IsDeleted = 0;
            """;

        AddParameter(
            command,
            "@ResponseId",
            responseId);

        AddParameter(
            command,
            "@DeletedByUserId",
            deletedByUserId);

        var affectedRows = command.ExecuteNonQuery();

        return affectedRows > 0;
    }

    private static ComplaintResponse MapResponse(
        IDataRecord reader)
    {
        return new ComplaintResponse
        {
            ResponseId = Convert.ToInt64(
                reader["ResponseId"]),

            ComplaintId = Convert.ToInt64(
                reader["ComplaintId"]),

            UserId = Convert.ToInt64(
                reader["UserId"]),

            ResponseText = Convert.ToString(
                reader["ResponseText"]) ?? string.Empty,

            CreatedAt = Convert.ToDateTime(
                reader["CreatedAt"]),

            UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                ? null
                : Convert.ToDateTime(
                    reader["UpdatedAt"]),

            IsDeleted = Convert.ToBoolean(
                reader["IsDeleted"]),

            DeletedAt = reader["DeletedAt"] == DBNull.Value
                ? null
                : Convert.ToDateTime(
                    reader["DeletedAt"]),

            DeletedByUserId =
                reader["DeletedByUserId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(
                        reader["DeletedByUserId"])
        };
    }

    private static void AddParameter(
        IDbCommand command,
        string parameterName,
        object value)
    {
        var parameter = command.CreateParameter();

        parameter.ParameterName = parameterName;
        parameter.Value = value ?? DBNull.Value;

        command.Parameters.Add(parameter);
    }
}