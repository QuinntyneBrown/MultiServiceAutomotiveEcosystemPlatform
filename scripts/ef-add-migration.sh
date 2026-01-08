#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $(basename "$0") MigrationName" >&2
  exit 2
fi

NAME="$1"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}"

echo "Adding EF migration '${NAME}'..."
dotnet ef migrations add "${NAME}" \
  --project "src/MultiServiceAutomotiveEcosystemPlatform.Infrastructure/MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj" \
  --startup-project "src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj" \
  --context MultiServiceAutomotiveEcosystemPlatformContext \
  --output-dir Migrations
