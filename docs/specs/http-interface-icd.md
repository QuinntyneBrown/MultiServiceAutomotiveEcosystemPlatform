# HTTP Interface Control Document (ICD)
## MultiServiceAutomotiveEcosystemPlatform - Phase 1

**Document Version:** 1.0
**Date:** 2026-01-08
**Status:** Final
**Phase:** 1 - MVP Referral Core

---

## 1. Introduction

### 1.1 Purpose
This Interface Control Document (ICD) defines the HTTP communication interfaces between the frontend Angular application and the backend .NET API for Phase 1 of the MultiServiceAutomotiveEcosystemPlatform. It serves as the authoritative reference for API endpoint specifications, request/response data structures, and integration requirements.

### 1.2 Scope
This document covers all HTTP endpoints required for Phase 1 MVP functionality:
- Multi-Tenancy (Core)
- Customer Management (Core)
- Service Professionals (Core)
- Referral System (Complete MVP)

### 1.3 API Base Configuration

**Backend Base URL (Development):**
```
http://localhost:3200
```

**Frontend Configuration:**
```typescript
// environment.ts
export const environment = {
  baseUrl: 'http://localhost:3200'  // Note: Does NOT include /api
};
```

**Request URL Pattern:**
```
{baseUrl}/api/{resource}
```

### 1.4 Common Headers

All requests MUST include:

| Header | Value | Description |
|--------|-------|-------------|
| `Content-Type` | `application/json` | Required for POST/PUT requests |
| `Accept` | `application/json` | Expected response format |
| `Authorization` | `Bearer {token}` | JWT token (authenticated endpoints) |
| `X-Tenant-Id` | `{guid}` | Tenant identifier (optional, can be resolved from subdomain or JWT) |

### 1.5 Common Response Codes

| Code | Meaning | Usage |
|------|---------|-------|
| `200` | OK | Successful GET, PUT |
| `201` | Created | Successful POST creating a resource |
| `204` | No Content | Successful DELETE |
| `400` | Bad Request | Validation errors |
| `401` | Unauthorized | Missing or invalid authentication |
| `403` | Forbidden | Insufficient permissions |
| `404` | Not Found | Resource does not exist |
| `409` | Conflict | Duplicate or business rule violation |
| `422` | Unprocessable Entity | Business logic validation failure |
| `500` | Internal Server Error | Server-side error |

### 1.6 Standard Error Response

All error responses follow this structure:

```json
{
  "type": "string",
  "title": "string",
  "status": 400,
  "detail": "string",
  "instance": "string",
  "errors": {
    "fieldName": ["Error message 1", "Error message 2"]
  },
  "traceId": "string"
}
```

### 1.7 Pagination Standard

All list endpoints support pagination:

**Request Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number (1-indexed) |
| `pageSize` | integer | 20 | Items per page (max 100) |
| `sortBy` | string | varies | Field to sort by |
| `sortDirection` | string | `asc` | `asc` or `desc` |

**Response Envelope:**
```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 150,
  "totalPages": 8,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

## 2. Authentication & Tenant Context

### 2.1 POST /api/auth/login
Authenticate a user and receive JWT token.

**Request:**
```json
{
  "email": "string",
  "password": "string",
  "rememberMe": false
}
```

**Response (200):**
```json
{
  "token": "string",
  "refreshToken": "string",
  "expiresAt": "2026-01-08T12:00:00Z",
  "user": {
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "firstName": "string",
    "lastName": "string",
    "role": "Customer | Professional | Admin",
    "tenantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

**Error Response (401):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid email or password"
}
```

---

### 2.2 POST /api/auth/register
Register a new customer account.

**Request:**
```json
{
  "email": "string",
  "password": "string",
  "confirmPassword": "string",
  "firstName": "string",
  "lastName": "string",
  "phone": "string",
  "marketingConsent": false,
  "termsAccepted": true,
  "referralCode": "string | null"
}
```

**Response (201):**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "string",
  "emailVerificationRequired": true,
  "referralApplied": true,
  "referralDiscount": {
    "discountType": "Percentage | Fixed",
    "discountValue": 10.00,
    "discountCode": "WELCOME10"
  }
}
```

---

### 2.3 POST /api/auth/refresh
Refresh an expired JWT token.

**Request:**
```json
{
  "token": "string",
  "refreshToken": "string"
}
```

**Response (200):**
```json
{
  "token": "string",
  "refreshToken": "string",
  "expiresAt": "2026-01-08T12:00:00Z"
}
```

---

### 2.4 POST /api/auth/forgot-password
Initiate password reset flow.

**Request:**
```json
{
  "email": "string"
}
```

**Response (200):**
```json
{
  "message": "If an account exists with this email, a password reset link has been sent."
}
```

---

### 2.5 POST /api/auth/reset-password
Complete password reset.

**Request:**
```json
{
  "token": "string",
  "newPassword": "string",
  "confirmPassword": "string"
}
```

**Response (200):**
```json
{
  "message": "Password has been reset successfully."
}
```

---

### 2.6 GET /api/tenant/resolve
Resolve tenant from subdomain or identifier.

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `slug` | string | Yes | Tenant slug from subdomain |

**Response (200):**
```json
{
  "tenantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "slug": "network1",
  "name": "Network One",
  "displayName": "Network One Auto Services",
  "logoUrl": "https://cdn.example.com/logos/network1.png",
  "primaryColor": "#1976D2",
  "status": "Active | Suspended | Inactive",
  "configuration": {
    "features": {
      "loyaltyProgram": false,
      "professionalReferrals": true,
      "customerReferrals": true
    },
    "referralRewardAmount": 25.00,
    "referralExpirationDays": 90
  }
}
```

**Error Response (404):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Tenant not found"
}
```

---

## 3. Customer Management API

### 3.1 Customer Data Transfer Objects

#### CustomerDto
```typescript
interface CustomerDto {
  customerId: string;           // UUID
  tenantId: string;             // UUID
  ownerProfessionalId: string | null;  // UUID

  // Contact Information
  email: string;
  phone: string;
  phoneSecondary: string | null;

