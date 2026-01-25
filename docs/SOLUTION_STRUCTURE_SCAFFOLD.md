# Solution Structure Scaffold

This document provides the concrete file structure and key configurations for the microservices migration.

---

## Directory Structure

```
MultiServiceAutomotiveEcosystemPlatform/
│
├── src/
│   ├── AppHost/
│   │   ├── AppHost.csproj
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ServiceDefaults/
│   │   ├── ServiceDefaults.csproj
│   │   └── Extensions.cs
│   │
│   ├── ApiGateway/
│   │   ├── ApiGateway.csproj
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── Shared/
│   │   ├── Shared.Contracts/
│   │   │   ├── Shared.Contracts.csproj
│   │   │   ├── Events/
│   │   │   │   ├── IIntegrationEvent.cs
│   │   │   │   ├── Tenant/
│   │   │   │   │   └── TenantEvents.cs
│   │   │   │   ├── Customer/
│   │   │   │   │   └── CustomerEvents.cs
│   │   │   │   ├── Professional/
│   │   │   │   │   └── ProfessionalEvents.cs
│   │   │   │   └── Referral/
│   │   │   │       └── ReferralEvents.cs
│   │   │   ├── DTOs/
│   │   │   │   ├── Common/
│   │   │   │   │   ├── PagedResult.cs
│   │   │   │   │   ├── PaginationRequest.cs
│   │   │   │   │   └── ApiResponse.cs
│   │   │   │   ├── TenantDto.cs
│   │   │   │   ├── CustomerDto.cs
│   │   │   │   ├── ProfessionalDto.cs
│   │   │   │   └── ReferralDto.cs
│   │   │   ├── Enums/
│   │   │   │   ├── TenantStatus.cs
│   │   │   │   ├── CustomerStatus.cs
│   │   │   │   ├── ProfessionalStatus.cs
│   │   │   │   ├── ReferralStatus.cs
│   │   │   │   └── RewardStatus.cs
│   │   │   └── Interfaces/
│   │   │       ├── IEntity.cs
│   │   │       ├── IAggregateRoot.cs
│   │   │       └── ITenantEntity.cs
│   │   │
│   │   ├── Shared.Messaging/
│   │   │   ├── Shared.Messaging.csproj
│   │   │   ├── Extensions/
│   │   │   │   └── MessagingExtensions.cs
│   │   │   ├── Configuration/
│   │   │   │   └── RabbitMqSettings.cs
│   │   │   └── Behaviors/
│   │   │       └── PublishEventBehavior.cs
│   │   │
│   │   └── Shared.Infrastructure/
│   │       ├── Shared.Infrastructure.csproj
│   │       ├── Persistence/
│   │       │   ├── BaseDbContext.cs
│   │       │   ├── OutboxMessage.cs
│   │       │   └── OutboxProcessor.cs
│   │       ├── Middleware/
│   │       │   ├── TenantContextMiddleware.cs
│   │       │   ├── CorrelationIdMiddleware.cs
│   │       │   └── ExceptionHandlingMiddleware.cs
│   │       ├── Extensions/
│   │       │   ├── ServiceCollectionExtensions.cs
│   │       │   └── ApplicationBuilderExtensions.cs
│   │       ├── Services/
│   │       │   ├── ITenantContext.cs
│   │       │   └── TenantContext.cs
│   │       └── Behaviors/
│   │           ├── ValidationBehavior.cs
│   │           └── LoggingBehavior.cs
│   │
│   └── Services/
│       ├── Tenant/
│       │   ├── Tenant.Api/
│       │   │   ├── Tenant.Api.csproj
│       │   │   ├── Program.cs
│       │   │   ├── appsettings.json
│       │   │   ├── Controllers/
│       │   │   │   └── TenantsController.cs
│       │   │   ├── Commands/
│       │   │   │   ├── CreateTenant/
│       │   │   │   │   ├── CreateTenantCommand.cs
│       │   │   │   │   ├── CreateTenantCommandHandler.cs
│       │   │   │   │   └── CreateTenantCommandValidator.cs
│       │   │   │   └── UpdateTenant/
│       │   │   │       ├── UpdateTenantCommand.cs
│       │   │   │       ├── UpdateTenantCommandHandler.cs
│       │   │   │       └── UpdateTenantCommandValidator.cs
│       │   │   └── Queries/
│       │   │       ├── GetTenant/
│       │   │       │   ├── GetTenantQuery.cs
│       │   │       │   └── GetTenantQueryHandler.cs
│       │   │       └── GetTenants/
│       │   │           ├── GetTenantsQuery.cs
│       │   │           └── GetTenantsQueryHandler.cs
│       │   ├── Tenant.Core/
│       │   │   ├── Tenant.Core.csproj
│       │   │   ├── Entities/
│       │   │   │   └── Tenant.cs
│       │   │   └── Interfaces/
│       │   │       └── ITenantRepository.cs
│       │   └── Tenant.Infrastructure/
│       │       ├── Tenant.Infrastructure.csproj
│       │       ├── Data/
│       │       │   ├── TenantDbContext.cs
│       │       │   └── Configurations/
│       │       │       └── TenantConfiguration.cs
│       │       └── Migrations/
│       │
│       ├── Customer/
│       │   ├── Customer.Api/
│       │   │   ├── Customer.Api.csproj
│       │   │   ├── Program.cs
│       │   │   ├── appsettings.json
│       │   │   ├── Controllers/
│       │   │   │   └── CustomersController.cs
│       │   │   ├── Commands/
│       │   │   │   └── CreateCustomer/
│       │   │   │       ├── CreateCustomerCommand.cs
│       │   │   │       ├── CreateCustomerCommandHandler.cs
│       │   │   │       └── CreateCustomerCommandValidator.cs
│       │   │   ├── Queries/
│       │   │   │   └── GetCustomer/
│       │   │   │       ├── GetCustomerQuery.cs
│       │   │   │       └── GetCustomerQueryHandler.cs
│       │   │   └── Consumers/
│       │   │       ├── TenantCreatedConsumer.cs
│       │   │       └── ProfessionalCreatedConsumer.cs
│       │   ├── Customer.Core/
│       │   │   ├── Customer.Core.csproj
│       │   │   ├── Entities/
│       │   │   │   ├── Customer.cs
│       │   │   │   └── CustomerOwnershipHistory.cs
│       │   │   └── Interfaces/
│       │   │       └── ICustomerRepository.cs
│       │   └── Customer.Infrastructure/
│       │       ├── Customer.Infrastructure.csproj
│       │       ├── Data/
│       │       │   ├── CustomerDbContext.cs
│       │       │   ├── TenantCache.cs
│       │       │   └── Configurations/
│       │       │       ├── CustomerConfiguration.cs
│       │       │       └── CustomerOwnershipHistoryConfiguration.cs
│       │       └── Migrations/
│       │
│       ├── Professional/
│       │   ├── Professional.Api/
│       │   │   ├── Professional.Api.csproj
│       │   │   ├── Program.cs
│       │   │   ├── appsettings.json
│       │   │   ├── Controllers/
│       │   │   │   ├── ProfessionalsController.cs
│       │   │   │   └── SpecialtiesController.cs
│       │   │   ├── Commands/
│       │   │   │   └── CreateProfessional/
│       │   │   │       ├── CreateProfessionalCommand.cs
│       │   │   │       ├── CreateProfessionalCommandHandler.cs
│       │   │   │       └── CreateProfessionalCommandValidator.cs
│       │   │   ├── Queries/
│       │   │   │   └── GetProfessional/
│       │   │   │       ├── GetProfessionalQuery.cs
│       │   │   │       └── GetProfessionalQueryHandler.cs
│       │   │   └── Consumers/
│       │   │       ├── TenantCreatedConsumer.cs
│       │   │       └── CustomerCreatedConsumer.cs
│       │   ├── Professional.Core/
│       │   │   ├── Professional.Core.csproj
│       │   │   ├── Entities/
│       │   │   │   ├── Professional.cs
│       │   │   │   ├── ProfessionalSpecialty.cs
│       │   │   │   └── SpecialtyCatalog.cs
│       │   │   └── Interfaces/
│       │   │       └── IProfessionalRepository.cs
│       │   └── Professional.Infrastructure/
│       │       ├── Professional.Infrastructure.csproj
│       │       ├── Data/
│       │       │   ├── ProfessionalDbContext.cs
│       │       │   ├── TenantCache.cs
│       │       │   └── Configurations/
│       │       │       ├── ProfessionalConfiguration.cs
│       │       │       ├── ProfessionalSpecialtyConfiguration.cs
│       │       │       └── SpecialtyCatalogConfiguration.cs
│       │       └── Migrations/
│       │
│       └── Referral/
│           ├── Referral.Api/
│           │   ├── Referral.Api.csproj
│           │   ├── Program.cs
│           │   ├── appsettings.json
│           │   ├── Controllers/
│           │   │   ├── CustomerReferralsController.cs
│           │   │   ├── ProfessionalReferralsController.cs
│           │   │   └── ReferralCodesController.cs
│           │   ├── Commands/
│           │   │   ├── CreateCustomerReferral/
│           │   │   │   ├── CreateCustomerReferralCommand.cs
│           │   │   │   ├── CreateCustomerReferralCommandHandler.cs
│           │   │   │   └── CreateCustomerReferralCommandValidator.cs
│           │   │   └── CreateProfessionalReferral/
│           │   │       ├── CreateProfessionalReferralCommand.cs
│           │   │       ├── CreateProfessionalReferralCommandHandler.cs
│           │   │       └── CreateProfessionalReferralCommandValidator.cs
│           │   ├── Queries/
│           │   │   └── GetReferral/
│           │   │       ├── GetReferralQuery.cs
│           │   │       └── GetReferralQueryHandler.cs
│           │   ├── Consumers/
│           │   │   ├── CustomerCreatedConsumer.cs
│           │   │   ├── CustomerOwnershipChangedConsumer.cs
│           │   │   └── ProfessionalCreatedConsumer.cs
│           │   └── Sagas/
│           │       ├── ReferralConversion/
│           │       │   ├── ReferralConversionSaga.cs
│           │       │   └── ReferralConversionSagaState.cs
│           │       └── RewardPayment/
│           │           ├── RewardPaymentSaga.cs
│           │           └── RewardPaymentSagaState.cs
│           ├── Referral.Core/
│           │   ├── Referral.Core.csproj
│           │   ├── Entities/
│           │   │   ├── CustomerReferral.cs
│           │   │   ├── ProfessionalReferral.cs
│           │   │   ├── ReferralCode.cs
│           │   │   └── ReferralStats.cs
│           │   ├── Services/
│           │   │   └── ReferralCodeGenerator.cs
│           │   └── Interfaces/
│           │       └── IReferralRepository.cs
│           └── Referral.Infrastructure/
│               ├── Referral.Infrastructure.csproj
│               ├── Data/
│               │   ├── ReferralDbContext.cs
│               │   ├── CustomerCache.cs
│               │   ├── ProfessionalCache.cs
│               │   └── Configurations/
│               │       ├── CustomerReferralConfiguration.cs
│               │       ├── ProfessionalReferralConfiguration.cs
│               │       ├── ReferralCodeConfiguration.cs
│               │       └── ReferralStatsConfiguration.cs
│               └── Migrations/
│
├── tests/
│   ├── Tenant.Api.Tests/
│   │   ├── Tenant.Api.Tests.csproj
│   │   ├── Unit/
│   │   │   └── Handlers/
│   │   │       └── CreateTenantCommandHandlerTests.cs
│   │   └── Integration/
│   │       └── Controllers/
│   │           └── TenantsControllerTests.cs
│   ├── Customer.Api.Tests/
│   │   ├── Customer.Api.Tests.csproj
│   │   ├── Unit/
│   │   └── Integration/
│   ├── Professional.Api.Tests/
│   │   ├── Professional.Api.Tests.csproj
│   │   ├── Unit/
│   │   └── Integration/
│   ├── Referral.Api.Tests/
│   │   ├── Referral.Api.Tests.csproj
│   │   ├── Unit/
│   │   └── Integration/
│   ├── Integration.Tests/
│   │   ├── Integration.Tests.csproj
│   │   ├── Scenarios/
│   │   │   ├── CustomerReferralScenarioTests.cs
│   │   │   └── ProfessionalReferralScenarioTests.cs
│   │   └── Infrastructure/
│   │       └── IntegrationTestFixture.cs
│   └── Testing/
│       ├── Testing.csproj
│       ├── Builders/
│       ├── Fixtures/
│       └── Fakes/
│
└── MultiServiceAutomotiveEcosystemPlatform.sln
```

