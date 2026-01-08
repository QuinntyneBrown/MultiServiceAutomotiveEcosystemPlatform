# Professional Profile Page - Angular Implementation Specification

## Overview

The Professional Profile page is a **PUBLIC** individual professional profile page that displays comprehensive information about a service professional. This page implements **REQ-SP-F002** from the Service Professionals frontend requirements.

**Route**: `/professionals/:id` or `/p/:slug`

**Access Level**: Public (no authentication required)

**Layout**: Public header/navbar + footer (same as professional-directory)

---

## Component Architecture

### Main Component

**Component**: `ProfessionalProfileComponent`

**Path**: `src/app/public/professional-profile/professional-profile.component.ts`

**Responsibilities**:
- Fetch professional profile data via route parameter
- Handle page layout and data orchestration
- Manage SEO metadata
- Coordinate child components

### Child Components

1. **ProfileHeroComponent**
   - Cover photo display
   - Profile avatar with verified badge
   - Business name and professional name
   - Business type badge
   - Rating display
   - Contact action buttons

2. **ProfileAboutComponent**
   - Bio/description text
   - Statistics (years in business, service area, team size)
   - Key information display

3. **ProfileSpecialtiesComponent**
   - Specialties grid with icons
   - Certifications list
   - Service offerings

4. **ProfileGalleryComponent** (Phase 2)
   - Photo gallery grid
   - Lightbox functionality
   - Image lazy loading

5. **ProfileReviewsComponent** (Phase 3)
   - Reviews list
   - Rating breakdown
   - Pagination

6. **ProfileSidebarComponent**
   - Contact information card
   - Business hours card
   - Location map card
   - Inquiry form card

7. **InquiryFormComponent**
   - Contact form
   - Form validation
   - Submission handling

---

## Data Model

### Professional Profile Interface

```typescript
export interface ProfessionalProfile {
  // Basic Information
  id: string;
  slug: string;
  businessName: string;
  professionalName: string;
  title: string;
  businessType: BusinessType;

  // Verification
  isVerified: boolean;
  verificationDate?: Date;

  // Media
  profilePhotoUrl: string;
  coverPhotoUrl?: string;
  logoUrl?: string;

  // About
  bio: string;
  yearsInBusiness: number;
  serviceRadius: number;
  teamSize?: number;

  // Contact
  contactInfo: ContactInformation;

  // Specialties
  specialties: Specialty[];
  certifications: Certification[];

  // Gallery (Phase 2)
  galleryImages?: GalleryImage[];

  // Reviews (Phase 3)
  rating: Rating;
  reviews?: Review[];

  // Hours
  businessHours: BusinessHours;

  // Location
  location: Location;

  // Metadata
  createdAt: Date;
  updatedAt: Date;
  status: 'active' | 'inactive' | 'suspended';
}

export interface ContactInformation {
  phone: string;
  email: string;
  website?: string;
  socialLinks?: SocialLinks;
}

export interface Specialty {
  id: string;
  name: string;
  iconName: string;
  yearsExperience?: number;
}

export interface Certification {
  id: string;
  name: string;
  organization: string;
  certifiedDate: Date;
  expirationDate?: Date;
  certificateUrl?: string;
}

export interface Rating {
  average: number;
  count: number;
  distribution: {
    5: number;
    4: number;
    3: number;
    2: number;
    1: number;
  };
}

export interface BusinessHours {
  monday: DayHours;
  tuesday: DayHours;
  wednesday: DayHours;
  thursday: DayHours;
  friday: DayHours;
  saturday: DayHours;
  sunday: DayHours;
  timezone: string;
}

export interface DayHours {
  isOpen: boolean;
  openTime?: string; // HH:mm format
  closeTime?: string; // HH:mm format
}

export interface Location {
  address: Address;
  coordinates: Coordinates;
}

export interface Address {
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
}

export interface Coordinates {
  latitude: number;
  longitude: number;
}

export interface GalleryImage {
  id: string;
  url: string;
  thumbnailUrl: string;
  caption?: string;
  uploadedAt: Date;
  order: number;
}

export interface Review {
  id: string;
  customerName: string;
  customerAvatar?: string;
  rating: number;
  comment: string;
  reviewDate: Date;
  professionalResponse?: string;
  responseDate?: Date;
  isVerifiedCustomer: boolean;
}

export enum BusinessType {
  MECHANIC = 'mechanic',
  BODY_SHOP = 'body_shop',
  DETAILING = 'detailing',
  TIRE_SHOP = 'tire_shop',
  GLASS_REPAIR = 'glass_repair',
  OTHER = 'other'
}
```