  // Personal Information
  firstName: string;
  lastName: string;
  fullName: string;             // Computed: firstName + lastName
  dateOfBirth: string | null;   // ISO 8601 date

  // Address
  addressLine1: string | null;
  addressLine2: string | null;
  city: string | null;
  state: string | null;
  postalCode: string | null;
  country: string;              // Default: 'US'

  // Preferences
  preferredContactMethod: 'Email' | 'Phone' | 'Sms';
  marketingConsent: boolean;
  newsletterSubscribed: boolean;

  // Status
  status: 'Active' | 'Inactive' | 'Blocked';
  emailVerified: boolean;
  phoneVerified: boolean;

  // Metadata
  source: string | null;
  notes: string | null;
  tags: string[];

  // Timestamps
  createdAt: string;            // ISO 8601 datetime
  updatedAt: string;            // ISO 8601 datetime
  lastActivityAt: string | null;

  // Ownership (expanded when requested)
  owner: ProfessionalSummaryDto | null;
}
```

#### CustomerSummaryDto
```typescript
interface CustomerSummaryDto {
  customerId: string;
  email: string;
  phone: string;
  firstName: string;
  lastName: string;
  fullName: string;
  status: 'Active' | 'Inactive' | 'Blocked';
  ownerProfessionalId: string | null;
  ownerProfessionalName: string | null;
  createdAt: string;
  lastActivityAt: string | null;
}
```

#### CreateCustomerRequest
```typescript
interface CreateCustomerRequest {
  email: string;                // Required, unique per tenant
  phone: string;                // Required
  phoneSecondary?: string;
  firstName: string;            // Required
  lastName: string;             // Required
  dateOfBirth?: string;         // ISO 8601 date
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;             // Default: 'US'
  preferredContactMethod?: 'Email' | 'Phone' | 'Sms';
  marketingConsent?: boolean;
  newsletterSubscribed?: boolean;
  source?: string;
  notes?: string;
  tags?: string[];
}
```

#### UpdateCustomerRequest
```typescript
interface UpdateCustomerRequest {
  phone?: string;
  phoneSecondary?: string;
  firstName?: string;
  lastName?: string;
  dateOfBirth?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  preferredContactMethod?: 'Email' | 'Phone' | 'Sms';
  marketingConsent?: boolean;
  newsletterSubscribed?: boolean;
  notes?: string;
  tags?: string[];
}
```

---

### 3.2 POST /api/customers
Create a new customer.

**Authorization:** Professional, Admin

**Request Body:** `CreateCustomerRequest`

**Response (201):**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "customer@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "createdAt": "2026-01-08T10:00:00Z"
}
```

**Validation Errors (400):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "email": ["Email is required", "Email format is invalid"],
    "phone": ["Phone number format is invalid"]
  }
}
```

**Duplicate Error (409):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "title": "Conflict",
  "status": 409,
  "detail": "A customer with this email already exists"
}
```

---

### 3.3 GET /api/customers
List customers with pagination and filtering.

**Authorization:** Professional, Admin

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `sortBy` | string | `createdAt` | Sort field |
| `sortDirection` | string | `desc` | Sort direction |
| `status` | string | - | Filter by status |
| `ownership` | string | - | `mine`, `referred`, `all` |
| `search` | string | - | Search by name/email/phone |

**Response (200):**
```json
{
  "items": [
    {
      "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "john.doe@example.com",
      "phone": "+1234567890",
      "firstName": "John",
      "lastName": "Doe",
      "fullName": "John Doe",
      "status": "Active",
      "ownerProfessionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "ownerProfessionalName": "German Auto Specialists",
      "createdAt": "2026-01-08T10:00:00Z",
      "lastActivityAt": "2026-01-08T14:30:00Z"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 150,
  "totalPages": 8,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

### 3.4 GET /api/customers/{customerId}
Get customer details.

**Authorization:** Professional (owner or referred), Admin

**Path Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `customerId` | UUID | Customer identifier |

**Response (200):** `CustomerDto`

---

### 3.5 PUT /api/customers/{customerId}
Update customer information.

**Authorization:** Professional (owner), Admin

**Path Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `customerId` | UUID | Customer identifier |

**Request Body:** `UpdateCustomerRequest`

**Response (200):** `CustomerDto`

---

### 3.6 DELETE /api/customers/{customerId}
Soft delete a customer.

**Authorization:** Admin

**Path Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `customerId` | UUID | Customer identifier |

**Response (204):** No content

---

### 3.7 GET /api/customers/{customerId}/owner
Get the current owner of a customer.

**Authorization:** Professional, Admin

**Response (200):**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "currentOwner": {
    "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "businessName": "German Auto Specialists",
    "firstName": "Hans",
    "lastName": "Mueller",
    "assignedAt": "2026-01-01T10:00:00Z"
  }
}
```

---

### 3.8 PUT /api/customers/{customerId}/owner
Transfer customer ownership.

**Authorization:** Professional (current owner), Admin

**Request Body:**
```json
{
  "newOwnerProfessionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "reason": "Customer moving to new service area"
}
```

**Response (200):**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "previousOwnerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "newOwnerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferredAt": "2026-01-08T10:00:00Z",
  "reason": "Customer moving to new service area"
}
```

---

### 3.9 GET /api/customers/{customerId}/ownership-history
Get ownership history.

**Authorization:** Professional, Admin

**Response (200):**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "history": [
    {
      "ownershipHistoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "previousOwnerId": null,
      "previousOwnerName": null,
      "newOwnerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "newOwnerName": "German Auto Specialists",
      "reason": "Initial assignment",
      "transferredBy": "System",
      "transferredAt": "2026-01-01T10:00:00Z"
    }
  ]
}
```

---

## 4. Service Professionals API

### 4.1 Professional Data Transfer Objects

