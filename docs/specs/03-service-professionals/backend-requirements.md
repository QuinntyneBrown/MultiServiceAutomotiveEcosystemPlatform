# Service Professionals - Backend Requirements

## Feature Overview

Management of service professional profiles, specialties, availability, and their promotion on the platform. This feature enables professionals to showcase their expertise and be discovered by customers.

---

## Data Model Requirements

### REQ-SP-B001: Professional Entity [Phase 1] [P0]
**Description**: Core professional data model.

**Acceptance Criteria**:
- Professional has unique identifier
- Professional linked to user account
- Professional has business information
- Professional has specialty/services
- Professional is scoped to tenant

**Data Fields**:
```
Professional {
  id: UUID (PK)
  tenant_id: UUID (FK)
  user_id: UUID (FK) // Auth user account

  // Business Information
  business_name: string
  business_type: enum (see below)
  license_number: string (nullable)
  tax_id: string (nullable, encrypted)

  // Personal Information
  first_name: string
  last_name: string
  title: string (nullable) // e.g., "Master Technician"
  bio: text (nullable)

  // Contact Information
  email: string
  phone: string
  phone_business: string (nullable)
  website: string (nullable)

  // Address
  address_line1: string
  address_line2: string (nullable)
  city: string
  state: string
  postal_code: string
  country: string (default: 'US')

  // Location
  latitude: decimal (nullable)
  longitude: decimal (nullable)
  service_radius_miles: integer (nullable)

  // Media
  profile_photo_url: string (nullable)
  cover_photo_url: string (nullable)
  logo_url: string (nullable)

  // Status
  status: enum (pending, active, suspended, inactive)
  verified: boolean (default: false)
  featured: boolean (default: false)

  // Settings
  accepts_referrals: boolean (default: true)
  auto_accept_inquiries: boolean (default: false)
  notification_preferences: JSON

  // Timestamps
  created_at: timestamp
  updated_at: timestamp
  verified_at: timestamp (nullable)
}

// Business Types Enum
BusinessType {
  MECHANIC_DOMESTIC     // Domestic vehicle mechanic
  MECHANIC_GERMAN       // German vehicle specialist
  MECHANIC_ASIAN        // Asian vehicle specialist
  MECHANIC_EUROPEAN     // European vehicle specialist
  AUTO_BODY             // Auto body and collision
  DEALER_USED           // Used car dealer
  DEALER_NEW            // New car dealer
  FINANCE               // Auto financing
  BUYER_SELLER          // Car buyer/seller
  ELECTRICIAN_EV        // EV charger installation
  OTHER                 // Other automotive service
}
```

---

### REQ-SP-B002: Professional Specialty Entity [Phase 1] [P0]
**Description**: Track specific specialties and certifications.

**Acceptance Criteria**:
- Multiple specialties per professional
- Link to standard specialty catalog
- Custom specialty support
- Certification tracking
- Specialty verification

**Data Fields**:
```
ProfessionalSpecialty {
  id: UUID (PK)
  professional_id: UUID (FK)

  specialty_id: UUID (FK, nullable) // Link to catalog
  custom_name: string (nullable) // For unlisted specialties

  description: text (nullable)
  years_experience: integer (nullable)
  certification_name: string (nullable)
  certification_issuer: string (nullable)
  certification_date: date (nullable)
  certification_expiry: date (nullable)
  certification_document_url: string (nullable)

  verified: boolean (default: false)
  verified_at: timestamp (nullable)
  verified_by: UUID (FK, nullable)

  display_order: integer (default: 0)

  created_at: timestamp
  updated_at: timestamp
}

SpecialtyCatalog {
  id: UUID (PK)
  tenant_id: UUID (FK, nullable) // null for global

  name: string
  slug: string
  category: string
  description: text (nullable)
  icon: string (nullable)

  is_active: boolean (default: true)
  created_at: timestamp
}
```

---

### REQ-SP-B003: Professional Gallery Entity [Phase 2] [P1]
**Description**: Portfolio of work for professionals.

**Acceptance Criteria**:
- Multiple images per professional
- Image categorization
- Captions and descriptions
- Display order management
- Image optimization

