# Professional Profile Page Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-SP-F002` |
| **Component Name** | ProfessionalProfilePage |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive public-facing professional profile page that showcases automotive service professionals to potential customers. The page presents business information, specialties, certifications, contact details, and business hours in a structured, visually appealing format that builds trust and encourages customer engagement.

## User Stories

1. **As a customer**, I want to view a professional's credentials and services, so I can decide if they meet my needs.
2. **As a customer**, I want to easily contact a professional, so I can schedule service or ask questions.
3. **As a customer**, I want to see business hours and location, so I can plan my visit.
4. **As a mobile user**, I want to quickly call or email the professional, so I can get service while on-the-go.
5. **As a search engine**, I want structured data, so I can properly index and display the professional in search results.

## Component Structure

```
ProfessionalProfilePage/
├── index.ts                              # Public exports
├── ProfessionalProfilePage.tsx           # Main component (React)
├── ProfessionalProfilePage.component.ts  # Main component (Angular)
├── ProfessionalProfilePage.module.scss   # Styles
├── ProfessionalProfilePage.test.tsx      # Unit tests
├── hooks/
│   ├── useProfessionalProfile.ts         # Data fetching
│   └── useContactActions.ts              # Contact button handlers
├── components/
│   ├── HeroSection/                      # Hero with cover & profile
│   │   ├── CoverPhoto.tsx
│   │   ├── ProfilePhoto.tsx
│   │   ├── ProfileHeader.tsx
│   │   └── ContactButtons.tsx
│   ├── AboutSection/                     # About & business info
│   │   ├── AboutText.tsx
│   │   └── BusinessStats.tsx
│   ├── SpecialtiesSection/               # Services & certifications
│   │   ├── SpecialtiesList.tsx
│   │   └── CertificationsList.tsx
│   ├── ContactSection/                   # Contact info & hours
│   │   ├── ContactInfo.tsx
│   │   ├── BusinessHours.tsx
│   │   └── MapPreview.tsx
│   └── Sidebar/                          # Sidebar container
│       ├── ContactCard.tsx
│       └── HoursCard.tsx
└── utils/
    ├── formatters.ts                     # Phone, address formatting
    └── seo.ts                            # SEO metadata generation
```

## Props / Inputs

### React

```typescript
interface ProfessionalProfilePageProps {
  /** Professional ID to load */
  professionalId: string;

  /** Pre-loaded professional data (optional) */
  professionalData?: ProfessionalProfile;

  /** Loading state override */
  isLoading?: boolean;

  /** Error state override */
  error?: Error;

  /** Callback when contact button clicked */
  onContactClick?: (action: ContactAction) => void;

  /** Callback when inquiry submitted */
  onInquirySubmit?: (inquiry: InquiryData) => void;

  /** Enable/disable map integration */
  enableMap?: boolean;

  /** Enable/disable reviews section (Phase 3) */
  enableReviews?: boolean;

  /** Enable/disable gallery section (Phase 2) */
  enableGallery?: boolean;
}

interface ProfessionalProfile {
  id: string;
  businessName: string;
  professionalName: string;
  title: string;
  businessType: BusinessType;
  verified: boolean;
  profilePhoto?: string;
  coverPhoto?: string;
  bio: string;
  yearsInBusiness: number;
  serviceArea: string;
  rating: {
    average: number;
    count: number;
  };
  specialties: Specialty[];
  certifications: Certification[];
  contact: ContactInfo;
  businessHours: BusinessHours;
  location: Location;
}

interface Specialty {
  id: string;
  name: string;
  icon?: string;
  experienceYears?: number;
}

interface Certification {
  id: string;
  name: string;
  issuedBy: string;
  issuedDate?: string;
  expiryDate?: string;
}

interface ContactInfo {
  phone: string;
  email: string;
  website?: string;
}

interface BusinessHours {
  monday: DayHours;
  tuesday: DayHours;
  wednesday: DayHours;
  thursday: DayHours;
  friday: DayHours;
  saturday: DayHours;
  sunday: DayHours;
}

interface DayHours {
  isOpen: boolean;
  openTime?: string;  // "HH:mm" format
  closeTime?: string; // "HH:mm" format
}

interface Location {
  address: string;
  city: string;
  state: string;
  zipCode: string;
  latitude?: number;
  longitude?: number;
}

type BusinessType =
  | 'auto_repair_shop'
  | 'mobile_mechanic'
  | 'detailing_service'
  | 'towing_service'
  | 'tire_shop'
  | 'body_shop'
  | 'other';

type ContactAction = 'call' | 'email' | 'inquiry';

interface InquiryData {
  name: string;
  email: string;
  phone: string;
  message: string;
}
```

