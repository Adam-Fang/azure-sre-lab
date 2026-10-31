using DocumentProcessing.Api.Models;
using DocumentProcessing.Api.Services;
using DocumentProcessing.Api.Storage;
using Xunit;

namespace DocumentProcessing.Api.Tests;

public sealed class JobServiceTests
{
    [Fact]
    public void Create_ValidName_SavesPendingJob()
    {
        // Arrange：准备独立的存储和服务
        var store = new InMemoryJobStore();
        var service = new JobService(store);

        // Act：创建任务
        var job = service.Create("  invoice.pdf  ");

        // Assert：验证业务结果
        Assert.NotEqual(Guid.Empty, job.Id);
        Assert.Equal("invoice.pdf", job.DocumentName);
        Assert.Equal(JobStatus.Pending, job.Status);
        Assert.Equal(job, service.GetById(job.Id));
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        var service = new JobService(new InMemoryJobStore());

        var result = service.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_BlankName_ThrowsArgumentException(string name)
    {
        var service = new JobService(new InMemoryJobStore());

        Assert.Throws<ArgumentException>(() => service.Create(name));
    }
}