[CmdletBinding()]
param(
  [ValidateSet('Debug','Release')]
  [string]$Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

# Some environments set these variables, which can force dotnet/MSBuild to use a mismatched SDK.
# Clearing them for this process ensures we use the SDK selected by `dotnet`.
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

Write-Host "Restoring $apiProject" -ForegroundColor Cyan
& dotnet restore $apiProject -v minimal

Write-Host "Building API ($Configuration)" -ForegroundColor Cyan
& dotnet build $apiProject -c $Configuration -v minimal
