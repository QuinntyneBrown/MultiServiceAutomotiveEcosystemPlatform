# Service Catalog - Backend Requirements

## Feature Overview

The service catalog displays the services offered by each professional on the platform, enabling customers to browse, understand, and request specific services.

---

## Data Model Requirements

### REQ-SC-B001: Service Category Entity [Phase 2] [P0]
**Description**: Categories for organizing services.

**Data Fields**:
```
ServiceCategory {
  id: UUID (PK)
  tenant_id: UUID (FK, nullable) // null = platform default

  name: string
  slug: string
  description: text (nullable)
  icon: string (nullable)
  image_url: string (nullable)

  parent_id: UUID (FK, nullable) // For subcategories
  display_order: integer (default: 0)

  is_active: boolean (default: true)
  created_at: timestamp
  updated_at: timestamp
}
```

**Default Categories**:
- Mechanical Repair
  - Engine
  - Transmission
  - Brakes
  - Suspension
  - Electrical
- Body Work
  - Collision Repair
  - Paint
  - Dent Removal
- Vehicle Sales
  - Used Cars
  - Trade-In
- Financing
  - Auto Loans
  - Lease Options
- EV Services
  - Charger Installation
  - EV Maintenance

---

### REQ-SC-B002: Service Entity [Phase 2] [P0]
**Description**: Individual service offerings.

**Data Fields**:
```
Service {
  id: UUID (PK)
  tenant_id: UUID (FK)
  professional_id: UUID (FK)
  category_id: UUID (FK)

  name: string
  slug: string
  description: text
  short_description: string (nullable)

  // Pricing
  price_type: enum (fixed, starting_at, range, quote_required)
  price_min: decimal (nullable)
  price_max: decimal (nullable)
  price_unit: string (nullable) // 'per hour', 'per service', etc.

  // Duration
  duration_min: integer (nullable) // minutes
  duration_max: integer (nullable) // minutes
  duration_unit: string (default: 'minutes')

  // Details
  features: string[] (nullable) // What's included
  requirements: string[] (nullable) // What customer needs to provide
  warranty_info: text (nullable)

  // Media
  image_url: string (nullable)
  gallery_urls: string[] (nullable)

  // Availability
  is_active: boolean (default: true)
  requires_appointment: boolean (default: false)

  // SEO
  meta_title: string (nullable)
  meta_description: string (nullable)

  display_order: integer (default: 0)
  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-SC-B003: Service Add-On Entity [Phase 3] [P2]
**Description**: Optional add-ons for services.

**Data Fields**:
```
ServiceAddOn {
  id: UUID (PK)
  service_id: UUID (FK)

  name: string
  description: text (nullable)
  price: decimal
  duration_additional: integer (nullable) // minutes

  is_active: boolean (default: true)
  display_order: integer (default: 0)
}
```

---

### REQ-SC-B004: Service Package Entity [Phase 3] [P2]
**Description**: Bundled service packages.

**Data Fields**:
```
ServicePackage {
  id: UUID (PK)
  tenant_id: UUID (FK)
  professional_id: UUID (FK)

  name: string
  description: text
  image_url: string (nullable)

  // Pricing
  regular_price: decimal // Sum of individual services
  package_price: decimal // Discounted price
  savings_display: string (nullable) // "Save $50!"

  is_active: boolean (default: true)
  created_at: timestamp
}

ServicePackageItem {
  id: UUID (PK)
  package_id: UUID (FK)
  service_id: UUID (FK)
  display_order: integer (default: 0)
}
```

---

### REQ-SC-B005: Vehicle Compatibility Entity [Phase 3] [P2]
**Description**: Service compatibility with vehicles.

**Data Fields**:
```
ServiceVehicleCompatibility {
  id: UUID (PK)
  service_id: UUID (FK)

  // Vehicle criteria (null = all)
  makes: string[] (nullable)
  models: string[] (nullable)
  year_min: integer (nullable)
  year_max: integer (nullable)
  body_types: string[] (nullable) // sedan, suv, truck, etc.

  // Compatibility
  compatibility: enum (compatible, not_compatible, requires_quote)
  notes: text (nullable)
}
```

---

## API Requirements

### REQ-SC-B006: Category API [Phase 2] [P0]
**Description**: Service category management.

**Endpoints**:
```
GET    /api/categories                      - List categories
GET    /api/categories/{slug}               - Get category
GET    /api/categories/{slug}/services      - Services in category