**Data Fields**:
```
ProfessionalGallery {
  id: UUID (PK)
  professional_id: UUID (FK)

  image_url: string
  thumbnail_url: string
  title: string (nullable)
  description: text (nullable)
  category: string (nullable)

  display_order: integer (default: 0)
  is_featured: boolean (default: false)

  created_at: timestamp
}
```

---

### REQ-SP-B004: Professional Availability [Phase 3] [P2]
**Description**: Track professional availability for appointments.

**Acceptance Criteria**:
- Regular business hours
- Holiday/vacation blocking
- Real-time availability
- Buffer time between appointments
- Service duration estimates

**Data Fields**:
```
ProfessionalHours {
  id: UUID (PK)
  professional_id: UUID (FK)

  day_of_week: integer (0-6)
  open_time: time
  close_time: time
  is_closed: boolean (default: false)
}

ProfessionalBlackout {
  id: UUID (PK)
  professional_id: UUID (FK)

  start_date: date
  end_date: date
  reason: string (nullable)
}
```

---

### REQ-SP-B005: Professional Review Entity [Phase 3] [P2]
**Description**: Customer reviews and ratings.

**Acceptance Criteria**:
- Rating (1-5 stars)
- Written review text
- Review moderation
- Response capability
- Verified customer reviews

**Data Fields**:
```
ProfessionalReview {
  id: UUID (PK)
  professional_id: UUID (FK)
  customer_id: UUID (FK)

  rating: integer (1-5)
  title: string (nullable)
  content: text

  status: enum (pending, approved, rejected, flagged)
  is_verified_customer: boolean (default: false)

  response: text (nullable)
  response_at: timestamp (nullable)

  created_at: timestamp
  updated_at: timestamp
}
```

---

## API Requirements

### REQ-SP-B006: Professional CRUD API [Phase 1] [P0]
**Description**: Basic professional management endpoints.

**Endpoints**:
```
POST   /api/professionals              - Create professional (admin)
GET    /api/professionals              - List professionals
GET    /api/professionals/{id}         - Get professional details
PUT    /api/professionals/{id}         - Update professional
DELETE /api/professionals/{id}         - Deactivate professional
```

**Acceptance Criteria**:
- Tenant scoping enforced
- Public listing shows active only
- Full details for authenticated
- Owner can edit own profile
- Admin can manage all

---

### REQ-SP-B007: Professional Public API [Phase 1] [P0]
**Description**: Public-facing professional discovery.

**Endpoints**:
```
GET    /api/public/professionals                    - List active professionals
GET    /api/public/professionals/{slug}             - Get by URL slug
GET    /api/public/professionals/featured           - Get featured professionals
GET    /api/public/professionals/by-type/{type}     - Filter by business type
GET    /api/public/professionals/near               - Search by location
```

**Acceptance Criteria**:
- No authentication required
- Limited fields exposed
- Cached responses
- SEO-friendly slugs
- Location-based sorting

---

### REQ-SP-B008: Professional Specialty API [Phase 1] [P0]
**Description**: Manage professional specialties.

**Endpoints**:
```
GET    /api/professionals/{id}/specialties          - List specialties
POST   /api/professionals/{id}/specialties          - Add specialty
PUT    /api/professionals/{id}/specialties/{sid}    - Update specialty
DELETE /api/professionals/{id}/specialties/{sid}    - Remove specialty
GET    /api/specialty-catalog                       - List available specialties
```

**Acceptance Criteria**:
- Specialty catalog is tenant-configurable
- Custom specialties allowed
- Verification workflow (Phase 3)
- Display order management

---

### REQ-SP-B009: Professional Gallery API [Phase 2] [P1]
**Description**: Manage professional portfolio.

**Endpoints**:
```
GET    /api/professionals/{id}/gallery              - List gallery images
POST   /api/professionals/{id}/gallery              - Upload image
PUT    /api/professionals/{id}/gallery/{gid}        - Update image details
DELETE /api/professionals/{id}/gallery/{gid}        - Delete image
POST   /api/professionals/{id}/gallery/reorder      - Reorder images
```

