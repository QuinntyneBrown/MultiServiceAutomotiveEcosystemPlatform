# Professional Directory - Angular Implementation Specification

**Feature**: REQ-SP-F001 - Professional Directory Page
**Type**: Public Page
**Phase**: 1
**Priority**: P0

---

## Overview

The Professional Directory is a public-facing page that allows visitors to browse, search, and filter automotive service professionals. This is a critical entry point for customer acquisition and must be optimized for both user experience and search engine visibility.

---

## Component Architecture

### Primary Component

```
src/app/pages/professional-directory/
├── professional-directory.component.ts
├── professional-directory.component.html
├── professional-directory.component.scss
├── professional-directory.component.spec.ts
└── components/
    ├── professional-card/
    │   ├── professional-card.component.ts
    │   ├── professional-card.component.html
    │   ├── professional-card.component.scss
    │   └── professional-card.component.spec.ts
    ├── filter-sidebar/
    │   ├── filter-sidebar.component.ts
    │   ├── filter-sidebar.component.html
    │   ├── filter-sidebar.component.scss
    │   └── filter-sidebar.component.spec.ts
    ├── search-hero/
    │   ├── search-hero.component.ts
    │   ├── search-hero.component.html
    │   ├── search-hero.component.scss
    │   └── search-hero.component.spec.ts
    └── pagination/
        ├── pagination.component.ts
        ├── pagination.component.html
        ├── pagination.component.scss
        └── pagination.component.spec.ts
```

---

## Data Models

### Professional Interface

```typescript
export interface Professional {
  id: string;
  businessName: string;
  professionalName: string;
  title?: string;
  businessType: BusinessType;
  profilePhotoUrl?: string;
  coverPhotoUrl?: string;
  verified: boolean;
  featured: boolean;
  rating: number;
  reviewCount: number;
  specialties: Specialty[];
  location: {
    address: string;
    city: string;
    state: string;
    zipCode: string;
    latitude?: number;
    longitude?: number;
  };
  contactInfo: {
    phone: string;
    email: string;
    website?: string;
  };
  bio?: string;
  yearsInBusiness?: number;
  certifications?: Certification[];
  createdAt: Date;
  updatedAt: Date;
}

export interface Specialty {
  id: string;
  name: string;
  category: string;
  displayOrder: number;
}

export interface Certification {
  id: string;
  name: string;
  issuingOrganization: string;
  issuedDate: Date;
  expirationDate?: Date;
}

export enum BusinessType {
  MECHANIC = 'Mechanic/Repair Shop',
  BODY_SHOP = 'Body Shop',
  DETAILING = 'Detailing Service',
  TIRE_SHOP = 'Tire Shop',
  GLASS_REPAIR = 'Glass Repair',
  TOWING = 'Towing Service',
  ELECTRICAL = 'Electrical Specialist',
  OTHER = 'Other'
}
```

### Filter State Interface

```typescript
export interface DirectoryFilters {
  searchQuery: string;
  location: string;
  businessTypes: BusinessType[];
  specialties: string[];
  verifiedOnly: boolean;
  featuredOnly: boolean;
  openNow: boolean;
  sortBy: SortOption;
  page: number;
  pageSize: number;
}

export enum SortOption {
  FEATURED = 'featured',
  A_TO_Z = 'a-z',
  HIGHEST_RATED = 'rating',
  MOST_REVIEWS = 'reviews',
  NEAREST = 'nearest'
}

export interface DirectoryResponse {
  professionals: Professional[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  filters: {
    availableBusinessTypes: { type: BusinessType; count: number }[];
    availableSpecialties: { specialty: string; count: number }[];
  };
}
```

---

## Component Specifications

### 1. ProfessionalDirectoryComponent

**Responsibility**: Main container component managing state and coordination

