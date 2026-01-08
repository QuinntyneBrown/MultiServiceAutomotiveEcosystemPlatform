[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$webAppRoot = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.WebApp'

Set-Location $webAppRoot

Write-Host "Installing frontend dependencies" -ForegroundColor Cyan
& npm install