---

## Service Layer

### ProfessionalProfileService

**Path**: `src/app/core/services/professional-profile.service.ts`

```typescript
@Injectable({
  providedIn: 'root'
})
export class ProfessionalProfileService {
  private apiUrl = '/api/professionals';

  constructor(private http: HttpClient) {}

  // Get professional profile by ID
  getProfileById(id: string): Observable<ProfessionalProfile> {
    return this.http.get<ProfessionalProfile>(`${this.apiUrl}/${id}`);
  }

  // Get professional profile by slug
  getProfileBySlug(slug: string): Observable<ProfessionalProfile> {
    return this.http.get<ProfessionalProfile>(`${this.apiUrl}/slug/${slug}`);
  }

  // Submit inquiry
  submitInquiry(professionalId: string, inquiry: InquiryRequest): Observable<InquiryResponse> {
    return this.http.post<InquiryResponse>(
      `${this.apiUrl}/${professionalId}/inquiries`,
      inquiry
    );
  }

  // Get reviews (Phase 3)
  getReviews(professionalId: string, page: number, limit: number): Observable<PaginatedReviews> {
    return this.http.get<PaginatedReviews>(
      `${this.apiUrl}/${professionalId}/reviews`,
      { params: { page: page.toString(), limit: limit.toString() } }
    );
  }
}

export interface InquiryRequest {
  name: string;
  email: string;
  phone?: string;
  message: string;
  source?: string;
}

export interface InquiryResponse {
  success: boolean;
  inquiryId: string;
  message: string;
}

export interface PaginatedReviews {
  reviews: Review[];
  total: number;
  page: number;
  limit: number;
  hasMore: boolean;
}
```

---

## Component Implementation Details

### 1. ProfessionalProfileComponent

**Template Structure**:
```html
<app-public-navbar></app-public-navbar>

<main class="profile-page">
  <app-profile-hero
    [profile]="profile$ | async"
    (callClick)="handleCallClick($event)"
    (emailClick)="handleEmailClick($event)"
    (inquiryClick)="scrollToInquiryForm()">
  </app-profile-hero>

  <div class="profile-container">
    <div class="profile-content">
      <app-profile-about [profile]="profile$ | async"></app-profile-about>
      <app-profile-specialties [profile]="profile$ | async"></app-profile-specialties>
      <app-profile-gallery [profile]="profile$ | async" *ngIf="phase2Enabled"></app-profile-gallery>
      <app-profile-reviews [profile]="profile$ | async" *ngIf="phase3Enabled"></app-profile-reviews>
    </div>

    <aside class="profile-sidebar">
      <app-profile-sidebar [profile]="profile$ | async"></app-profile-sidebar>
    </aside>
  </div>
</main>

<app-public-footer></app-public-footer>
```

