using System.Collections.Concurrent;
using DocumentProcessing.Api.Models;

namespace DocumentProcessing.Api.Storage;

public sealed class InMemoryJobStore
{
    private readonly ConcurrentDictionary<Guid, DocumentJob> _jobs = new();

    public bool TryAdd(DocumentJob job)
    {
        return _jobs.TryAdd(job.Id, job);
    }

    public DocumentJob? GetById(Guid id)
    {
        return _jobs.TryGetValue(id, out var job) ? job : null;
    }
}