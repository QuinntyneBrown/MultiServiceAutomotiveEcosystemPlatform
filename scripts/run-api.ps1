[CmdletBinding()]
param(
  [ValidateSet('Development','Staging','Production')]
  [string]$Environment = 'Development'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$msbuildOverrideEnvVars = @(
  'MSBUILD_EXE_PATH',
  'MSBuildExtensionsPath',
  'MSBuildSDKsPath'
)

foreach ($name in $msbuildOverrideEnvVars) {
  if (Test-Path "Env:$name") {
    Remove-Item "Env:$name" -ErrorAction SilentlyContinue
  }
}

$apiProject = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj'

Write-Host "Running API (ASPNETCORE_ENVIRONMENT=$Environment)" -ForegroundColor Cyan
$env:ASPNETCORE_ENVIRONMENT = $Environment

& dotnet run --project $apiProject