**Template Structure**:
```html
<app-public-navbar></app-public-navbar>
<app-search-hero (search)="onSearch($event)"></app-search-hero>

<main class="main-content">
  <div class="content-wrapper container">
    <app-filter-sidebar
      [filters]="filters"
      [availableFilters]="availableFilters"
      (filtersChanged)="onFiltersChanged($event)">
    </app-filter-sidebar>

    <section class="results">
      <div class="results__header">
        <div class="results__info">
          Showing <strong>{{ total }} professionals</strong>
          <span *ngIf="hasActiveFilters()"> matching your filters</span>
        </div>

        <div class="results__controls">
          <app-view-toggle
            [viewMode]="viewMode"
            (viewModeChanged)="onViewModeChanged($event)">
          </app-view-toggle>

          <app-sort-select
            [sortBy]="filters.sortBy"
            (sortChanged)="onSortChanged($event)">
          </app-sort-select>
        </div>
      </div>

      <!-- Grid View -->
      <div *ngIf="viewMode === 'grid'" class="professionals-grid">
        <app-professional-card
          *ngFor="let professional of professionals; trackBy: trackByProfessionalId"
          [professional]="professional">
        </app-professional-card>
      </div>

      <!-- List View -->
      <div *ngIf="viewMode === 'list'" class="professionals-list">
        <app-professional-list-item
          *ngFor="let professional of professionals; trackBy: trackByProfessionalId"
          [professional]="professional">
        </app-professional-list-item>
      </div>

      <!-- Loading State -->
      <div *ngIf="loading" class="loading-state">
        <app-skeleton-grid [count]="6"></app-skeleton-grid>
      </div>

      <!-- Empty State -->
      <div *ngIf="!loading && professionals.length === 0" class="empty-state">
        <app-empty-state
          icon="search"
          title="No professionals found"
          message="Try adjusting your filters or search criteria"
          [actions]="emptyStateActions">
        </app-empty-state>
      </div>

      <!-- Pagination -->
      <app-pagination
        *ngIf="!loading && professionals.length > 0"
        [currentPage]="filters.page"
        [totalPages]="totalPages"
        (pageChanged)="onPageChanged($event)">
      </app-pagination>
    </section>
  </div>
</main>

<app-public-footer></app-public-footer>
```

