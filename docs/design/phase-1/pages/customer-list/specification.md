# Customer List Page Specification

## Overview

**Requirement ID:** REQ-CM-F006
**Phase:** 1
**Priority:** P0

The Customer List page provides professionals with a comprehensive view of their customers, including filtering, searching, and quick actions.

---

## Page Purpose

- Display paginated list of customers
- Filter by ownership (my customers, referred, all)
- Filter by customer status
- Search by name, email, or phone
- Quick access to customer details and editing
- Add new customers

---

## Component Architecture

```
CustomerListPage/
├── customer-list.ts             # Component class
├── customer-list.html           # Template
├── customer-list.scss           # Styles
├── components/
│   ├── customer-table/          # Desktop table view
│   ├── customer-card/           # Mobile card view
│   ├── customer-filters/        # Filter controls
│   └── customer-search/         # Search component
└── index.ts                     # Barrel export
```

---

## Layout Structure

### Desktop Layout (≥ 992px)
- Fixed sidebar navigation (280px)
- Main content area with top bar
- Table view for customer list
- Pagination at bottom

### Tablet Layout (768px - 991px)
- Collapsible sidebar (hidden by default)
- Simplified table (fewer columns)
- Floating action button for adding customers

### Mobile Layout (< 768px)
- Bottom navigation or hamburger menu
- Card-based customer list
- Touch-optimized interactions

---

## Component Structure

### Angular Component

```typescript
@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.html',
  styleUrls: ['./customer-list.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CustomerTableComponent,
    CustomerCardComponent,
    CustomerFiltersComponent,
    PaginationComponent
  ]
})
export class CustomerList {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly customerService = inject(CustomerService);

  // Query parameters
  private queryParams$ = this.route.queryParams;

  // Filters
  ownershipFilter$ = new BehaviorSubject<'all' | 'mine' | 'referred'>('all');
  statusFilter$ = new BehaviorSubject<string>('all');
  searchQuery$ = new BehaviorSubject<string>('');
  sortBy$ = new BehaviorSubject<string>('name');
  sortDirection$ = new BehaviorSubject<'asc' | 'desc'>('asc');
  currentPage$ = new BehaviorSubject<number>(1);
  pageSize = 10;

  // Combined filters for API call
  private filters$ = combineLatest([
    this.ownershipFilter$,
    this.statusFilter$,
    this.searchQuery$.pipe(debounceTime(300)),
    this.sortBy$,
    this.sortDirection$,
    this.currentPage$
  ]).pipe(
    map(([ownership, status, search, sortBy, sortDir, page]) => ({
      ownership,
      status,
      search,
      sortBy,
      sortDirection: sortDir,
      page,
      pageSize: this.pageSize
    }))
  );

  // Customer data
  customersResponse$ = this.filters$.pipe(
    switchMap(filters => this.customerService.getCustomers(filters)),
    shareReplay(1)
  );

  customers$ = this.customersResponse$.pipe(map(r => r.items));
  totalCount$ = this.customersResponse$.pipe(map(r => r.totalCount));
  totalPages$ = this.customersResponse$.pipe(
    map(r => Math.ceil(r.totalCount / this.pageSize))
  );

  isLoading$ = new BehaviorSubject<boolean>(false);

  onOwnershipChange(value: 'all' | 'mine' | 'referred'): void {
    this.ownershipFilter$.next(value);
    this.currentPage$.next(1);
  }

  onStatusChange(value: string): void {
    this.statusFilter$.next(value);
    this.currentPage$.next(1);
  }

  onSearch(query: string): void {
    this.searchQuery$.next(query);
    this.currentPage$.next(1);
  }

  onSort(column: string): void {
    if (this.sortBy$.value === column) {
      this.sortDirection$.next(
        this.sortDirection$.value === 'asc' ? 'desc' : 'asc'
      );
    } else {
      this.sortBy$.next(column);
      this.sortDirection$.next('asc');
    }
  }

  onPageChange(page: number): void {
    this.currentPage$.next(page);
  }

  onViewCustomer(customerId: string): void {
    this.router.navigate(['/customers', customerId]);
  }

  onEditCustomer(customerId: string): void {
    this.router.navigate(['/customers', customerId, 'edit']);
  }

  onAddCustomer(): void {
    this.router.navigate(['/customers', 'new']);
  }
}
```

---

## Filter Options

### Ownership Filter (Tabs)
| Option | Description |
|--------|-------------|
| All | All customers the professional can view |
| My Customers | Customers where professional is primary owner |
| Referred | Customers received via referral |

### Status Filter (Dropdown)
| Option | Description |
|--------|-------------|
| All Statuses | No status filter |
| Active | Customers with recent activity |
| Pending | New customers awaiting first service |
| Inactive | No activity in 90+ days |

### Sort Options
| Option | Field |
|--------|-------|
| Name | lastName, firstName |
| Date Added | createdAt |
| Last Activity | lastActivityAt |

---

## Table Columns

