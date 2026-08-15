using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class EvidenceService : IEvidenceService
{
    private readonly IEvidenceRepository _evidenceRepository;
    private readonly IComplaintRepository _complaintRepository;
    private readonly IFileStorageService _fileStorageService;

    public EvidenceService(
     IEvidenceRepository evidenceRepository,
     IComplaintRepository complaintRepository,
     IFileStorageService fileStorageService)
    {
        _evidenceRepository = evidenceRepository;
        _complaintRepository = complaintRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<long> AddAsync(
    long complaintId,
    long uploadedByUserId,
    Stream fileStream,
    string fileName,
    string contentType,
    long fileSize,
    string? description,
    CancellationToken cancellationToken = default)
    {
        var complaint =
            await _complaintRepository.GetByIdAsync(
                complaintId,
                cancellationToken);

        if (complaint is null)
        {
            throw new KeyNotFoundException(
                $"Complaint {complaintId} was not found.");
        }

        if (fileStream is null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(
                "File name is required.");
        }

        if (string.IsNullOrWhiteSpace(contentType))
        {
            throw new ArgumentException(
                "Content type is required.");
        }

        if (fileSize <= 0)
        {
            throw new ArgumentException(
                "File size must be greater than zero.");
        }

        var storageKey =
            await _fileStorageService.SaveAsync(
                fileStream,
                fileName,
                cancellationToken);

        var evidence = new Evidence
        {
            ComplaintId = complaintId,
            UploadedByUserId = uploadedByUserId,
            FileName = fileName,
            StorageKey = storageKey,
            ContentType = contentType,
            FileSize = fileSize,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        return await _evidenceRepository.AddAsync(
            evidence,
            cancellationToken);
    }

    public async Task<Evidence?> GetByIdAsync(
        long evidenceId,
        CancellationToken cancellationToken = default)
    {
        return await _evidenceRepository.GetByIdAsync(
            evidenceId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Evidence>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        return await _evidenceRepository.GetByComplaintIdAsync(
            complaintId,
            cancellationToken);
    }
}