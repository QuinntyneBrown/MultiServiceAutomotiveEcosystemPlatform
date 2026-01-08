#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

unset MSBUILD_EXE_PATH MSBuildExtensionsPath MSBuildSDKsPath 2>/dev/null || true

ENVIRONMENT="${1:-Development}"
export ASPNETCORE_ENVIRONMENT="${ENVIRONMENT}"

echo "Running API (ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT})..."
dotnet run --project "src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj"