#### ProfessionalDto
```typescript
interface ProfessionalDto {
  professionalId: string;       // UUID
  tenantId: string;             // UUID
  userId: string;               // UUID - auth user account

  // Business Information
  businessName: string;
  businessType: BusinessType;
  licenseNumber: string | null;

  // Personal Information
  firstName: string;
  lastName: string;
  fullName: string;
  title: string | null;
  bio: string | null;

  // Contact Information
  email: string;
  phone: string;
  phoneBusiness: string | null;
  website: string | null;

  // Address
  addressLine1: string;
  addressLine2: string | null;
  city: string;
  state: string;
  postalCode: string;
  country: string;

  // Location
  latitude: number | null;
  longitude: number | null;
  serviceRadiusMiles: number | null;

  // Media
  profilePhotoUrl: string | null;
  coverPhotoUrl: string | null;
  logoUrl: string | null;

  // Status
  status: 'Pending' | 'Active' | 'Suspended' | 'Inactive';
  verified: boolean;
  featured: boolean;

  // Settings
  acceptsReferrals: boolean;
  autoAcceptInquiries: boolean;

  // Timestamps
  createdAt: string;
  updatedAt: string;
  verifiedAt: string | null;

  // Related data (expanded when requested)
  specialties: ProfessionalSpecialtyDto[];
  slug: string;                 // URL-friendly identifier
}
```

#### BusinessType (Enum)
```typescript
type BusinessType =
  | 'MechanicDomestic'
  | 'MechanicGerman'
  | 'MechanicAsian'
  | 'MechanicEuropean'
  | 'AutoBody'
  | 'DealerUsed'
  | 'DealerNew'
  | 'Finance'
  | 'BuyerSeller'
  | 'ElectricianEv'
  | 'Other';
```

#### ProfessionalSummaryDto
```typescript
interface ProfessionalSummaryDto {
  professionalId: string;
  businessName: string;
  businessType: BusinessType;
  firstName: string;
  lastName: string;
  fullName: string;
  title: string | null;
  profilePhotoUrl: string | null;
  city: string;
  state: string;
  verified: boolean;
  featured: boolean;
  slug: string;
  topSpecialties: string[];     // Top 3 specialty names
  averageRating: number | null; // Phase 3
  reviewCount: number;          // Phase 3
}
```

#### ProfessionalPublicDto
```typescript
interface ProfessionalPublicDto {
  professionalId: string;
  businessName: string;
  businessType: BusinessType;
  businessTypeDisplay: string;  // Human-readable
  firstName: string;
  lastName: string;
  fullName: string;
  title: string | null;
  bio: string | null;

  // Public contact
  phoneBusiness: string | null;
  website: string | null;

  // Location (city/state only, not full address)
  city: string;
  state: string;
  serviceRadiusMiles: number | null;

  // Media
  profilePhotoUrl: string | null;
  coverPhotoUrl: string | null;
  logoUrl: string | null;

  // Status
  verified: boolean;
  featured: boolean;

  // Specialties
  specialties: ProfessionalSpecialtyDto[];

  slug: string;

  // Stats (Phase 3)
  averageRating: number | null;
  reviewCount: number;
}
```

#### ProfessionalSpecialtyDto
```typescript
interface ProfessionalSpecialtyDto {
  professionalSpecialtyId: string;
  specialtyId: string | null;
  specialtyName: string;
  customName: string | null;
  description: string | null;
  yearsExperience: number | null;
  certificationName: string | null;
  certificationIssuer: string | null;
  certificationDate: string | null;
  certificationExpiry: string | null;
  verified: boolean;
  displayOrder: number;
}
```

#### CreateProfessionalRequest
```typescript
interface CreateProfessionalRequest {
  businessName: string;         // Required
  businessType: BusinessType;   // Required
  licenseNumber?: string;
  firstName: string;            // Required
  lastName: string;             // Required
  title?: string;
  bio?: string;
  email: string;                // Required
  phone: string;                // Required
  phoneBusiness?: string;
  website?: string;
  addressLine1: string;         // Required
  addressLine2?: string;
  city: string;                 // Required
  state: string;                // Required
  postalCode: string;           // Required
  country?: string;             // Default: 'US'
  serviceRadiusMiles?: number;
  acceptsReferrals?: boolean;   // Default: true
}
```

#### UpdateProfessionalRequest
```typescript
interface UpdateProfessionalRequest {
  businessName?: string;
  licenseNumber?: string;
  firstName?: string;
  lastName?: string;
  title?: string;
  bio?: string;
  phone?: string;
  phoneBusiness?: string;
  website?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  serviceRadiusMiles?: number;
  acceptsReferrals?: boolean;
  autoAcceptInquiries?: boolean;
}
```

---

### 4.2 POST /api/professionals
Create a new professional (Admin only).

**Authorization:** Admin

**Request Body:** `CreateProfessionalRequest`

**Response (201):**
```json
{
  "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "businessName": "German Auto Specialists",
  "slug": "german-auto-specialists",
  "status": "Pending",
  "createdAt": "2026-01-08T10:00:00Z"
}
```

---

### 4.3 GET /api/professionals
List professionals (authenticated).

**Authorization:** Professional, Admin

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `status` | string | - | Filter by status |
| `businessType` | string | - | Filter by business type |
| `verified` | boolean | - | Filter by verification |

**Response (200):** Paginated list of `ProfessionalSummaryDto`

---

### 4.4 GET /api/professionals/{professionalId}
Get professional details.

**Authorization:** Professional (self), Admin

**Response (200):** `ProfessionalDto`

---

### 4.5 PUT /api/professionals/{professionalId}
Update professional profile.

**Authorization:** Professional (self), Admin

**Request Body:** `UpdateProfessionalRequest`

**Response (200):** `ProfessionalDto`

---

### 4.6 DELETE /api/professionals/{professionalId}
Deactivate a professional.

**Authorization:** Admin

**Response (204):** No content

---

### 4.7 GET /api/public/professionals
List active professionals (public, no auth).

**Authorization:** None

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `businessType` | string | - | Filter by type |
| `sortBy` | string | `businessName` | Sort field |

**Response (200):** Paginated list of `ProfessionalPublicDto`

---

### 4.8 GET /api/public/professionals/{slug}
Get professional by URL slug (public).

**Authorization:** None

**Response (200):** `ProfessionalPublicDto`

---

### 4.9 GET /api/public/professionals/featured
Get featured professionals (public).

**Authorization:** None

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `limit` | integer | 6 | Max professionals to return |

