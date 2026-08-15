using FairPoint.Application.Interfaces;

namespace FairPoint.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _storageRoot;

    public LocalFileStorageService()
    {
        _storageRoot = Path.Combine(
            AppContext.BaseDirectory,
            "Storage",
            "Evidence");

        Directory.CreateDirectory(_storageRoot);
    }

    public async Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName);

        var storageKey =
            $"{Guid.NewGuid():N}{extension}";

        var fullPath =
            Path.Combine(_storageRoot, storageKey);

        await using var outputStream =
            new FileStream(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None);

        await fileStream.CopyToAsync(
            outputStream,
            cancellationToken);

        return storageKey;
    }

    public Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        var fullPath =
            Path.Combine(_storageRoot, storageKey);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}