**Component Class**:
```typescript
@Component({
  selector: 'app-professional-directory',
  templateUrl: './professional-directory.component.html',
  styleUrls: ['./professional-directory.component.scss']
})
export class ProfessionalDirectoryComponent implements OnInit, OnDestroy {
  professionals: Professional[] = [];
  total = 0;
  totalPages = 0;
  loading = false;
  viewMode: 'grid' | 'list' = 'grid';

  filters: DirectoryFilters = {
    searchQuery: '',
    location: '',
    businessTypes: [],
    specialties: [],
    verifiedOnly: false,
    featuredOnly: false,
    openNow: false,
    sortBy: SortOption.FEATURED,
    page: 1,
    pageSize: 12
  };

  availableFilters: DirectoryResponse['filters'] | null = null;
  emptyStateActions = [
    { label: 'Clear Filters', handler: () => this.clearFilters() },
    { label: 'View All Professionals', handler: () => this.viewAll() }
  ];

  private destroy$ = new Subject<void>();

  constructor(
    private professionalService: ProfessionalService,
    private route: ActivatedRoute,
    private router: Router,
    private seo: SeoService
  ) {}

  ngOnInit(): void {
    this.initializeFromQueryParams();
    this.loadProfessionals();
    this.setSeoMetadata();
    this.trackPageView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeFromQueryParams(): void {
    this.route.queryParams
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        if (params['q']) this.filters.searchQuery = params['q'];
        if (params['location']) this.filters.location = params['location'];
        if (params['type']) this.filters.businessTypes = params['type'].split(',');
        if (params['specialty']) this.filters.specialties = params['specialty'].split(',');
        if (params['verified']) this.filters.verifiedOnly = params['verified'] === 'true';
        if (params['sort']) this.filters.sortBy = params['sort'];
        if (params['page']) this.filters.page = +params['page'];
      });
  }

  loadProfessionals(): void {
    this.loading = true;

    this.professionalService.searchProfessionals(this.filters)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.loading = false)
      )
      .subscribe({
        next: (response) => {
          this.professionals = response.professionals;
          this.total = response.total;
          this.totalPages = response.totalPages;
          this.availableFilters = response.filters;
        },
        error: (error) => {
          console.error('Error loading professionals:', error);
          // Show error toast/notification
        }
      });
  }

  onSearch(searchData: { query: string; location: string }): void {
    this.filters.searchQuery = searchData.query;
    this.filters.location = searchData.location;
    this.filters.page = 1;
    this.updateQueryParams();
    this.loadProfessionals();
  }

  onFiltersChanged(filters: Partial<DirectoryFilters>): void {
    this.filters = { ...this.filters, ...filters, page: 1 };
    this.updateQueryParams();
    this.loadProfessionals();
  }

  onSortChanged(sortBy: SortOption): void {
    this.filters.sortBy = sortBy;
    this.filters.page = 1;
    this.updateQueryParams();
    this.loadProfessionals();
  }

  onPageChanged(page: number): void {
    this.filters.page = page;
    this.updateQueryParams();
    this.loadProfessionals();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onViewModeChanged(viewMode: 'grid' | 'list'): void {
    this.viewMode = viewMode;
    localStorage.setItem('professionalDirectoryViewMode', viewMode);
  }

  clearFilters(): void {
    this.filters = {
      ...this.filters,
      businessTypes: [],
      specialties: [],
      verifiedOnly: false,
      featuredOnly: false,
      openNow: false,
      page: 1
    };
    this.updateQueryParams();
    this.loadProfessionals();
  }

  viewAll(): void {
    this.filters = {
      searchQuery: '',
      location: '',
      businessTypes: [],
      specialties: [],
      verifiedOnly: false,
      featuredOnly: false,
      openNow: false,
      sortBy: SortOption.FEATURED,
      page: 1,
      pageSize: 12
    };
    this.updateQueryParams();
    this.loadProfessionals();
  }

  hasActiveFilters(): boolean {
    return this.filters.businessTypes.length > 0
      || this.filters.specialties.length > 0
      || this.filters.verifiedOnly
      || this.filters.featuredOnly
      || this.filters.openNow
      || this.filters.searchQuery !== ''
      || this.filters.location !== '';
  }

  trackByProfessionalId(index: number, professional: Professional): string {
    return professional.id;
  }

  private updateQueryParams(): void {
    const queryParams: any = {};

    if (this.filters.searchQuery) queryParams.q = this.filters.searchQuery;
    if (this.filters.location) queryParams.location = this.filters.location;
    if (this.filters.businessTypes.length) queryParams.type = this.filters.businessTypes.join(',');
    if (this.filters.specialties.length) queryParams.specialty = this.filters.specialties.join(',');
    if (this.filters.verifiedOnly) queryParams.verified = 'true';
    if (this.filters.sortBy !== SortOption.FEATURED) queryParams.sort = this.filters.sortBy;
    if (this.filters.page > 1) queryParams.page = this.filters.page;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge'
    });
  }

  private setSeoMetadata(): void {
    this.seo.setTitle('Find Trusted Automotive Professionals | AutoPro Network');
    this.seo.setDescription('Browse verified automotive professionals including mechanics, body shops, detailers, and more. Find trusted service providers in your area.');
    this.seo.setKeywords('automotive professionals, mechanics, auto repair, body shop, car detailing');
    this.seo.setCanonicalUrl('/professionals');
  }

  private trackPageView(): void {
    // Analytics tracking
    // this.analytics.trackPageView('Professional Directory');
  }
}
```

---

### 2. ProfessionalCardComponent

**Responsibility**: Display individual professional in card format

**Inputs**:
```typescript
@Input() professional!: Professional;
@Input() featured = false;
```

