# Customer List View Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-CM-F006` |
| **Component Name** | CustomerListView |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive list view for professionals to manage their customers. Includes filtering by ownership type, search functionality, sorting, and quick actions for common customer operations.

## User Stories

1. **As a professional**, I want to see all my customers in one place, so I can manage my client relationships.
2. **As a professional**, I want to filter customers by ownership type, so I can focus on specific groups.
3. **As a professional**, I want to search for customers quickly, so I can find specific clients.
4. **As a professional**, I want quick access to customer actions, so I can work efficiently.

## Component Structure

```
CustomerListView/
├── index.ts                       # Public exports
├── CustomerListView.tsx           # Main component (React)
├── CustomerListView.component.ts  # Main component (Angular)
├── CustomerListView.module.scss   # Styles
├── CustomerListView.test.tsx      # Unit tests
├── hooks/
│   ├── useCustomerFilters.ts      # Filter state management
│   ├── useCustomerSearch.ts       # Search functionality
│   └── useCustomerSort.ts         # Sort state management
├── components/
│   ├── StatsCards/                # Summary statistics
│   ├── FilterBar/                 # Search and filters
│   ├── CustomerTable/             # Data table
│   ├── CustomerRow/               # Individual row
│   ├── Pagination/                # Page controls
│   └── EmptyState/                # No results state
└── types/
    └── customer.types.ts          # Type definitions
```

## Props / Inputs

### React

```typescript
interface CustomerListViewProps {
  /** Initial filter state */
  initialFilters?: CustomerFilters;

  /** Initial sort configuration */
  initialSort?: CustomerSort;

  /** Callback when customer is selected */
  onCustomerSelect?: (customerId: string) => void;

  /** Callback when navigating to customer detail */
  onViewCustomer?: (customerId: string) => void;

  /** Callback when editing customer */
  onEditCustomer?: (customerId: string) => void;

  /** Callback when referring customer */
  onReferCustomer?: (customerId: string) => void;

  /** Enable/disable stats display */
  showStats?: boolean;

  /** Custom page size options */
  pageSizeOptions?: number[];
}

interface CustomerFilters {
  search: string;
  ownership: 'all' | 'my-customers' | 'referred' | 'shared';
  status: 'all' | 'active' | 'inactive';
}

interface CustomerSort {
  field: 'name' | 'email' | 'ownership' | 'status' | 'lastActivity';
  direction: 'asc' | 'desc';
}
```

### Angular

```typescript
@Component({
  selector: 'app-customer-list-view',
  templateUrl: './customer-list-view.component.html',
  styleUrls: ['./customer-list-view.component.scss']
})
export class CustomerListViewComponent {
  @Input() initialFilters?: CustomerFilters;
  @Input() initialSort?: CustomerSort;
  @Input() showStats = true;
  @Input() pageSizeOptions = [10, 25, 50, 100];

  @Output() customerSelect = new EventEmitter<string>();
  @Output() viewCustomer = new EventEmitter<string>();
  @Output() editCustomer = new EventEmitter<string>();
  @Output() referCustomer = new EventEmitter<string>();
}
```

## Data Models

### Customer

```typescript
interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  avatar?: string;
  ownership: 'owned' | 'referred' | 'shared';
  status: 'active' | 'inactive';
  lastActivityAt: Date;
  createdAt: Date;
  referredBy?: {
    id: string;
    name: string;
  };
}
```

### Customer Stats

```typescript
interface CustomerStats {
  total: number;
  totalChange: number;
  owned: number;
  ownedChange: number;
  referred: number;
  referredChange: number;
  activeThisMonth: number;
  activePercentChange: number;
}
```

## State Management

```typescript
interface CustomerListState {
  // Data
  customers: Customer[];
  stats: CustomerStats;

  // Loading states
  isLoading: boolean;
  isLoadingMore: boolean;

  // Filters
  filters: CustomerFilters;

  // Sort
  sort: CustomerSort;

  // Pagination
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;

  // Selection
  selectedCustomerIds: string[];

  // Error
  error: string | null;
}
```

## Visual Specifications

### Layout

- **Sidebar**: 260px fixed width
- **Content Area**: Fluid, max-width 1200px
- **Stats Grid**: 4 columns (responsive)
- **Table**: Full width with horizontal scroll on mobile

### Stats Cards

| Stat | Label | Color |
|------|-------|-------|
| Total Customers | Gray | --color-gray-600 |
| My Customers | Green | --color-success |
| Referred to Me | Blue | --color-info |
| Active This Month | Green | --color-success |

### Table Columns

| Column | Width | Sortable | Content |
|--------|-------|----------|---------|
| Customer | 25% | Yes | Avatar, name, email |
| Phone | 15% | No | Phone number |
| Ownership | 15% | Yes | Badge |
| Status | 10% | Yes | Badge |
| Last Activity | 15% | Yes | Relative date |
| Actions | 10% | No | Action buttons |

### Ownership Badges

