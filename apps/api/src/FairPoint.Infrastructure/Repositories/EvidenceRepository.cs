using System.Data.Common;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace FairPoint.Infrastructure.Repositories;

public class EvidenceRepository : IEvidenceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EvidenceRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> AddAsync(
        Evidence evidence,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO Evidence
            (
                ComplaintId,
                UploadedByUserId,
                FileName,
                StorageKey,
                ContentType,
                FileSize,
                FileHash,
                Description,
                CreatedAt,
                IsDeleted
            )
            OUTPUT INSERTED.EvidenceId
            VALUES
            (
                @ComplaintId,
                @UploadedByUserId,
                @FileName,
                @StorageKey,
                @ContentType,
                @FileSize,
                @FileHash,
                @Description,
                @CreatedAt,
                @IsDeleted
            );
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@ComplaintId",
                evidence.ComplaintId));

        command.Parameters.Add(
            new SqlParameter(
                "@UploadedByUserId",
                evidence.UploadedByUserId));

        command.Parameters.Add(
            new SqlParameter(
                "@FileName",
                evidence.FileName));

        command.Parameters.Add(
            new SqlParameter(
                "@StorageKey",
                evidence.StorageKey));

        command.Parameters.Add(
            new SqlParameter(
                "@ContentType",
                evidence.ContentType));

        command.Parameters.Add(
            new SqlParameter(
                "@FileSize",
                evidence.FileSize));

        command.Parameters.Add(
            new SqlParameter(
                "@FileHash",
                (object?)evidence.FileHash ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@Description",
                (object?)evidence.Description ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@CreatedAt",
                evidence.CreatedAt));

        command.Parameters.Add(
            new SqlParameter(
                "@IsDeleted",
                evidence.IsDeleted));

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt64(result);
    }

    public async Task<Evidence?> GetByIdAsync(
        long evidenceId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                EvidenceId,
                ComplaintId,
                UploadedByUserId,
                FileName,
                StorageKey,
                ContentType,
                FileSize,
                FileHash,
                Description,
                CreatedAt,
                IsDeleted,
                DeletedAt,
                DeletedByUserId
            FROM Evidence
            WHERE EvidenceId = @EvidenceId
              AND IsDeleted = 0;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@EvidenceId",
                evidenceId));

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapEvidence(reader);
    }

    public async Task<IReadOnlyList<Evidence>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                EvidenceId,
                ComplaintId,
                UploadedByUserId,
                FileName,
                StorageKey,
                ContentType,
                FileSize,
                FileHash,
                Description,
                CreatedAt,
                IsDeleted,
                DeletedAt,
                DeletedByUserId
            FROM Evidence
            WHERE ComplaintId = @ComplaintId
              AND IsDeleted = 0
            ORDER BY CreatedAt DESC;
            """;

        var evidenceList = new List<Evidence>();

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

        while (await reader.ReadAsync(cancellationToken))
        {
            evidenceList.Add(MapEvidence(reader));
        }

        return evidenceList;
    }

    private static Evidence MapEvidence(DbDataReader reader)
    {
        return new Evidence
        {
            EvidenceId =
                reader.GetInt64(
                    reader.GetOrdinal("EvidenceId")),

            ComplaintId =
                reader.GetInt64(
                    reader.GetOrdinal("ComplaintId")),

            UploadedByUserId =
                reader.GetInt64(
                    reader.GetOrdinal("UploadedByUserId")),

            FileName =
                reader.GetString(
                    reader.GetOrdinal("FileName")),

            StorageKey =
                reader.GetString(
                    reader.GetOrdinal("StorageKey")),

            ContentType =
                reader.GetString(
                    reader.GetOrdinal("ContentType")),

            FileSize =
                reader.GetInt64(
                    reader.GetOrdinal("FileSize")),

            FileHash =
                reader.IsDBNull(
                    reader.GetOrdinal("FileHash"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("FileHash")),

            Description =
                reader.IsDBNull(
                    reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Description")),

            CreatedAt =
                reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt")),

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
        };
    }
}