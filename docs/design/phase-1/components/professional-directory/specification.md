# Professional Directory Page - Component Specification

## Component Overview

**Component ID:** REQ-SP-F001
**Component Name:** Professional Directory Page
**Phase:** Phase 1
**Priority:** P0 (Critical Path)
**Status:** Design Complete
**Last Updated:** 2026-01-08

## Description

The Professional Directory Page is a searchable, filterable directory that allows customers to discover and browse automotive professionals and businesses within the platform's ecosystem. It serves as the primary discovery interface for customers seeking automotive services and professionals.

## User Stories

- **As a customer**, I want to search for automotive professionals by business type, specialty, or keyword so that I can find the right service provider for my needs.
- **As a customer**, I want to filter professionals by business type and specialties so that I can narrow down my search results.
- **As a customer**, I want to see professional ratings and verification status so that I can make informed decisions.
- **As a customer**, I want to toggle between grid and list views so that I can browse professionals in my preferred format.

## Component Props/Inputs

### React Props

```typescript
interface ProfessionalDirectoryProps {
  // Initial data
  professionals: Professional[];
  totalCount: number;

  // Filter options
  businessTypes: FilterOption[];
  specialties: FilterOption[];

  // Pagination
  currentPage: number;
  itemsPerPage: number;

  // Callbacks
  onSearch: (query: string) => void;
  onFilterChange: (filters: DirectoryFilters) => void;
  onPageChange: (page: number) => void;
  onViewChange: (view: 'grid' | 'list') => void;
  onSortChange: (sortBy: SortOption) => void;
  onViewProfile: (professionalId: string) => void;

  // Optional
  defaultView?: 'grid' | 'list';
  defaultSort?: SortOption;
  isLoading?: boolean;
  error?: string | null;
}
```

### Angular Inputs/Outputs

```typescript
@Component({
  selector: 'app-professional-directory',
  templateUrl: './professional-directory.component.html',
  styleUrls: ['./professional-directory.component.scss']
})
export class ProfessionalDirectoryComponent {
  // Inputs
  @Input() professionals: Professional[] = [];
  @Input() totalCount: number = 0;
  @Input() businessTypes: FilterOption[] = [];
  @Input() specialties: FilterOption[] = [];
  @Input() currentPage: number = 1;
  @Input() itemsPerPage: number = 12;
  @Input() defaultView: 'grid' | 'list' = 'grid';
  @Input() defaultSort: SortOption = 'relevance';
  @Input() isLoading: boolean = false;
  @Input() error: string | null = null;

  // Outputs
  @Output() search = new EventEmitter<string>();
  @Output() filterChange = new EventEmitter<DirectoryFilters>();
  @Output() pageChange = new EventEmitter<number>();
  @Output() viewChange = new EventEmitter<'grid' | 'list'>();
  @Output() sortChange = new EventEmitter<SortOption>();
  @Output() viewProfile = new EventEmitter<string>();
}
```

## Data Models

### Professional

```typescript
interface Professional {
  id: string;
  businessName: string;
  professionalName: string;
  businessType: BusinessType;
  photoUrl?: string;
  initials?: string;
  specialties: Specialty[];
  rating: number;
  reviewCount: number;
  isVerified: boolean;
  location?: {
    city: string;
    state: string;
    distance?: number;
  };
  createdAt: Date;
  lastActive: Date;
}
```

### BusinessType

```typescript
type BusinessType =
  | 'auto_repair'
  | 'body_shop'
  | 'dealership'
  | 'detailing'
  | 'tire_service'
  | 'towing'
  | 'glass_repair'
  | 'oil_change'
  | 'other';

interface BusinessTypeInfo {
  id: BusinessType;
  label: string;
  icon?: string;
}
```

### Specialty

```typescript
interface Specialty {
  id: string;
  name: string;
  category?: string;
}

// Common specialties
const COMMON_SPECIALTIES = [
  'Brake Service',
  'Engine Repair',
  'Transmission',
  'Electrical Systems',
  'A/C & Heating',
  'Diagnostics',
  'Oil Changes',
  'Tire Installation',
  'Wheel Alignment',
  'Collision Repair',
  'Paint & Refinishing',
  'Interior Detailing',
  'Exterior Detailing',
  'Ceramic Coating',
  // ... more specialties
];
```

### FilterOption

```typescript
interface FilterOption {
  id: string;
  label: string;
  count: number;
  checked?: boolean;
}
```

### DirectoryFilters

```typescript
interface DirectoryFilters {
  businessTypes: string[];
  specialties: string[];
  verified?: boolean;
  minRating?: number;
  location?: {
    lat: number;
    lng: number;
    radius: number;
  };
}
```

### SortOption