**TypeScript**:
```typescript
export class ProfessionalProfileComponent implements OnInit, OnDestroy {
  profile$: Observable<ProfessionalProfile>;
  loading = true;
  error: string | null = null;

  phase2Enabled = false; // Gallery feature flag
  phase3Enabled = false; // Reviews feature flag

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private profileService: ProfessionalProfileService,
    private seoService: SeoService,
    private titleService: Title,
    private metaService: Meta
  ) {}

  ngOnInit(): void {
    // Get profile from route parameter (id or slug)
    this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');
        const slug = params.get('slug');

        if (slug) {
          return this.profileService.getProfileBySlug(slug);
        } else if (id) {
          return this.profileService.getProfileById(id);
        } else {
          throw new Error('No profile identifier provided');
        }
      }),
      tap(profile => this.updateSEO(profile)),
      catchError(error => {
        this.error = 'Failed to load professional profile';
        return of(null);
      }),
      takeUntil(this.destroy$)
    ).subscribe(profile => {
      if (profile) {
        this.loading = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  handleCallClick(phone: string): void {
    window.location.href = `tel:${phone}`;
  }

  handleEmailClick(email: string): void {
    window.location.href = `mailto:${email}`;
  }

  scrollToInquiryForm(): void {
    const inquiryForm = document.getElementById('inquiry-form');
    if (inquiryForm) {
      inquiryForm.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  private updateSEO(profile: ProfessionalProfile): void {
    // Update page title
    this.titleService.setTitle(
      `${profile.businessName} - ${profile.professionalName} | AutoPro Network`
    );

    // Update meta description
    this.metaService.updateTag({
      name: 'description',
      content: profile.bio.substring(0, 155)
    });

    // Open Graph tags
    this.metaService.updateTag({
      property: 'og:title',
      content: profile.businessName
    });

    this.metaService.updateTag({
      property: 'og:description',
      content: profile.bio.substring(0, 155)
    });

    this.metaService.updateTag({
      property: 'og:image',
      content: profile.profilePhotoUrl
    });

    this.metaService.updateTag({
      property: 'og:type',
      content: 'profile'
    });

    // Schema.org structured data
    this.seoService.addStructuredData({
      '@context': 'https://schema.org',
      '@type': 'LocalBusiness',
      'name': profile.businessName,
      'description': profile.bio,
      'telephone': profile.contactInfo.phone,
      'email': profile.contactInfo.email,
      'address': {
        '@type': 'PostalAddress',
        'streetAddress': profile.location.address.street,
        'addressLocality': profile.location.address.city,
        'addressRegion': profile.location.address.state,
        'postalCode': profile.location.address.zipCode,
        'addressCountry': profile.location.address.country
      },
      'geo': {
        '@type': 'GeoCoordinates',
        'latitude': profile.location.coordinates.latitude,
        'longitude': profile.location.coordinates.longitude
      },
      'aggregateRating': {
        '@type': 'AggregateRating',
        'ratingValue': profile.rating.average,
        'reviewCount': profile.rating.count
      }
    });
  }
}
```

---

### 2. ProfileHeroComponent

**Inputs**:
- `profile: ProfessionalProfile`

**Outputs**:
- `callClick: EventEmitter<string>`
- `emailClick: EventEmitter<string>`
- `inquiryClick: EventEmitter<void>`

**Key Features**:
- Display cover photo with fallback gradient
- Show profile avatar with verified badge overlay
- Render business type badge
- Display rating stars (use shared rating component)
- Responsive layout (stacks on mobile)
- Click-to-call and click-to-email buttons

---

### 3. ProfileAboutComponent

**Inputs**:
- `profile: ProfessionalProfile`

**Key Features**:
- Render bio with proper line breaks
- Display statistics grid (years, service area, team size)
- Icons for each stat using shared icon component
- Responsive grid layout

---

### 4. ProfileSpecialtiesComponent

**Inputs**:
- `profile: ProfessionalProfile`

**Key Features**:
- Grid of specialty cards with icons
- Certifications list with badges
- Visual distinction for active certifications
- Responsive grid (1-3 columns based on viewport)

**Icon Mapping**:
Use icon service to map specialty names to icon components

---

### 5. ProfileSidebarComponent

**Inputs**:
- `profile: ProfessionalProfile`

**Key Features**:
- Sticky positioning on desktop (top offset for navbar)
- Contact information display
- Business hours with "Open Now" indicator
- Location map integration (Google Maps or Mapbox)
- Inquiry form embedded

**Business Hours Logic**:
```typescript
getCurrentDayHours(hours: BusinessHours): { day: string; hours: DayHours; isToday: boolean } {
  const days = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];
  const today = new Date().getDay();
  const dayName = days[today];

  return {
    day: dayName,
    hours: hours[dayName],
    isToday: true
  };
}

isCurrentlyOpen(hours: DayHours): boolean {
  if (!hours.isOpen) return false;

  const now = new Date();
  const currentTime = `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}`;

  return currentTime >= hours.openTime && currentTime <= hours.closeTime;
}
```

