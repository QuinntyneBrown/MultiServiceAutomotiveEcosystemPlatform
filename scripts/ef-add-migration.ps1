[CmdletBinding()]
param(
  [Parameter(Mandatory = $true)]
  [string]$Name
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$infrastructureProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj'
$startupProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj'

Write-Host "Adding EF migration '$Name'" -ForegroundColor Cyan
& dotnet ef migrations add $Name `
  --project $infrastructureProject `
  --startup-project $startupProject `
  --context MultiServiceAutomotiveEcosystemPlatformContext `
  --output-dir Migrations