```typescript
type SortOption =
  | 'relevance'
  | 'rating'
  | 'name'
  | 'distance'
  | 'recent';
```

## Filter and Search Behavior

### Search Functionality

1. **Search Scope**: Searches across:
   - Business names
   - Professional names
   - Business types
   - Specialties
   - Location (city, state)

2. **Search Behavior**:
   - Debounced input (300ms delay)
   - Minimum 2 characters to trigger search
   - Case-insensitive
   - Partial word matching
   - Clear button to reset search

3. **Search Results**:
   - Display count of matching results
   - Highlight matching terms (optional)
   - Return to page 1 when search changes

### Filter Behavior

1. **Business Type Filter**:
   - Multi-select checkboxes
   - OR logic (any selected type matches)
   - Display count of available professionals per type
   - Persist filter state in URL query params

2. **Specialty Filter**:
   - Multi-select checkboxes
   - OR logic (any selected specialty matches)
   - Display count of available professionals per specialty
   - Show top 10 specialties initially, "Show More" button for rest

3. **Filter Combination**:
   - AND logic between filter groups
   - Example: (BusinessType1 OR BusinessType2) AND (Specialty1 OR Specialty2)
   - Real-time filter application
   - Update counts dynamically

4. **Clear Filters**:
   - Single button to clear all active filters
   - Does not clear search query
   - Returns to page 1

### Sort Functionality

1. **Sort Options**:
   - **Relevance**: Default, based on search query match and professional activity
   - **Rating**: Highest to lowest rating, then by review count
   - **Name**: Alphabetical by business name
   - **Distance**: Nearest to farthest (requires location permission)
   - **Recent**: Most recently joined professionals first

2. **Sort Behavior**:
   - Persist in URL query params
   - Maintain current page when changing sort
   - Indicate active sort in dropdown

## View Toggle

### Grid View (Default)
- Cards displayed in responsive grid
- 3 columns on desktop (>1024px)
- 2 columns on tablet (768px - 1024px)
- 1 column on mobile (<768px)
- Card width: min 320px, max 400px
- Gap between cards: 1.5rem

### List View
- Cards displayed in single column
- Full width with horizontal layout
- Photo on left, info in middle, button on right
- Optimized for scanning many results

## Professional Card Components

### Card Layout

1. **Header Section**:
   - Professional photo (80px circle) or initials
   - Business name (truncate at 2 lines)
   - Verified badge (if applicable)
   - Professional name
   - Business type badge

2. **Specialties Section**:
   - Display top 3 specialties
   - Specialty tags with rounded corners
   - Truncate specialty names if needed

3. **Footer Section**:
   - Star rating (1-5 stars)
   - Numeric rating (1 decimal place)
   - Review count
   - "View Profile" button

### Card Interactions

- **Hover State**:
  - Elevate card with increased shadow
  - Slight upward transform (-4px)
  - Smooth transition (0.3s)

- **Click Behavior**:
  - Entire card is clickable
  - Navigate to professional profile page
  - Open in same tab (SPA navigation)

- **Button Focus**:
  - Visible focus ring for accessibility
  - Tab navigation support

## Pagination

### Pagination Behavior

1. **Items Per Page**: 12 professionals (configurable)

2. **Pagination Controls**:
   - Previous/Next buttons
   - Page number buttons
   - Current page highlighted
   - Ellipsis for large page ranges
   - Show pages: [Prev] [1] [2] [3] [4] [...] [Last] [Next]

3. **Pagination Logic**:
   - Disable Previous on page 1
   - Disable Next on last page
   - Show max 5 page numbers
   - Always show first and last page
   - Scroll to top on page change

4. **URL Integration**:
   - Page number in query param: `?page=2`
   - Deep linkable
   - Browser back/forward support

## Responsive Layout

### Desktop (>1024px)
- Sidebar: 280px fixed width, sticky positioning
- Content: Flex-grow, max-width 1120px
- Grid: 3 columns
- Header: Search bar centered, max 600px wide

### Tablet (768px - 1024px)
- Sidebar: Full width at top, collapsible
- Content: Full width below sidebar
- Grid: 2 columns
- Header: Stacked layout

### Mobile (<768px)
- Sidebar: Modal overlay or bottom sheet
- Content: Full width
- Grid: 1 column
- Header: Stacked, search full width
- Filter button in sticky header

## Style Guide Compliance

### Colors
- **Primary Blue**: `#00529F` - Header, buttons, badges
- **Accent Yellow**: `#FFD520` - Verified badges, stars, highlights
- **Text Dark**: `#333333` - Primary text
- **Text Light**: `#666666` - Secondary text
- **Border Gray**: `#E0E0E0` - Borders, dividers
- **Background Light**: `#F5F5F5` - Page background
- **White**: `#FFFFFF` - Cards, sidebar

