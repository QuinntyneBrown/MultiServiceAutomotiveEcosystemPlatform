[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$infrastructureProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj'
$startupProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj'

Write-Host "Applying EF migrations to database" -ForegroundColor Cyan
& dotnet ef database update `
  --project $infrastructureProject `
  --startup-project $startupProject `
  --context MultiServiceAutomotiveEcosystemPlatformContext
