# Scripts

This folder contains helper scripts for common backend/frontend tasks.

Each task is available in three formats:

- PowerShell: `*.ps1`
- Windows Batch: `*.bat`
- Bash: `*.sh`

All scripts are intended to be run from anywhere; they locate the repo root based on the script location.

## Backend (API/.NET)

- Build API:
  - `./scripts/build-api.ps1`
  - `./scripts/build-api.bat`
  - `./scripts/build-api.sh`

- Run API (Development):
  - `./scripts/run-api.ps1`
  - `./scripts/run-api.bat`
  - `./scripts/run-api.sh`

- Build solution:
  - `./scripts/build-backend.ps1`
  - `./scripts/build-backend.bat`
  - `./scripts/build-backend.sh`

- Test solution:
  - `./scripts/test-backend.ps1`
  - `./scripts/test-backend.bat`
  - `./scripts/test-backend.sh`

## EF Core

All EF scripts use:

- Project: `src/MultiServiceAutomotiveEcosystemPlatform.Infrastructure/MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj`
- Startup project: `src/MultiServiceAutomotiveEcosystemPlatform.Api/MultiServiceAutomotiveEcosystemPlatform.Api.csproj`
- Context: `MultiServiceAutomotiveEcosystemPlatformContext`

- Add migration (name required):
  - `./scripts/ef-add-migration.ps1 -Name MyMigration`
  - `./scripts/ef-add-migration.bat MyMigration`
  - `./scripts/ef-add-migration.sh MyMigration`

- Remove last migration:
  - `./scripts/ef-remove-last-migration.ps1`
  - `./scripts/ef-remove-last-migration.bat`
  - `./scripts/ef-remove-last-migration.sh`

- Apply migrations to DB:
  - `./scripts/ef-update-db.ps1`
  - `./scripts/ef-update-db.bat`
  - `./scripts/ef-update-db.sh`

## Frontend (Angular workspace)

Frontend working directory:

- `src/MultiServiceAutomotiveEcosystemPlatform.WebApp`

- Install dependencies:
  - `./scripts/frontend-install.ps1`
  - `./scripts/frontend-install.bat`
  - `./scripts/frontend-install.sh`

- Start dev server:
  - `./scripts/frontend-start.ps1`
  - `./scripts/frontend-start.bat`
  - `./scripts/frontend-start.sh`

- Build:
  - `./scripts/frontend-build.ps1`
  - `./scripts/frontend-build.bat`
  - `./scripts/frontend-build.sh`

- Unit tests (non-watch):
  - `./scripts/frontend-test.ps1`
  - `./scripts/frontend-test.bat`
  - `./scripts/frontend-test.sh`

- E2E tests (Playwright):
  - `./scripts/frontend-e2e.ps1`
  - `./scripts/frontend-e2e.bat`
  - `./scripts/frontend-e2e.sh`