# Admin
POST   /api/admin/categories                - Create category
PUT    /api/admin/categories/{id}           - Update category
DELETE /api/admin/categories/{id}           - Delete category
```

**Acceptance Criteria**:
- Hierarchical category support
- Tenant can customize defaults
- Cached for performance

---

### REQ-SC-B007: Service API [Phase 2] [P0]
**Description**: Service management.

**Endpoints**:
```
# Public
GET    /api/services                        - List all services
GET    /api/services/{slug}                 - Get service details
GET    /api/services/search                 - Search services
GET    /api/professionals/{id}/services     - Professional's services

# Professional
POST   /api/professional/services           - Create service
PUT    /api/professional/services/{id}      - Update service
DELETE /api/professional/services/{id}      - Delete service
POST   /api/professional/services/reorder   - Reorder services
```

**Search Parameters**:
- `q` - Text search
- `category` - Category filter
- `professional` - Professional filter
- `price_min` / `price_max` - Price range
- `vehicle_make` / `model` / `year` - Compatibility

**Acceptance Criteria**:
- Full-text search on name/description
- Filter by category, price, professional
- Sort by price, name, popularity
- Vehicle compatibility filtering (Phase 3)

---

### REQ-SC-B008: Service Package API [Phase 3] [P2]
**Description**: Service package management.

**Endpoints**:
```
GET    /api/packages                        - List packages
GET    /api/packages/{id}                   - Package details
GET    /api/professionals/{id}/packages     - Professional's packages

# Professional
POST   /api/professional/packages           - Create package
PUT    /api/professional/packages/{id}      - Update package
DELETE /api/professional/packages/{id}      - Delete package
```

---

### REQ-SC-B009: Service Pricing API [Phase 3] [P1]
**Description**: Dynamic pricing quotes.

**Endpoints**:
```
POST   /api/services/{id}/quote             - Get price quote
```

**Quote Request**:
```json
{
  "vehicle_make": "BMW",
  "vehicle_model": "535i",
  "vehicle_year": 2018,
  "add_ons": ["addon-uuid-1", "addon-uuid-2"],
  "notes": "Additional details..."
}
```

**Acceptance Criteria**:
- Calculate based on vehicle
- Include selected add-ons
- Return estimated duration
- Note if quote required

---

## Business Logic Requirements

### REQ-SC-B010: Service Search Engine [Phase 2] [P0]
**Description**: Full-text service search.

**Acceptance Criteria**:
- Search in name, description, category
- Fuzzy matching for typos
- Highlight matching terms
- Relevance scoring
- Faceted results (by category, price range)

---

### REQ-SC-B011: Service Visibility Rules [Phase 2] [P0]
**Description**: Control service visibility.

**Rules**:
- Inactive services not shown publicly
- Professional can preview inactive
- Admin sees all services
- Tenant can hide categories

**Acceptance Criteria**:
- Visibility enforced on all queries
- Professional dashboard shows inactive
- Admin can override visibility

---

### REQ-SC-B012: Price Display Logic [Phase 2] [P0]
**Description**: Format price display.

**Display Rules**:
| Price Type | Display |
|------------|---------|
| Fixed | "$150.00" |
| Starting At | "From $100.00" |
| Range | "$100.00 - $200.00" |
| Quote Required | "Contact for Quote" |

**Acceptance Criteria**:
- Consistent formatting
- Currency symbol from tenant config
- Handle null prices gracefully

---

### REQ-SC-B013: Vehicle Compatibility Check [Phase 3] [P2]
**Description**: Check service compatibility with vehicle.

**Acceptance Criteria**:
- Match against compatibility rules
- Return compatibility status
- Suggest alternatives if not compatible
- Log compatibility queries

---

## Integration Requirements

### REQ-SC-B014: Service Event Publishing [Phase 2] [P1]
**Description**: Publish service events.

**Events**:
```
service.created
service.updated
service.deleted
service.viewed
```

**Acceptance Criteria**:
- Events include full service data
- Events trigger cache invalidation
- View events for analytics

---

### REQ-SC-B015: Search Index Integration [Phase 2] [P1]
**Description**: Index services for search.

**Acceptance Criteria**:
- Real-time index updates
- Searchable fields configured
- Facet fields for filtering
- Support for Elasticsearch/Algolia/Meilisearch

---

## Security Requirements

### REQ-SC-B016: Service Access Control [Phase 2] [P0]
**Description**: Control service data access.

**Acceptance Criteria**:
- Public can view active services
- Professional can CRUD own services
- Admin can manage all services
- Audit logging for changes

---

## Performance Requirements

### REQ-SC-B017: Service Catalog Performance [Phase 2] [P0]
**Description**: Fast service catalog access.

**Acceptance Criteria**:
- Category list < 50ms
- Service list < 100ms
- Search results < 200ms
- CDN caching for images
- Database query optimization