| Column | Desktop | Tablet | Mobile |
|--------|---------|--------|--------|
| Customer (name + email) | ✓ | ✓ | Card |
| Phone | ✓ | ✓ | Card |
| Status | ✓ | ✗ | Card |
| Ownership | ✓ | ✗ | Card |
| Added | ✓ | ✗ | Card |
| Actions | ✓ | ✓ | Card |

---

## Customer Status Badges

| Status | Color | Background |
|--------|-------|------------|
| Active | Success green | Success light |
| Referred | Info blue | Info light |
| Pending | Warning orange | Warning light |
| Inactive | Gray | Gray light |

---

## Actions Menu

### Row Actions (Desktop)
- View customer details
- Edit customer
- More options dropdown:
  - Send referral
  - Add note
  - View activity
  - Delete (with confirmation)

### Card Actions (Mobile)
- View button
- Edit button
- Swipe actions (optional)

---

## Pagination

### Display
- "Showing X-Y of Z customers"
- Page number buttons (1, 2, 3, ..., N)
- Previous/Next buttons
- Disabled state when at boundaries

### Behavior
- 10 customers per page (configurable)
- Preserve filters when paginating
- Scroll to top on page change
- Update URL query parameters

---

## Empty States

### No Customers
```html
<div class="empty-state">
  <icon>users</icon>
  <h3>No customers yet</h3>
  <p>Start building your customer base by adding your first customer.</p>
  <button>Add Customer</button>
</div>
```

### No Search Results
```html
<div class="empty-state">
  <icon>search</icon>
  <h3>No customers found</h3>
  <p>Try adjusting your search or filters.</p>
  <button>Clear filters</button>
</div>
```

---

## Loading States

### Initial Load
- Show skeleton loaders for table rows
- 5 skeleton rows matching table structure

### Filter/Search Loading
- Show loading indicator in filter area
- Keep existing data visible
- Subtle opacity change

### Pagination Loading
- Show spinner in pagination area
- Disable pagination buttons

---

## Accessibility Requirements

| Requirement | Implementation |
|-------------|----------------|
| Table semantics | Proper table markup with headers |
| Sort indication | aria-sort on sortable columns |
| Action labels | aria-label on icon buttons |
| Keyboard navigation | Tab through rows and actions |
| Focus management | Focus first row on page change |
| Screen reader | Announce filter changes |

---

## Analytics Events

| Event Name | Trigger | Data |
|------------|---------|------|
| `customer_list_view` | Page load | `totalCustomers` |
| `customer_filter_change` | Filter applied | `filterType`, `filterValue` |
| `customer_search` | Search performed | `queryLength`, `resultsCount` |
| `customer_sort` | Sort changed | `column`, `direction` |
| `customer_view_click` | View button clicked | `customerId` |
| `customer_edit_click` | Edit button clicked | `customerId` |
| `customer_add_click` | Add button clicked | - |

---

## API Integration

### Get Customers Endpoint
```
GET /api/customers

Query Parameters:
- ownership: 'all' | 'mine' | 'referred'
- status: string
- search: string
- sortBy: string
- sortDirection: 'asc' | 'desc'
- page: number
- pageSize: number

Response:
{
  "items": [
    {
      "customerId": "uuid",
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "phone": "string",
      "status": "active" | "pending" | "inactive",
      "ownershipType": "primary" | "referred" | "shared",
      "ownerProfessionalName": "string",
      "referredByProfessionalName": "string",
      "createdAt": "datetime",
      "lastActivityAt": "datetime"
    }
  ],
  "totalCount": number,
  "page": number,
  "pageSize": number
}
```

---

## URL Query Parameters

Update URL to preserve state:
```
/customers?ownership=mine&status=active&search=john&sort=name&dir=asc&page=2
```

This enables:
- Browser back/forward navigation
- Bookmarkable filtered views
- Shareable links

---

## Dependencies

| Dependency | Purpose |
|------------|---------|
| `@angular/common` | CommonModule |
| `@angular/router` | RouterModule |
| `CustomerService` | API calls |
| `RxJS` | Reactive filtering |

---

## Testing Checklist

### Unit Tests
- [ ] Filters update correctly
- [ ] Search debounce works
- [ ] Pagination calculates correctly
- [ ] Sort toggles direction
- [ ] Empty states display correctly

### Integration Tests
- [ ] API calls with correct parameters
- [ ] Navigation to customer detail works
- [ ] Add customer flow works
- [ ] URL parameters sync with state

### Visual Tests
- [ ] Desktop table layout
- [ ] Tablet responsive layout
- [ ] Mobile card layout
- [ ] Loading skeletons
- [ ] Empty states

### Accessibility Tests
- [ ] Table is navigable by keyboard
- [ ] Screen reader announces changes
- [ ] Focus management works

---

## File Checklist

- [ ] `customer-list.ts` - Component class
- [ ] `customer-list.html` - Template
- [ ] `customer-list.scss` - Styles
- [ ] `customer-list.spec.ts` - Unit tests
- [ ] `components/customer-table/` - Table component
- [ ] `components/customer-card/` - Mobile card
- [ ] `components/customer-filters/` - Filter controls
- [ ] `index.ts` - Barrel export