---

### 6. InquiryFormComponent

**Inputs**:
- `professionalId: string`

**Outputs**:
- `submitted: EventEmitter<InquiryRequest>`

**Form Structure**:
```typescript
inquiryForm = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(2)]],
  email: ['', [Validators.required, Validators.email]],
  phone: [''],
  message: ['', [Validators.required, Validators.minLength(10)]]
});
```

**Validation**:
- Name: Required, min 2 characters
- Email: Required, valid email format
- Phone: Optional, valid phone format
- Message: Required, min 10 characters

**Submission Flow**:
1. Validate form
2. Show loading state on button
3. Call service to submit inquiry
4. Show success message
5. Reset form
6. Track conversion event (analytics)

---

## State Management

### Use RxJS for State

```typescript
// In component
profile$ = this.route.paramMap.pipe(
  switchMap(params => this.profileService.getProfileBySlug(params.get('slug'))),
  shareReplay(1)
);
```

### Loading States

- Show skeleton loaders for each section while loading
- Handle error states with user-friendly messages
- Retry mechanism for failed requests

---

## Routing Configuration

```typescript
const routes: Routes = [
  {
    path: 'professionals/:slug',
    component: ProfessionalProfileComponent,
    data: {
      title: 'Professional Profile',
      meta: {
        description: 'View professional automotive service provider profile'
      }
    }
  },
  {
    path: 'p/:slug', // Short URL
    redirectTo: 'professionals/:slug'
  }
];
```

---

## Styling Guidelines

### Layout
- Max width: 1200px container
- Main content: 2-column grid (content + sidebar)
- Content column: flexible width
- Sidebar: 360px fixed width
- Mobile: Single column, sidebar below content

### Spacing
- Section spacing: `var(--spacing-12)` (desktop), `var(--spacing-8)` (mobile)
- Card padding: `var(--spacing-8)` (desktop), `var(--spacing-6)` (mobile)
- Element gaps: `var(--spacing-6)`

