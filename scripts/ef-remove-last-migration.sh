#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

echo "Removing last EF migration..."
dotnet ef migrations remove \
  --project "src/MultiServiceAutomotiveEcosystemPlatform.Infrastructure/MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj" \
  --startup-project "src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj" \
  --context MultiServiceAutomotiveEcosystemPlatformContext