**Template Structure**:
```html
<article class="pro-card" [class.pro-card--featured]="professional.featured">
  <span *ngIf="professional.featured" class="featured-badge">
    <mat-icon>star</mat-icon>
    Featured
  </span>

  <div class="pro-card__header">
    <div class="pro-card__avatar">
      <img
        [src]="professional.profilePhotoUrl || defaultAvatar"
        [alt]="professional.businessName"
        loading="lazy">
      <div *ngIf="professional.verified" class="pro-card__verified">
        <mat-icon>verified</mat-icon>
      </div>
    </div>

    <div class="pro-card__info">
      <h3 class="pro-card__business-name">{{ professional.businessName }}</h3>
      <p class="pro-card__pro-name">{{ professional.professionalName }}<span *ngIf="professional.title">, {{ professional.title }}</span></p>
      <span class="pro-card__type-badge">{{ professional.businessType }}</span>

      <div class="pro-card__rating">
        <app-rating-stars [rating]="professional.rating"></app-rating-stars>
        <span class="rating-count">{{ professional.rating.toFixed(1) }} ({{ professional.reviewCount }})</span>
      </div>
    </div>
  </div>

  <div class="pro-card__body">
    <div class="pro-card__specialties">
      <span
        *ngFor="let specialty of getTopSpecialties()"
        class="specialty-chip">
        {{ specialty.name }}
      </span>
    </div>
  </div>

  <div class="pro-card__footer">
    <a
      [routerLink]="['/professionals', professional.id]"
      class="btn"
      [class.btn--primary]="professional.featured"
      [class.btn--secondary]="!professional.featured"
      class="btn--md">
      View Profile
    </a>
  </div>
</article>
```

**Component Class**:
```typescript
@Component({
  selector: 'app-professional-card',
  templateUrl: './professional-card.component.html',
  styleUrls: ['./professional-card.component.scss']
})
export class ProfessionalCardComponent {
  @Input() professional!: Professional;

  defaultAvatar = 'assets/images/default-avatar.png';

  getTopSpecialties(): Specialty[] {
    return this.professional.specialties
      .sort((a, b) => a.displayOrder - b.displayOrder)
      .slice(0, 3);
  }
}
```

---

### 3. FilterSidebarComponent

**Responsibility**: Manage filter UI and emit filter changes

**Inputs/Outputs**:
```typescript
@Input() filters!: DirectoryFilters;
@Input() availableFilters!: DirectoryResponse['filters'];
@Output() filtersChanged = new EventEmitter<Partial<DirectoryFilters>>();
```

**Template Structure**:
```html
<aside class="filters">
  <div class="filters__header">
    <h2 class="filters__title">Filters</h2>
    <button
      class="filters__clear"
      (click)="clearFilters()"
      [disabled]="!hasActiveFilters()">
      Clear All
    </button>
  </div>

  <!-- Business Type Filter -->
  <div class="filter-group">
    <h3 class="filter-group__title">Business Type</h3>
    <div
      *ngFor="let type of availableFilters?.availableBusinessTypes"
      class="filter-option">
      <input
        type="checkbox"
        [id]="'type-' + type.type"
        [checked]="isBusinessTypeSelected(type.type)"
        (change)="toggleBusinessType(type.type)">
      <label [for]="'type-' + type.type">
        <span>{{ type.type }}</span>
        <span class="filter-option__count">{{ type.count }}</span>
      </label>
    </div>
  </div>

  <!-- Specialty Filter -->
  <div class="filter-group">
    <h3 class="filter-group__title">Specialty</h3>
    <div
      *ngFor="let specialty of getTopSpecialties()"
      class="filter-option">
      <input
        type="checkbox"
        [id]="'specialty-' + specialty.specialty"
        [checked]="isSpecialtySelected(specialty.specialty)"
        (change)="toggleSpecialty(specialty.specialty)">
      <label [for]="'specialty-' + specialty.specialty">
        <span>{{ specialty.specialty }}</span>
        <span class="filter-option__count">{{ specialty.count }}</span>
      </label>
    </div>
    <button
      *ngIf="availableFilters?.availableSpecialties.length > 5"
      class="filter-group__show-more"
      (click)="showAllSpecialties = !showAllSpecialties">
      {{ showAllSpecialties ? 'Show Less' : 'Show More' }}
    </button>
  </div>

  <!-- Additional Filters -->
  <div class="filter-group">
    <h3 class="filter-group__title">Additional Filters</h3>

    <div class="filter-option">
      <input
        type="checkbox"
        id="filter-verified"
        [checked]="filters.verifiedOnly"
        (change)="toggleVerifiedOnly()">
      <label for="filter-verified">
        <span>Verified Only</span>
      </label>
    </div>

    <div class="filter-option">
      <input
        type="checkbox"
        id="filter-featured"
        [checked]="filters.featuredOnly"
        (change)="toggleFeaturedOnly()">
      <label for="filter-featured">
        <span>Featured</span>
      </label>
    </div>

    <div class="filter-option">
      <input
        type="checkbox"
        id="filter-open-now"
        [checked]="filters.openNow"
        (change)="toggleOpenNow()">
      <label for="filter-open-now">
        <span>Open Now</span>
      </label>
    </div>
  </div>
</aside>
```

