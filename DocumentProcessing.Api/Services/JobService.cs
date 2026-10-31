using DocumentProcessing.Api.Models;
using DocumentProcessing.Api.Storage;

namespace DocumentProcessing.Api.Services;

public sealed class JobService
{
    private readonly InMemoryJobStore _store;

    public JobService(InMemoryJobStore store)
    {
        _store = store;
    }

    public DocumentJob Create(string documentName)
    {
        if (string.IsNullOrWhiteSpace(documentName))
        {
            throw new ArgumentException(
                "Document name is required.",
                nameof(documentName));
        }

        var job = new DocumentJob(
            Guid.NewGuid(),
            documentName.Trim(),
            JobStatus.Pending,
            DateTimeOffset.UtcNow);

        if (!_store.TryAdd(job))
        {
            throw new InvalidOperationException(
                "Could not save the job because its ID already exists.");
        }

        return job;
    }

    public DocumentJob? GetById(Guid id)
    {
        return _store.GetById(id);
    }
}