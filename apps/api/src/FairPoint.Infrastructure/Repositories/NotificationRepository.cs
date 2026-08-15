using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NotificationRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAsync(
        Notification notification,
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
            INSERT INTO dbo.Notifications
            (
                UserId,
                NotificationType,
                Title,
                Message,
                RelatedEntityType,
                RelatedEntityId,
                IsRead,
                CreatedAt
            )
            OUTPUT INSERTED.NotificationId
            VALUES
            (
                @UserId,
                @NotificationType,
                @Title,
                @Message,
                @RelatedEntityType,
                @RelatedEntityId,
                0,
                SYSUTCDATETIME()
            );
            """;

        AddParameter(command, "@UserId", notification.UserId);
        AddParameter(command, "@NotificationType", notification.NotificationType);
        AddParameter(command, "@Title", notification.Title);
        AddParameter(command, "@Message", notification.Message);
        AddParameter(command, "@RelatedEntityType", notification.RelatedEntityType);
        AddParameter(command, "@RelatedEntityId", notification.RelatedEntityId);

        var result = command.ExecuteScalar();

        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<Notification>>
        GetByUserIdAsync(
            long userId,
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
                NotificationId,
                UserId,
                NotificationType,
                Title,
                Message,
                RelatedEntityType,
                RelatedEntityId,
                IsRead,
                CreatedAt,
                ReadAt
            FROM dbo.Notifications
            WHERE UserId = @UserId
            ORDER BY CreatedAt DESC;
            """;

        AddParameter(command, "@UserId", userId);

        var notifications =
            new List<Notification>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            notifications.Add(new Notification
            {
                NotificationId =
                    Convert.ToInt64(reader["NotificationId"]),

                UserId =
                    Convert.ToInt64(reader["UserId"]),

                NotificationType =
                    Convert.ToString(reader["NotificationType"])
                    ?? string.Empty,

                Title =
                    Convert.ToString(reader["Title"])
                    ?? string.Empty,

                Message =
                    Convert.ToString(reader["Message"])
                    ?? string.Empty,

                RelatedEntityType =
                    reader["RelatedEntityType"] == DBNull.Value
                        ? null
                        : Convert.ToString(
                            reader["RelatedEntityType"]),

                RelatedEntityId =
                    reader["RelatedEntityId"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(
                            reader["RelatedEntityId"]),

                IsRead =
                    Convert.ToBoolean(reader["IsRead"]),

                CreatedAt =
                    Convert.ToDateTime(reader["CreatedAt"]),

                ReadAt =
                    reader["ReadAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["ReadAt"])
            });
        }

        return notifications;
    }

    public async Task<bool> MarkAsReadAsync(
        long notificationId,
        long userId,
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
            UPDATE dbo.Notifications
            SET
                IsRead = 1,
                ReadAt = SYSUTCDATETIME()
            WHERE NotificationId = @NotificationId
              AND UserId = @UserId
              AND IsRead = 0;
            """;

        AddParameter(
            command,
            "@NotificationId",
            notificationId);

        AddParameter(
            command,
            "@UserId",
            userId);

        var affectedRows =
            command.ExecuteNonQuery();

        return affectedRows > 0;
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