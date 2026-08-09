using System.Data.Common;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;
using Microsoft.Data.SqlClient;

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
        const string sql = """
            INSERT INTO ComplaintResponses
            (
                ComplaintId,
                UserId,
                ResponseText,
                CreatedAt,
                UpdatedAt,
                IsDeleted,
                DeletedAt,
                DeletedByUserId
            )
            OUTPUT INSERTED.ResponseId
            VALUES
            (
                @ComplaintId,
                @UserId,
                @ResponseText,
                @CreatedAt,
                @UpdatedAt,
                @IsDeleted,
                @DeletedAt,
                @DeletedByUserId
            );
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter("@ComplaintId", response.ComplaintId));

        command.Parameters.Add(
            new SqlParameter("@UserId", response.UserId));

        command.Parameters.Add(
            new SqlParameter("@ResponseText", response.ResponseText));

        command.Parameters.Add(
            new SqlParameter("@CreatedAt", response.CreatedAt));

        command.Parameters.Add(
            new SqlParameter(
                "@UpdatedAt",
                (object?)response.UpdatedAt ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter("@IsDeleted", response.IsDeleted));

        command.Parameters.Add(
            new SqlParameter(
                "@DeletedAt",
                (object?)response.DeletedAt ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@DeletedByUserId",
                (object?)response.DeletedByUserId ?? DBNull.Value));

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<ComplaintResponse>>
        GetByComplaintIdAsync(
            long complaintId,
            CancellationToken cancellationToken = default)
    {
        const string sql = """
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
            FROM ComplaintResponses
            WHERE ComplaintId = @ComplaintId
              AND IsDeleted = 0
            ORDER BY CreatedAt ASC;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter("@ComplaintId", complaintId));

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        var responses = new List<ComplaintResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            responses.Add(new ComplaintResponse
            {
                ResponseId =
                    reader.GetInt64(
                        reader.GetOrdinal("ResponseId")),

                ComplaintId =
                    reader.GetInt64(
                        reader.GetOrdinal("ComplaintId")),

                UserId =
                    reader.GetInt64(
                        reader.GetOrdinal("UserId")),

                ResponseText =
                    reader.GetString(
                        reader.GetOrdinal("ResponseText")),

                CreatedAt =
                    reader.GetDateTime(
                        reader.GetOrdinal("CreatedAt")),

                UpdatedAt =
                    reader.IsDBNull(
                        reader.GetOrdinal("UpdatedAt"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("UpdatedAt")),

                IsDeleted =
                    reader.GetBoolean(
                        reader.GetOrdinal("IsDeleted")),

                DeletedAt =
                    reader.IsDBNull(
                        reader.GetOrdinal("DeletedAt"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("DeletedAt")),

                DeletedByUserId =
                    reader.IsDBNull(
                        reader.GetOrdinal("DeletedByUserId"))
                        ? null
                        : reader.GetInt64(
                            reader.GetOrdinal("DeletedByUserId"))
            });
        }

        return responses;
    }
}