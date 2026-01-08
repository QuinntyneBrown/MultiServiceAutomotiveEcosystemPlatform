#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

CONFIGURATION="${1:-Debug}"

unset MSBUILD_EXE_PATH MSBuildExtensionsPath MSBuildSDKsPath 2>/dev/null || true

echo "Restoring API..."
dotnet restore "src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj" -v minimal

echo "Building API (${CONFIGURATION})..."
dotnet build "src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj" -c "${CONFIGURATION}" -v minimal
