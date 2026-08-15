using System.Data;
using FairPoint.Application.DTOs.Complaints;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IDbConnectionFactory _connectionFactory;

    public ComplaintService(
        IComplaintRepository complaintRepository, IDbConnectionFactory connectionFactory)
    {
        _complaintRepository = complaintRepository;
        _connectionFactory = connectionFactory;
    }

    public async Task<Complaint> CreateAsync(
        long userId,
        CreateComplaintRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException(
                "Complaint title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException(
                "Complaint description is required.");
        }

        var complaint = new Complaint
        {
            ComplaintNumber = GenerateComplaintNumber(),

            CreatedByUserId = userId,

            CategoryId = request.CategoryId,

            StatusId = 2, // SUBMITTED

            SeverityId = request.SeverityId,

            Title = request.Title.Trim(),

            Description = request.Description.Trim(),

            VisibilityCode = request.VisibilityCode,

            CreatedAt = DateTime.UtcNow
        };

        var complaintId =
            await _complaintRepository.CreateAsync(
                complaint,
                cancellationToken);

        complaint.ComplaintId = complaintId;

        return complaint;
    }

    public async Task<Complaint?> GetByIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        return await _complaintRepository.GetByIdAsync(
            complaintId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Complaint>> GetMyComplaintsAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return await _complaintRepository.GetByUserIdAsync(
            userId,
            cancellationToken);
    }
    public async Task<IReadOnlyList<Complaint>>
    GetPendingForModerationAsync(
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
            ComplaintId,
            CreatedByUserId,
            CategoryId,
            SeverityId,
            StatusId,
            Title,
            Description,
            CreatedAt,
            UpdatedAt
        FROM dbo.Complaints
        WHERE StatusId IN (2, 3, 4, 5, 6)
        ORDER BY CreatedAt ASC;
        """;

        var complaints =
            new List<Complaint>();

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            complaints.Add(new Complaint
            {
                ComplaintId =
                    Convert.ToInt64(
                        reader["ComplaintId"]),

                CreatedByUserId =
                    Convert.ToInt64(
                        reader["CreatedByUserId"]),

                CategoryId =
                    Convert.ToInt32(
                        reader["CategoryId"]),

                SeverityId =
                    Convert.ToInt32(
                        reader["SeverityId"]),

                StatusId =
                    Convert.ToInt32(
                        reader["StatusId"]),

                Title =
                    Convert.ToString(
                        reader["Title"])
                    ?? string.Empty,

                Description =
                    Convert.ToString(
                        reader["Description"])
                    ?? string.Empty,

                CreatedAt =
                    Convert.ToDateTime(
                        reader["CreatedAt"]),

                UpdatedAt =
                    reader["UpdatedAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["UpdatedAt"])
            });
        }

        return complaints;
    }
    private static string GenerateComplaintNumber()
    {
        return $"FP-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }
}