**Response (200):**
```json
{
  "professionals": [
    {
      "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "businessName": "German Auto Specialists",
      "businessType": "MechanicGerman",
      "businessTypeDisplay": "German Vehicle Specialist",
      "fullName": "Hans Mueller",
      "profilePhotoUrl": "https://...",
      "city": "Austin",
      "state": "TX",
      "verified": true,
      "featured": true,
      "slug": "german-auto-specialists",
      "topSpecialties": ["BMW Repair", "Mercedes Service", "Audi Maintenance"]
    }
  ]
}
```

---

### 4.10 GET /api/public/professionals/by-type/{businessType}
Get professionals by business type (public).

**Authorization:** None

**Response (200):** Paginated list of `ProfessionalPublicDto`

---

### 4.11 GET /api/public/professionals/near
Search professionals by location (public).

**Authorization:** None

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `latitude` | number | Yes | Latitude |
| `longitude` | number | Yes | Longitude |
| `radiusMiles` | number | No | Search radius (default: 25) |
| `businessType` | string | No | Filter by type |

**Response (200):**
```json
{
  "items": [
    {
      "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "businessName": "German Auto Specialists",
      "distanceMiles": 5.2,
      ...
    }
  ],
  "searchCenter": {
    "latitude": 30.2672,
    "longitude": -97.7431
  },
  "radiusMiles": 25
}
```

---

### 4.12 GET /api/professionals/{professionalId}/specialties
Get professional's specialties.

**Authorization:** None (public data)

**Response (200):**
```json
{
  "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "specialties": [
    {
      "professionalSpecialtyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "specialtyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "specialtyName": "BMW Repair",
      "customName": null,
      "description": "Factory-trained BMW technician",
      "yearsExperience": 15,
      "certificationName": "BMW Master Technician",
      "certificationIssuer": "BMW North America",
      "certificationDate": "2020-05-15",
      "certificationExpiry": "2025-05-15",
      "verified": true,
      "displayOrder": 1
    }
  ]
}
```

---

### 4.13 POST /api/professionals/{professionalId}/specialties
Add a specialty to professional.

**Authorization:** Professional (self), Admin

**Request Body:**
```json
{
  "specialtyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customName": null,
  "description": "Factory-trained BMW technician",
  "yearsExperience": 15,
  "certificationName": "BMW Master Technician",
  "certificationIssuer": "BMW North America",
  "certificationDate": "2020-05-15",
  "certificationExpiry": "2025-05-15"
}
```

**Response (201):** `ProfessionalSpecialtyDto`

---

### 4.14 PUT /api/professionals/{professionalId}/specialties/{specialtyId}
Update a specialty.

**Authorization:** Professional (self), Admin

**Response (200):** `ProfessionalSpecialtyDto`

---

### 4.15 DELETE /api/professionals/{professionalId}/specialties/{specialtyId}
Remove a specialty.

**Authorization:** Professional (self), Admin

**Response (204):** No content

---

### 4.16 GET /api/specialty-catalog
Get available specialties catalog.

**Authorization:** None

**Response (200):**
```json
{
  "categories": [
    {
      "category": "Mechanical",
      "specialties": [
        {
          "specialtyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "Engine Repair",
          "slug": "engine-repair",
          "description": "Complete engine diagnostics and repair",
          "icon": "engine"
        }
      ]
    }
  ]
}
```

---

## 5. Referral System API

### 5.1 Customer Referral Data Transfer Objects

#### CustomerReferralDto
```typescript
interface CustomerReferralDto {
  customerReferralId: string;
  tenantId: string;

  // Referrer
  referrerCustomerId: string;
  referrerCode: string;
  referrerName: string;

  // Referee
  refereeCustomerId: string | null;
  refereeEmail: string | null;
  refereeName: string | null;
  refereePhone: string | null;

  // Target
  targetProfessionalId: string | null;
  targetProfessionalName: string | null;
  targetServiceType: string | null;

  // Status
  status: 'Pending' | 'Contacted' | 'Converted' | 'Expired' | 'Cancelled';
  convertedAt: string | null;

  // Reward
  rewardStatus: 'Pending' | 'Approved' | 'Paid' | 'Cancelled';
  rewardAmount: number | null;
  rewardType: 'Cash' | 'Credit' | 'Discount';
  rewardPaidAt: string | null;

  // Tracking
  referralSource: string | null;

  // Timestamps
  createdAt: string;
  updatedAt: string;
  expiresAt: string | null;
}
```

#### CreateCustomerReferralRequest
```typescript
interface CreateCustomerReferralRequest {
  refereeEmail?: string;
  refereeName?: string;
  refereePhone?: string;
  targetProfessionalId?: string;
  message?: string;
}
```

#### CustomerReferralStatsDto
```typescript
interface CustomerReferralStatsDto {
  customerId: string;
  totalReferralsSent: number;
  successfulReferrals: number;
  pendingReferrals: number;
  totalRewardsEarned: number;
  rewardsPending: number;
  conversionRate: number;
}
```

---

### 5.2 Professional Referral Data Transfer Objects

#### ProfessionalReferralDto
```typescript
interface ProfessionalReferralDto {
  professionalReferralId: string;
  tenantId: string;

  // Parties
  sourceProfessionalId: string;
  sourceProfessionalName: string;
  targetProfessionalId: string;
  targetProfessionalName: string;
  customerId: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string;

  // Context
  reason: string | null;
  serviceNeeded: string | null;
  notes: string | null;
  priority: 'Normal' | 'High' | 'Urgent';

  // Status
  status: 'Pending' | 'Accepted' | 'Declined' | 'Completed' | 'Expired';
  acceptedAt: string | null;
  completedAt: string | null;
  declinedReason: string | null;

  // Discount
  discountOffered: boolean;
  discountType: 'Percentage' | 'Fixed' | 'None';
  discountValue: number | null;
  discountCode: string | null;
  discountUsed: boolean;

  // Follow-up
  followUpRequired: boolean;
  followUpDate: string | null;
  followUpNotes: string | null;

  // Timestamps
  createdAt: string;
  updatedAt: string;
  expiresAt: string | null;
}
```

