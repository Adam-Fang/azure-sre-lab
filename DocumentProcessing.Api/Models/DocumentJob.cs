namespace DocumentProcessing.Api.Models;

public enum JobStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

public sealed record DocumentJob(
    Guid Id,
    string DocumentName,
    JobStatus Status,
    DateTimeOffset CreatedAt);