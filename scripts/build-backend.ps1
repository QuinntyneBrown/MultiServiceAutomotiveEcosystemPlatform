[CmdletBinding()]
param(
  [ValidateSet('Debug','Release')]
  [string]$Configuration = 'Debug'
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

$sln = Join-Path $repoRoot 'MultiServiceAutomotiveEcosystemPlatform.sln'

Write-Host "Restoring solution" -ForegroundColor Cyan
& dotnet restore $sln -v minimal

Write-Host "Building solution ($Configuration)" -ForegroundColor Cyan
& dotnet build $sln -c $Configuration -v minimal
