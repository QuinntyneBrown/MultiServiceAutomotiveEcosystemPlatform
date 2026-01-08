# MultiServiceAutomotiveEcosystemPlatform

Multi-Service Automotive Ecosystem Platform: a multi-tenant system connecting customers with automotive service professionals (referrals, customer/professional management, and related platform features).

This repository contains:
- A .NET 9 API (ASP.NET Core) with EF Core (SQL Server)
- A .NET Core domain layer + infrastructure layer
- An Angular workspace for the web application

## Repository Structure

- `src/`
	- `MultiServiceAutomotiveEcosystemPlatform.Api/` (ASP.NET Core API, Swagger in Development)
	- `MultiServiceAutomotiveEcosystemPlatform.Core/` (domain models, interfaces, core services)
	- `MultiServiceAutomotiveEcosystemPlatform.Infrastructure/` (EF Core DbContext, migrations, infra services)
	- `MultiServiceAutomotiveEcosystemPlatform.WebApp/` (Angular workspace)
- `tests/` (unit + integration tests)
- `docs/` (requirements/specs, design artifacts)
- `scripts/` (helper scripts)
- `infra/` (infrastructure-related assets)

## Prerequisites

- .NET SDK 9.x (projects target `net9.0`)
- Node.js (recommended: Node 20+) and npm (package manager in `src/MultiServiceAutomotiveEcosystemPlatform.WebApp/package.json` is `npm@10.9.4`)
- SQL Server (default config uses SQL Server Express)

## Quick Start

### API (Backend)

Build:

```powershell
dotnet build .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj
```

Run (Development):

```powershell
dotnet run --project .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj
```

Notes:
- The console output will show the listening URLs.
- Swagger UI is enabled in Development (look for `/swagger` on the printed URL).
- CORS is configured to allow `http://localhost:4200` (Angular dev server).

### WebApp (Frontend)

From the repo root:

```powershell
cd .\src\MultiServiceAutomotiveEcosystemPlatform.WebApp
npm install
npm start
```

Then open `http://localhost:4200/`.

## Database

### Connection String

The default API connection string lives in [src/MultiServiceAutomotiveEcosystemPlatform.Api/appsettings.json](src/MultiServiceAutomotiveEcosystemPlatform.Api/appsettings.json):

```text
Server=.\SQLEXPRESS;Database=MultiServiceAutomotiveEcosystemPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

### Migrations

EF Core migrations are stored in the Infrastructure project (`src/MultiServiceAutomotiveEcosystemPlatform.Infrastructure/Migrations`).

To apply migrations (from repo root):

```powershell
dotnet ef database update `
	--project .\src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj `
	--startup-project .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj
```

If you don’t have `dotnet-ef` installed, install it once:

```powershell
dotnet tool install --global dotnet-ef
```

## Tests

Run all tests:

```powershell
dotnet test .\MultiServiceAutomotiveEcosystemPlatform.sln
```

Or run the API test project only:

```powershell
dotnet test .\tests\MultiServiceAutomotiveEcosystemPlatform.Api.Tests\MultiServiceAutomotiveEcosystemPlatform.Api.Tests.csproj
```

Note: some integration tests use SQL Server LocalDB (`(localdb)\\mssqllocaldb`). If LocalDB isn't installed/available (common on Windows ARM64), point tests at an available SQL Server instance or adjust the test harness.

## Specs & Docs

- High-level docs: [docs/README.md](docs/README.md)
- Requirements + implementation constraints: [docs/specs/implementation-specs.md](docs/specs/implementation-specs.md)
- Feature breakdown by area (multi-tenancy, customers, professionals, referrals, etc.): [docs/specs/README.md](docs/specs/README.md)
- Roadmap/phases: [docs/specs/ROADMAP.md](docs/specs/ROADMAP.md)

## Troubleshooting

### MSBuild/.NET SDK mismatch

If you see restore/build failures referencing an unexpected .NET SDK folder, your shell may have MSBuild override environment variables set (`MSBUILD_EXE_PATH`, `MSBuildExtensionsPath`, `MSBuildSDKsPath`).

Use the helper script (clears those variables for the current process only) before building/running from that shell:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\build-api.ps1
```