---

## Project Files

### AppHost/AppHost.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsAspireHost>true</IsAspireHost>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting" Version="9.0.0" />
    <PackageReference Include="Aspire.Hosting.AppHost" Version="9.0.0" />
    <PackageReference Include="Aspire.Hosting.RabbitMQ" Version="9.0.0" />
    <PackageReference Include="Aspire.Hosting.Redis" Version="9.0.0" />
    <PackageReference Include="Aspire.Hosting.SqlServer" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ApiGateway\ApiGateway.csproj" />
    <ProjectReference Include="..\Services\Tenant\Tenant.Api\Tenant.Api.csproj" />
    <ProjectReference Include="..\Services\Customer\Customer.Api\Customer.Api.csproj" />
    <ProjectReference Include="..\Services\Professional\Professional.Api\Professional.Api.csproj" />
    <ProjectReference Include="..\Services\Referral\Referral.Api\Referral.Api.csproj" />
  </ItemGroup>

</Project>
```

### ServiceDefaults/ServiceDefaults.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsAspireSharedProject>true</IsAspireSharedProject>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" Version="9.0.0" />
    <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.9.0" />
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.9.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.9.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.9.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" Version="1.9.0" />
  </ItemGroup>

</Project>
```

### ApiGateway/ApiGateway.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Yarp.ReverseProxy" Version="2.1.0" />
    <PackageReference Include="AspNetCore.HealthChecks.UI.Client" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ServiceDefaults\ServiceDefaults.csproj" />
  </ItemGroup>

