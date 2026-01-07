# Inquiry & Routing - Backend Requirements

## Feature Overview

The inquiry system allows customers to submit service inquiries which are routed to the most appropriate professional based on service type, specialty, location, and availability.

---

## Data Model Requirements

### REQ-IR-B001: Inquiry Entity [Phase 2] [P0]
**Description**: Customer service inquiry data model.

**Data Fields**:
```
Inquiry {
  id: UUID (PK)
  tenant_id: UUID (FK)

  // Customer
  customer_id: UUID (FK, nullable) // null for guest inquiries

  // Guest Contact (if not logged in)
  guest_name: string (nullable)
  guest_email: string (nullable)
  guest_phone: string (nullable)

  // Inquiry Details
  service_type: enum (see below)
  subject: string
  description: text
  preferred_contact: enum (email, phone, either)

  // Vehicle Info
  vehicle_make: string (nullable)
  vehicle_model: string (nullable)
  vehicle_year: integer (nullable)
  vehicle_vin: string (nullable)

  // Location
  customer_zip: string (nullable)
  customer_city: string (nullable)

  // Urgency
  urgency: enum (routine, soon, urgent, emergency)
  preferred_date: date (nullable)

  // Routing
  target_professional_id: UUID (FK, nullable) // if directly to professional
  assigned_professional_id: UUID (FK, nullable)
  assigned_at: timestamp (nullable)

  // Status
  status: enum (new, assigned, in_progress, quoted, completed, closed, cancelled)

  // Resolution
  resolution: text (nullable)
  resolution_type: enum (serviced, referred, no_response, customer_cancelled, other)

  // Attachments
  attachment_urls: string[] (nullable)

  // Tracking
  source: string (nullable) // website, referral, direct, etc.
  referral_code: string (nullable)

  created_at: timestamp
  updated_at: timestamp
  responded_at: timestamp (nullable)
  closed_at: timestamp (nullable)
}

// Service Types
ServiceType {
  REPAIR_DOMESTIC
  REPAIR_GERMAN
  REPAIR_ASIAN
  REPAIR_EUROPEAN
  BODY_WORK
  PAINT
  CAR_PURCHASE
  CAR_SALE
  FINANCING
  EV_CHARGER
  GENERAL
}
```

---

### REQ-IR-B002: Inquiry Response Entity [Phase 2] [P0]
**Description**: Track responses to inquiries.

**Data Fields**:
```
InquiryResponse {
  id: UUID (PK)
  inquiry_id: UUID (FK)
  professional_id: UUID (FK)

  message: text
  is_quote: boolean (default: false)
  quote_amount: decimal (nullable)
  quote_valid_until: date (nullable)

  attachment_urls: string[] (nullable)

  created_at: timestamp
}
```

---

### REQ-IR-B003: Routing Rule Entity [Phase 2] [P1]
**Description**: Rules for automatic inquiry routing.

