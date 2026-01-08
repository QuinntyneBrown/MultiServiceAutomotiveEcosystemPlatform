[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$infrastructureProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj'
$startupProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj'

Write-Host "Removing last EF migration" -ForegroundColor Cyan
& dotnet ef migrations remove `
  --project $infrastructureProject `
  --startup-project $startupProject `
  --context MultiServiceAutomotiveEcosystemPlatformContext