</Project>
```

### Shared/Shared.Contracts/Shared.Contracts.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

### Shared/Shared.Messaging/Shared.Messaging.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MassTransit" Version="8.2.5" />
    <PackageReference Include="MassTransit.RabbitMQ" Version="8.2.5" />
    <PackageReference Include="MassTransit.EntityFrameworkCore" Version="8.2.5" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Shared.Contracts\Shared.Contracts.csproj" />
  </ItemGroup>

</Project>
```

### Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.1.1" />
    <PackageReference Include="MediatR" Version="14.0.0" />
    <PackageReference Include="Polly" Version="8.3.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Shared.Contracts\Shared.Contracts.csproj" />
    <ProjectReference Include="..\Shared.Messaging\Shared.Messaging.csproj" />
  </ItemGroup>

</Project>
```

### Services/Tenant/Tenant.Api/Tenant.Api.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageReference Include="Aspire.StackExchange.Redis.DistributedCaching" Version="9.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.7.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\..\ServiceDefaults\ServiceDefaults.csproj" />
    <ProjectReference Include="..\..\..\Shared\Shared.Infrastructure\Shared.Infrastructure.csproj" />
    <ProjectReference Include="..\Tenant.Core\Tenant.Core.csproj" />
    <ProjectReference Include="..\Tenant.Infrastructure\Tenant.Infrastructure.csproj" />
  </ItemGroup>

</Project>
```

