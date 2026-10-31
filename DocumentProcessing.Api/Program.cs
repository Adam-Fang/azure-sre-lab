using DocumentProcessing.Api.Storage;
using DocumentProcessing.Api.Services;
using DocumentProcessing.Api.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<InMemoryJobStore>();
builder.Services.AddTransient<JobService>();

var app = builder.Build();

// Swagger UI for local API testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// SRE health endpoint
app.MapHealthChecks("/health");

// First business endpoint
app.MapPost("/api/jobs", (
    CreateJobRequest request,
    JobService service) =>
{
    if (string.IsNullOrWhiteSpace(request.DocumentName))
    {
        return Results.BadRequest(new
        {
            Error = "Document name is required."
        });
    }

    var job = service.Create(request.DocumentName);

    return Results.Created($"/api/jobs/{job.Id}", job);
});

app.MapGet("/api/jobs/{id:guid}", (
    Guid id,
    JobService service) =>
{
    var job = service.GetById(id);

    if (job is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(job);
});

app.Run();