### Typography
- **Headings**: Montserrat (400, 500, 600, 700)
  - Business Name: 18px / 1.125rem, weight 700
  - Section Headers: 14px / 0.875rem, weight 600, uppercase
  - Professional Name: 15px / 0.938rem, weight 400

- **Body Text**: Lato (300, 400, 700)
  - Card Text: 15px / 0.938rem
  - Specialty Tags: 13px / 0.813rem
  - Helper Text: 14px / 0.875rem

### Spacing
- Card Padding: 1.5rem (24px)
- Grid Gap: 1.5rem (24px)
- Section Margin: 2rem (32px)
- Element Gap: 0.5rem - 1rem (8px - 16px)

### Borders and Shadows
- **Border Radius**:
  - Cards: 12px
  - Buttons: 8px
  - Badges: 6px
  - Pills: 12px - 24px

- **Shadows**:
  - Default: `0 2px 8px rgba(0, 0, 0, 0.1)`
  - Hover: `0 4px 16px rgba(0, 0, 0, 0.15)`

## Accessibility Requirements

### WCAG 2.1 Level AA Compliance

1. **Semantic HTML**:
   - Use `<main>`, `<aside>`, `<article>`, `<nav>` landmarks
   - Proper heading hierarchy (h1 → h2 → h3)
   - Form labels for all inputs

2. **Keyboard Navigation**:
   - All interactive elements keyboard accessible
   - Logical tab order
   - Visible focus indicators
   - Skip to content link
   - Escape key to close modals/dropdowns

3. **Screen Reader Support**:
   - ARIA labels for icon buttons
   - ARIA live regions for dynamic content
   - ARIA expanded/collapsed states
   - Descriptive link text
   - Alt text for images (if used instead of initials)

4. **Color Contrast**:
   - Text on backgrounds: minimum 4.5:1
   - Large text (18px+): minimum 3:1
   - Interactive elements: minimum 3:1

5. **Focus Management**:
   - Maintain focus on filter/sort changes
   - Return focus after modal close
   - Announce page changes to screen readers

6. **Responsive Text**:
   - Support 200% zoom without horizontal scroll
   - Relative font sizes (rem/em)
   - No text in images

### ARIA Attributes

```html
<!-- Search -->
<input
  type="search"
  aria-label="Search professionals, businesses, or specialties"
  role="searchbox"
>

<!-- Filter Section -->
<aside
  aria-label="Filter professionals"
  role="complementary"
>
  <div role="group" aria-labelledby="business-type-heading">
    <h4 id="business-type-heading">Business Type</h4>
    <!-- checkboxes -->
  </div>
</aside>

<!-- Results Count -->
<div
  aria-live="polite"
  aria-atomic="true"
  role="status"
>
  Showing 24 professionals
</div>

<!-- Professional Cards -->
<article
  role="article"
  aria-labelledby="business-name-123"
>
  <h3 id="business-name-123">Smith's Auto Repair</h3>
  <!-- card content -->
</article>

<!-- Pagination -->
<nav aria-label="Pagination">
  <button aria-label="Go to previous page">Previous</button>
  <button aria-label="Go to page 1" aria-current="page">1</button>
  <button aria-label="Go to page 2">2</button>
  <!-- more pages -->
</nav>
```

## State Management

### Component State

```typescript
interface DirectoryState {
  // Data
  professionals: Professional[];
  totalCount: number;

  // UI State
  currentView: 'grid' | 'list';
  currentSort: SortOption;
  currentPage: number;

  // Filters
  activeFilters: DirectoryFilters;
  searchQuery: string;

  // Loading & Error
  isLoading: boolean;
  error: string | null;

  // Filter Options
  availableBusinessTypes: FilterOption[];
  availableSpecialties: FilterOption[];
}
```

### URL State Synchronization

```
/professionals?
  q=brake%20service
  &types=auto_repair,body_shop
  &specialties=brake_service,engine_repair
  &sort=rating
  &view=grid
  &page=2
```

## API Integration

### Endpoints

#### GET /api/professionals

**Query Parameters**:
```typescript
{
  q?: string;              // Search query
  types?: string[];        // Business types
  specialties?: string[];  // Specialty IDs
  verified?: boolean;      // Only verified
  minRating?: number;      // Minimum rating (1-5)
  lat?: number;            // Latitude
  lng?: number;            // Longitude
  radius?: number;         // Search radius (miles)
  sort?: SortOption;       // Sort order
  page?: number;           // Page number
  limit?: number;          // Items per page
}
```

**Response**:
```typescript
{
  data: Professional[];
  pagination: {
    total: number;
    page: number;
    limit: number;
    totalPages: number;
  };
  filters: {
    businessTypes: FilterOption[];
    specialties: FilterOption[];
  };
}
```

