#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

CONFIGURATION="${1:-Debug}"

unset MSBUILD_EXE_PATH MSBuildExtensionsPath MSBuildSDKsPath 2>/dev/null || true

echo "Restoring solution..."
dotnet restore "MultiServiceAutomotiveEcosystemPlatform.sln" -v minimal

echo "Building solution (${CONFIGURATION})..."
dotnet build "MultiServiceAutomotiveEcosystemPlatform.sln" -c "${CONFIGURATION}" -v minimal
