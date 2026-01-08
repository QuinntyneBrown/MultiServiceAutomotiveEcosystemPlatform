[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$webAppRoot = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.WebApp'

Set-Location $webAppRoot

Write-Host "Running frontend unit tests" -ForegroundColor Cyan
& npm test -- --watch=false
