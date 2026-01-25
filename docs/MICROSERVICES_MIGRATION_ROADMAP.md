# Event-Driven Microservices Migration Roadmap

## Using .NET Aspire, YARP API Gateway, and MassTransit

---

## Executive Summary

This roadmap outlines the comprehensive migration strategy for transforming the Multi-Service Automotive Ecosystem Platform from a monolithic architecture to an event-driven microservices architecture using:

- **.NET Aspire** - Cloud-native orchestration and service discovery
- **YARP (Yet Another Reverse Proxy)** - API Gateway for routing and load balancing
- **MassTransit with RabbitMQ** - Event-driven messaging infrastructure
- **Domain-Driven Design** - Bounded contexts and aggregate-based service decomposition

---

## Table of Contents

1. [Current State Analysis](#1-current-state-analysis)
2. [Target Architecture](#2-target-architecture)
3. [Solution Structure](#3-solution-structure)
4. [Microservices Decomposition](#4-microservices-decomposition)
5. [Event-Driven Architecture](#5-event-driven-architecture)
6. [Shared Library Design](#6-shared-library-design)
7. [API Gateway Configuration](#7-api-gateway-configuration)
8. [Database Strategy](#8-database-strategy)
9. [Migration Phases](#9-migration-phases)
10. [Technical Implementation Details](#10-technical-implementation-details)
11. [Testing Strategy](#11-testing-strategy)
12. [Deployment Strategy](#12-deployment-strategy)
13. [Monitoring and Observability](#13-monitoring-and-observability)

---

## 1. Current State Analysis

### Current Architecture
```
┌─────────────────────────────────────────────────────────────┐
│                    Angular UI (SPA)                         │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│         MultiServiceAutomotiveEcosystemPlatform.Api         │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────┐   │
│  │ Customers   │ │Professionals│ │     Referrals       │   │
│  │ Controller  │ │ Controller  │ │     Controller      │   │
│  └─────────────┘ └─────────────┘ └─────────────────────┘   │
│                         MediatR                             │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│    MultiServiceAutomotiveEcosystemPlatform.Core             │
│  ┌──────────┐ ┌────────────┐ ┌──────────┐ ┌──────────┐    │
│  │ Customer │ │Professional│ │ Referral │ │  Tenant  │    │
│  │Aggregate │ │ Aggregate  │ │Aggregate │ │Aggregate │    │
│  └──────────┘ └────────────┘ └──────────┘ └──────────┘    │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│  MultiServiceAutomotiveEcosystemPlatform.Infrastructure     │
│              Single SQL Server Database                      │
└─────────────────────────────────────────────────────────────┘
```

### Identified Bounded Contexts (Aggregates)
| Bounded Context | Entities | Current Location |
|-----------------|----------|------------------|
| **Tenant** | Tenant | Core/Aggregates/Tenant |
| **Customer** | Customer, CustomerOwnershipHistory | Core/Aggregates/Customer |
| **Professional** | Professional, ProfessionalSpecialty, SpecialtyCatalog | Core/Aggregates/Professional |
| **Referral** | CustomerReferral, ProfessionalReferral, ReferralCode, ReferralStats | Core/Aggregates/Referral |

### Current Technology Stack
- .NET 9.0
- Entity Framework Core 9.0
- SQL Server Express
- MediatR 14.0 (CQRS)
- FluentValidation 12.1
- Angular 17+ (UI)

---

## 2. Target Architecture

### High-Level Architecture
```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              Clients                                         │
│         (Angular SPA, Mobile Apps, Third-Party Integrations)                │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                        API Gateway (YARP)                                    │
│                         src/ApiGateway                                       │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌─────────────────────┐   │
│  │   Routing   │ │Load Balance │ │Rate Limiting│ │   Authentication    │   │
│  └─────────────┘ └─────────────┘ └─────────────┘ └─────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────────┘
          │                │                │                │
          ▼                ▼                ▼                ▼
┌─────────────┐  ┌─────────────────┐  ┌─────────────┐  ┌─────────────┐
│   Tenant    │  │    Customer     │  │ Professional│  │  Referral   │
│   Service   │  │    Service      │  │   Service   │  │   Service   │
│             │  │                 │  │             │  │             │
│ ┌─────────┐ │  │ ┌─────────────┐ │  │ ┌─────────┐ │  │ ┌─────────┐ │
│ │   API   │ │  │ │     API     │ │  │ │   API   │ │  │ │   API   │ │
│ ├─────────┤ │  │ ├─────────────┤ │  │ ├─────────┤ │  │ ├─────────┤ │
│ │  Core   │ │  │ │    Core     │ │  │ │  Core   │ │  │ │  Core   │ │
│ ├─────────┤ │  │ ├─────────────┤ │  │ ├─────────┤ │  │ ├─────────┤ │
│ │Infra    │ │  │ │   Infra     │ │  │ │Infra    │ │  │ │Infra    │ │
│ └─────────┘ │  │ └─────────────┘ │  │ └─────────┘ │  │ └─────────┘ │
└──────┬──────┘  └────────┬────────┘  └──────┬──────┘  └──────┬──────┘
       │                  │                  │                │
       ▼                  ▼                  ▼                ▼
   ┌───────┐          ┌───────┐          ┌───────┐        ┌───────┐
   │Tenant │          │Customer│         │ Prof  │        │Referral│
   │  DB   │          │  DB   │          │  DB   │        │  DB   │
   └───────┘          └───────┘          └───────┘        └───────┘

                              │
                    ┌─────────┴─────────┐
                    │                   │
                    ▼                   ▼
          ┌─────────────────┐  ┌─────────────────┐
          │    RabbitMQ     │  │   Redis Cache   │
          │  Message Broker │  │  (Distributed)  │
          └─────────────────┘  └─────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                        .NET Aspire AppHost                                   │
│                    (Orchestration & Service Discovery)                       │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 3. Solution Structure

### Target Folder Structure
```
MultiServiceAutomotiveEcosystemPlatform/
│
├── src/
│   │
│   ├── AppHost/                                    # .NET Aspire Orchestration
│   │   ├── AppHost.csproj
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ServiceDefaults/                            # Aspire Service Defaults
│   │   ├── ServiceDefaults.csproj
│   │   └── Extensions.cs
│   │
│   ├── ApiGateway/                                 # YARP API Gateway
│   │   ├── ApiGateway.csproj
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── yarp.json                               # YARP routing configuration
│   │
│   ├── Shared/                                     # Shared Libraries
│   │   ├── Shared.Contracts/                       # DTOs, Events, Interfaces
│   │   │   ├── Shared.Contracts.csproj
│   │   │   ├── Events/
│   │   │   │   ├── Tenant/
│   │   │   │   ├── Customer/
│   │   │   │   ├── Professional/
│   │   │   │   └── Referral/
│   │   │   ├── DTOs/
│   │   │   └── Interfaces/
│   │   │
│   │   ├── Shared.Messaging/                       # MassTransit Configuration
│   │   │   ├── Shared.Messaging.csproj
│   │   │   ├── Extensions/
│   │   │   ├── Consumers/
│   │   │   └── Configuration/
│   │   │
│   │   └── Shared.Infrastructure/                  # Common Infrastructure
│   │       ├── Shared.Infrastructure.csproj
│   │       ├── Persistence/
│   │       ├── Middleware/
│   │       ├── Extensions/
│   │       └── Services/
│   │
│   ├── Services/
│   │   │
│   │   ├── Tenant/                                 # Tenant Microservice
│   │   │   ├── Tenant.Api/
│   │   │   │   ├── Tenant.Api.csproj
│   │   │   │   ├── Program.cs
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   └── Consumers/
│   │   │   ├── Tenant.Core/
│   │   │   │   ├── Tenant.Core.csproj
│   │   │   │   ├── Entities/
│   │   │   │   ├── Events/
│   │   │   │   ├── Interfaces/
│   │   │   │   └── Services/
│   │   │   └── Tenant.Infrastructure/
│   │   │       ├── Tenant.Infrastructure.csproj
│   │   │       ├── Data/
│   │   │       └── Migrations/
│   │   │
│   │   ├── Customer/                               # Customer Microservice
│   │   │   ├── Customer.Api/
│   │   │   │   ├── Customer.Api.csproj
│   │   │   │   ├── Program.cs
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   └── Consumers/
│   │   │   ├── Customer.Core/
│   │   │   │   ├── Customer.Core.csproj
│   │   │   │   ├── Entities/
│   │   │   │   ├── Events/
│   │   │   │   ├── Interfaces/
│   │   │   │   └── Services/
│   │   │   └── Customer.Infrastructure/
│   │   │       ├── Customer.Infrastructure.csproj
│   │   │       ├── Data/
│   │   │       └── Migrations/
│   │   │
│   │   ├── Professional/                           # Professional Microservice
│   │   │   ├── Professional.Api/
│   │   │   │   ├── Professional.Api.csproj
│   │   │   │   ├── Program.cs
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   └── Consumers/
│   │   │   ├── Professional.Core/
│   │   │   │   ├── Professional.Core.csproj
│   │   │   │   ├── Entities/
│   │   │   │   ├── Events/
│   │   │   │   ├── Interfaces/
│   │   │   │   └── Services/
│   │   │   └── Professional.Infrastructure/
│   │   │       ├── Professional.Infrastructure.csproj
│   │   │       ├── Data/
│   │   │       └── Migrations/
│   │   │
│   │   └── Referral/                               # Referral Microservice
│   │       ├── Referral.Api/
│   │       │   ├── Referral.Api.csproj
│   │       │   ├── Program.cs
│   │       │   ├── Controllers/
│   │       │   ├── Commands/
│   │       │   ├── Queries/
│   │       │   ├── Consumers/
│   │       │   └── Sagas/
│   │       ├── Referral.Core/
│   │       │   ├── Referral.Core.csproj
│   │       │   ├── Entities/
│   │       │   ├── Events/
│   │       │   ├── Interfaces/
│   │       │   └── Services/
│   │       └── Referral.Infrastructure/
│   │           ├── Referral.Infrastructure.csproj
│   │           ├── Data/
│   │           └── Migrations/
│   │
│   └── Ui/                                         # Angular Frontend (existing)
│       └── (existing Angular application)
│
├── tests/
│   ├── Tenant.Api.Tests/
│   ├── Tenant.Core.Tests/
│   ├── Customer.Api.Tests/
│   ├── Customer.Core.Tests/
│   ├── Professional.Api.Tests/
│   ├── Professional.Core.Tests/
│   ├── Referral.Api.Tests/
│   ├── Referral.Core.Tests/
│   ├── Integration.Tests/                          # Cross-service integration tests
│   └── Testing/                                    # Shared test utilities
│
├── docs/
│   ├── architecture/
│   ├── api-specifications/
│   └── runbooks/
│
├── scripts/
│   ├── migrations/
│   └── deployment/
│
└── MultiServiceAutomotiveEcosystemPlatform.sln
```

---

## 4. Microservices Decomposition

### Service Definitions

#### 4.1 Tenant Service
**Responsibility:** Multi-tenancy management, tenant configuration, branding

| Aspect | Details |
|--------|---------|
| **Port** | 5001 |
| **Base Route** | `/api/tenants` |
| **Database** | TenantDb |
| **Entities** | Tenant |
| **Events Published** | TenantCreated, TenantUpdated, TenantSuspended, TenantActivated |
| **Events Consumed** | None (foundational service) |

**API Endpoints:**
```
POST   /api/tenants              Create tenant
GET    /api/tenants/{id}         Get tenant by ID
GET    /api/tenants/slug/{slug}  Get tenant by slug
GET    /api/tenants              List tenants (admin only)
PUT    /api/tenants/{id}         Update tenant
PATCH  /api/tenants/{id}/status  Update tenant status
DELETE /api/tenants/{id}         Soft delete tenant
```

---

#### 4.2 Customer Service
**Responsibility:** Customer management, ownership tracking, customer lifecycle

| Aspect | Details |
|--------|---------|
| **Port** | 5002 |
| **Base Route** | `/api/customers` |
| **Database** | CustomerDb |
| **Entities** | Customer, CustomerOwnershipHistory |
| **Events Published** | CustomerCreated, CustomerUpdated, CustomerOwnershipChanged, CustomerActivated, CustomerBlocked |
| **Events Consumed** | TenantCreated (cache tenant info), ProfessionalCreated (for ownership assignment) |

**API Endpoints:**
```
POST   /api/customers                           Create customer
GET    /api/customers/{id}                      Get customer by ID
GET    /api/customers                           List customers (paginated)
PUT    /api/customers/{id}                      Update customer
PATCH  /api/customers/{id}/status               Update customer status
POST   /api/customers/{id}/assign-owner         Assign to professional
POST   /api/customers/{id}/transfer             Transfer ownership
GET    /api/customers/{id}/ownership-history    Get ownership history
POST   /api/customers/{id}/verify-email         Verify email
POST   /api/customers/{id}/verify-phone         Verify phone
```

---

#### 4.3 Professional Service
**Responsibility:** Professional management, specialties, verification

| Aspect | Details |
|--------|---------|
| **Port** | 5003 |
| **Base Route** | `/api/professionals` |
| **Database** | ProfessionalDb |
| **Entities** | Professional, ProfessionalSpecialty, SpecialtyCatalog |
| **Events Published** | ProfessionalCreated, ProfessionalUpdated, ProfessionalVerified, ProfessionalSuspended, SpecialtyAdded |
| **Events Consumed** | TenantCreated (cache tenant info), CustomerCreated (for statistics) |

**API Endpoints:**
```
POST   /api/professionals                          Create professional
GET    /api/professionals/{id}                     Get professional by ID
GET    /api/professionals/slug/{slug}              Get by slug
GET    /api/professionals                          List professionals (paginated)
PUT    /api/professionals/{id}                     Update professional
PATCH  /api/professionals/{id}/status              Update status
POST   /api/professionals/{id}/verify              Verify professional
POST   /api/professionals/{id}/specialties         Add specialty
DELETE /api/professionals/{id}/specialties/{sid}   Remove specialty
GET    /api/specialties                            List specialty catalog
POST   /api/specialties                            Create specialty (admin)
```

---

#### 4.4 Referral Service
**Responsibility:** Referral lifecycle, rewards, referral codes, statistics

| Aspect | Details |
|--------|---------|
| **Port** | 5004 |
| **Base Route** | `/api/referrals` |
| **Database** | ReferralDb |
| **Entities** | CustomerReferral, ProfessionalReferral, ReferralCode, ReferralStats |
| **Events Published** | CustomerReferralCreated, CustomerReferralConverted, ProfessionalReferralCreated, ProfessionalReferralAccepted, RewardApproved, RewardPaid |
| **Events Consumed** | CustomerCreated, ProfessionalCreated, CustomerOwnershipChanged |
| **Sagas** | ReferralConversionSaga, RewardPaymentSaga |

**API Endpoints:**
```
# Customer Referrals
POST   /api/referrals/customer                        Create customer referral
GET    /api/referrals/customer/{customerId}           Get customer referrals
GET    /api/referrals/customer/{id}/details           Get referral details
PATCH  /api/referrals/customer/{id}/status            Update referral status
POST   /api/referrals/customer/{id}/convert           Mark as converted

# Professional Referrals
POST   /api/referrals/professional                    Create professional referral
GET    /api/referrals/professional/{professionalId}   Get professional referrals
GET    /api/referrals/professional/{id}/details       Get referral details
PATCH  /api/referrals/professional/{id}/accept        Accept referral
PATCH  /api/referrals/professional/{id}/decline       Decline referral

# Referral Codes
POST   /api/referrals/codes                           Create referral code
GET    /api/referrals/codes/{code}                    Get code details
GET    /api/referrals/codes/owner/{ownerId}           Get codes by owner
POST   /api/referrals/codes/{code}/validate           Validate code
POST   /api/referrals/codes/{code}/use                Record code usage

# Statistics
GET    /api/referrals/stats/{ownerId}                 Get referral statistics
GET    /api/referrals/stats/summary                   Get summary stats (admin)
```

---

## 5. Event-Driven Architecture

### 5.1 Event Catalog

#### Tenant Events
```csharp
namespace Shared.Contracts.Events.Tenant;

public record TenantCreated(
    Guid TenantId,
    string Name,
    string Slug,
    DateTime CreatedAt
);

public record TenantUpdated(
    Guid TenantId,
    string Name,
    string DisplayName,
    string? LogoUrl,
    string? PrimaryColor,
    DateTime UpdatedAt
);

public record TenantStatusChanged(
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
);
```

#### Customer Events
```csharp
namespace Shared.Contracts.Events.Customer;

public record CustomerCreated(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    DateTime CreatedAt
);

public record CustomerUpdated(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    DateTime UpdatedAt
);

public record CustomerOwnershipChanged(
    Guid CustomerId,
    Guid TenantId,
    Guid? PreviousOwnerId,
    Guid NewOwnerId,
    string Reason,
    DateTime ChangedAt
);

public record CustomerStatusChanged(
    Guid CustomerId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
);

public record CustomerVerified(
    Guid CustomerId,
    Guid TenantId,
    string VerificationType, // Email, Phone
    DateTime VerifiedAt
);
```

#### Professional Events
```csharp
namespace Shared.Contracts.Events.Professional;

public record ProfessionalCreated(
    Guid ProfessionalId,
    Guid TenantId,
    string BusinessName,
    string Email,
    string Slug,
    DateTime CreatedAt
);

public record ProfessionalUpdated(
    Guid ProfessionalId,
    Guid TenantId,
    string BusinessName,
    DateTime UpdatedAt
);

public record ProfessionalVerified(
    Guid ProfessionalId,
    Guid TenantId,
    DateTime VerifiedAt
);

public record ProfessionalStatusChanged(
    Guid ProfessionalId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
);

public record ProfessionalSpecialtyAdded(
    Guid ProfessionalId,
    Guid TenantId,
    Guid SpecialtyId,
    string SpecialtyName,
    DateTime AddedAt
);
```

#### Referral Events
```csharp
namespace Shared.Contracts.Events.Referral;

public record CustomerReferralCreated(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferringCustomerId,
    string ReferredEmail,
    string? ReferralCode,
    DateTime CreatedAt
);

public record CustomerReferralStatusChanged(
    Guid ReferralId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
);

public record CustomerReferralConverted(
    Guid ReferralId,
    Guid TenantId,
    Guid NewCustomerId,
    decimal? RewardAmount,
    DateTime ConvertedAt
);

public record ProfessionalReferralCreated(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferringProfessionalId,
    Guid ReferredProfessionalId,
    DateTime CreatedAt
);

public record ProfessionalReferralAccepted(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferredProfessionalId,
    DateTime AcceptedAt
);

public record ProfessionalReferralDeclined(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferredProfessionalId,
    string? Reason,
    DateTime DeclinedAt
);

public record RewardApproved(
    Guid ReferralId,
    Guid TenantId,
    Guid RecipientId,
    string RecipientType, // Customer, Professional
    decimal Amount,
    string RewardType,
    DateTime ApprovedAt
);

public record RewardPaid(
    Guid ReferralId,
    Guid TenantId,
    Guid RecipientId,
    decimal Amount,
    DateTime PaidAt
);
```

### 5.2 Event Flow Diagrams

#### Customer Referral Flow
```
┌──────────────┐     POST /referrals/customer     ┌──────────────────┐
│    Client    │ ────────────────────────────────►│ Referral Service │
└──────────────┘                                  └────────┬─────────┘
                                                           │
                                                           │ Publish
                                                           ▼
                                                  ┌────────────────────┐
                                                  │ CustomerReferral   │
                                                  │     Created        │
                                                  └────────┬───────────┘
                           ┌───────────────────────────────┼───────────────────────┐
                           │                               │                       │
                           ▼                               ▼                       ▼
                  ┌─────────────────┐           ┌─────────────────┐    ┌─────────────────────┐
                  │Customer Service │           │Notification Svc │    │  Analytics Service  │
                  │(Update stats)   │           │ (Send email)    │    │   (Track metrics)   │
                  └─────────────────┘           └─────────────────┘    └─────────────────────┘
```

#### Referral Conversion Saga
```
┌────────────────────────────────────────────────────────────────────────────────┐
│                         Referral Conversion Saga                                │
├────────────────────────────────────────────────────────────────────────────────┤
│                                                                                │
│  1. CustomerReferralConverted (Trigger)                                        │
│           │                                                                    │
│           ▼                                                                    │
│  2. Validate referral code usage                                               │
│           │                                                                    │
│           ▼                                                                    │
│  3. Create new customer (→ CustomerCreated)                                    │
│           │                                                                    │
│           ▼                                                                    │
│  4. Calculate reward amount                                                    │
│           │                                                                    │
│           ▼                                                                    │
│  5. Approve reward (→ RewardApproved)                                          │
│           │                                                                    │
│           ▼                                                                    │
│  6. Update referral statistics                                                 │
│           │                                                                    │
│           ▼                                                                    │
│  7. Saga Complete                                                              │
│                                                                                │
│  Compensating Actions (on failure):                                            │
│  - Rollback customer creation                                                  │
│  - Cancel reward approval                                                      │
│  - Restore referral code usage count                                           │
│                                                                                │
└────────────────────────────────────────────────────────────────────────────────┘
```

### 5.3 Message Broker Configuration

**RabbitMQ Exchanges and Queues:**
```
Exchanges:
├── tenant-events (topic)
│   ├── tenant.created
│   ├── tenant.updated
│   └── tenant.status-changed
│
├── customer-events (topic)
│   ├── customer.created
│   ├── customer.updated
│   ├── customer.ownership-changed
│   └── customer.status-changed
│
├── professional-events (topic)
│   ├── professional.created
│   ├── professional.updated
│   ├── professional.verified
│   └── professional.status-changed
│
└── referral-events (topic)
    ├── referral.customer.created
    ├── referral.customer.converted
    ├── referral.professional.created
    ├── referral.professional.accepted
    ├── reward.approved
    └── reward.paid

Queues:
├── customer-service-tenant-events
├── customer-service-professional-events
├── professional-service-tenant-events
├── professional-service-customer-events
├── referral-service-customer-events
├── referral-service-professional-events
└── saga-referral-conversion
```

---

## 6. Shared Library Design

### 6.1 Shared.Contracts

**Purpose:** Contains all shared contracts including events, DTOs, and interfaces.

```csharp
// Shared.Contracts.csproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

**Structure:**
```
Shared.Contracts/
├── Events/
│   ├── IIntegrationEvent.cs
│   ├── Tenant/
│   │   └── TenantEvents.cs
│   ├── Customer/
│   │   └── CustomerEvents.cs
│   ├── Professional/
│   │   └── ProfessionalEvents.cs
│   └── Referral/
│       └── ReferralEvents.cs
├── DTOs/
│   ├── Common/
│   │   ├── PagedResult.cs
│   │   ├── PaginationRequest.cs
│   │   └── ApiResponse.cs
│   ├── Tenant/
│   │   └── TenantDto.cs
│   ├── Customer/
│   │   └── CustomerDto.cs
│   ├── Professional/
│   │   └── ProfessionalDto.cs
│   └── Referral/
│       └── ReferralDto.cs
├── Enums/
│   ├── TenantStatus.cs
│   ├── CustomerStatus.cs
│   ├── ProfessionalStatus.cs
│   └── ReferralStatus.cs
└── Interfaces/
    ├── IEntity.cs
    ├── IAggregateRoot.cs
    └── ITenantEntity.cs
```

### 6.2 Shared.Messaging

**Purpose:** MassTransit configuration and common messaging infrastructure.

```csharp
// Shared.Messaging.csproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MassTransit" Version="8.2.0" />
    <PackageReference Include="MassTransit.RabbitMQ" Version="8.2.0" />
    <PackageReference Include="MassTransit.EntityFrameworkCore" Version="8.2.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Shared.Contracts\Shared.Contracts.csproj" />
  </ItemGroup>
</Project>
```

**Key Components:**
```csharp
// Extensions/MessagingExtensions.cs
namespace Shared.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddMassTransit(x =>
        {
            configureConsumers?.Invoke(x);

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSettings = configuration
                    .GetSection("RabbitMQ")
                    .Get<RabbitMqSettings>()!;

                cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
```

### 6.3 Shared.Infrastructure

**Purpose:** Common infrastructure components shared across services.

```csharp
// Shared.Infrastructure.csproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.1.1" />
    <PackageReference Include="MediatR" Version="14.0.0" />
    <PackageReference Include="Polly" Version="8.3.0" />
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.7.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Shared.Contracts\Shared.Contracts.csproj" />
    <ProjectReference Include="..\Shared.Messaging\Shared.Messaging.csproj" />
  </ItemGroup>
</Project>
```

**Key Components:**
```
Shared.Infrastructure/
├── Persistence/
│   ├── BaseDbContext.cs
│   ├── OutboxMessage.cs
│   └── OutboxProcessor.cs
├── Middleware/
│   ├── TenantContextMiddleware.cs
│   ├── CorrelationIdMiddleware.cs
│   └── ExceptionHandlingMiddleware.cs
├── Extensions/
│   ├── ServiceCollectionExtensions.cs
│   ├── ApplicationBuilderExtensions.cs
│   └── HealthCheckExtensions.cs
├── Services/
│   ├── TenantContext.cs
│   ├── ITenantContext.cs
│   └── CurrentUserService.cs
└── Behaviors/
    ├── ValidationBehavior.cs
    ├── LoggingBehavior.cs
    └── TransactionBehavior.cs
```

---

## 7. API Gateway Configuration

### 7.1 YARP Configuration

**Project Structure:**
```
src/ApiGateway/
├── ApiGateway.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── yarp.json
```

**ApiGateway.csproj:**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Yarp.ReverseProxy" Version="2.1.0" />
    <PackageReference Include="AspNetCore.HealthChecks.UI.Client" Version="8.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ServiceDefaults\ServiceDefaults.csproj" />
  </ItemGroup>
</Project>
```

**Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Identity:Authority"];
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 100;
        opt.QueueLimit = 10;
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowUI");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapReverseProxy();
app.MapDefaultEndpoints(); // Aspire health endpoints

app.Run();
```

**yarp.json:**
```json
{
  "ReverseProxy": {
    "Routes": {
      "tenant-route": {
        "ClusterId": "tenant-cluster",
        "Match": {
          "Path": "/api/tenants/{**catch-all}"
        },
        "Transforms": [
          { "PathPattern": "/api/tenants/{**catch-all}" }
        ]
      },
      "customer-route": {
        "ClusterId": "customer-cluster",
        "Match": {
          "Path": "/api/customers/{**catch-all}"
        },
        "Transforms": [
          { "PathPattern": "/api/customers/{**catch-all}" }
        ]
      },
      "professional-route": {
        "ClusterId": "professional-cluster",
        "Match": {
          "Path": "/api/professionals/{**catch-all}"
        },
        "Transforms": [
          { "PathPattern": "/api/professionals/{**catch-all}" }
        ]
      },
      "professional-specialties-route": {
        "ClusterId": "professional-cluster",
        "Match": {
          "Path": "/api/specialties/{**catch-all}"
        },
        "Transforms": [
          { "PathPattern": "/api/specialties/{**catch-all}" }
        ]
      },
      "referral-route": {
        "ClusterId": "referral-cluster",
        "Match": {
          "Path": "/api/referrals/{**catch-all}"
        },
        "Transforms": [
          { "PathPattern": "/api/referrals/{**catch-all}" }
        ]
      }
    },
    "Clusters": {
      "tenant-cluster": {
        "Destinations": {
          "tenant-api": {
            "Address": "http://tenant-api"
          }
        },
        "HealthCheck": {
          "Active": {
            "Enabled": true,
            "Interval": "00:00:10",
            "Timeout": "00:00:05",
            "Path": "/health"
          }
        }
      },
      "customer-cluster": {
        "Destinations": {
          "customer-api": {
            "Address": "http://customer-api"
          }
        },
        "HealthCheck": {
          "Active": {
            "Enabled": true,
            "Interval": "00:00:10",
            "Timeout": "00:00:05",
            "Path": "/health"
          }
        }
      },
      "professional-cluster": {
        "Destinations": {
          "professional-api": {
            "Address": "http://professional-api"
          }
        },
        "HealthCheck": {
          "Active": {
            "Enabled": true,
            "Interval": "00:00:10",
            "Timeout": "00:00:05",
            "Path": "/health"
          }
        }
      },
      "referral-cluster": {
        "Destinations": {
          "referral-api": {
            "Address": "http://referral-api"
          }
        },
        "HealthCheck": {
          "Active": {
            "Enabled": true,
            "Interval": "00:00:10",
            "Timeout": "00:00:05",
            "Path": "/health"
          }
        }
      }
    }
  }
}
```

---

## 8. Database Strategy

### 8.1 Database per Service

| Service | Database Name | Tables |
|---------|---------------|--------|
| **Tenant** | TenantDb | Tenants, OutboxMessages |
| **Customer** | CustomerDb | Customers, CustomerOwnershipHistories, TenantCache, OutboxMessages |
| **Professional** | ProfessionalDb | Professionals, ProfessionalSpecialties, SpecialtyCatalogs, TenantCache, OutboxMessages |
| **Referral** | ReferralDb | CustomerReferrals, ProfessionalReferrals, ReferralCodes, ReferralStats, CustomerCache, ProfessionalCache, OutboxMessages |

### 8.2 Data Migration Strategy

**Transactional Outbox Pattern:**
```csharp
// Each service implements outbox for reliable event publishing
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; }
    public string? Error { get; set; }
}
```

**Cache Tables for Cross-Service Data:**
```csharp
// Customer Service - TenantCache
public class TenantCache
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}

// Referral Service - CustomerCache
public class CustomerCache
{
    public Guid CustomerId { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
```

### 8.3 Migration Scripts

**Phase 1: Create new databases**
```sql
-- Create separate databases
CREATE DATABASE TenantDb;
CREATE DATABASE CustomerDb;
CREATE DATABASE ProfessionalDb;
CREATE DATABASE ReferralDb;
```

**Phase 2: Data migration (per service)**
```sql
-- Example: Migrate Tenants
INSERT INTO TenantDb.dbo.Tenants
SELECT * FROM MultiServiceAutomotiveEcosystemPlatform.dbo.Tenants;

-- Example: Migrate Customers
INSERT INTO CustomerDb.dbo.Customers
SELECT * FROM MultiServiceAutomotiveEcosystemPlatform.dbo.Customers;
```

---

## 9. Migration Phases

### Phase 1: Foundation (Weeks 1-2)

**Objectives:**
- Set up .NET Aspire infrastructure
- Create shared libraries
- Establish messaging infrastructure

**Tasks:**
1. Create AppHost project
2. Create ServiceDefaults project
3. Create Shared.Contracts project
4. Create Shared.Messaging project
5. Create Shared.Infrastructure project
6. Set up RabbitMQ container
7. Set up Redis container (caching)
8. Define all integration events
9. Create base DbContext with outbox support

**Deliverables:**
- [ ] AppHost running with service discovery
- [ ] Shared libraries published
- [ ] RabbitMQ configured and running
- [ ] Event contracts defined

---

### Phase 2: API Gateway (Weeks 2-3)

**Objectives:**
- Implement YARP-based API Gateway
- Configure routing and load balancing
- Add cross-cutting concerns

**Tasks:**
1. Create ApiGateway project
2. Configure YARP reverse proxy
3. Implement authentication pass-through
4. Add rate limiting
5. Configure CORS
6. Add health check aggregation
7. Implement request/response logging
8. Add correlation ID propagation

**Deliverables:**
- [ ] API Gateway running
- [ ] All routes configured
- [ ] Authentication working
- [ ] Health checks aggregated

---

### Phase 3: Tenant Service (Weeks 3-4)

**Objectives:**
- Extract Tenant bounded context
- First microservice operational

**Tasks:**
1. Create Tenant.Api project
2. Create Tenant.Core project
3. Create Tenant.Infrastructure project
4. Migrate Tenant entity and configuration
5. Implement commands and queries
6. Add event publishing (TenantCreated, etc.)
7. Create database and migrations
8. Write unit and integration tests
9. Register in AppHost
10. Configure gateway routing

**Deliverables:**
- [ ] Tenant service running independently
- [ ] API endpoints functional
- [ ] Events publishing to RabbitMQ
- [ ] Tests passing

---

### Phase 4: Customer Service (Weeks 4-6)

**Objectives:**
- Extract Customer bounded context
- Implement event consumption

**Tasks:**
1. Create Customer.Api project
2. Create Customer.Core project
3. Create Customer.Infrastructure project
4. Migrate Customer and CustomerOwnershipHistory entities
5. Implement commands and queries
6. Add event publishing
7. Implement TenantCreated consumer (cache tenant info)
8. Create database and migrations
9. Write unit and integration tests
10. Register in AppHost

**Deliverables:**
- [ ] Customer service running independently
- [ ] Consuming tenant events
- [ ] Publishing customer events
- [ ] Tests passing

---

### Phase 5: Professional Service (Weeks 6-8)

**Objectives:**
- Extract Professional bounded context
- Handle specialty catalog

**Tasks:**
1. Create Professional.Api project
2. Create Professional.Core project
3. Create Professional.Infrastructure project
4. Migrate Professional, ProfessionalSpecialty, SpecialtyCatalog entities
5. Implement commands and queries
6. Add event publishing
7. Implement TenantCreated consumer
8. Implement CustomerCreated consumer (for statistics)
9. Create database and migrations
10. Write unit and integration tests
11. Register in AppHost

**Deliverables:**
- [ ] Professional service running independently
- [ ] Consuming tenant and customer events
- [ ] Publishing professional events
- [ ] Specialty catalog management working
- [ ] Tests passing

---

### Phase 6: Referral Service (Weeks 8-10)

**Objectives:**
- Extract Referral bounded context
- Implement saga patterns

**Tasks:**
1. Create Referral.Api project
2. Create Referral.Core project
3. Create Referral.Infrastructure project
4. Migrate all referral entities
5. Implement commands and queries
6. Add event publishing
7. Implement event consumers (Customer, Professional)
8. Implement ReferralConversionSaga
9. Implement RewardPaymentSaga
10. Create database and migrations
11. Write unit and integration tests
12. Register in AppHost

**Deliverables:**
- [ ] Referral service running independently
- [ ] All event consumers working
- [ ] Sagas handling complex workflows
- [ ] Referral code system functional
- [ ] Tests passing

---

### Phase 7: Integration & Testing (Weeks 10-11)

**Objectives:**
- End-to-end testing
- Performance testing
- Bug fixes

**Tasks:**
1. Create Integration.Tests project
2. Write end-to-end test scenarios
3. Test saga compensation
4. Load testing with k6 or NBomber
5. Fix discovered issues
6. Verify data consistency
7. Test failover scenarios

**Deliverables:**
- [ ] All integration tests passing
- [ ] Performance benchmarks met
- [ ] Resilience verified
- [ ] Bug-free services

---

### Phase 8: UI Integration (Weeks 11-12)

**Objectives:**
- Update Angular UI
- Point to API Gateway

**Tasks:**
1. Update Angular environment configurations
2. Update API base URL to gateway
3. Test all UI flows
4. Handle new error responses
5. Update any changed DTOs

**Deliverables:**
- [ ] UI working with gateway
- [ ] All features functional
- [ ] Error handling updated

---

### Phase 9: Deprecate Monolith (Weeks 12-13)

**Objectives:**
- Sunset original monolith
- Clean up

**Tasks:**
1. Verify all data migrated
2. Run parallel for safety period
3. Redirect all traffic to gateway
4. Archive monolith code
5. Remove old database (after backup)
6. Update documentation

**Deliverables:**
- [ ] Monolith decommissioned
- [ ] All traffic through microservices
- [ ] Documentation updated

---

## 10. Technical Implementation Details

### 10.1 AppHost Configuration

**AppHost/Program.cs:**
```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var redis = builder.AddRedis("redis");

// Databases
var tenantDb = builder.AddSqlServer("sql")
    .AddDatabase("tenant-db");

var customerDb = builder.AddSqlServer("sql")
    .AddDatabase("customer-db");

var professionalDb = builder.AddSqlServer("sql")
    .AddDatabase("professional-db");

var referralDb = builder.AddSqlServer("sql")
    .AddDatabase("referral-db");

// Services
var tenantApi = builder.AddProject<Projects.Tenant_Api>("tenant-api")
    .WithReference(tenantDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

var customerApi = builder.AddProject<Projects.Customer_Api>("customer-api")
    .WithReference(customerDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

var professionalApi = builder.AddProject<Projects.Professional_Api>("professional-api")
    .WithReference(professionalDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

var referralApi = builder.AddProject<Projects.Referral_Api>("referral-api")
    .WithReference(referralDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

// API Gateway
builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(tenantApi)
    .WithReference(customerApi)
    .WithReference(professionalApi)
    .WithReference(referralApi);

builder.Build().Run();
```

### 10.2 Service Template

**Example: Customer.Api/Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add database
builder.AddSqlServerDbContext<CustomerDbContext>("customer-db");

// Add Redis caching
builder.AddRedisDistributedCache("redis");

// Add messaging
builder.Services.AddMessaging(builder.Configuration, x =>
{
    x.AddConsumer<TenantCreatedConsumer>();
    x.AddConsumer<ProfessionalCreatedConsumer>();
});

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add services
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<TenantContextMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints(); // Aspire health endpoints

app.Run();
```

### 10.3 Event Consumer Example

```csharp
// Customer.Api/Consumers/TenantCreatedConsumer.cs
namespace Customer.Api.Consumers;

public class TenantCreatedConsumer : IConsumer<TenantCreated>
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<TenantCreatedConsumer> _logger;

    public TenantCreatedConsumer(
        CustomerDbContext dbContext,
        ILogger<TenantCreatedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TenantCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received TenantCreated event for tenant {TenantId}",
            message.TenantId);

        var tenantCache = new TenantCache
        {
            TenantId = message.TenantId,
            Name = message.Name,
            Slug = message.Slug,
            LastUpdated = DateTime.UtcNow
        };

        _dbContext.TenantCache.Add(tenantCache);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Cached tenant {TenantId} in Customer service",
            message.TenantId);
    }
}
```

### 10.4 Command with Event Publishing

```csharp
// Customer.Api/Commands/CreateCustomerCommandHandler.cs
namespace Customer.Api.Commands;

public class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly CustomerDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITenantContext _tenantContext;

    public CreateCustomerCommandHandler(
        CustomerDbContext dbContext,
        IPublishEndpoint publishEndpoint,
        ITenantContext tenantContext)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerDto> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = new Core.Entities.Customer(
            _tenantContext.TenantId,
            request.Email,
            request.Phone,
            request.FirstName,
            request.LastName);

        _dbContext.Customers.Add(customer);

        // Add outbox message for reliable publishing
        var @event = new CustomerCreated(
            customer.Id,
            customer.TenantId,
            customer.Email,
            customer.FirstName,
            customer.LastName,
            customer.Phone,
            DateTime.UtcNow);

        _dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(CustomerCreated).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(@event),
            CreatedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Publish immediately (outbox processor handles failures)
        await _publishEndpoint.Publish(@event, cancellationToken);

        return customer.ToDto();
    }
}
```

---

## 11. Testing Strategy

### 11.1 Test Types

| Test Type | Scope | Tools |
|-----------|-------|-------|
| **Unit Tests** | Domain logic, Handlers | xUnit, Moq, FluentAssertions |
| **Integration Tests** | API + Database | WebApplicationFactory, Testcontainers |
| **Contract Tests** | Event schemas | Pact |
| **End-to-End Tests** | Full workflows | Playwright (existing) |
| **Load Tests** | Performance | k6, NBomber |

### 11.2 Test Project Structure

```
tests/
├── Tenant.Api.Tests/
│   ├── Unit/
│   │   └── Handlers/
│   └── Integration/
│       └── Controllers/
├── Customer.Api.Tests/
│   ├── Unit/
│   │   ├── Handlers/
│   │   └── Consumers/
│   └── Integration/
│       └── Controllers/
├── Integration.Tests/
│   ├── Scenarios/
│   │   ├── CustomerReferralScenarioTests.cs
│   │   └── ProfessionalReferralScenarioTests.cs
│   └── Infrastructure/
│       └── TestFixture.cs
└── Testing/
    ├── Builders/
    ├── Fixtures/
    └── Fakes/
```

### 11.3 Integration Test Example

```csharp
// Integration.Tests/Scenarios/CustomerReferralScenarioTests.cs
public class CustomerReferralScenarioTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;

    public CustomerReferralScenarioTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CustomerReferral_WhenConverted_ShouldCreateNewCustomerAndApproveReward()
    {
        // Arrange
        var referringCustomer = await _fixture.CreateCustomerAsync();
        var referralCode = await _fixture.CreateReferralCodeAsync(referringCustomer.Id);

        // Act - Create referral
        var referral = await _fixture.ReferralClient.CreateCustomerReferralAsync(
            new CreateCustomerReferralRequest
            {
                ReferringCustomerId = referringCustomer.Id,
                ReferredEmail = "newcustomer@example.com",
                ReferralCode = referralCode.Code
            });

        // Act - Convert referral
        await _fixture.ReferralClient.ConvertReferralAsync(referral.Id, new ConvertReferralRequest
        {
            NewCustomerFirstName = "New",
            NewCustomerLastName = "Customer"
        });

        // Assert - Wait for saga to complete
        await _fixture.WaitForEventAsync<RewardApproved>(
            e => e.ReferralId == referral.Id,
            TimeSpan.FromSeconds(30));

        // Assert - New customer created
        var newCustomer = await _fixture.CustomerClient.GetCustomerByEmailAsync("newcustomer@example.com");
        newCustomer.Should().NotBeNull();

        // Assert - Reward approved
        var updatedReferral = await _fixture.ReferralClient.GetReferralAsync(referral.Id);
        updatedReferral.RewardStatus.Should().Be("Approved");
    }
}
```

---

## 12. Deployment Strategy

### 12.1 Container Configuration

**Each service Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Services/Customer/Customer.Api/Customer.Api.csproj", "Services/Customer/Customer.Api/"]
COPY ["src/Services/Customer/Customer.Core/Customer.Core.csproj", "Services/Customer/Customer.Core/"]
COPY ["src/Services/Customer/Customer.Infrastructure/Customer.Infrastructure.csproj", "Services/Customer/Customer.Infrastructure/"]
COPY ["src/Shared/", "Shared/"]
RUN dotnet restore "Services/Customer/Customer.Api/Customer.Api.csproj"
COPY . .
WORKDIR "/src/Services/Customer/Customer.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Api.dll"]
```

### 12.2 Kubernetes Deployment (Optional)

```yaml
# k8s/customer-service.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: customer-api
  labels:
    app: customer-api
spec:
  replicas: 2
  selector:
    matchLabels:
      app: customer-api
  template:
    metadata:
      labels:
        app: customer-api
    spec:
      containers:
      - name: customer-api
        image: customer-api:latest
        ports:
        - containerPort: 8080
        env:
        - name: ConnectionStrings__customer-db
          valueFrom:
            secretKeyRef:
              name: db-secrets
              key: customer-connection-string
        - name: RabbitMQ__Host
          value: rabbitmq-service
        livenessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
        resources:
          limits:
            cpu: "500m"
            memory: "512Mi"
          requests:
            cpu: "250m"
            memory: "256Mi"
```

---

## 13. Monitoring and Observability

### 13.1 OpenTelemetry Integration

**ServiceDefaults/Extensions.cs:**
```csharp
public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
{
    builder.ConfigureOpenTelemetry();
    builder.AddDefaultHealthChecks();

    builder.Services.AddServiceDiscovery();
    builder.Services.ConfigureHttpClientDefaults(http =>
    {
        http.AddStandardResilienceHandler();
        http.AddServiceDiscovery();
    });

    return builder;
}

public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
{
    builder.Logging.AddOpenTelemetry(logging =>
    {
        logging.IncludeFormattedMessage = true;
        logging.IncludeScopes = true;
    });

    builder.Services.AddOpenTelemetry()
        .WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddRuntimeInstrumentation();
        })
        .WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddEntityFrameworkCoreInstrumentation()
                   .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
        });

    builder.AddOpenTelemetryExporters();

    return builder;
}
```

### 13.2 Health Checks

```csharp
// Per service health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<CustomerDbContext>()
    .AddRabbitMQ()
    .AddRedis("redis-connection-string");
```

### 13.3 Metrics Dashboard

**Key Metrics to Monitor:**
- Request rate per service
- Error rate per service
- Response latency (p50, p95, p99)
- Message queue depth
- Message processing time
- Database connection pool usage
- Memory and CPU utilization

---

## Appendix A: NuGet Package Versions

```xml
<!-- Shared package versions -->
<PropertyGroup>
  <AspireVersion>9.0.0</AspireVersion>
  <MassTransitVersion>8.2.0</MassTransitVersion>
  <EfCoreVersion>9.0.0</EfCoreVersion>
  <MediatRVersion>14.0.0</MediatRVersion>
  <FluentValidationVersion>12.1.1</FluentValidationVersion>
  <YarpVersion>2.1.0</YarpVersion>
  <PollyVersion>8.3.0</PollyVersion>
</PropertyGroup>
```

---

## Appendix B: Risk Assessment

| Risk | Impact | Mitigation |
|------|--------|------------|
| Data inconsistency during migration | High | Use outbox pattern, extensive testing |
| Service communication failures | Medium | Implement retry policies with Polly |
| Performance degradation | Medium | Load testing, caching strategy |
| Complex debugging | Medium | Distributed tracing, correlation IDs |
| Team learning curve | Low | Training sessions, documentation |

---

## Appendix C: Decision Log

| Decision | Rationale | Date |
|----------|-----------|------|
| Use MassTransit over raw RabbitMQ | Better abstraction, saga support, outbox pattern | TBD |
| Use YARP over Ocelot | Better performance, native .NET integration | TBD |
| Database per service | True microservice isolation, independent scaling | TBD |
| Event-driven over sync HTTP | Loose coupling, resilience, eventual consistency | TBD |
| .NET Aspire for orchestration | Native .NET experience, excellent local dev | TBD |

---

## Summary

This roadmap provides a comprehensive guide for migrating the Multi-Service Automotive Ecosystem Platform to an event-driven microservices architecture. The migration is structured in 9 phases over approximately 13 weeks, with clear deliverables and milestones.

**Key architectural decisions:**
1. **4 Microservices**: Tenant, Customer, Professional, Referral
2. **Event-Driven**: MassTransit with RabbitMQ
3. **API Gateway**: YARP for routing and load balancing
4. **Orchestration**: .NET Aspire for service discovery and development
5. **Database Strategy**: Database per service with cache tables

**Success Criteria:**
- All services running independently
- Full event-driven communication
- Zero data loss during migration
- All existing functionality preserved
- Improved scalability and maintainability
