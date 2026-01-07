# Service Catalog - Frontend Requirements

## Feature Overview

User interfaces for browsing, searching, and managing the service catalog, including public service listings and professional service management.

---

## Public Service Catalog

### REQ-SC-F001: Service Catalog Homepage [Phase 2] [P0]
**Description**: Main service catalog browsing page.

**Acceptance Criteria**:
- Featured services section
- Category navigation
- Search bar
- Popular services
- Recently viewed (Phase 3)

**Page Layout**:
```
┌─────────────────────────────────────────────────┐
│ [Search services...                    🔍]      │
├─────────────────────────────────────────────────┤
│ Browse by Category                              │
│ ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐       │
│ │ 🔧 │ │ 🎨 │ │ 🚗 │ │ 💰 │ │ ⚡ │       │
│ │Repair│ │Body │ │Sales│ │Loans│ │ EV  │       │
│ └─────┘ └─────┘ └─────┘ └─────┘ └─────┘       │
├─────────────────────────────────────────────────┤
│ Featured Services                               │
│ ┌─────────────┐ ┌─────────────┐ ┌─────────────┐│
│ │ Service 1   │ │ Service 2   │ │ Service 3   ││
│ │ From $99    │ │ $150        │ │ Contact     ││
│ └─────────────┘ └─────────────┘ └─────────────┘│
└─────────────────────────────────────────────────┘
```

---

### REQ-SC-F002: Category Page [Phase 2] [P0]
**Description**: Services within a category.

**Acceptance Criteria**:
- Category header with description
- Subcategory navigation
- Service grid/list
- Filter sidebar
- Sort options

**Filters**:
- Price range slider
- Professional filter
- Vehicle compatibility (Phase 3)

**Sort Options**:
- Relevance
- Price: Low to High
- Price: High to Low
- Name: A-Z
- Most Popular

---

### REQ-SC-F003: Service Card Component [Phase 2] [P0]
**Description**: Service display card.

**Card Content**:
```
┌─────────────────────────────────────────┐
│ [Service Image                        ] │
├─────────────────────────────────────────┤
│ Category Badge                          │
│ Service Name                            │
│ Short description text here...          │
│                                         │
│ Professional: German Auto Specialists   │
│                                         │
│ From $150.00        ⏱ ~2 hours         │
│                                         │
│ [View Details]  [Request Service]       │
└─────────────────────────────────────────┘
```

---

### REQ-SC-F004: Service Detail Page [Phase 2] [P0]
**Description**: Full service information page.

**Acceptance Criteria**:
- Service description
- Pricing information
- Duration estimate
- What's included
- Requirements
- Professional info
- Related services
- Inquiry button

**Page Sections**:
```
1. Header
   - Service name
   - Category breadcrumb
   - Price display
   - Duration
   - CTA button

2. Description
   - Full service description
   - Image gallery

3. What's Included
   - Feature list with checkmarks

4. Requirements
   - What customer needs to provide/prepare

5. Add-Ons (Phase 3)
   - Optional add-on services
   - Price for each

6. Professional
   - Professional card
   - Link to profile

7. Warranty/Guarantee
   - Warranty information

8. Related Services
   - Other services from professional
   - Similar services in category
```

---

### REQ-SC-F005: Service Search Results [Phase 2] [P0]
**Description**: Search results for services.

**Acceptance Criteria**:
- Search results count
- Filter refinement
- Sort options
- Highlighted search terms
- No results state

**Search Interface**:
```
┌─────────────────────────────────────────────────┐
│ Search: "brake repair"                          │
│ 23 services found                               │
├─────────────────────────────────────────────────┤
│ Filters          │ Results                      │
│ ┌─────────────┐  │ ┌─────────────────────────┐  │
│ │ Category    │  │ │ Brake Pad Replacement   │  │
│ │ ☑ Brakes   │  │ │ From $150 | German Auto │  │
│ │ ☐ Engine   │  │ └─────────────────────────┘  │
│ │             │  │ ┌─────────────────────────┐  │
│ │ Price Range │  │ │ Full Brake Service      │  │
│ │ [$50 - $500]│  │ │ $299 | Domestic Repair  │  │
│ └─────────────┘  │ └─────────────────────────┘  │
└─────────────────────────────────────────────────┘
```

---

### REQ-SC-F006: Service Inquiry Modal [Phase 2] [P0]
**Description**: Quick inquiry from service page.

**Acceptance Criteria**:
- Pre-filled service info
- Vehicle information
- Additional notes
- Contact preference
- Submit to professional

