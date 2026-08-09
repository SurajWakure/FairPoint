using System.Data.Common;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace FairPoint.Infrastructure.Repositories;

public class ComplaintPartyRepository : IComplaintPartyRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ComplaintPartyRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> AddAsync(
    ComplaintParty party,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        INSERT INTO ComplaintParties
        (
            ComplaintId,
            UserId,
            PartyTypeId,
            IsIdentified,
            CreatedAt,
            UpdatedAt
        )
        OUTPUT INSERTED.ComplaintPartyId
        VALUES
        (
            @ComplaintId,
            @UserId,
            @PartyTypeId,
            @IsIdentified,
            @CreatedAt,
            @UpdatedAt
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
                party.ComplaintId));

        command.Parameters.Add(
            new SqlParameter(
                "@UserId",
                (object?)party.UserId ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@PartyTypeId",
                party.PartyTypeId));

        command.Parameters.Add(
            new SqlParameter(
                "@IsIdentified",
                party.IsIdentified));

        command.Parameters.Add(
            new SqlParameter(
                "@CreatedAt",
                party.CreatedAt));

        command.Parameters.Add(
            new SqlParameter(
                "@UpdatedAt",
                (object?)party.UpdatedAt ?? DBNull.Value));

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt64(result);
    }

    public async Task AddDetailsAsync(
    ComplaintPartyDetails details,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        INSERT INTO ComplaintPartyDetails
        (
            ComplaintPartyId,
            VehicleNumber,
            VehicleType,
            VehicleColor,
            Description,
            IdentificationNotes,
            CreatedAt,
            UpdatedAt
        )
        VALUES
        (
            @ComplaintPartyId,
            @VehicleNumber,
            @VehicleType,
            @VehicleColor,
            @Description,
            @IdentificationNotes,
            @CreatedAt,
            @UpdatedAt
        );
        """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter(
                "@ComplaintPartyId",
                details.ComplaintPartyId));

        command.Parameters.Add(
            new SqlParameter(
                "@VehicleNumber",
                (object?)details.VehicleNumber ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@VehicleType",
                (object?)details.VehicleType ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@VehicleColor",
                (object?)details.VehicleColor ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@Description",
                (object?)details.Description ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@IdentificationNotes",
                (object?)details.IdentificationNotes ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter(
                "@CreatedAt",
                details.CreatedAt));

        command.Parameters.Add(
            new SqlParameter(
                "@UpdatedAt",
                (object?)details.UpdatedAt ?? DBNull.Value));

        await command.ExecuteNonQueryAsync(cancellationToken);
    } 

    public async Task<IReadOnlyList<ComplaintParty>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ComplaintPartyId,
                ComplaintId,
                UserId,
                PartyTypeId,
                CreatedAt
            FROM ComplaintParties
            WHERE ComplaintId = @ComplaintId
            ORDER BY CreatedAt;
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

        var parties = new List<ComplaintParty>();

        while (await reader.ReadAsync(cancellationToken))
        {
            parties.Add(new ComplaintParty
            {
                ComplaintPartyId =
                    reader.GetInt64(
                        reader.GetOrdinal("ComplaintPartyId")),

                ComplaintId =
                    reader.GetInt64(
                        reader.GetOrdinal("ComplaintId")),

                UserId =
                    reader.GetInt64(
                        reader.GetOrdinal("UserId")),

                PartyTypeId =
                    reader.GetInt32(
                        reader.GetOrdinal("PartyTypeId")),

                CreatedAt =
                    reader.GetDateTime(
                        reader.GetOrdinal("CreatedAt"))
            });
        }

        return parties;
    }
}