#### CreateProfessionalReferralRequest
```typescript
interface CreateProfessionalReferralRequest {
  targetProfessionalId: string;   // Required
  customerId: string;             // Required
  reason?: string;
  serviceNeeded?: string;
  priority?: 'Normal' | 'High' | 'Urgent';
  notes?: string;
  discountType?: 'Percentage' | 'Fixed' | 'None';
  discountValue?: number;
}
```

---

### 5.3 Referral Code Data Transfer Objects

#### ReferralCodeDto
```typescript
interface ReferralCodeDto {
  referralCodeId: string;
  code: string;
  codeType: 'Customer' | 'Professional' | 'Campaign';
  customerId: string | null;
  professionalId: string | null;
  campaignId: string | null;
  maxUses: number | null;
  currentUses: number;
  rewardAmount: number | null;
  discountPercentage: number | null;
  isActive: boolean;
  expiresAt: string | null;
  createdAt: string;
  qrCodeUrl: string;
  shareUrl: string;
}
```

#### ReferralCodeInfoDto (Public)
```typescript
interface ReferralCodeInfoDto {
  code: string;
  isValid: boolean;
  ownerName: string | null;
  discountPercentage: number | null;
  discountDescription: string | null;
  expiresAt: string | null;
}
```

---

### 5.4 POST /api/referrals/customer
Create a customer referral.

**Authorization:** Customer

**Request Body:** `CreateCustomerReferralRequest`

**Response (201):**
```json
{
  "customerReferralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "referralCode": "JOHN7K2M",
  "shareUrl": "https://network1.platform.com/r/JOHN7K2M",
  "status": "Pending",
  "createdAt": "2026-01-08T10:00:00Z",
  "expiresAt": "2026-04-08T10:00:00Z",
  "notificationSent": true
}
```

---

### 5.5 GET /api/referrals/customer
Get customer's referrals.

**Authorization:** Customer

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `status` | string | - | Filter by status |

**Response (200):** Paginated list of `CustomerReferralDto`

---

### 5.6 GET /api/referrals/customer/{referralId}
Get customer referral details.

**Authorization:** Customer (owner)

**Response (200):** `CustomerReferralDto`

---

### 5.7 PUT /api/referrals/customer/{referralId}
Update a customer referral.

**Authorization:** Customer (owner)

**Request Body:**
```json
{
  "refereeEmail": "string",
  "refereeName": "string",
  "refereePhone": "string"
}
```

**Response (200):** `CustomerReferralDto`

---

### 5.8 DELETE /api/referrals/customer/{referralId}
Cancel a customer referral.

**Authorization:** Customer (owner)

**Response (204):** No content

---

### 5.9 GET /api/referrals/customer/stats
Get customer's referral statistics.

**Authorization:** Customer

**Response (200):** `CustomerReferralStatsDto`

---

### 5.10 GET /api/referrals/customer/rewards
Get customer's referral rewards.

**Authorization:** Customer

**Response (200):**
```json
{
  "availableBalance": 75.00,
  "pendingBalance": 25.00,
  "lifetimeEarned": 125.00,
  "rewards": [
    {
      "rewardId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "referralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "refereeName": "Jane Doe",
      "amount": 25.00,
      "status": "Paid",
      "earnedAt": "2026-01-05T10:00:00Z",
      "paidAt": "2026-01-08T10:00:00Z"
    }
  ]
}
```

---

### 5.11 POST /api/referrals/customer/share
Generate share link with tracking.

**Authorization:** Customer

**Request Body:**
```json
{
  "channel": "Email" | "Sms" | "Social" | "Direct",
  "targetProfessionalId": "string | null"
}
```

**Response (200):**
```json
{
  "shareUrl": "https://network1.platform.com/r/JOHN7K2M?utm_source=email",
  "shortUrl": "https://plat.fm/JOHN7K2M",
  "qrCodeUrl": "https://cdn.platform.com/qr/JOHN7K2M.png",
  "referralCode": "JOHN7K2M"
}
```

---

### 5.12 POST /api/referrals/professional
Create a professional referral.

**Authorization:** Professional

**Request Body:** `CreateProfessionalReferralRequest`

**Response (201):**
```json
{
  "professionalReferralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Pending",
  "discountCode": "REF-ABC123",
  "notificationsSent": {
    "targetProfessional": true,
    "customer": true
  },
  "createdAt": "2026-01-08T10:00:00Z"
}
```

---

### 5.13 GET /api/referrals/professional/sent
Get referrals sent by professional.

**Authorization:** Professional

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `status` | string | - | Filter by status |

**Response (200):** Paginated list of `ProfessionalReferralDto`

---

### 5.14 GET /api/referrals/professional/received
Get referrals received by professional.

**Authorization:** Professional

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | integer | 1 | Page number |
| `pageSize` | integer | 20 | Items per page |
| `status` | string | - | Filter by status |

**Response (200):** Paginated list of `ProfessionalReferralDto`

---

### 5.15 GET /api/referrals/professional/{referralId}
Get professional referral details.

**Authorization:** Professional (source or target)

**Response (200):** `ProfessionalReferralDto`

---

### 5.16 PUT /api/referrals/professional/{referralId}
Update a professional referral.

**Authorization:** Professional (source)

**Request Body:**
```json
{
  "notes": "string",
  "priority": "Normal" | "High" | "Urgent",
  "followUpRequired": true,
  "followUpDate": "2026-01-15",
  "followUpNotes": "string"
}
```

**Response (200):** `ProfessionalReferralDto`

---

### 5.17 POST /api/referrals/professional/{referralId}/accept
Accept a professional referral.

**Authorization:** Professional (target)

**Request Body:**
```json
{
  "messageToSender": "string | null",
  "honorDiscount": true,
  "expectedFollowUpDate": "2026-01-10"
}
```

**Response (200):**
```json
{
  "professionalReferralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Accepted",
  "acceptedAt": "2026-01-08T10:00:00Z",
  "customer": {
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "+1234567890"
  },
  "discountCode": "REF-ABC123",
  "discountValue": 10,
  "discountType": "Percentage"
}
```

---

### 5.18 POST /api/referrals/professional/{referralId}/decline
Decline a professional referral.

**Authorization:** Professional (target)