**Modal Content**:
```
┌─────────────────────────────────────────────────┐
│ Request: Brake Pad Replacement                  │
│ From: German Auto Specialists                   │
├─────────────────────────────────────────────────┤
│ Your Vehicle:                                   │
│ [BMW ▼] [535i ▼] [2018 ▼]                      │
│                                                 │
│ Additional Details:                             │
│ [                                             ] │
│                                                 │
│ Preferred Contact: [Email ▼]                    │
│                                                 │
│ [Cancel]                [Submit Inquiry]        │
└─────────────────────────────────────────────────┘
```

---

## Professional Service Management

### REQ-SC-F007: Service List (Professional) [Phase 2] [P0]
**Description**: Professional's service management list.

**Acceptance Criteria**:
- List all my services
- Status indicators
- Edit/delete actions
- Reorder (drag-drop)
- Bulk actions

**List Interface**:
```
┌─────────────────────────────────────────────────┐
│ My Services                      [+ Add Service]│
├─────────────────────────────────────────────────┤
│ ☰ Brake Pad Replacement    Active    [$150]    │
│   Category: Brakes         [Edit] [Deactivate] │
├─────────────────────────────────────────────────┤
│ ☰ Full Brake Service       Active    [$299]    │
│   Category: Brakes         [Edit] [Deactivate] │
├─────────────────────────────────────────────────┤
│ ☰ Engine Diagnostic        Inactive  [$89]     │
│   Category: Engine         [Edit] [Activate]   │
└─────────────────────────────────────────────────┘
```

---

### REQ-SC-F008: Service Create/Edit Form [Phase 2] [P0]
**Description**: Form for creating and editing services.

**Form Sections**:
```
1. Basic Information
   - Service Name (required)
   - Category (required, dropdown)
   - Short Description (required, 150 chars)
   - Full Description (required, rich text)

2. Pricing
   - Price Type (fixed/starting/range/quote)
   - Price Amount(s)
   - Price Unit (optional)

3. Duration
   - Estimated Duration (min/max)
   - Duration Unit

4. Details
   - What's Included (list builder)
   - Requirements (list builder)
   - Warranty Info (optional)

5. Media
   - Service Image (upload)
   - Gallery Images (multiple upload)

6. Settings
   - Active/Inactive toggle
   - Requires Appointment toggle

7. SEO (Phase 3)
   - Meta Title
   - Meta Description
```

**Acceptance Criteria**:
- Validation on all required fields
- Image preview
- Auto-save draft
- Preview before publish

---

### REQ-SC-F009: Add-On Management [Phase 3] [P2]
**Description**: Manage service add-ons.

**Acceptance Criteria**:
- Add/edit/delete add-ons
- Price for each
- Duration impact
- Reorder add-ons

---

### REQ-SC-F010: Service Package Builder [Phase 3] [P2]
**Description**: Create service packages.

**Acceptance Criteria**:
- Select services to bundle
- Set package price
- Display savings
- Package description
- Activate/deactivate

---

## Admin Interface

### REQ-SC-F011: Category Management [Phase 2] [P1]
**Description**: Admin interface for categories.

**Acceptance Criteria**:
- List all categories
- Create/edit categories
- Set hierarchy (parent/child)
- Set display order
- Upload icons/images
- Activate/deactivate

---

### REQ-SC-F012: Service Moderation [Phase 3] [P2]
**Description**: Admin review of services.

**Acceptance Criteria**:
- Review new services
- Approve/reject with notes
- Edit any service
- Featured service selection

---

## Mobile Requirements

### REQ-SC-F013: Mobile Service Catalog [Phase 2] [P0]
**Description**: Mobile-optimized catalog browsing.

**Acceptance Criteria**:
- Touch-friendly navigation
- Swipeable categories
- Responsive grid/list
- Sticky filter button
- Bottom sheet filters

---

### REQ-SC-F014: Mobile Service Inquiry [Phase 2] [P0]
**Description**: Mobile service request flow.

**Acceptance Criteria**:
- Simplified form
- Camera for vehicle photos
- Voice input for notes
- One-tap contact preference

---

## SEO Requirements

### REQ-SC-F015: Service SEO [Phase 2] [P1]
**Description**: SEO optimization for services.

**Acceptance Criteria**:
- Unique meta tags per service
- Schema.org Service markup
- Clean URLs with slugs
- Sitemap inclusion
- Open Graph tags

**Schema Example**:
```json
{
  "@type": "Service",
  "name": "Brake Pad Replacement",
  "description": "...",
  "provider": {...},
  "offers": {
    "@type": "Offer",
    "price": "150.00",
    "priceCurrency": "USD"
  }
}
```

---

## Performance Requirements

### REQ-SC-F016: Catalog Performance [Phase 2] [P0]
**Description**: Fast catalog loading.

**Acceptance Criteria**:
- Lazy load images
- Virtual scrolling for long lists
- Search debouncing
- Filter state in URL
- Cache service data
