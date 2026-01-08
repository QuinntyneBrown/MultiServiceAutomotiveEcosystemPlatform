# Customer Detail Page Specification

## Overview

**Requirement ID:** REQ-CM-F007
**Phase:** 1
**Priority:** P0

The Customer Detail page provides professionals with a comprehensive view of a single customer, including their contact information, ownership details, referral relationships, and activity history.

---

## Page Purpose

- Display complete customer profile information
- Show customer contact details with quick actions (call, email, message)
- Display ownership and referral relationships
- Show chronological activity timeline
- Provide quick access to common actions (edit, refer, message)
- Placeholder for vehicle information (Phase 2)

---

## Component Architecture

```
CustomerDetailPage/
├── customer-detail.ts             # Component class
├── customer-detail.html           # Template
├── customer-detail.scss           # Styles
├── components/
│   ├── contact-info-card/         # Contact information display
│   ├── ownership-card/            # Ownership details
│   ├── referral-chain/            # Referral visualization
│   ├── activity-timeline/         # Activity history
│   └── quick-actions/             # Quick action buttons
└── index.ts                       # Barrel export
```

---

## Layout Structure

### Desktop Layout (≥ 992px)
- Fixed sidebar navigation (280px)
- Main content area with top bar
- Two-column grid (8 cols main / 4 cols sidebar)
- Customer header spans full width
- Activity timeline in main column
- Quick actions and ownership in sidebar

### Tablet Layout (768px - 991px)
- Collapsible sidebar (hidden by default)
- Single column layout
- Stacked cards
- All information accessible by scrolling

### Mobile Layout (< 768px)
- Bottom navigation or hamburger menu
- Single column layout
- Simplified header with avatar
- Touch-optimized action buttons
- Collapsible sections

---

## Component Structure

### Angular Component

```typescript
@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.html',
  styleUrls: ['./customer-detail.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ContactInfoCardComponent,
    OwnershipCardComponent,
    ReferralChainComponent,
    ActivityTimelineComponent,
    QuickActionsComponent
  ]
})
export class CustomerDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly customerService = inject(CustomerService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialogService = inject(DialogService);

  // Customer ID from route
  customerId$ = this.route.params.pipe(
    map(params => params['id'])
  );

  // Customer data
  customer$ = this.customerId$.pipe(
    switchMap(id => this.customerService.getCustomer(id)),
    shareReplay(1)
  );

  // Activity timeline
  activities$ = this.customerId$.pipe(
    switchMap(id => this.customerService.getCustomerActivities(id)),
    shareReplay(1)
  );

  // Ownership information
  ownership$ = this.customer$.pipe(
    map(customer => ({
      currentOwner: customer.currentOwner,
      ownedSince: customer.ownedSince,
      ownershipType: customer.ownershipType
    }))
  );

  // Referral chain
  referralChain$ = this.customer$.pipe(
    map(customer => customer.referralChain || [])
  );

  isLoading$ = new BehaviorSubject<boolean>(false);
  error$ = new BehaviorSubject<string | null>(null);

  ngOnInit(): void {
    // Track page view
    this.customer$.subscribe(customer => {
      this.trackPageView(customer.customerId);
    });
  }

  onEditCustomer(customerId: string): void {
    this.router.navigate(['/customers', customerId, 'edit']);
  }

  onReferCustomer(customerId: string): void {
    this.router.navigate(['/referrals', 'send'], {
      queryParams: { customerId }
    });
  }

  onSendEmail(email: string): void {
    window.location.href = `mailto:${email}`;
    this.trackAction('email_clicked', { email });
  }

  onCallPhone(phone: string): void {
    window.location.href = `tel:${phone}`;
    this.trackAction('phone_clicked', { phone });
  }

  onSendMessage(customerId: string): void {
    this.dialogService.open(MessageDialogComponent, {
      data: { customerId }
    });
  }

  onAddNote(customerId: string): void {
    this.dialogService.open(AddNoteDialogComponent, {
      data: { customerId }
    }).afterClosed().subscribe(result => {
      if (result) {
        this.refreshActivities();
        this.notificationService.success('Note added successfully');
      }
    });
  }

  onDeleteCustomer(customerId: string): void {
    this.dialogService.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Customer',
        message: 'Are you sure you want to delete this customer? This action cannot be undone.',
        confirmText: 'Delete',
        confirmColor: 'error'
      }
    }).afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.customerService.deleteCustomer(customerId).subscribe({
          next: () => {
            this.notificationService.success('Customer deleted successfully');
            this.router.navigate(['/customers']);
          },
          error: (error) => {
            this.notificationService.error('Failed to delete customer');
            console.error('Delete error:', error);
          }
        });
      }
    });
  }

  private refreshActivities(): void {
    // Force refresh of activities
    this.customerService.invalidateCustomerCache();
  }

  private trackPageView(customerId: string): void {
    // Analytics tracking
    window.analytics?.track('customer_detail_view', {
      customerId
    });
  }

  private trackAction(action: string, data: any): void {
    window.analytics?.track(action, data);
  }
}
```

