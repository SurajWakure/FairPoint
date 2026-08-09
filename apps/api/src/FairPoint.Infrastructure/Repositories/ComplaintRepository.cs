using System.Data.Common;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace FairPoint.Infrastructure.Repositories;

public class ComplaintRepository : IComplaintRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ComplaintRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAsync(
        Complaint complaint,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO Complaints
            (
                ComplaintNumber,
                CreatedByUserId,
                CategoryId,
                StatusId,
                SeverityId,
                Title,
                Description,
                VisibilityCode,
                CreatedAt,
                UpdatedAt,
                ResolvedAt,
                ClosedAt
            )
            OUTPUT INSERTED.ComplaintId
            VALUES
            (
                @ComplaintNumber,
                @CreatedByUserId,
                @CategoryId,
                @StatusId,
                @SeverityId,
                @Title,
                @Description,
                @VisibilityCode,
                @CreatedAt,
                @UpdatedAt,
                @ResolvedAt,
                @ClosedAt
            );
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@ComplaintNumber",
                complaint.ComplaintNumber));

        command.Parameters.Add(
            new SqlParameter(
                "@CreatedByUserId",
                complaint.CreatedByUserId));

        command.Parameters.Add(
            new SqlParameter(
                "@CategoryId",
                complaint.CategoryId));

        command.Parameters.Add(
            new SqlParameter(
                "@StatusId",
                complaint.StatusId));

        command.Parameters.Add(
            new SqlParameter(
                "@SeverityId",
                complaint.SeverityId));

        command.Parameters.Add(
            new SqlParameter(
                "@Title",
                complaint.Title));

        command.Parameters.Add(
            new SqlParameter(
                "@Description",
                complaint.Description));

        command.Parameters.Add(
            new SqlParameter(
                "@VisibilityCode",
                complaint.VisibilityCode));

        command.Parameters.Add(
            new SqlParameter(
                "@CreatedAt",
                complaint.CreatedAt));

        command.Parameters.Add(
            new SqlParameter(
                "@UpdatedAt",
                (object?)complaint.UpdatedAt ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@ResolvedAt",
                (object?)complaint.ResolvedAt ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@ClosedAt",
                (object?)complaint.ClosedAt ?? DBNull.Value));

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt64(result);
    }

    public async Task<Complaint?> GetByIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ComplaintId,
                ComplaintNumber,
                CreatedByUserId,
                CategoryId,
                StatusId,
                SeverityId,
                Title,
                Description,
                VisibilityCode,
                CreatedAt,
                UpdatedAt,
                ResolvedAt,
                ClosedAt
            FROM Complaints
            WHERE ComplaintId = @ComplaintId;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@ComplaintId",
                complaintId));

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapComplaint(reader);
    }

    public async Task<IReadOnlyList<Complaint>> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ComplaintId,
                ComplaintNumber,
                CreatedByUserId,
                CategoryId,
                StatusId,
                SeverityId,
                Title,
                Description,
                VisibilityCode,
                CreatedAt,
                UpdatedAt,
                ResolvedAt,
                ClosedAt
            FROM Complaints
            WHERE CreatedByUserId = @UserId
            ORDER BY CreatedAt DESC;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@UserId",
                userId));

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        var complaints = new List<Complaint>();

        while (await reader.ReadAsync(cancellationToken))
        {
            complaints.Add(MapComplaint(reader));
        }

        return complaints;
    }

    private static Complaint MapComplaint(
        DbDataReader reader)
    {
        return new Complaint
        {
            ComplaintId =
                reader.GetInt64(
                    reader.GetOrdinal("ComplaintId")),

            ComplaintNumber =
                reader.GetString(
                    reader.GetOrdinal("ComplaintNumber")),

            CreatedByUserId =
                reader.GetInt64(
                    reader.GetOrdinal("CreatedByUserId")),

            CategoryId =
                reader.GetInt32(
                    reader.GetOrdinal("CategoryId")),

            StatusId =
                reader.GetInt32(
                    reader.GetOrdinal("StatusId")),

            SeverityId =
                reader.GetInt32(
                    reader.GetOrdinal("SeverityId")),

            Title =
                reader.GetString(
                    reader.GetOrdinal("Title")),

            Description =
                reader.GetString(
                    reader.GetOrdinal("Description")),

            VisibilityCode =
                reader.GetString(
                    reader.GetOrdinal("VisibilityCode")),

            CreatedAt =
                reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt")),

            UpdatedAt =
                reader.IsDBNull(
                    reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("UpdatedAt")),

            ResolvedAt =
                reader.IsDBNull(
                    reader.GetOrdinal("ResolvedAt"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("ResolvedAt")),

            ClosedAt =
                reader.IsDBNull(
                    reader.GetOrdinal("ClosedAt"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("ClosedAt"))
        };
    }
}