**Request Body:**
```json
{
  "reason": "TooBusy" | "OutsideServiceArea" | "NotMySpecialty" | "CustomerAlreadyKnown" | "Other",
  "customReason": "string | null",
  "suggestAlternative": "string | null"
}
```

**Response (200):**
```json
{
  "professionalReferralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Declined",
  "declinedReason": "TooBusy"
}
```

---

### 5.19 POST /api/referrals/professional/{referralId}/complete
Mark a professional referral as completed.

**Authorization:** Professional (target)

**Request Body:**
```json
{
  "discountApplied": true,
  "serviceProvided": "Engine diagnostic and repair",
  "feedbackToSource": "Great referral, customer was very satisfied"
}
```

**Response (200):**
```json
{
  "professionalReferralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Completed",
  "completedAt": "2026-01-15T14:00:00Z"
}
```

---

### 5.20 GET /api/referral-codes/my-code
Get customer's personal referral code.

**Authorization:** Customer

**Response (200):**
```json
{
  "referralCodeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "JOHN7K2M",
  "shareUrl": "https://network1.platform.com/r/JOHN7K2M",
  "qrCodeUrl": "https://cdn.platform.com/qr/JOHN7K2M.png",
  "currentUses": 5,
  "successfulConversions": 3,
  "rewardAmount": 25.00,
  "isActive": true,
  "expiresAt": null
}
```

---

### 5.21 POST /api/referral-codes/generate
Generate a new referral code.

**Authorization:** Customer, Professional

**Request Body:**
```json
{
  "codeType": "Customer" | "Professional",
  "customCode": "string | null"
}
```

**Response (201):** `ReferralCodeDto`

---

### 5.22 POST /api/referral-codes/validate
Validate a referral code.

**Authorization:** None (public)

**Request Body:**
```json
{
  "code": "JOHN7K2M"
}
```

**Response (200):**
```json
{
  "isValid": true,
  "code": "JOHN7K2M",
  "ownerName": "John D.",
  "discountPercentage": 10,
  "discountDescription": "10% off your first service",
  "expiresAt": null
}
```

**Invalid Code Response (200):**
```json
{
  "isValid": false,
  "code": "INVALID",
  "reason": "Code not found or expired"
}
```

---

### 5.23 GET /api/referral-codes/{code}/info
Get public information about a referral code.

**Authorization:** None (public)

**Response (200):** `ReferralCodeInfoDto`

---

### 5.24 POST /api/referrals/convert
Convert a referral (internal use).

**Authorization:** System (internal)

