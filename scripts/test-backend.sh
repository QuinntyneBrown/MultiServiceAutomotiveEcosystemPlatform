#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

unset MSBUILD_EXE_PATH MSBuildExtensionsPath MSBuildSDKsPath 2>/dev/null || true

NORESTORE=""
if [[ "${1:-}" == "--no-restore" ]]; then
  NORESTORE="--no-restore"
fi

echo "Running backend tests..."
dotnet test "MultiServiceAutomotiveEcosystemPlatform.sln" ${NORESTORE}