#### GET /api/professionals/filters

**Response**:
```typescript
{
  businessTypes: FilterOption[];
  specialties: FilterOption[];
}
```

## Performance Considerations

1. **Initial Load**:
   - Lazy load professional photos
   - Prefetch first page results
   - Cache filter options

2. **Filtering/Sorting**:
   - Debounce search input (300ms)
   - Client-side filtering for small datasets (<100)
   - Server-side for large datasets
   - Optimistic UI updates

3. **Pagination**:
   - Prefetch next page on hover
   - Virtual scrolling for large lists (future enhancement)
   - Infinite scroll option (future enhancement)

4. **Images**:
   - Use initials by default (no HTTP request)
   - Lazy load profile photos
   - Serve responsive images (srcset)
   - Use WebP with fallback

## Error Handling

### Error States

1. **No Results**:
   ```
   No professionals found matching your criteria.
   Try adjusting your filters or search terms.
   [Clear Filters]
   ```

2. **Network Error**:
   ```
   Unable to load professionals. Please check your connection.
   [Retry]
   ```

3. **Server Error**:
   ```
   Something went wrong. Please try again later.
   [Retry]
   ```

4. **Location Permission Denied**:
   ```
   Location access denied. Distance sorting unavailable.
   [Continue without location]
   ```

## Testing Requirements

### Unit Tests

1. **Component Rendering**:
   - Renders with default props
   - Renders loading state
   - Renders error state
   - Renders empty state

2. **Filter Logic**:
   - Filters by single business type
   - Filters by multiple business types
   - Filters by specialties
   - Combines multiple filters
   - Clears filters

3. **Search Logic**:
   - Searches by business name
   - Searches by professional name
   - Searches by specialty
   - Debounces search input
   - Clears search

4. **Pagination**:
   - Changes page
   - Disables previous on page 1
   - Disables next on last page
   - Calculates page range correctly

5. **View Toggle**:
   - Switches between grid and list
   - Maintains state on page change

### Integration Tests

1. **API Integration**:
   - Fetches professionals on mount
   - Fetches with filters applied
   - Handles pagination
   - Handles sort changes
   - Error handling

2. **User Flows**:
   - Search → Filter → View Profile
   - Filter → Sort → Paginate
   - Clear filters → View reset
   - URL param changes update UI

### Accessibility Tests

1. **Automated Tests**:
   - Run axe-core or similar
   - Check color contrast
   - Validate ARIA attributes
   - Check semantic HTML

2. **Manual Tests**:
   - Keyboard navigation
   - Screen reader testing (NVDA, JAWS, VoiceOver)
   - Zoom to 200%
   - High contrast mode

### Visual Regression Tests

1. **Snapshot Tests**:
   - Desktop grid view
   - Desktop list view
   - Tablet responsive
   - Mobile responsive
   - Hover states
   - Focus states

### Performance Tests

1. **Load Time**:
   - Initial page load < 2s
   - Filter application < 200ms
   - Page change < 500ms

2. **Bundle Size**:
   - Component + dependencies < 50KB gzipped

## Future Enhancements

1. **Phase 2**:
   - Map view toggle
   - Save favorite professionals
   - Compare professionals side-by-side
   - Advanced filters (price range, availability)

2. **Phase 3**:
   - AI-powered recommendations
   - Instant booking integration
   - Live chat with professionals
   - Virtual tours of facilities

## Dependencies

### Required Components
- Button component
- Input/Search component
- Checkbox component
- Select/Dropdown component
- Card component
- Badge component
- Rating component
- Pagination component
- Loading spinner
- Error boundary

### External Libraries
- React Router (routing, URL params)
- React Query or SWR (data fetching, caching)
- Axios or Fetch (HTTP requests)
- Lodash (debounce, utility functions)
- date-fns (date formatting)

## Implementation Checklist

- [ ] Create component structure (React/Angular)
- [ ] Implement search functionality
- [ ] Implement filter sidebar
- [ ] Implement professional card
- [ ] Implement grid/list view toggle
- [ ] Implement pagination
- [ ] Implement sort functionality
- [ ] Add responsive layouts
- [ ] Add loading states
- [ ] Add error handling
- [ ] Implement URL state sync
- [ ] Add accessibility attributes
- [ ] Write unit tests
- [ ] Write integration tests
- [ ] Perform accessibility audit
- [ ] Optimize performance
- [ ] Document component usage

## References

- [CarMax Style Guide](../../style-guide.md)
- [Platform Requirements](../../requirements.md)
- [API Documentation](../../../api/README.md)
- [Component Library](../../../components/README.md)

---

**Document Version:** 1.0
**Last Updated:** 2026-01-08
**Author:** System Architect
**Reviewers:** TBD