### Services/Tenant/Tenant.Core/Tenant.Core.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\..\Shared\Shared.Contracts\Shared.Contracts.csproj" />
  </ItemGroup>

</Project>
```

### Services/Tenant/Tenant.Infrastructure/Tenant.Infrastructure.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Tenant.Core\Tenant.Core.csproj" />
  </ItemGroup>

</Project>
```

---

## Solution File Updates

Add to `MultiServiceAutomotiveEcosystemPlatform.sln`:

```
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "AppHost", "src\AppHost\AppHost.csproj", "{NEW-GUID-1}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "ServiceDefaults", "src\ServiceDefaults\ServiceDefaults.csproj", "{NEW-GUID-2}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "ApiGateway", "src\ApiGateway\ApiGateway.csproj", "{NEW-GUID-3}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Shared.Contracts", "src\Shared\Shared.Contracts\Shared.Contracts.csproj", "{NEW-GUID-4}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Shared.Messaging", "src\Shared\Shared.Messaging\Shared.Messaging.csproj", "{NEW-GUID-5}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Shared.Infrastructure", "src\Shared\Shared.Infrastructure\Shared.Infrastructure.csproj", "{NEW-GUID-6}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Tenant.Api", "src\Services\Tenant\Tenant.Api\Tenant.Api.csproj", "{NEW-GUID-7}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Tenant.Core", "src\Services\Tenant\Tenant.Core\Tenant.Core.csproj", "{NEW-GUID-8}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Tenant.Infrastructure", "src\Services\Tenant\Tenant.Infrastructure\Tenant.Infrastructure.csproj", "{NEW-GUID-9}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Customer.Api", "src\Services\Customer\Customer.Api\Customer.Api.csproj", "{NEW-GUID-10}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Customer.Core", "src\Services\Customer\Customer.Core\Customer.Core.csproj", "{NEW-GUID-11}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Customer.Infrastructure", "src\Services\Customer\Customer.Infrastructure\Customer.Infrastructure.csproj", "{NEW-GUID-12}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Professional.Api", "src\Services\Professional\Professional.Api\Professional.Api.csproj", "{NEW-GUID-13}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Professional.Core", "src\Services\Professional\Professional.Core\Professional.Core.csproj", "{NEW-GUID-14}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Professional.Infrastructure", "src\Services\Professional\Professional.Infrastructure\Professional.Infrastructure.csproj", "{NEW-GUID-15}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Referral.Api", "src\Services\Referral\Referral.Api\Referral.Api.csproj", "{NEW-GUID-16}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Referral.Core", "src\Services\Referral\Referral.Core\Referral.Core.csproj", "{NEW-GUID-17}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Referral.Infrastructure", "src\Services\Referral\Referral.Infrastructure\Referral.Infrastructure.csproj", "{NEW-GUID-18}"
EndProject
```

