# Customer Management - Backend Requirements

## Feature Overview

Comprehensive customer management system enabling customer registration, profile management, ownership tracking, and relationship management with service professionals.

---

## Data Model Requirements

### REQ-CM-B001: Customer Entity [Phase 1] [P0]
**Description**: Core customer data model for storing customer information.

**Acceptance Criteria**:
- Customer has unique identifier (UUID)
- Customer has contact information (email, phone)
- Customer has personal details (name, address)
- Customer has preferences and settings
- Customer is scoped to tenant
- Customer has owner (service professional)

**Data Fields**:
```
Customer {
  id: UUID (PK)
  tenant_id: UUID (FK)
  owner_professional_id: UUID (FK, nullable)

  // Contact Information
  email: string (unique per tenant)
  phone: string
  phone_secondary: string (nullable)

  // Personal Information
  first_name: string
  last_name: string
  date_of_birth: date (nullable)

  // Address
  address_line1: string (nullable)
  address_line2: string (nullable)
  city: string (nullable)
  province: string (nullable)
  postal_code: string (nullable)
  country: string (default: 'CA')

  // Preferences
  preferred_contact_method: enum (email, phone, sms)
  marketing_consent: boolean (default: false)
  newsletter_subscribed: boolean (default: false)

  // Status
  status: enum (active, inactive, blocked)
  email_verified: boolean (default: false)
  phone_verified: boolean (default: false)

  // Metadata
  source: string (nullable) // How they found us
  notes: text (nullable)
  tags: string[] (nullable)

  // Timestamps
  created_at: timestamp
  updated_at: timestamp
  last_activity_at: timestamp (nullable)
}
```

---

### REQ-CM-B002: Customer Ownership Model [Phase 1] [P0]
**Description**: Track which service professional "owns" a customer relationship.

**Acceptance Criteria**:
- First professional to engage customer becomes owner
- Ownership can be transferred with audit trail
- Owner receives priority for customer inquiries
- Owner notified of all customer activities
- Ownership history maintained

**Data Fields**:
```
CustomerOwnershipHistory {
  id: UUID (PK)
  tenant_id: UUID (FK)
  customer_id: UUID (FK)
  previous_owner_id: UUID (FK, nullable)
  new_owner_id: UUID (FK)
  reason: string
  transferred_by: UUID (FK) // User who made the change
  transferred_at: timestamp
}
```

---

### REQ-CM-B003: Customer Vehicle Association [Phase 2] [P1]
**Description**: Track vehicles associated with a customer.

**Acceptance Criteria**:
- Customer can have multiple vehicles
- Vehicle includes make, model, year, VIN
- Vehicle has status (owned, sold, serviced)
- Service history linked to vehicle
- Primary vehicle designation

