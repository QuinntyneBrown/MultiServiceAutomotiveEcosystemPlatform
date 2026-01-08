# MultiServiceAutomotiveEcosystemPlatform

## Build API

- From the repo root:
	- `dotnet build .\src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj`

If you see restore/build failures referencing an unexpected .NET SDK folder (for example `C:\Program Files\dotnet\sdk\9.0.xxx`), your shell may have MSBuild override environment variables set (`MSBUILD_EXE_PATH`, `MSBuildExtensionsPath`, `MSBuildSDKsPath`).

- Use the helper script (clears those variables for the current process only):
	- `powershell -ExecutionPolicy Bypass -File .\scripts\build-api.ps1`