**Component Class**:
```typescript
@Component({
  selector: 'app-filter-sidebar',
  templateUrl: './filter-sidebar.component.html',
  styleUrls: ['./filter-sidebar.component.scss']
})
export class FilterSidebarComponent {
  @Input() filters!: DirectoryFilters;
  @Input() availableFilters!: DirectoryResponse['filters'];
  @Output() filtersChanged = new EventEmitter<Partial<DirectoryFilters>>();

  showAllSpecialties = false;

  isBusinessTypeSelected(type: BusinessType): boolean {
    return this.filters.businessTypes.includes(type);
  }

  isSpecialtySelected(specialty: string): boolean {
    return this.filters.specialties.includes(specialty);
  }

  toggleBusinessType(type: BusinessType): void {
    const types = [...this.filters.businessTypes];
    const index = types.indexOf(type);

    if (index > -1) {
      types.splice(index, 1);
    } else {
      types.push(type);
    }

    this.filtersChanged.emit({ businessTypes: types });
  }

  toggleSpecialty(specialty: string): void {
    const specialties = [...this.filters.specialties];
    const index = specialties.indexOf(specialty);

    if (index > -1) {
      specialties.splice(index, 1);
    } else {
      specialties.push(specialty);
    }

    this.filtersChanged.emit({ specialties });
  }

  toggleVerifiedOnly(): void {
    this.filtersChanged.emit({ verifiedOnly: !this.filters.verifiedOnly });
  }

  toggleFeaturedOnly(): void {
    this.filtersChanged.emit({ featuredOnly: !this.filters.featuredOnly });
  }

  toggleOpenNow(): void {
    this.filtersChanged.emit({ openNow: !this.filters.openNow });
  }

  clearFilters(): void {
    this.filtersChanged.emit({
      businessTypes: [],
      specialties: [],
      verifiedOnly: false,
      featuredOnly: false,
      openNow: false
    });
  }

  hasActiveFilters(): boolean {
    return this.filters.businessTypes.length > 0
      || this.filters.specialties.length > 0
      || this.filters.verifiedOnly
      || this.filters.featuredOnly
      || this.filters.openNow;
  }

  getTopSpecialties(): any[] {
    if (!this.availableFilters?.availableSpecialties) return [];
    return this.showAllSpecialties
      ? this.availableFilters.availableSpecialties
      : this.availableFilters.availableSpecialties.slice(0, 5);
  }
}
```

---

## Service Layer

### ProfessionalService

```typescript
@Injectable({
  providedIn: 'root'
})
export class ProfessionalService {
  private apiUrl = environment.apiUrl + '/professionals';

  constructor(private http: HttpClient) {}

  searchProfessionals(filters: DirectoryFilters): Observable<DirectoryResponse> {
    const params = this.buildQueryParams(filters);

    return this.http.get<DirectoryResponse>(`${this.apiUrl}/search`, { params })
      .pipe(
        catchError(this.handleError)
      );
  }

  getProfessionalById(id: string): Observable<Professional> {
    return this.http.get<Professional>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private buildQueryParams(filters: DirectoryFilters): HttpParams {
    let params = new HttpParams();

    if (filters.searchQuery) params = params.set('q', filters.searchQuery);
    if (filters.location) params = params.set('location', filters.location);
    if (filters.businessTypes.length) params = params.set('types', filters.businessTypes.join(','));
    if (filters.specialties.length) params = params.set('specialties', filters.specialties.join(','));
    if (filters.verifiedOnly) params = params.set('verified', 'true');
    if (filters.featuredOnly) params = params.set('featured', 'true');
    if (filters.openNow) params = params.set('openNow', 'true');
    if (filters.sortBy) params = params.set('sort', filters.sortBy);
    params = params.set('page', filters.page.toString());
    params = params.set('pageSize', filters.pageSize.toString());

    return params;
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('Professional Service Error:', error);
    return throwError(() => new Error('Failed to fetch professionals'));
  }
}
```

