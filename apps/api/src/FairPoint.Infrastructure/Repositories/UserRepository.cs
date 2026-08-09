using System.Data.Common;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace FairPoint.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                PasswordHash,
                IsActive,
                IsVerified,
                ReputationScore,
                LastLoginAt,
                CreatedAt,
                UpdatedAt
            FROM Users
            WHERE UserId = @UserId;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        var parameter = new SqlParameter("@UserId", userId);

        command.Parameters.Add(parameter);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapUser(reader);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                PasswordHash,
                IsActive,
                IsVerified,
                ReputationScore,
                LastLoginAt,
                CreatedAt,
                UpdatedAt
            FROM Users
            WHERE Email = @Email;
            """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        var parameter = new SqlParameter("@Email", email);

        command.Parameters.Add(parameter);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapUser(reader);
    }

    public async Task<long> CreateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO Users
            (
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                PasswordHash,
                IsActive,
                IsVerified,
                ReputationScore,
                LastLoginAt,
                CreatedAt,
                UpdatedAt
            )
            OUTPUT INSERTED.UserId
            VALUES
            (
                @FirstName,
                @LastName,
                @Email,
                @PhoneNumber,
                @PasswordHash,
                @IsActive,
                @IsVerified,
                @ReputationScore,
                @LastLoginAt,
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
            new SqlParameter("@FirstName", user.FirstName));

        command.Parameters.Add(
            new SqlParameter("@LastName", (object?)user.LastName ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter("@Email", user.Email));

        command.Parameters.Add(
            new SqlParameter("@PhoneNumber",
                (object?)user.PhoneNumber ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter("@PasswordHash", user.PasswordHash));

        command.Parameters.Add(
            new SqlParameter("@IsActive", user.IsActive));

        command.Parameters.Add(
            new SqlParameter("@IsVerified", user.IsVerified));

        command.Parameters.Add(
            new SqlParameter("@ReputationScore", user.ReputationScore));

        command.Parameters.Add(
            new SqlParameter("@LastLoginAt",
                (object?)user.LastLoginAt ?? DBNull.Value));

        command.Parameters.Add(
            new SqlParameter("@CreatedAt", user.CreatedAt));

        command.Parameters.Add(
            new SqlParameter("@UpdatedAt",
                (object?)user.UpdatedAt ?? DBNull.Value));

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt64(result);
    }

    private static User MapUser(DbDataReader reader)
    {
        return new User
        {
            UserId = reader.GetInt64(reader.GetOrdinal("UserId")),

            FirstName =
                reader.GetString(reader.GetOrdinal("FirstName")),

            LastName =
                reader.IsDBNull(reader.GetOrdinal("LastName"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("LastName")),

            Email =
                reader.GetString(reader.GetOrdinal("Email")),

            PhoneNumber =
                reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("PhoneNumber")),

            PasswordHash =
                reader.GetString(reader.GetOrdinal("PasswordHash")),

            IsActive =
                reader.GetBoolean(reader.GetOrdinal("IsActive")),

            IsVerified =
                reader.GetBoolean(reader.GetOrdinal("IsVerified")),

            ReputationScore =
                reader.GetInt32(reader.GetOrdinal("ReputationScore")),

            LastLoginAt =
                reader.IsDBNull(reader.GetOrdinal("LastLoginAt"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("LastLoginAt")),

            CreatedAt =
                reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt")),

            UpdatedAt =
                reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("UpdatedAt"))
        };
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(
    long userId,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        SELECT
            r.RoleName
        FROM UserRoles ur
        INNER JOIN Roles r
            ON r.RoleId = ur.RoleId
        WHERE ur.UserId = @UserId
        ORDER BY r.RoleName;
        """;

        await using var connection =
            (DbConnection)_connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(
            new SqlParameter("@UserId", userId));

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        var roles = new List<string>();

        while (await reader.ReadAsync(cancellationToken))
        {
            roles.Add(reader.GetString(0));
        }

        return roles;
    }
}