### Angular

```typescript
@Component({
  selector: 'app-professional-profile-page',
  templateUrl: './professional-profile-page.component.html',
  styleUrls: ['./professional-profile-page.component.scss']
})
export class ProfessionalProfilePageComponent implements OnInit {
  @Input() professionalId!: string;
  @Input() professionalData?: ProfessionalProfile;
  @Input() isLoading?: boolean;
  @Input() error?: Error;
  @Input() enableMap = true;
  @Input() enableReviews = false;
  @Input() enableGallery = false;

  @Output() contactClick = new EventEmitter<ContactAction>();
  @Output() inquirySubmit = new EventEmitter<InquiryData>();

  profile$: Observable<ProfessionalProfile>;
  loading$ = new BehaviorSubject<boolean>(false);
  error$ = new BehaviorSubject<Error | null>(null);
}
```

## Section Components Breakdown

### 1. Hero Section

**Purpose**: Create strong first impression with professional branding

**Components**:
- Cover photo (gradient background if no image)
- Profile photo (circular, with border)
- Business name (prominent headline)
- Professional name and title
- Business type badge
- Verified badge (if verified)
- Rating summary (stars + count)
- Contact action buttons (Call, Email, Inquiry)

**Specifications**:
- Cover height: 280px desktop, 180px mobile
- Profile photo: 160px diameter desktop, 120px mobile
- Profile photo overlaps cover by 80px
- Card elevation: shadow-md
- Card radius: 12px

### 2. About Section

**Purpose**: Provide business background and build credibility

**Content**:
- Bio/description text (rich text, 2-3 paragraphs)
- Years in business
- Service area/coverage

**Layout**:
- Full-width card in main content area
- 2-column grid for stats (desktop), stacked (mobile)
- Max width for text: 760px

### 3. Specialties Section

**Purpose**: Highlight services and qualifications

**Components**:
- Specialties grid (2 columns desktop, 1 column mobile)
- Each specialty: icon + name
- Certifications list with icons
- Visual indicators (badges, colored borders)

**Specifications**:
- Specialty items: left border accent, icon, text
- Background: gray-50
- Border: 3px solid primary color
- Icons: 24px, primary color

### 4. Contact Section (Sidebar)

**Purpose**: Provide easy access to contact methods

**Components**:
- Contact Information Card
  - Phone (clickable tel: link)
  - Email (clickable mailto: link)
  - Address (with map icon)
- Business Hours Card
  - Weekly schedule
  - Current day highlighted
  - Map preview/placeholder

**Specifications**:
- Sidebar width: 380px desktop, full-width mobile
- Cards stack vertically with 24px gap
- Icons: 20px, primary color
- Today's hours highlighted in primary color

### 5. Map Preview

**Purpose**: Visual location reference

**Implementation**:
- Placeholder in Phase 1
- Google Maps/Mapbox integration in Phase 2
- Height: 200px
- Click to open full map view
- Border radius: 8px

## Visual Specifications

### Layout

| Element | Desktop | Tablet | Mobile |
|---------|---------|--------|--------|
| Container Max Width | 1200px | 100% | 100% |
| Content Grid | 2-column (760px + 380px) | 2-column | 1-column (stacked) |
| Grid Gap | 24px | 24px | 24px |
| Section Padding | 64px vertical | 48px vertical | 32px vertical |
| Card Padding | 24px | 24px | 24px |

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Cover Background (gradient start) | --color-primary | #00529F |
| Cover Background (gradient end) | --color-primary-dark | #003366 |
| Profile Photo Border | --color-white | #FFFFFF |
| Card Background | --color-white | #FFFFFF |
| Verified Badge BG | --color-success | #2E7D32 |
| Business Type Badge BG | --color-primary | #00529F |
| Stars Color | --color-accent | #FFD520 |
| Button Primary BG | --color-primary | #00529F |
| Button Secondary Border | --color-primary | #00529F |
| Button Accent BG | --color-accent | #FFD520 |
| Specialty Border | --color-primary | #00529F |
| Specialty Background | --color-gray-50 | #FAFAFA |
| Section Divider | --color-gray-100 | #F2F2F2 |

