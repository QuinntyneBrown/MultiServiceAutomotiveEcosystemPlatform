#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="${SCRIPT_DIR}/.."
cd "${REPO_ROOT}/src/MultiServiceAutomotiveEcosystemPlatform.WebApp"

echo "Installing frontend dependencies..."
npm install