---

## Solution Folders Structure

Organize the solution in Visual Studio/Rider with these solution folders:

```
Solution 'MultiServiceAutomotiveEcosystemPlatform'
│
├── src/
│   ├── Aspire/
│   │   ├── AppHost
│   │   └── ServiceDefaults
│   │
│   ├── Gateway/
│   │   └── ApiGateway
│   │
│   ├── Shared/
│   │   ├── Shared.Contracts
│   │   ├── Shared.Messaging
│   │   └── Shared.Infrastructure
│   │
│   ├── Services/
│   │   ├── Tenant/
│   │   │   ├── Tenant.Api
│   │   │   ├── Tenant.Core
│   │   │   └── Tenant.Infrastructure
│   │   │
│   │   ├── Customer/
│   │   │   ├── Customer.Api
│   │   │   ├── Customer.Core
│   │   │   └── Customer.Infrastructure
│   │   │
│   │   ├── Professional/
│   │   │   ├── Professional.Api
│   │   │   ├── Professional.Core
│   │   │   └── Professional.Infrastructure
│   │   │
│   │   └── Referral/
│   │       ├── Referral.Api
│   │       ├── Referral.Core
│   │       └── Referral.Infrastructure
│   │
│   └── Legacy/ (Existing monolith - to be deprecated)
│       ├── MultiServiceAutomotiveEcosystemPlatform.Api
│       ├── MultiServiceAutomotiveEcosystemPlatform.Core
│       └── MultiServiceAutomotiveEcosystemPlatform.Infrastructure
│
├── tests/
│   ├── Service.Tests/
│   │   ├── Tenant.Api.Tests
│   │   ├── Customer.Api.Tests
│   │   ├── Professional.Api.Tests
│   │   └── Referral.Api.Tests
│   │
│   ├── Integration.Tests
│   │
│   └── Testing (Shared utilities)
│
└── docs/
```