### Typography

| Element | Font Family | Size | Weight | Color |
|---------|-------------|------|--------|-------|
| Business Name | Montserrat | 32px / 24px (mobile) | 700 | gray-900 |
| Professional Title | Lato | 18px / 16px (mobile) | 400 | gray-700 |
| Section Titles | Montserrat | 24px / 20px (mobile) | 700 | gray-900 |
| Body Text | Lato | 16px | 400 | gray-700 |
| Button Text | Lato | 16px | 600 | varies |
| Badge Text | Lato | 14px / 12px | 600 | white |
| Info Labels | Lato | 12px | 600 (uppercase) | gray-600 |
| Info Values | Lato | 16px | 600 | gray-900 |

### Component Styles

#### Buttons

```scss
.btn-primary {
  background: var(--color-primary);
  color: var(--color-white);
  padding: 12px 24px;
  border-radius: var(--radius-md);
  font-weight: 600;
  transition: background 0.2s;

  &:hover {
    background: var(--color-primary-dark);
  }
}

.btn-secondary {
  background: var(--color-white);
  color: var(--color-primary);
  border: 2px solid var(--color-primary);
  padding: 12px 24px;
  border-radius: var(--radius-md);
  font-weight: 600;
  transition: background 0.2s;

  &:hover {
    background: var(--color-blue-100);
  }
}

.btn-accent {
  background: var(--color-accent);
  color: var(--color-gray-900);
  padding: 12px 24px;
  border-radius: var(--radius-md);
  font-weight: 600;
  transition: background 0.2s;

  &:hover {
    background: var(--color-yellow-800);
  }
}
```

#### Cards

```scss
.card {
  background: var(--color-white);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  padding: var(--spacing-6);
  transition: box-shadow 0.2s;
}

.card-title {
  font-family: var(--font-primary);
  font-size: 24px;
  font-weight: 700;
  color: var(--color-gray-900);
  margin-bottom: var(--spacing-4);
  padding-bottom: var(--spacing-3);
  border-bottom: 2px solid var(--color-gray-100);
}
```

#### Badges

```scss
.badge {
  display: inline-flex;
  align-items: center;
  padding: 6px 12px;
  border-radius: var(--radius-full);
  font-size: 14px;
  font-weight: 600;
}

.badge-primary {
  background: var(--color-primary);
  color: var(--color-white);
}

.verified-badge {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  background: var(--color-success);
  color: var(--color-white);
  padding: 4px 10px;
  border-radius: var(--radius-full);
  font-size: 12px;
  font-weight: 600;
}
```

## Mobile Responsiveness (REQ-SP-F014)

### Breakpoints

| Breakpoint | Width | Layout Changes |
|------------|-------|----------------|
| Desktop | ≥992px | 2-column grid, side-by-side buttons |
| Tablet | 768px-991px | 2-column grid, wrapped buttons |
| Mobile | <768px | 1-column stacked, full-width buttons |

### Mobile Optimizations

**Hero Section**:
- Cover height reduced to 180px
- Profile photo reduced to 120px diameter
- Business name font size reduced to 24px
- Center-aligned text
- Stacked contact buttons (full-width)

**Content Grid**:
- Single column layout
- Sidebar appears after main content
- Full-width cards

**Specialties**:
- Single column grid
- Touch-friendly spacing (min 44px height)

**Contact Buttons**:
- Full-width buttons
- Stacked vertically
- 48px height for better touch targets
- Icons remain visible

**Business Hours**:
- Condensed day labels
- Smaller font sizes
- Maintained readability

### Touch Interactions