**Data Fields**:
```
CustomerVehicle {
  id: UUID (PK)
  tenant_id: UUID (FK)
  customer_id: UUID (FK)

  make: string
  model: string
  year: integer
  vin: string (nullable, unique if provided)
  license_plate: string (nullable)
  color: string (nullable)
  mileage: integer (nullable)

  is_primary: boolean (default: false)
  status: enum (owned, sold, prospective)

  notes: text (nullable)
  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-CM-B004: Customer Activity Log [Phase 2] [P1]
**Description**: Track all customer interactions and activities.

**Acceptance Criteria**:
- Log all customer interactions
- Include interaction type and details
- Link to related entities (inquiry, referral)
- Searchable and filterable
- Retention policy configurable

**Data Fields**:
```
CustomerActivity {
  id: UUID (PK)
  tenant_id: UUID (FK)
  customer_id: UUID (FK)
  professional_id: UUID (FK, nullable)

  activity_type: enum (inquiry, referral, service, communication, status_change)
  description: string
  metadata: JSON (nullable)

  related_entity_type: string (nullable)
  related_entity_id: UUID (nullable)

  created_at: timestamp
  created_by: UUID (FK, nullable)
}
```

---

## API Requirements

### REQ-CM-B005: Customer CRUD API [Phase 1] [P0]
**Description**: Basic customer management endpoints.

**Endpoints**:
```
POST   /api/customers              - Create customer
GET    /api/customers              - List customers (paginated)
GET    /api/customers/{id}         - Get customer details
PUT    /api/customers/{id}         - Update customer
DELETE /api/customers/{id}         - Soft delete customer
```

**Acceptance Criteria**:
- Tenant scoping enforced
- Validation for required fields
- Email uniqueness per tenant
- Phone number normalization
- Return created/updated timestamps

---

### REQ-CM-B006: Customer Search API [Phase 2] [P1]
**Description**: Advanced customer search capabilities.

**Endpoints**:
```
GET    /api/customers/search       - Search customers
POST   /api/customers/search       - Advanced search with filters
```

**Search Fields**:
- Name (first, last, full)
- Email
- Phone
- Vehicle (make, model, VIN)
- Tags
- Status
- Owner
- Date ranges (created, last activity)

**Acceptance Criteria**:
- Full-text search support
- Fuzzy matching for names
- Phone number partial matching
- Results sorted by relevance
- Pagination support

---

### REQ-CM-B007: Customer Ownership API [Phase 1] [P0]
**Description**: Manage customer ownership relationships.

**Endpoints**:
```
GET    /api/customers/{id}/owner           - Get current owner
PUT    /api/customers/{id}/owner           - Transfer ownership
GET    /api/customers/{id}/ownership-history - Get ownership history
```

**Acceptance Criteria**:
- Only admins or current owner can transfer
- Reason required for transfer
- Notification sent to both parties
- Audit trail maintained

---

### REQ-CM-B008: Customer Vehicle API [Phase 2] [P1]
**Description**: Manage customer vehicles.

**Endpoints**:
```
POST   /api/customers/{id}/vehicles        - Add vehicle
GET    /api/customers/{id}/vehicles        - List vehicles
PUT    /api/customers/{id}/vehicles/{vid}  - Update vehicle
DELETE /api/customers/{id}/vehicles/{vid}  - Remove vehicle
```

**Acceptance Criteria**:
- VIN validation and decoding
- Duplicate VIN prevention
- Primary vehicle management
- Vehicle history preservation

---

### REQ-CM-B009: Customer Activity API [Phase 2] [P1]
**Description**: Access customer activity history.

**Endpoints**:
```
GET    /api/customers/{id}/activities      - List activities
POST   /api/customers/{id}/activities      - Log manual activity
```

**Acceptance Criteria**:
- Filterable by activity type
- Filterable by date range
- Include related entity details
- Pagination support

---

### REQ-CM-B010: Customer Bulk Operations API [Phase 3] [P2]
**Description**: Bulk operations for customer management.

**Endpoints**:
```
POST   /api/customers/bulk/import          - Import customers
POST   /api/customers/bulk/export          - Export customers
POST   /api/customers/bulk/update          - Bulk update
POST   /api/customers/bulk/tag             - Bulk tagging
```

**Acceptance Criteria**:
- CSV/Excel import support
- Field mapping configuration
- Duplicate detection
- Progress tracking for large imports
- Error reporting per row

---

## Business Logic Requirements

### REQ-CM-B011: Automatic Owner Assignment [Phase 1] [P0]
**Description**: Automatically assign customer owner on first professional interaction.

**Acceptance Criteria**:
- Owner assigned when professional responds to inquiry
- Owner assigned when professional creates customer
- Owner assigned when referral is accepted
- Owner not changed if already set
- Owner assignment logged

---

### REQ-CM-B012: Customer Deduplication [Phase 3] [P2]
**Description**: Detect and merge duplicate customers.

**Acceptance Criteria**:
- Match on email (exact)
- Match on phone (normalized)
- Suggest potential duplicates
- Merge preserves all history
- Merge requires confirmation

---

### REQ-CM-B013: Customer Data Validation [Phase 1] [P0]
**Description**: Validate customer data on input.

**Acceptance Criteria**:
- Email format validation
- Phone number format validation
- Postal code format validation
- Required field enforcement
- Sanitize input for XSS

---

### REQ-CM-B014: Customer Privacy Compliance [Phase 2] [P1]
**Description**: Support privacy regulation compliance.

**Acceptance Criteria**:
- Data export for customer (GDPR)
- Data deletion request handling
- Consent tracking
- Marketing preference management
- Data retention policies

---

## Integration Requirements

### REQ-CM-B015: Customer Event Publishing [Phase 2] [P1]
**Description**: Publish customer events for other services.

**Events**:
```
customer.created
customer.updated
customer.deleted
customer.ownership.transferred
customer.vehicle.added
customer.activity.logged
```

**Acceptance Criteria**:
- Events published to message queue
- Events include tenant context
- Events are idempotent
- Failed events are retried

---

## Security Requirements

### REQ-CM-B016: Customer Data Access Control [Phase 1] [P0]
**Description**: Control access to customer data.

**Acceptance Criteria**:
- Professionals see their own customers
- Professionals see customers referred to them
- Admins see all tenant customers
- Owner sees full customer details
- Non-owners see limited details

---

### REQ-CM-B017: Customer PII Protection [Phase 1] [P0]
**Description**: Protect personally identifiable information.

**Acceptance Criteria**:
- PII fields encrypted at rest
- PII masked in logs
- PII excluded from search indexes (except designated fields)
- API responses can exclude sensitive fields