---

## Responsive Behavior

### Mobile (< 768px)
- Filter sidebar collapses to a modal/drawer
- Grid switches to single column
- Search inputs stack vertically
- Simplified sorting controls
- Bottom navigation for filters

### Tablet (768px - 991px)
- Filter sidebar remains visible
- 2-column grid
- Full search controls
- Compact view toggle

### Desktop (≥ 992px)
- Full sidebar + 3-column grid
- All controls visible
- Optimal card size and spacing

---

## Performance Optimizations

1. **Lazy Loading**: Use Angular's lazy loading for route module
2. **Virtual Scrolling**: Implement `cdk-virtual-scroll` for long lists
3. **Image Optimization**: Use responsive images with srcset
4. **Pagination**: Server-side pagination to limit data transfer
5. **Caching**: Cache filter metadata in service layer
6. **TrackBy**: Use trackBy functions in *ngFor loops
7. **OnPush Strategy**: Use ChangeDetectionStrategy.OnPush where possible

---

## Testing Requirements

### Unit Tests
- Filter logic
- Sort functionality
- Pagination calculations
- Empty state conditions
- Card component rendering

### Integration Tests
- Filter + search interaction
- Pagination navigation
- URL query parameter sync
- Service API calls

### E2E Tests
- Complete search flow
- Filter application
- Navigation to profile
- Mobile responsive behavior

---

## Accessibility Requirements

1. **Keyboard Navigation**: All interactive elements keyboard accessible
2. **ARIA Labels**: Proper labeling for screen readers
3. **Focus Management**: Clear focus indicators
4. **Semantic HTML**: Use proper heading hierarchy (h1 → h2 → h3)
5. **Alt Text**: All images have descriptive alt text
6. **Color Contrast**: Meet WCAG 2.1 AA standards (4.5:1 for normal text)
7. **Screen Reader Announcements**: Announce filter changes and results count

---

## SEO Considerations

1. **Meta Tags**: Dynamic title, description, keywords based on filters
2. **Canonical URL**: Set proper canonical URL
3. **Schema Markup**: LocalBusiness and ItemList structured data
4. **Open Graph**: OG tags for social sharing
5. **Sitemap**: Include in sitemap.xml
6. **Clean URLs**: Use descriptive, SEO-friendly route paths
7. **Server-Side Rendering**: Consider Angular Universal for SSR

---

## API Endpoints

### GET /api/professionals/search
**Query Parameters**:
- `q` - Search query
- `location` - Location/zip code
- `types` - Comma-separated business types
- `specialties` - Comma-separated specialties
- `verified` - Boolean
- `featured` - Boolean
- `openNow` - Boolean
- `sort` - Sort option
- `page` - Page number
- `pageSize` - Items per page

**Response**:
```json
{
  "professionals": [...],
  "total": 42,
  "page": 1,
  "pageSize": 12,
  "totalPages": 4,
  "filters": {
    "availableBusinessTypes": [
      { "type": "Mechanic/Repair Shop", "count": 42 }
    ],
    "availableSpecialties": [
      { "specialty": "Engine Repair", "count": 38 }
    ]
  }
}
```

---

## State Management

Consider using NgRx or Akita for complex state management if needed:

```typescript
// Store structure
interface DirectoryState {
  professionals: Professional[];
  filters: DirectoryFilters;
  availableFilters: DirectoryResponse['filters'];
  loading: boolean;
  error: string | null;
  viewMode: 'grid' | 'list';
  total: number;
  totalPages: number;
}
```

---

## Future Enhancements (Phase 2+)

1. Map view integration
2. Save search/filters
3. Compare professionals
4. Quick inquiry modal
5. Favorite professionals
6. Distance-based sorting (with geolocation)
7. Advanced filters (price range, availability)
8. Infinite scroll option