- Minimum touch target: 44x44px
- Click-to-call on phone numbers
- Click-to-email on email addresses
- Click-to-navigate on address (opens maps app)
- Tap-to-expand for long descriptions

## SEO Requirements (REQ-SP-F016)

### Meta Tags

```html
<title>{businessName} - {businessType} in {city}, {state}</title>
<meta name="description" content="{bio excerpt (160 chars)}" />
<meta name="keywords" content="{specialties joined}, {city}, {businessType}" />

<!-- Open Graph -->
<meta property="og:type" content="business.business" />
<meta property="og:title" content="{businessName}" />
<meta property="og:description" content="{bio excerpt}" />
<meta property="og:image" content="{profilePhoto or coverPhoto}" />
<meta property="og:url" content="{canonical URL}" />

<!-- Twitter Card -->
<meta name="twitter:card" content="summary_large_image" />
<meta name="twitter:title" content="{businessName}" />
<meta name="twitter:description" content="{bio excerpt}" />
<meta name="twitter:image" content="{profilePhoto or coverPhoto}" />
```

### Schema.org Structured Data

```json
{
  "@context": "https://schema.org",
  "@type": "LocalBusiness",
  "@id": "{canonical URL}",
  "name": "{businessName}",
  "description": "{bio}",
  "image": [
    "{profilePhoto}",
    "{coverPhoto}"
  ],
  "telephone": "{phone}",
  "email": "{email}",
  "address": {
    "@type": "PostalAddress",
    "streetAddress": "{address}",
    "addressLocality": "{city}",
    "addressRegion": "{state}",
    "postalCode": "{zipCode}",
    "addressCountry": "US"
  },
  "geo": {
    "@type": "GeoCoordinates",
    "latitude": "{latitude}",
    "longitude": "{longitude}"
  },
  "url": "{website}",
  "aggregateRating": {
    "@type": "AggregateRating",
    "ratingValue": "{rating.average}",
    "reviewCount": "{rating.count}"
  },
  "openingHoursSpecification": [
    {
      "@type": "OpeningHoursSpecification",
      "dayOfWeek": "Monday",
      "opens": "{businessHours.monday.openTime}",
      "closes": "{businessHours.monday.closeTime}"
    }
    // ... repeat for each day
  ],
  "priceRange": "$$",
  "paymentAccepted": "Cash, Credit Card",
  "currenciesAccepted": "USD"
}
```

### URL Structure

```
/professionals/{slug}
/professionals/{professionalId}/{business-name-slug}
```

**Example**: `/professionals/12345/mikes-auto-repair`

### Sitemap Integration

- Include all verified professional profiles
- Update frequency: weekly
- Priority: 0.7

## Accessibility Requirements (REQ-SP-F017)

### WCAG 2.1 AA Compliance

**Color Contrast**:
- All text meets 4.5:1 ratio (normal text)
- Large text (≥18px bold, ≥24px) meets 3:1 ratio
- Interactive elements meet 3:1 ratio

**Heading Hierarchy**:
```html
<h1>Business Name</h1>
<h2>Section Titles (About, Specialties, etc.)</h2>
<h3>Subsection Titles (Certifications)</h3>
```

**Landmarks**:
```html
<header role="banner">
  <!-- Hero section -->
</header>
<main role="main">
  <section aria-labelledby="about-heading">
    <h2 id="about-heading">About</h2>
  </section>
  <section aria-labelledby="specialties-heading">
    <h2 id="specialties-heading">Specialties</h2>
  </section>
</main>
<aside role="complementary" aria-label="Contact information">
  <!-- Sidebar -->
</aside>
```

**Screen Reader Support**:
```html
<!-- Profile Photo -->
<img src="{profilePhoto}" alt="Profile photo of {professionalName}" />

<!-- Verified Badge -->
<span class="verified-badge" aria-label="Verified professional">
  <svg aria-hidden="true">...</svg>
  Verified
</span>

<!-- Rating -->
<div class="rating-summary" role="img" aria-label="Rated {average} out of 5 stars based on {count} reviews">
  <div class="stars" aria-hidden="true">★★★★★</div>
  <span class="rating-text">{average}</span>
  <span class="rating-count">({count} reviews)</span>
</div>

<!-- Contact Buttons -->
<button aria-label="Call {businessName} at {phone}">
  <svg aria-hidden="true">...</svg>
  Call Now
</button>

<!-- Business Hours -->
<div class="hours-row">
  <span class="hours-day" aria-current="day">Monday</span>
  <span class="hours-time">8:00 AM - 6:00 PM</span>
</div>
```