**Acceptance Criteria**:
- Image upload to cloud storage
- Automatic thumbnail generation
- Image size limits
- Format validation (JPG, PNG, WebP)
- Maximum gallery size limit

---

### REQ-SP-B010: Professional Search API [Phase 2] [P1]
**Description**: Advanced professional search.

**Endpoints**:
```
GET    /api/professionals/search                    - Search professionals
```

**Search Parameters**:
- `q` - Full-text search
- `type` - Business type filter
- `specialty` - Specialty filter
- `location` - Location (lat, lng)
- `radius` - Search radius in miles
- `verified` - Verified only
- `featured` - Featured only
- `sort` - Sort field (distance, rating, name)

**Acceptance Criteria**:
- Full-text search on name, bio, specialties
- Geospatial search support
- Faceted filtering
- Result ranking algorithm

---

### REQ-SP-B011: Professional Statistics API [Phase 3] [P1]
**Description**: Professional performance metrics.

**Endpoints**:
```
GET    /api/professionals/{id}/stats               - Get statistics
GET    /api/professionals/{id}/stats/referrals     - Referral statistics
GET    /api/professionals/{id}/stats/customers     - Customer statistics
```

**Statistics**:
- Total customers
- New customers (period)
- Referrals sent/received
- Referral conversion rate
- Average rating
- Response time

---

### REQ-SP-B012: Professional Review API [Phase 3] [P2]
**Description**: Customer review management.

**Endpoints**:
```
GET    /api/professionals/{id}/reviews             - List reviews
POST   /api/professionals/{id}/reviews             - Submit review (customer)
PUT    /api/professionals/{id}/reviews/{rid}       - Update review
DELETE /api/professionals/{id}/reviews/{rid}       - Delete review
POST   /api/professionals/{id}/reviews/{rid}/respond - Respond to review
```

**Acceptance Criteria**:
- One review per customer per professional
- Review moderation queue
- Owner can respond to reviews
- Rating aggregation

---

## Business Logic Requirements

### REQ-SP-B013: Professional Verification [Phase 3] [P1]
**Description**: Verify professional credentials.

**Acceptance Criteria**:
- Document upload for verification
- Admin verification workflow
- Verified badge on profile
- License number validation
- Certification expiry tracking

---

### REQ-SP-B014: Professional Ranking Algorithm [Phase 3] [P2]
**Description**: Rank professionals for search results.

**Ranking Factors**:
- Verification status
- Average rating
- Number of reviews
- Response rate
- Response time
- Activity level
- Featured status
- Distance (for location search)

**Acceptance Criteria**:
- Configurable weight for each factor
- Boost for featured professionals
- Penalty for low response rates
- A/B testing support

---

### REQ-SP-B015: Professional Slug Generation [Phase 1] [P0]
**Description**: Generate URL-friendly slugs.

**Acceptance Criteria**:
- Generate from business name
- Handle duplicates with suffix
- URL-safe characters only
- Slug is immutable once set (or redirect)

---

## Integration Requirements

### REQ-SP-B016: Professional Event Publishing [Phase 2] [P1]
**Description**: Publish professional events.

**Events**:
```
professional.created
professional.updated
professional.verified
professional.suspended
professional.review.created
professional.review.responded
```

**Acceptance Criteria**:
- Events published to message queue
- Events include tenant context
- Events are idempotent

---

### REQ-SP-B017: Geocoding Integration [Phase 2] [P1]
**Description**: Geocode professional addresses.

**Acceptance Criteria**:
- Automatic geocoding on address change
- Support multiple geocoding providers
- Cache geocoding results
- Handle geocoding failures gracefully

---

## Security Requirements

### REQ-SP-B018: Professional Data Access Control [Phase 1] [P0]
**Description**: Control access to professional data.

**Acceptance Criteria**:
- Public fields viewable by all
- Sensitive fields (tax_id) viewable by owner/admin
- Profile editable by owner only
- Admin can edit any profile

**Public Fields**:
- Business name, type
- First/last name, title, bio
- Business phone, website
- City, state (not full address)
- Profile/cover photos
- Specialties
- Rating and reviews

**Private Fields**:
- Tax ID
- License number
- Full address
- Personal email/phone
- Revenue/statistics