---

## UI Sections

### 1. Customer Header
- Large avatar with initials (80px x 80px on desktop, 64px on mobile)
- Customer full name (H2 heading)
- Customer ID badge
- Status badge (Active, Pending, Referred, Inactive)
- Primary action buttons (Edit, Refer)

**Data Required:**
- firstName
- lastName
- customerId
- status

### 2. Contact Information Card
- Email address (clickable mailto link)
- Phone number (clickable tel link)
- Full address (multi-line)
- Customer since date
- Edit action button

**Data Required:**
- email
- phone
- address (street, city, state, zip)
- createdAt

### 3. Vehicle Information Card (Phase 1 Placeholder)
- Placeholder message: "No vehicles added yet"
- Note: "Vehicle management available in Phase 2"
- "Add Vehicle" action (disabled with tooltip)

**Phase 2 Requirements:**
- List of customer vehicles
- Primary vehicle indicator
- Make, model, year, VIN
- Service history link

### 4. Ownership Card
- Ownership badge (My Customer / Referred)
- Current owner name
- Owned since date
- Transfer ownership button (if applicable)

**Data Required:**
- ownershipType
- currentOwner
- ownedSince

### 5. Referral Chain Card
- Visual representation of referral path
- Customer → Referrer → Current Owner
- Professional avatars and names
- Professional business type/specialty

**Data Required:**
- referralChain array:
  - professionalId
  - professionalName
  - businessType
  - position in chain

### 6. Activity Timeline
- Chronological list of activities (newest first)
- Activity type icons:
  - Inquiry: Question mark circle
  - Referral: Share/network icon
  - Note: Document icon
  - Status Change: Flag/checkmark icon
  - Service: Wrench icon (Phase 2)
- Activity title and description
- Timestamp (relative: "2 hours ago", "3 days ago")
- Expandable details for long content
- "Add Note" action button

**Activity Types:**
```typescript
enum ActivityType {
  CUSTOMER_REGISTERED = 'customer_registered',
  STATUS_CHANGED = 'status_changed',
  NOTE_ADDED = 'note_added',
  REFERRAL_RECEIVED = 'referral_received',
  REFERRAL_SENT = 'referral_sent',
  EMAIL_SENT = 'email_sent',
  INQUIRY_SUBMITTED = 'inquiry_submitted',
  SERVICE_COMPLETED = 'service_completed', // Phase 2
  APPOINTMENT_SCHEDULED = 'appointment_scheduled' // Phase 3
}
```

### 7. Quick Actions
- Send Email
- Send Message
- Call Phone
- Add Note
- Grid layout (2x2 on desktop, 2x2 on mobile)
- Icon + label format

---

## Data Models

### Customer Detail Model

```typescript
interface CustomerDetail {
  customerId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: {
    street: string;
    street2?: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
  };
  status: 'active' | 'pending' | 'referred' | 'inactive';
  ownershipType: 'primary' | 'referred' | 'shared';
  currentOwner: {
    professionalId: string;
    professionalName: string;
    isCurrentUser: boolean;
  };
  ownedSince: string; // ISO date
  createdAt: string; // ISO date
  updatedAt: string; // ISO date
  referralChain?: ReferralChainItem[];
  vehicles?: Vehicle[]; // Phase 2
  metadata?: {
    lastActivityAt?: string;
    totalInquiries?: number;
    totalReferrals?: number;
  };
}

interface ReferralChainItem {
  professionalId: string;
  professionalName: string;
  businessName: string;
  businessType: string;
  position: number; // 0 = customer, 1 = first referrer, etc.
  referredAt: string; // ISO date
}

interface CustomerActivity {
  activityId: string;
  customerId: string;
  activityType: ActivityType;
  title: string;
  description: string;
  performedBy?: {
    professionalId: string;
    professionalName: string;
  };
  metadata?: Record<string, any>;
  createdAt: string; // ISO date
}

interface Vehicle {
  vehicleId: string;
  make: string;
  model: string;
  year: number;
  vin?: string;
  licensePlate?: string;
  color?: string;
  isPrimary: boolean;
  createdAt: string;
}
```

---

## API Integration

### Get Customer Detail Endpoint