---

## Port Assignments

| Service | HTTP Port | HTTPS Port |
|---------|-----------|------------|
| AppHost Dashboard | 15000 | 15001 |
| API Gateway | 5000 | 5001 |
| Tenant Service | 5010 | 5011 |
| Customer Service | 5020 | 5021 |
| Professional Service | 5030 | 5031 |
| Referral Service | 5040 | 5041 |
| RabbitMQ | 5672 | - |
| RabbitMQ Management | 15672 | - |
| Redis | 6379 | - |
| SQL Server | 1433 | - |

---

## Launch Configuration

### launchSettings.json for AppHost

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:15001;http://localhost:15000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "DOTNET_ENVIRONMENT": "Development",
        "DOTNET_DASHBOARD_OTLP_ENDPOINT_URL": "https://localhost:21001",
        "DOTNET_RESOURCE_SERVICE_ENDPOINT_URL": "https://localhost:22001"
      }
    }
  }
}
```

---

## Quick Start Commands

```powershell
# Create the solution structure
dotnet new aspire -n MultiServiceAutomotiveEcosystemPlatform -o .

# Add service defaults
cd src
dotnet new aspire-servicedefaults -n ServiceDefaults

# Add API Gateway
dotnet new webapi -n ApiGateway
cd ApiGateway
dotnet add package Yarp.ReverseProxy

# Add Shared projects
cd ../Shared
dotnet new classlib -n Shared.Contracts
dotnet new classlib -n Shared.Messaging
dotnet new classlib -n Shared.Infrastructure

# Add Tenant microservice
cd ../Services/Tenant
dotnet new webapi -n Tenant.Api
dotnet new classlib -n Tenant.Core
dotnet new classlib -n Tenant.Infrastructure

# Repeat for Customer, Professional, Referral services...

# Run the solution
cd ../../..
dotnet run --project src/AppHost
```

---

## Migration Checklist

### Phase 1: Foundation
- [ ] Create AppHost project
- [ ] Create ServiceDefaults project
- [ ] Create Shared.Contracts project
- [ ] Create Shared.Messaging project
- [ ] Create Shared.Infrastructure project
- [ ] Define all integration events
- [ ] Configure RabbitMQ in AppHost
- [ ] Configure Redis in AppHost

### Phase 2: API Gateway
- [ ] Create ApiGateway project
- [ ] Configure YARP routing
- [ ] Add authentication middleware
- [ ] Add rate limiting
- [ ] Configure CORS
- [ ] Add health check endpoints

### Phase 3-6: Microservices
For each service (Tenant, Customer, Professional, Referral):
- [ ] Create [Service].Api project
- [ ] Create [Service].Core project
- [ ] Create [Service].Infrastructure project
- [ ] Migrate entities from monolith
- [ ] Implement commands/queries
- [ ] Add event publishing
- [ ] Add event consumers
- [ ] Create database migrations
- [ ] Write unit tests
- [ ] Write integration tests
- [ ] Register in AppHost
- [ ] Configure gateway routing

### Phase 7: Integration Testing
- [ ] Create Integration.Tests project
- [ ] Write end-to-end scenarios
- [ ] Test saga workflows
- [ ] Perform load testing
- [ ] Verify data consistency

### Phase 8: UI Integration
- [ ] Update Angular environment files
- [ ] Test all UI workflows
- [ ] Update error handling

### Phase 9: Deprecate Monolith
- [ ] Verify data migration
- [ ] Archive monolith code
- [ ] Update documentation