**Request Body:**
```json
{
  "referralCode": "JOHN7K2M",
  "newCustomerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Response (200):**
```json
{
  "converted": true,
  "referralId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "referrerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "rewardPending": 25.00,
  "discountApplied": {
    "discountCode": "WELCOME10",
    "discountType": "Percentage",
    "discountValue": 10
  }
}
```

---

### 5.25 POST /api/referrals/claim
Claim a referral by code during signup.

**Authorization:** None (used during registration)

**Request Body:**
```json
{
  "code": "JOHN7K2M",
  "email": "newcustomer@example.com",
  "phone": "+1234567890"
}
```

**Response (200):**
```json
{
  "claimed": true,
  "referrerName": "John D.",
  "discount": {
    "discountCode": "WELCOME10",
    "discountType": "Percentage",
    "discountValue": 10,
    "description": "10% off your first service"
  }
}
```

---

## 6. Dashboard & Statistics API

### 6.1 GET /api/dashboard/customer
Get customer dashboard data.

**Authorization:** Customer

**Response (200):**
```json
{
  "customer": {
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "John",
    "lastName": "Doe"
  },
  "referralStats": {
    "totalReferralsSent": 5,
    "successfulReferrals": 3,
    "pendingReferrals": 2,
    "rewardsEarned": 75.00,
    "rewardsPending": 50.00
  },
  "recentActivity": [
    {
      "activityId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "type": "ReferralConverted",
      "title": "Referral converted!",
      "description": "Your friend Jane signed up",
      "timestamp": "2026-01-08T10:00:00Z"
    }
  ],
  "referralCode": {
    "code": "JOHN7K2M",
    "shareUrl": "https://network1.platform.com/r/JOHN7K2M"
  }
}
```

---

### 6.2 GET /api/dashboard/professional
Get professional dashboard data.

**Authorization:** Professional

**Response (200):**
```json
{
  "professional": {
    "professionalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "businessName": "German Auto Specialists",
    "profileComplete": 85
  },
  "stats": {
    "totalCustomers": 45,
    "newCustomersThisMonth": 5,
    "referralsSentThisMonth": 8,
    "referralsReceivedThisMonth": 3,
    "pendingReferrals": 2
  },
  "recentActivity": [
    {
      "activityId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "type": "ReferralReceived",
      "title": "New referral received",
      "description": "Auto Body Pro sent you a customer",
      "timestamp": "2026-01-08T10:00:00Z",
      "actionUrl": "/referrals/received/3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ],
  "pendingActions": [
    {
      "actionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "type": "PendingReferral",
      "title": "Referral awaiting response",
      "priority": "High",
      "actionUrl": "/referrals/received/3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ]
}
```

---

## 7. File Upload API

### 7.1 POST /api/uploads/image
Upload an image file.

**Authorization:** Professional, Admin

**Request:** `multipart/form-data`
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `file` | File | Yes | Image file (JPG, PNG, WebP) |
| `type` | string | Yes | `profile`, `cover`, `logo`, `gallery` |
| `entityId` | string | No | Associated entity ID |

**Response (201):**
```json
{
  "uploadId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "url": "https://cdn.platform.com/images/...",
  "thumbnailUrl": "https://cdn.platform.com/images/thumb/...",
  "fileName": "profile.jpg",
  "fileSize": 245678,
  "mimeType": "image/jpeg",
  "uploadedAt": "2026-01-08T10:00:00Z"
}
```

**Validation Errors (400):**
```json
{
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "file": ["File size exceeds maximum of 5MB", "Invalid file type"]
  }
}
```

---

## 8. Health & System API

### 8.1 GET /api/health
Health check endpoint.

**Authorization:** None

**Response (200):**
```json
{
  "status": "Healthy",
  "timestamp": "2026-01-08T10:00:00Z",
  "version": "1.0.0",
  "checks": {
    "database": "Healthy",
    "cache": "Healthy"
  }
}
```

---

### 8.2 GET /api/system/enums
Get all system enumerations.

**Authorization:** None

**Response (200):**
```json
{
  "businessTypes": [
    { "value": "MechanicDomestic", "display": "Domestic Vehicle Mechanic" },
    { "value": "MechanicGerman", "display": "German Vehicle Specialist" },
    { "value": "MechanicAsian", "display": "Asian Vehicle Specialist" },
    { "value": "MechanicEuropean", "display": "European Vehicle Specialist" },
    { "value": "AutoBody", "display": "Auto Body & Collision" },
    { "value": "DealerUsed", "display": "Used Car Dealer" },
    { "value": "DealerNew", "display": "New Car Dealer" },
    { "value": "Finance", "display": "Auto Finance" },
    { "value": "BuyerSeller", "display": "Car Buyer/Seller" },
    { "value": "ElectricianEv", "display": "EV Charger Installation" },
    { "value": "Other", "display": "Other Automotive Service" }
  ],
  "customerStatuses": [
    { "value": "Active", "display": "Active" },
    { "value": "Inactive", "display": "Inactive" },
    { "value": "Blocked", "display": "Blocked" }
  ],
  "professionalStatuses": [
    { "value": "Pending", "display": "Pending Approval" },
    { "value": "Active", "display": "Active" },
    { "value": "Suspended", "display": "Suspended" },
    { "value": "Inactive", "display": "Inactive" }
  ],
  "referralStatuses": [
    { "value": "Pending", "display": "Pending" },
    { "value": "Contacted", "display": "Contacted" },
    { "value": "Converted", "display": "Converted" },
    { "value": "Expired", "display": "Expired" },
    { "value": "Cancelled", "display": "Cancelled" }
  ],
  "professionalReferralStatuses": [
    { "value": "Pending", "display": "Pending" },
    { "value": "Accepted", "display": "Accepted" },
    { "value": "Declined", "display": "Declined" },
    { "value": "Completed", "display": "Completed" },
    { "value": "Expired", "display": "Expired" }
  ],
  "referralPriorities": [
    { "value": "Normal", "display": "Normal" },
    { "value": "High", "display": "High Priority" },
    { "value": "Urgent", "display": "Urgent" }
  ],
  "contactMethods": [
    { "value": "Email", "display": "Email" },
    { "value": "Phone", "display": "Phone" },
    { "value": "Sms", "display": "SMS" }
  ],
  "discountTypes": [
    { "value": "Percentage", "display": "Percentage" },
    { "value": "Fixed", "display": "Fixed Amount" },
    { "value": "None", "display": "No Discount" }
  ]
}
```

---

## 9. Frontend Service Integration Guide

### 9.1 Angular HTTP Service Pattern

All frontend HTTP services should follow this pattern:

```typescript
// base-http.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class BaseHttpService {
  protected http = inject(HttpClient);
  protected baseUrl = environment.baseUrl;

  protected get<T>(path: string, params?: HttpParams): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/api${path}`, { params });
  }

  protected post<T>(path: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/api${path}`, body);
  }

  protected put<T>(path: string, body: unknown): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/api${path}`, body);
  }

  protected delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/api${path}`);
  }
}
```

### 9.2 Example Customer Service

```typescript
// customer.service.ts
import { Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from './base-http.service';
import {
  CustomerDto,
  CustomerSummaryDto,
  CreateCustomerRequest,
  UpdateCustomerRequest,
  PaginatedResponse
} from '../models';

@Injectable({ providedIn: 'root' })
export class CustomerService extends BaseHttpService {

  getCustomers(
    page = 1,
    pageSize = 20,
    filters?: { status?: string; ownership?: string; search?: string }
  ): Observable<PaginatedResponse<CustomerSummaryDto>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (filters?.status) params = params.set('status', filters.status);
    if (filters?.ownership) params = params.set('ownership', filters.ownership);
    if (filters?.search) params = params.set('search', filters.search);

    return this.get<PaginatedResponse<CustomerSummaryDto>>('/customers', params);
  }

  getCustomer(customerId: string): Observable<CustomerDto> {
    return this.get<CustomerDto>(`/customers/${customerId}`);
  }

  createCustomer(request: CreateCustomerRequest): Observable<CustomerDto> {
    return this.post<CustomerDto>('/customers', request);
  }

  updateCustomer(customerId: string, request: UpdateCustomerRequest): Observable<CustomerDto> {
    return this.put<CustomerDto>(`/customers/${customerId}`, request);
  }

  deleteCustomer(customerId: string): Observable<void> {
    return this.delete<void>(`/customers/${customerId}`);
  }
}
```

### 9.3 HTTP Interceptor for Authentication

```typescript
// auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};
```

### 9.4 HTTP Interceptor for Tenant Context

```typescript
// tenant.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TenantService } from './tenant.service';