**Keyboard Navigation**:
- All interactive elements focusable
- Logical tab order
- Visible focus indicators
- Skip to main content link
- Escape to close modals/dialogs

**Focus Management**:
```scss
.btn:focus-visible,
a:focus-visible {
  outline: none;
  box-shadow: var(--shadow-focus);
}
```

## API Integration

### Get Professional Profile Endpoint

```typescript
// GET /api/v1/professionals/{professionalId}
interface GetProfessionalResponse {
  success: boolean;
  data?: ProfessionalProfile;
  error?: {
    code: string;
    message: string;
  };
}
```

### Send Inquiry Endpoint

```typescript
// POST /api/v1/professionals/{professionalId}/inquiries
interface SendInquiryRequest {
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  message: string;
  source: 'profile_page';
}

interface SendInquiryResponse {
  success: boolean;
  data?: {
    inquiryId: string;
    createdAt: string;
  };
  error?: {
    code: string;
    message: string;
  };
}
```

### Track Profile View

```typescript
// POST /api/v1/professionals/{professionalId}/views
interface TrackViewRequest {
  source: string;
  referrer?: string;
  utmParams?: Record<string, string>;
}
```

## State Management

```typescript
interface ProfessionalProfileState {
  // Data
  profile: ProfessionalProfile | null;

  // Loading states
  isLoading: boolean;
  isSubmittingInquiry: boolean;

  // Error states
  loadError: Error | null;
  inquiryError: Error | null;

  // UI states
  isInquiryModalOpen: boolean;
  selectedContactAction: ContactAction | null;

  // Analytics
  viewTracked: boolean;
}
```

## Error Handling

### Load Errors

| Error Code | Message | Behavior |
|------------|---------|----------|
| NOT_FOUND | "Professional not found" | Show 404 page |
| SUSPENDED | "This profile is temporarily unavailable" | Show suspension notice |
| NOT_VERIFIED | "This profile is pending verification" | Show pending notice |
| NETWORK_ERROR | "Unable to load profile. Please try again." | Show retry button |

### Inquiry Errors

| Error Code | Message | Behavior |
|------------|---------|----------|
| VALIDATION_ERROR | Field-specific message | Show below field |
| RATE_LIMITED | "Too many inquiries. Please try again later." | Show alert |
| SERVER_ERROR | "Unable to send inquiry. Please try again." | Show alert, enable retry |

## Analytics Events

```typescript
// Track page view
trackEvent('professional_profile_viewed', {
  professionalId: string,
  businessType: string,
  verified: boolean,
  source: string
});

// Track contact actions
trackEvent('contact_action_clicked', {
  professionalId: string,
  action: 'call' | 'email' | 'inquiry',
  source: 'profile_page'
});

// Track inquiry submission
trackEvent('inquiry_submitted', {
  professionalId: string,
  source: 'profile_page'
});

// Track scroll depth
trackEvent('profile_scroll_depth', {
  professionalId: string,
  depth: '25%' | '50%' | '75%' | '100%'
});
```

## Performance Requirements

### Loading Performance

- **Initial Load**: < 3 seconds (3G connection)
- **Time to Interactive**: < 4 seconds
- **First Contentful Paint**: < 1.5 seconds
- **Largest Contentful Paint**: < 2.5 seconds

### Optimization Strategies

1. **Image Optimization**:
   - Lazy load below-the-fold images
   - Responsive images (srcset)
   - WebP format with fallback
   - Profile photo: max 400x400px
   - Cover photo: max 1600x600px

2. **Code Splitting**:
   - Lazy load inquiry modal
   - Lazy load map component
   - Separate bundle for gallery (Phase 2)

3. **Caching**:
   - Cache profile data (5 minutes)
   - CDN for static assets
   - Service worker for offline support