**Data Fields**:
```
RoutingRule {
  id: UUID (PK)
  tenant_id: UUID (FK)

  name: string
  priority: integer (default: 0) // higher = checked first
  is_active: boolean (default: true)

  // Conditions (all must match)
  service_types: string[] (nullable) // null = any
  vehicle_makes: string[] (nullable) // null = any
  zip_codes: string[] (nullable) // null = any
  urgency_levels: string[] (nullable) // null = any

  // Action
  assign_to_professional_id: UUID (FK, nullable) // direct assignment
  assign_to_pool: string[] (nullable) // pool of professional IDs

  // Round-robin tracking
  last_assigned_idx: integer (default: 0)

  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-IR-B004: Inquiry Activity Log [Phase 2] [P1]
**Description**: Track all inquiry activities.

**Data Fields**:
```
InquiryActivity {
  id: UUID (PK)
  inquiry_id: UUID (FK)

  activity_type: enum (created, assigned, responded, status_change, note_added)
  actor_type: enum (system, customer, professional, admin)
  actor_id: UUID (nullable)

  description: string
  metadata: JSON (nullable)

  created_at: timestamp
}
```

---

## API Requirements

### REQ-IR-B005: Inquiry Submission API [Phase 2] [P0]
**Description**: Submit new inquiries.

**Endpoints**:
```
POST   /api/inquiries                    - Submit inquiry
POST   /api/public/inquiries             - Guest inquiry (no auth)
```

**Request Body**:
```json
{
  "service_type": "REPAIR_GERMAN",
  "subject": "BMW 535i check engine light",
  "description": "Check engine light came on...",
  "vehicle_make": "BMW",
  "vehicle_model": "535i",
  "vehicle_year": 2018,
  "urgency": "soon",
  "preferred_contact": "phone",
  "customer_zip": "12345",
  "preferred_date": "2024-01-15"
}
```

**Acceptance Criteria**:
- Validation of required fields
- Auto-associate logged-in customer
- Trigger routing logic
- Send confirmation notification
- Support file attachments

---

### REQ-IR-B006: Customer Inquiry API [Phase 2] [P0]
**Description**: Customer inquiry management.

**Endpoints**:
```
GET    /api/inquiries                    - List my inquiries
GET    /api/inquiries/{id}               - Get inquiry details
PUT    /api/inquiries/{id}               - Update inquiry
DELETE /api/inquiries/{id}               - Cancel inquiry
POST   /api/inquiries/{id}/messages      - Add message
GET    /api/inquiries/{id}/messages      - Get messages
```

**Acceptance Criteria**:
- Customers see only their inquiries
- Can add messages to open inquiries
- Can cancel pending inquiries
- Cannot modify closed inquiries

---

### REQ-IR-B007: Professional Inquiry API [Phase 2] [P0]
**Description**: Professional inquiry management.

**Endpoints**:
```
GET    /api/professional/inquiries                    - Inquiries assigned to me
GET    /api/professional/inquiries/available          - Available to claim
POST   /api/professional/inquiries/{id}/claim         - Claim inquiry
POST   /api/professional/inquiries/{id}/respond       - Respond to inquiry
POST   /api/professional/inquiries/{id}/quote         - Send quote
PUT    /api/professional/inquiries/{id}/status        - Update status
POST   /api/professional/inquiries/{id}/refer         - Refer to colleague
```

**Acceptance Criteria**:
- Professionals see assigned and claimable
- Claim prevents others from claiming
- Response triggers notification
- Quote includes validity period

---

### REQ-IR-B008: Inquiry Routing API [Phase 2] [P1]
**Description**: Admin routing configuration.

**Endpoints**:
```
GET    /api/admin/routing-rules                       - List rules
POST   /api/admin/routing-rules                       - Create rule
PUT    /api/admin/routing-rules/{id}                  - Update rule
DELETE /api/admin/routing-rules/{id}                  - Delete rule
POST   /api/admin/inquiries/{id}/reassign             - Manual reassign
```

---

## Business Logic Requirements

### REQ-IR-B009: Automatic Routing Engine [Phase 2] [P0]
**Description**: Automatically route inquiries to professionals.

**Routing Logic**:
1. If inquiry targets specific professional → assign directly
2. Match against routing rules (highest priority first)
3. If rule matches → assign to professional or pool
4. If pool → round-robin or least-busy assignment
5. If no match → assign to admin queue

**Acceptance Criteria**:
- Routing completes within 1 second
- Notification sent to assigned professional
- Customer notified of assignment
- Fallback to admin if no match

---

### REQ-IR-B010: Professional Matching Algorithm [Phase 2] [P1]
**Description**: Match inquiries to best professional.

**Matching Factors**:
- Service type match (required)
- Specialty match (preferred)
- Location proximity
- Availability
- Response rate history
- Customer reviews

**Acceptance Criteria**:
- Configurable factor weights
- Returns ranked list of professionals
- Excludes suspended professionals
- Respects professional capacity settings

---

### REQ-IR-B011: Inquiry SLA Tracking [Phase 4] [P2]
**Description**: Track response time SLAs.

**SLA Levels**:
- Emergency: 1 hour response
- Urgent: 4 hour response
- Soon: 24 hour response
- Routine: 48 hour response

**Acceptance Criteria**:
- SLA clock starts on assignment
- Warning at 75% of SLA
- Escalation at SLA breach
- SLA reporting

---

### REQ-IR-B012: Guest Inquiry Conversion [Phase 2] [P0]
**Description**: Convert guest inquiries to customer accounts.

**Acceptance Criteria**:
- Prompt guest to create account
- Link inquiry to new account
- Preserve inquiry history
- Pre-fill registration with inquiry data

---

## Integration Requirements

### REQ-IR-B013: Inquiry Event Publishing [Phase 2] [P1]
**Description**: Publish inquiry events.

**Events**:
```
inquiry.created
inquiry.assigned
inquiry.responded
inquiry.quoted
inquiry.status_changed
inquiry.closed
```

---

### REQ-IR-B014: File Upload Integration [Phase 2] [P0]
**Description**: Handle inquiry attachments.

**Acceptance Criteria**:
- Image uploads (JPG, PNG, HEIC)
- Document uploads (PDF)
- Size limit (10MB per file)
- Virus scanning
- Secure storage with signed URLs

---

## Security Requirements

### REQ-IR-B015: Inquiry Access Control [Phase 2] [P0]
**Description**: Control access to inquiry data.

**Acceptance Criteria**:
- Customer sees own inquiries only
- Assigned professional sees full details
- Other professionals see limited details (for claiming)
- Admin sees all inquiries
- Sensitive data masked for unauthorized