```
GET /api/customers/:customerId

Response:
{
  "customerId": "uuid",
  "firstName": "Michael",
  "lastName": "Johnson",
  "email": "michael.j@email.com",
  "phone": "+15551234567",
  "address": {
    "street": "1234 Main Street",
    "street2": "Apartment 5B",
    "city": "Springfield",
    "state": "IL",
    "zipCode": "62701",
    "country": "US"
  },
  "status": "active",
  "ownershipType": "referred",
  "currentOwner": {
    "professionalId": "uuid",
    "professionalName": "John Smith",
    "isCurrentUser": true
  },
  "ownedSince": "2026-01-22T00:00:00Z",
  "createdAt": "2026-01-15T00:00:00Z",
  "updatedAt": "2026-01-22T10:30:00Z",
  "referralChain": [
    {
      "professionalId": "uuid",
      "professionalName": "Mike's Auto Body",
      "businessName": "Mike's Auto Body Shop",
      "businessType": "Auto Body",
      "position": 1,
      "referredAt": "2026-01-15T00:00:00Z"
    },
    {
      "professionalId": "uuid",
      "professionalName": "John Smith",
      "businessName": "Smith Automotive",
      "businessType": "Mechanic",
      "position": 2,
      "referredAt": "2026-01-22T00:00:00Z"
    }
  ],
  "metadata": {
    "lastActivityAt": "2026-01-22T14:30:00Z",
    "totalInquiries": 3,
    "totalReferrals": 1
  }
}
```

### Get Customer Activities Endpoint

```
GET /api/customers/:customerId/activities

Query Parameters:
- limit: number (default: 50)
- offset: number (default: 0)
- type: ActivityType (optional filter)

Response:
{
  "items": [
    {
      "activityId": "uuid",
      "customerId": "uuid",
      "activityType": "status_changed",
      "title": "Status changed to Active",
      "description": "Customer profile activated after email verification.",
      "performedBy": null,
      "metadata": {
        "oldStatus": "pending",
        "newStatus": "active"
      },
      "createdAt": "2026-01-22T14:30:00Z"
    },
    {
      "activityId": "uuid",
      "customerId": "uuid",
      "activityType": "note_added",
      "title": "Note added by John Smith",
      "description": "Customer interested in brake service. Mentioned unusual noise when braking.",
      "performedBy": {
        "professionalId": "uuid",
        "professionalName": "John Smith"
      },
      "metadata": {},
      "createdAt": "2026-01-19T10:15:00Z"
    }
  ],
  "totalCount": 15,
  "limit": 50,
  "offset": 0
}
```

---

## State Management

### Customer Detail State

```typescript
interface CustomerDetailState {
  customer: CustomerDetail | null;
  activities: CustomerActivity[];
  isLoading: boolean;
  error: string | null;
  selectedActivityId: string | null;
}

// Actions
class LoadCustomer {
  static readonly type = '[CustomerDetail] Load Customer';
  constructor(public customerId: string) {}
}

class LoadCustomerSuccess {
  static readonly type = '[CustomerDetail] Load Customer Success';
  constructor(public customer: CustomerDetail) {}
}

class LoadCustomerError {
  static readonly type = '[CustomerDetail] Load Customer Error';
  constructor(public error: string) {}
}

class LoadActivities {
  static readonly type = '[CustomerDetail] Load Activities';
  constructor(public customerId: string) {}
}

class AddActivity {
  static readonly type = '[CustomerDetail] Add Activity';
  constructor(public activity: Partial<CustomerActivity>) {}
}
```

---

## Responsive Behavior

### Breakpoint Changes

| Element | Desktop (≥992px) | Tablet (768-991px) | Mobile (<768px) |
|---------|------------------|--------------------| ---------------|
| **Layout** | 2-column grid | 1-column | 1-column |
| **Avatar Size** | 80px | 64px | 64px |
| **Header Actions** | Horizontal | Horizontal | Stacked (full width) |
| **Info Cards** | Side by side | Stacked | Stacked |
| **Quick Actions** | 2x2 grid | 2x2 grid | 2x2 grid |
| **Timeline Icons** | 40px | 40px | 32px |
| **Sidebar** | Visible | Hidden | Hidden |

---

## Loading States

### Initial Page Load
```html
<div class="customer-detail-skeleton">
  <div class="skeleton skeleton--header"></div>
  <div class="skeleton skeleton--card"></div>
  <div class="skeleton skeleton--card"></div>
  <div class="skeleton skeleton--timeline"></div>
</div>
```

### Refreshing Activities
- Show subtle loading indicator in timeline header
- Keep existing activities visible
- Smoothly insert new activities at top

---

## Error States

