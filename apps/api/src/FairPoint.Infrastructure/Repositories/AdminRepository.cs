using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AdminRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<User>> GetUsersAsync(
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                IsActive,
                IsVerified,
                ReputationScore,
                LastLoginAt,
                CreatedAt,
                UpdatedAt
            FROM dbo.Users
            ORDER BY CreatedAt DESC;
            """;

        var users = new List<User>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(MapUser(reader));
        }

        return users;
    }

    public async Task<User?> GetUserByIdAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                IsActive,
                IsVerified,
                ReputationScore,
                LastLoginAt,
                CreatedAt,
                UpdatedAt
            FROM dbo.Users
            WHERE UserId = @UserId;
            """;

        AddParameter(command, "@UserId", userId);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapUser(reader);
    }

    public async Task<bool> SetUserActiveAsync(
        long userId,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            UPDATE dbo.Users
            SET
                IsActive = @IsActive,
                UpdatedAt = SYSUTCDATETIME()
            WHERE UserId = @UserId;
            """;

        AddParameter(command, "@UserId", userId);
        AddParameter(command, "@IsActive", isActive);

        return command.ExecuteNonQuery() > 0;
    }

    public async Task<IReadOnlyList<int>>
        GetUserRoleIdsAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT RoleId
            FROM dbo.UserRoles
            WHERE UserId = @UserId;
            """;

        AddParameter(command, "@UserId", userId);

        var result = new List<int>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(Convert.ToInt32(reader["RoleId"]));
        }

        return result;
    }

    public async Task<bool> AssignRoleAsync(
        long userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            IF NOT EXISTS
            (
                SELECT 1
                FROM dbo.UserRoles
                WHERE UserId = @UserId
                  AND RoleId = @RoleId
            )
            BEGIN
                INSERT INTO dbo.UserRoles
                (
                    UserId,
                    RoleId,
                    CreatedAt
                )
                VALUES
                (
                    @UserId,
                    @RoleId,
                    SYSUTCDATETIME()
                );

                SELECT 1;
            END
            ELSE
            BEGIN
                SELECT 0;
            END;
            """;

        AddParameter(command, "@UserId", userId);
        AddParameter(command, "@RoleId", roleId);

        return Convert.ToInt32(
            command.ExecuteScalar()) == 1;
    }

    public async Task<bool> RemoveRoleAsync(
        long userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            DELETE FROM dbo.UserRoles
            WHERE UserId = @UserId
              AND RoleId = @RoleId;
            """;

        AddParameter(command, "@UserId", userId);
        AddParameter(command, "@RoleId", roleId);

        return command.ExecuteNonQuery() > 0;
    }

    public async Task<IReadOnlyList<Role>>
        GetRolesAsync(
            CancellationToken cancellationToken = default)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                RoleId,
                RoleName,
                Description,
                IsActive,
                CreatedAt
            FROM dbo.Roles
            ORDER BY RoleId;
            """;

        var roles = new List<Role>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            roles.Add(new Role
            {
                RoleId =
                    Convert.ToInt32(reader["RoleId"]),

                RoleName =
                    Convert.ToString(reader["RoleName"])
                    ?? string.Empty,

                Description =
                    reader["Description"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["Description"]),

                IsActive =
                    Convert.ToBoolean(reader["IsActive"]),

                CreatedAt =
                    Convert.ToDateTime(reader["CreatedAt"])
            });
        }

        return roles;
    }

    private static User MapUser(
        IDataRecord reader)
    {
        return new User
        {
            UserId =
                Convert.ToInt64(reader["UserId"]),

            FirstName =
                Convert.ToString(reader["FirstName"])
                ?? string.Empty,

            LastName =
                Convert.ToString(reader["LastName"])
                ?? string.Empty,

            Email =
                Convert.ToString(reader["Email"])
                ?? string.Empty,

            PhoneNumber =
                reader["PhoneNumber"] == DBNull.Value
                    ? null
                    : Convert.ToString(
                        reader["PhoneNumber"]),

            IsActive =
                Convert.ToBoolean(reader["IsActive"]),

            IsVerified =
                Convert.ToBoolean(reader["IsVerified"]),

            ReputationScore =
                Convert.ToInt32(
                    reader["ReputationScore"]),

            LastLoginAt =
                reader["LastLoginAt"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(
                        reader["LastLoginAt"]),

            CreatedAt =
                Convert.ToDateTime(
                    reader["CreatedAt"]),

            UpdatedAt =
                reader["UpdatedAt"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(
                        reader["UpdatedAt"])
        };
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