export const tenantInterceptor: HttpInterceptorFn = (req, next) => {
  const tenantService = inject(TenantService);
  const tenantId = tenantService.getCurrentTenantId();

  if (tenantId) {
    req = req.clone({
      setHeaders: {
        'X-Tenant-Id': tenantId
      }
    });
  }

  return next(req);
};
```

---

## 10. API Versioning

### 10.1 Version Strategy
The API uses URL path versioning. Phase 1 uses v1 implicitly (no version in path). Future versions will be explicitly versioned:

```
/api/v2/customers
```

### 10.2 Deprecation Policy
When endpoints are deprecated:
1. Response header `X-API-Deprecated: true` will be added
2. Response header `X-API-Sunset-Date: 2026-06-01` indicates removal date
3. Documentation will be updated with migration guide

---

## 11. Rate Limiting

### 11.1 Rate Limit Headers
All responses include rate limit information:

| Header | Description |
|--------|-------------|
| `X-RateLimit-Limit` | Maximum requests per window |
| `X-RateLimit-Remaining` | Remaining requests in window |
| `X-RateLimit-Reset` | Unix timestamp when window resets |

### 11.2 Rate Limits by Endpoint Type

| Endpoint Type | Limit | Window |
|--------------|-------|--------|
| Authentication | 10 | 1 minute |
| Public read | 100 | 1 minute |
| Authenticated read | 200 | 1 minute |
| Write operations | 50 | 1 minute |
| Referral code validation | 20 | 1 minute |

### 11.3 Rate Limit Exceeded Response (429)
```json
{
  "type": "https://tools.ietf.org/html/rfc6585#section-4",
  "title": "Too Many Requests",
  "status": 429,
  "detail": "Rate limit exceeded. Try again in 45 seconds.",
  "retryAfter": 45
}
```

---

## 12. Webhook Events (For Future Integration)

### 12.1 Available Events
The system publishes these events for webhook integration:

| Event | Trigger |
|-------|---------|
| `customer.created` | New customer registered |
| `customer.updated` | Customer profile updated |
| `professional.created` | New professional added |
| `professional.verified` | Professional verified |
| `referral.customer.created` | Customer referral created |
| `referral.customer.converted` | Customer referral converted |
| `referral.professional.created` | Professional referral created |
| `referral.professional.accepted` | Professional referral accepted |
| `referral.professional.completed` | Professional referral completed |

### 12.2 Webhook Payload Structure
```json
{
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventType": "referral.customer.converted",
  "tenantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "timestamp": "2026-01-08T10:00:00Z",
  "data": {
    // Event-specific payload
  }
}
```

---

## Appendix A: Complete Endpoint Reference

### Phase 1 Endpoints Summary

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| **Authentication** |
| POST | `/api/auth/login` | None | User login |
| POST | `/api/auth/register` | None | Customer registration |
| POST | `/api/auth/refresh` | None | Refresh token |
| POST | `/api/auth/forgot-password` | None | Initiate password reset |
| POST | `/api/auth/reset-password` | None | Complete password reset |
| **Tenant** |
| GET | `/api/tenant/resolve` | None | Resolve tenant by slug |
| **Customers** |
| POST | `/api/customers` | Prof/Admin | Create customer |
| GET | `/api/customers` | Prof/Admin | List customers |
| GET | `/api/customers/{id}` | Prof/Admin | Get customer |
| PUT | `/api/customers/{id}` | Prof/Admin | Update customer |
| DELETE | `/api/customers/{id}` | Admin | Delete customer |
| GET | `/api/customers/{id}/owner` | Prof/Admin | Get owner |
| PUT | `/api/customers/{id}/owner` | Prof/Admin | Transfer ownership |
| GET | `/api/customers/{id}/ownership-history` | Prof/Admin | Ownership history |
| **Professionals** |
| POST | `/api/professionals` | Admin | Create professional |
| GET | `/api/professionals` | Prof/Admin | List professionals |
| GET | `/api/professionals/{id}` | Prof/Admin | Get professional |
| PUT | `/api/professionals/{id}` | Prof/Admin | Update professional |
| DELETE | `/api/professionals/{id}` | Admin | Deactivate professional |
| GET | `/api/public/professionals` | None | List active professionals |
| GET | `/api/public/professionals/{slug}` | None | Get by slug |
| GET | `/api/public/professionals/featured` | None | Featured professionals |
| GET | `/api/public/professionals/by-type/{type}` | None | By business type |
| GET | `/api/public/professionals/near` | None | Search by location |
| GET | `/api/professionals/{id}/specialties` | None | Get specialties |
| POST | `/api/professionals/{id}/specialties` | Prof/Admin | Add specialty |
| PUT | `/api/professionals/{id}/specialties/{sid}` | Prof/Admin | Update specialty |
| DELETE | `/api/professionals/{id}/specialties/{sid}` | Prof/Admin | Remove specialty |
| GET | `/api/specialty-catalog` | None | Specialty catalog |
| **Customer Referrals** |
| POST | `/api/referrals/customer` | Customer | Create referral |
| GET | `/api/referrals/customer` | Customer | List my referrals |
| GET | `/api/referrals/customer/{id}` | Customer | Get referral |
| PUT | `/api/referrals/customer/{id}` | Customer | Update referral |
| DELETE | `/api/referrals/customer/{id}` | Customer | Cancel referral |
| GET | `/api/referrals/customer/stats` | Customer | Referral stats |
| GET | `/api/referrals/customer/rewards` | Customer | My rewards |
| POST | `/api/referrals/customer/share` | Customer | Generate share link |
| **Professional Referrals** |
| POST | `/api/referrals/professional` | Prof | Create referral |
| GET | `/api/referrals/professional/sent` | Prof | Sent referrals |
| GET | `/api/referrals/professional/received` | Prof | Received referrals |
| GET | `/api/referrals/professional/{id}` | Prof | Get referral |
| PUT | `/api/referrals/professional/{id}` | Prof | Update referral |
| POST | `/api/referrals/professional/{id}/accept` | Prof | Accept referral |
| POST | `/api/referrals/professional/{id}/decline` | Prof | Decline referral |
| POST | `/api/referrals/professional/{id}/complete` | Prof | Complete referral |
| **Referral Codes** |
| GET | `/api/referral-codes/my-code` | Customer | Get my code |
| POST | `/api/referral-codes/generate` | Cust/Prof | Generate code |
| POST | `/api/referral-codes/validate` | None | Validate code |
| GET | `/api/referral-codes/{code}/info` | None | Code info |
| POST | `/api/referrals/convert` | System | Convert referral |
| POST | `/api/referrals/claim` | None | Claim referral |
| **Dashboard** |
| GET | `/api/dashboard/customer` | Customer | Customer dashboard |
| GET | `/api/dashboard/professional` | Prof | Professional dashboard |
| **Uploads** |
| POST | `/api/uploads/image` | Prof/Admin | Upload image |
| **System** |
| GET | `/api/health` | None | Health check |
| GET | `/api/system/enums` | None | System enums |

---

## Appendix B: Document History

| Version | Date | Author | Description |
|---------|------|--------|-------------|
| 1.0 | 2026-01-08 | System | Initial ICD document for Phase 1 |

---

**End of HTTP Interface Control Document**
