[CmdletBinding()]
param(
  [switch]$NoRestore
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

$args = @('test', $sln)
if ($NoRestore) { $args += '--no-restore' }

Write-Host "Running backend tests" -ForegroundColor Cyan
& dotnet @args