### Customer Not Found (404)
```html
<div class="error-state">
  <svg class="error-state__icon">...</svg>
  <h2>Customer Not Found</h2>
  <p>The customer you're looking for doesn't exist or has been deleted.</p>
  <button (click)="navigateToList()">Back to Customers</button>
</div>
```

### Permission Denied (403)
```html
<div class="error-state">
  <svg class="error-state__icon">...</svg>
  <h2>Access Denied</h2>
  <p>You don't have permission to view this customer.</p>
  <button (click)="navigateToList()">Back to Customers</button>
</div>
```

### General Error
```html
<div class="error-state">
  <svg class="error-state__icon">...</svg>
  <h2>Something Went Wrong</h2>
  <p>We couldn't load the customer details. Please try again.</p>
  <button (click)="retry()">Retry</button>
</div>
```

---

## Accessibility Requirements

| Requirement | Implementation |
|-------------|----------------|
| Page title | Update document title with customer name |
| Heading hierarchy | H1 for customer name, H2 for section titles |
| Landmark regions | main, aside, nav with proper aria-labels |
| Keyboard navigation | All actions accessible via keyboard |
| Focus management | Focus header on load, manage modal focus |
| Screen reader | Announce activity additions via aria-live |
| Color contrast | All text meets WCAG AA standards |
| Link purpose | Clear link text, aria-label for icons |

---

## Analytics Events

| Event Name | Trigger | Data |
|------------|---------|------|
| `customer_detail_view` | Page load | `customerId` |
| `customer_edit_click` | Edit button clicked | `customerId` |
| `customer_refer_click` | Refer button clicked | `customerId` |
| `customer_email_click` | Email link clicked | `customerId`, `email` |
| `customer_phone_click` | Phone link clicked | `customerId`, `phone` |
| `customer_message_click` | Message button clicked | `customerId` |
| `customer_note_added` | Note saved | `customerId`, `noteLength` |
| `customer_delete_click` | Delete initiated | `customerId` |
| `customer_delete_confirm` | Delete confirmed | `customerId` |

---

## Dependencies

| Dependency | Purpose |
|------------|---------|
| `@angular/common` | CommonModule |
| `@angular/router` | RouterModule, routing |
| `CustomerService` | API calls |
| `NotificationService` | Toast messages |
| `DialogService` | Modals |
| `RxJS` | Reactive data flow |
| `date-fns` | Date formatting |

---

## Testing Checklist

### Unit Tests
- [ ] Loads customer data on init
- [ ] Handles customer not found error
- [ ] Formats phone numbers correctly
- [ ] Formats addresses correctly
- [ ] Builds referral chain correctly
- [ ] Sorts activities chronologically
- [ ] Edit navigation works
- [ ] Refer navigation works
- [ ] Delete confirmation works

### Integration Tests
- [ ] API call with correct customer ID
- [ ] Activities load separately
- [ ] Email link opens mailto
- [ ] Phone link opens tel
- [ ] Message dialog opens
- [ ] Note dialog opens and refreshes
- [ ] Delete API call on confirm

### Visual Tests
- [ ] Desktop layout (1200px)
- [ ] Tablet layout (768px)
- [ ] Mobile layout (375px)
- [ ] Loading skeletons
- [ ] Error states (404, 403, 500)
- [ ] Empty activity timeline
- [ ] Long customer names
- [ ] Long addresses
- [ ] Long referral chains

### Accessibility Tests
- [ ] Keyboard navigation complete
- [ ] Screen reader announces sections
- [ ] Focus management in modals
- [ ] All images have alt text
- [ ] Color contrast passes WCAG AA
- [ ] Heading hierarchy correct

---

## File Checklist

- [ ] `customer-detail.ts` - Component class
- [ ] `customer-detail.html` - Template
- [ ] `customer-detail.scss` - Styles
- [ ] `customer-detail.spec.ts` - Unit tests
- [ ] `components/contact-info-card/` - Contact card component
- [ ] `components/ownership-card/` - Ownership component
- [ ] `components/referral-chain/` - Referral visualization
- [ ] `components/activity-timeline/` - Timeline component
- [ ] `components/quick-actions/` - Quick actions component
- [ ] `index.ts` - Barrel export

---

## Phase 2 Enhancements

### Vehicle Management
- Display customer vehicles in dedicated card
- Show vehicle details (make, model, year, VIN)
- Link to service history for each vehicle
- Add/edit/remove vehicles
- Set primary vehicle

### Enhanced Activities
- Service history entries
- Appointment activities
- Payment/invoice activities
- Filter activities by type
- Export activity log

### Additional Features
- Customer tags/labels
- Custom fields
- Document attachments
- Communication history (emails, SMS)
- Appointment scheduling integration