| Type | Background | Text Color |
|------|------------|------------|
| My Customer | --color-success-light | --color-success |
| Referred | --color-info-light | --color-info |
| Shared | --color-warning-light | --color-warning |

### Status Badges

| Status | Background | Text Color |
|--------|------------|------------|
| Active | --color-success-light | --color-success |
| Inactive | --color-gray-100 | --color-gray-600 |

## Filter Behavior

### Search

- **Debounce**: 300ms
- **Fields searched**: name, email, phone
- **Minimum characters**: 2
- **Clear button**: Shows when search has value

### Ownership Tabs

```typescript
const ownershipTabs = [
  { value: 'all', label: 'All', count: 248 },
  { value: 'my-customers', label: 'My Customers', count: 156 },
  { value: 'referred', label: 'Referred', count: 67 },
  { value: 'shared', label: 'Shared', count: 25 },
];
```

### Status Dropdown

```typescript
const statusOptions = [
  { value: 'all', label: 'All Statuses' },
  { value: 'active', label: 'Active' },
  { value: 'inactive', label: 'Inactive' },
];
```

## Pagination

### Controls

- Previous/Next arrows
- Page numbers (show first, last, current, adjacent)
- Ellipsis for skipped pages
- Page size selector

### URL State

```typescript
// Sync with URL parameters
const urlParams = {
  page: 1,
  pageSize: 10,
  search: '',
  ownership: 'all',
  status: 'all',
  sortBy: 'name',
  sortDir: 'asc',
};
```

## Actions Menu

### Quick Actions

| Action | Icon | Description |
|--------|------|-------------|
| View | Eye | Open customer detail |
| Edit | Pencil | Open edit form |
| Refer | Share | Open referral modal |

### Bulk Actions (Phase 2)

- Export selected
- Send message
- Change status

## Empty States

### No Customers

```typescript
{
  icon: 'users',
  title: 'No customers yet',
  description: 'Start by adding your first customer or waiting for referrals.',
  action: {
    label: 'Add Customer',
    onClick: () => navigate('/customers/new')
  }
}
```

### No Search Results

```typescript
{
  icon: 'search',
  title: 'No results found',
  description: 'Try adjusting your search or filters.',
  action: {
    label: 'Clear Filters',
    onClick: () => clearFilters()
  }
}
```

## API Integration

### List Customers Endpoint

```typescript
// GET /api/v1/customers
interface ListCustomersRequest {
  page?: number;
  pageSize?: number;
  search?: string;
  ownership?: string;
  status?: string;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
}

interface ListCustomersResponse {
  data: Customer[];
  meta: {
    page: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
  };
  stats: CustomerStats;
}
```

### Virtual Scrolling (Large Lists)

For lists > 100 items, implement virtual scrolling:

```typescript
import { FixedSizeList } from 'react-window';

<FixedSizeList
  height={600}
  itemCount={customers.length}
  itemSize={72}
  width="100%"
>
  {CustomerRow}
</FixedSizeList>
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Table has proper headers and scope
- [ ] Sortable columns have aria-sort
- [ ] Action buttons have accessible labels
- [ ] Pagination has aria-label
- [ ] Search has associated label
- [ ] Loading states announced

### Keyboard Navigation

- Tab through filters, table rows, and actions
- Enter/Space to activate buttons
- Arrow keys to navigate table (optional)
- Escape to clear search/filters

### Screen Reader Support

```html
<table role="grid" aria-label="Customer list">
  <thead>
    <tr>
      <th scope="col" aria-sort="ascending">
        Customer
        <span class="sr-only">, sorted ascending</span>
      </th>
    </tr>
  </thead>
  <tbody>
    <tr aria-rowindex="1">
      <td>...</td>
    </tr>
  </tbody>
</table>
```

## Performance Optimization

### Data Loading

- Initial page load: Show skeleton
- Subsequent pages: Show loading indicator
- Prefetch next page on hover

### Caching

- Cache customer list by filter combination
- Cache customer stats (5 minute TTL)
- Invalidate on customer update

### Debouncing

- Search input: 300ms debounce
- Filter changes: Immediate
- Sort changes: Immediate

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerListView', () => {
  it('renders customer table', () => {});
  it('displays stats cards', () => {});
  it('filters by search term', () => {});
  it('filters by ownership type', () => {});
  it('filters by status', () => {});
  it('sorts by column', () => {});
  it('paginates results', () => {});
  it('handles empty state', () => {});
  it('triggers action callbacks', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Customer List', () => {
  it('loads and displays customers', () => {});
  it('searches customers', () => {});
  it('filters and sorts customers', () => {});
  it('navigates to customer detail', () => {});
  it('opens refer modal', () => {});
  it('exports customer list', () => {});
});
```

## Dependencies

- Table/DataGrid component
- Pagination component
- Search input component
- Dropdown/Select component
- Badge component
- Avatar component

## Related Components

- `CustomerDetailView` - Customer detail page
- `CustomerForm` - Create/Edit customer
- `ReferralModal` - Send referral
- `Sidebar` - Dashboard navigation
- `Header` - Page header

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