4. **Critical CSS**:
   - Inline above-the-fold styles
   - Defer non-critical CSS

## Testing Requirements

### Unit Tests

```typescript
describe('ProfessionalProfilePage', () => {
  it('renders profile data correctly', () => {});
  it('displays verified badge when verified', () => {});
  it('hides verified badge when not verified', () => {});
  it('formats phone number correctly', () => {});
  it('formats business hours correctly', () => {});
  it('highlights current day in hours', () => {});
  it('displays specialties grid', () => {});
  it('displays certifications list', () => {});
  it('handles missing optional fields', () => {});
  it('shows loading state', () => {});
  it('shows error state', () => {});
  it('tracks profile view on mount', () => {});
  it('is accessible (axe audit)', () => {});
});

describe('ContactButtons', () => {
  it('calls onContactClick with "call" action', () => {});
  it('calls onContactClick with "email" action', () => {});
  it('calls onContactClick with "inquiry" action', () => {});
  it('formats tel: link correctly', () => {});
  it('formats mailto: link correctly', () => {});
});

describe('BusinessHours', () => {
  it('displays all days of week', () => {});
  it('highlights current day', () => {});
  it('displays "Closed" for closed days', () => {});
  it('formats time correctly', () => {});
});
```

### Integration Tests

```typescript
describe('Profile Loading', () => {
  it('loads profile data on mount', () => {});
  it('displays loading skeleton while loading', () => {});
  it('displays profile when loaded', () => {});
  it('handles load error gracefully', () => {});
  it('retries on network error', () => {});
});

describe('Inquiry Flow', () => {
  it('opens inquiry modal on button click', () => {});
  it('submits inquiry with valid data', () => {});
  it('displays success message on submit', () => {});
  it('displays error on submit failure', () => {});
  it('validates form fields', () => {});
});
```

### E2E Tests

```typescript
describe('Professional Profile Page', () => {
  it('displays complete profile', () => {});
  it('allows calling professional', () => {});
  it('allows emailing professional', () => {});
  it('allows sending inquiry', () => {});
  it('is responsive on mobile', () => {});
  it('is accessible via keyboard', () => {});
  it('has proper SEO metadata', () => {});
});
```

### Visual Regression Tests

- Hero section layout
- Card layouts
- Button states (default, hover, active, focus)
- Mobile responsive breakpoints
- Dark mode compatibility (future)

## Dependencies

- Image optimization library (sharp, next/image, etc.)
- Phone formatting library (libphonenumber-js)
- Map integration (Google Maps API / Mapbox)
- Rich text renderer (for bio)
- SEO utilities (next-seo, react-helmet, etc.)
- Analytics library

## Related Components

- `Header` - Platform header
- `Footer` - Platform footer
- `Button` - Shared button component
- `Card` - Shared card component
- `Badge` - Shared badge component
- `LoadingSkeleton` - Loading state component
- `ErrorBoundary` - Error handling
- `InquiryModal` - Contact form modal (Phase 1)
- `MapPreview` - Location map component (Phase 2)
- `ImageGallery` - Portfolio gallery (Phase 2)
- `ReviewsList` - Customer reviews (Phase 3)

## Security Considerations

1. **Data Privacy**:
   - Only display verified business information
   - Mask email to prevent scraping (use contact form)
   - Rate limit inquiry submissions

2. **XSS Prevention**:
   - Sanitize all user-generated content (bio, etc.)
   - Escape HTML in text fields
   - Content Security Policy headers

3. **CSRF Protection**:
   - Include CSRF token in inquiry form
   - Validate token on server

4. **Rate Limiting**:
   - Max 5 inquiries per IP per hour
   - Max 3 inquiries per email per day

## Future Enhancements (Post-Phase 1)

### Phase 2
- Photo gallery with lightbox
- Social media links
- Share profile functionality
- Print-friendly version
- Service catalog integration

### Phase 3
- Customer reviews and ratings
- Q&A section
- Booking calendar integration
- Live chat widget
- Video introduction

### Phase 4
- Service pricing display
- Promotions/specials section
- Customer testimonials carousel
- Blog/articles integration
- Multi-language support

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-08 | Initial specification |