### Colors
- Primary brand: `var(--color-blue-700)` (#00529F)
- Accent: `var(--color-yellow-700)` (#FFD520)
- Success (verified): `var(--color-success)`
- Background: `var(--color-bg-secondary)` for main area
- Cards: `var(--color-bg-primary)` with `var(--shadow-sm)`

### Typography
- Business name: `var(--text-display-sm)` with `var(--font-family-primary)`
- Section titles: `var(--text-h3)` with `var(--font-family-primary)`
- Body text: `var(--text-body-lg)` for main content
- Labels: `var(--text-caption)` uppercase

### Responsive Breakpoints
- Mobile: < 768px
- Tablet: 768px - 991px
- Desktop: >= 992px

---

## Accessibility Requirements

### ARIA Labels
- All action buttons have `aria-label`
- Section headings use proper heading hierarchy
- Form inputs have associated labels
- Images have descriptive alt text

### Keyboard Navigation
- All interactive elements are keyboard accessible
- Focus states clearly visible
- Tab order follows logical flow
- Skip links for main content

### Screen Reader Support
- Verified badge announced as "Verified Professional"
- Rating announced as "Rated 5.0 out of 5 stars based on 127 reviews"
- Business hours status announced (Open/Closed)
- Loading states announced

### Color Contrast
- All text meets WCAG AA contrast requirements
- Verified badge uses accessible green color
- Links have sufficient contrast and underline on hover

---

## Performance Optimization

### Image Loading
- Lazy load gallery images (Phase 2)
- Use responsive images with srcset
- Optimize cover photo (WebP with JPEG fallback)
- Placeholder images while loading

### Code Splitting
- Lazy load review component (Phase 3)
- Lazy load map component on scroll
- Tree-shake unused icons

### Caching
- Cache profile data with HTTP cache headers
- Use service worker for offline support
- Implement stale-while-revalidate strategy

### Bundle Size
- Keep initial bundle < 250KB
- Lazy load non-critical features
- Use dynamic imports for Phase 2/3 features

---

## Testing Strategy

### Unit Tests

**Profile Component**:
- Should load profile on init
- Should handle route parameter changes
- Should update SEO metadata
- Should handle errors gracefully

**Hero Component**:
- Should display profile information correctly
- Should emit events on button clicks
- Should show verified badge when verified
- Should format rating correctly

**Sidebar Component**:
- Should calculate current day hours
- Should determine if currently open
- Should format business hours correctly
- Should handle missing data gracefully

**Inquiry Form**:
- Should validate required fields
- Should validate email format
- Should submit form data correctly
- Should show success message on submission
- Should reset form after submission

### Integration Tests
- Profile loading flow
- Inquiry submission flow
- Navigation between directory and profile
- SEO metadata updates

### E2E Tests
- User can view professional profile
- User can submit inquiry
- User can click call/email buttons
- Profile displays correctly on mobile
- Loading states work correctly

---

## Phase 2/3 Feature Flags

Use feature flags to control Phase 2 and 3 features:

```typescript
// environment.ts
export const environment = {
  features: {
    profileGallery: false,  // Phase 2
    profileReviews: false,  // Phase 3
    socialLinks: false      // Phase 3
  }
};

// In component
phase2Enabled = environment.features.profileGallery;
phase3Enabled = environment.features.profileReviews;
```

---

## Analytics Tracking

Track the following events:

1. **Profile View**
   - Professional ID
   - View source (direct, directory, search)
   - Timestamp

2. **Contact Actions**
   - Call button click
   - Email button click
   - Inquiry form submission
   - Professional ID

3. **Engagement**
   - Time on page
   - Scroll depth
   - Gallery interactions (Phase 2)
   - Review pagination (Phase 3)

```typescript
// Analytics service calls
this.analytics.trackEvent('profile_view', {
  professionalId: profile.id,
  businessName: profile.businessName,
  source: this.route.snapshot.queryParams['source'] || 'direct'
});

this.analytics.trackEvent('contact_action', {
  professionalId: profile.id,
  action: 'call',
  timestamp: new Date()
});
```

---

## Error Handling

### Error States

1. **Profile Not Found (404)**
   - Show friendly error message
   - Suggest returning to directory
   - Log error for monitoring

2. **Network Error**
   - Show retry button
   - Cache last successful response
   - Offline indicator if applicable

3. **Invalid Profile Data**
   - Show generic error
   - Log validation errors
   - Gracefully degrade (hide sections with missing data)

### Error Component

```typescript
<app-error-state
  *ngIf="error"
  [type]="errorType"
  [message]="errorMessage"
  [showRetry]="true"
  (retry)="loadProfile()">
</app-error-state>
```

---

## Mobile Considerations

### Touch Targets
- Minimum 48x48px for all buttons
- Adequate spacing between clickable elements
- Larger hit areas for small icons

### Mobile-Specific Features
- Click-to-call integration
- Click-to-email with pre-filled subject
- Native share API for profile sharing
- Geolocation for directions

### Performance
- Reduce image sizes on mobile
- Minimize initial JavaScript bundle
- Optimize for 3G connections
- Progressive enhancement approach

---

## Security Considerations

### Data Validation
- Sanitize user input in inquiry form
- Validate email format server-side
- Rate limit inquiry submissions
- CAPTCHA for spam prevention (if needed)

### Privacy
- Don't expose sensitive professional data
- Mask email addresses (show form instead)
- Respect privacy settings
- GDPR compliance for EU users

### XSS Prevention
- Sanitize bio/description HTML
- Use Angular's built-in sanitization
- Content Security Policy headers
- Escape user-generated content

---

## Future Enhancements (Post Phase 3)

1. **Social Sharing**
   - Share profile on social media
   - Generate share cards with Open Graph
   - Track share metrics

2. **Booking Integration**
   - Direct appointment booking
   - Calendar integration
   - Availability display

3. **Video Content**
   - Introduction video
   - Service demonstration videos
   - Video testimonials

4. **Chat Widget**
   - Real-time messaging
   - Quick responses
   - Automated FAQs

5. **Loyalty Program Display**
   - Show rewards offered
   - Loyalty tier benefits
   - Points calculator
