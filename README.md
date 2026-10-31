# Azure SRE Lab

A learning and portfolio project for building a document processing
platform with ASP.NET Core and gradually applying SRE practices.

## Current capabilities

- ASP.NET Core Web API targeting .NET 8
- Basic health endpoint
- Create and retrieve document job records
- In-memory job storage
- Swagger UI for manual API testing
- Automated tests for job service behavior

## Requirements

- .NET SDK 8.0.427, or a later patch in the 8.0.4xx feature band
- Optional: Visual Studio 2022 with the ASP.NET and web development workload

Docker and an Azure account are not required for the current implementation.

## Run locally

From the directory containing AzureSreLab.sln:

```powershell
dotnet restore AzureSreLab.sln
dotnet build AzureSreLab.sln --no-restore
dotnet run --project .\DocumentProcessing.Api\DocumentProcessing.Api.csproj --launch-profile https --no-build
```

The HTTPS launch profile listens on:

- HTTPS: https://localhost:7019
- HTTP: http://localhost:5033

Open:

- Swagger UI: https://localhost:7019/swagger/index.html
- Health check: https://localhost:7019/health

If HTTPS reports an untrusted development certificate, use:

```powershell
dotnet dev-certs https --trust
```

Approve the Windows certificate prompt only for your local development
environment.

## API

| Method | Endpoint | Behavior |
|--------|----------|----------|
| GET | /health | Returns 200 and Healthy for the basic health check |
| POST | /api/jobs | Creates a job record and returns 201 with a Location header |
| GET | /api/jobs/{id} | Returns 200 for an existing job, or 404 if not found |

Example POST request body:

```json
{
  "documentName": "invoice.pdf"
}
```

An empty or whitespace-only document name returns 400.

Use the returned job ID to query the job with GET /api/jobs/{id}.

## Run tests

```powershell
dotnet test AzureSreLab.sln
```

The current automated tests cover:

- Creating, trimming and retrieving a Pending job
- Looking up an unknown job ID
- Rejecting empty and whitespace-only document names

These tests exercise the service and in-memory storage.
They do not currently verify HTTP routing, serialization or DI wiring.

## Code structure

- Contracts: HTTP request data models
- Models: job data and status definitions
- Services: job creation and lookup behavior
- Storage: in-memory storage implementation
- Program.cs: application setup, DI registration and HTTP endpoints
- DocumentProcessing.Api.Tests: automated tests

## Current limitations

- Only document names are recorded; files are not uploaded or processed.
- Jobs remain Pending; no background worker is implemented.
- Job records are lost when the application process restarts.
- Storage is not shared between application instances.
- No durable queue, persistence, retries or fault tolerance is implemented.
- The health endpoint does not check processing readiness or external dependencies.
- Authentication and authorization are not implemented.
- The application is intended for local learning and development.

## Planned work

- Containerization and local Kubernetes
- Prometheus and Grafana observability
- GitHub Actions CI
- Terraform infrastructure
- Optional Azure deployment within an explicitly reviewed budget
- Load testing, SLOs, fault injection, runbooks and postmortems

Planned capabilities are not implemented unless listed above.