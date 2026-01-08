# MultiServiceAutomotiveEcosystemPlatform

## Prerequisites

- .NET SDK 9.0+ (API targets `net9.0`)

## API

### Build

- From the repo root:

	- `dotnet build .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj`

### Run (Dev)

- From the repo root:

	- `dotnet run --project .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj`

The console output will show the listening URLs.

### Database

- The default API connection string is configured in [src/MultiServiceAutomotiveEcosystemPlatform.Api/appsettings.json](src/MultiServiceAutomotiveEcosystemPlatform.Api/appsettings.json) and currently targets SQL Server Express:

	- `Server=.\SQLEXPRESS;Database=MultiServiceAutomotiveEcosystemPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`

### Tests

- From the repo root:

	- `dotnet test .\tests\MultiServiceAutomotiveEcosystemPlatform.Api.Tests\MultiServiceAutomotiveEcosystemPlatform.Api.Tests.csproj`

Note: the current integration tests use SQL Server LocalDB (`(localdb)\\mssqllocaldb`). If LocalDB isn't installed/available (common on Windows ARM64), these tests will fail until you point them at an available SQL Server instance or adjust the test harness.

## Troubleshooting

If you see restore/build failures referencing an unexpected .NET SDK folder (for example `C:\Program Files\dotnet\sdk\9.0.xxx`), your shell may have MSBuild override environment variables set (`MSBUILD_EXE_PATH`, `MSBuildExtensionsPath`, `MSBuildSDKsPath`).

- Use the helper script (clears those variables for the current process only) before building/running from that shell:

	- `powershell -ExecutionPolicy Bypass -File .\scripts\build-api.ps1`
