# Customer Detail View Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-CM-F007` |
| **Component Name** | CustomerDetailView |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive detail page displaying all customer information, including contact details, ownership information, vehicles, referral history, and activity timeline. Provides quick actions for common operations.

## User Stories

1. **As a professional**, I want to see complete customer information, so I can provide personalized service.
2. **As a professional**, I want to see referral history, so I can track customer relationships.
3. **As a professional**, I want quick access to contact options, so I can reach customers easily.
4. **As a professional**, I want to see activity history, so I can understand customer engagement.

## Component Structure

```
CustomerDetailView/
├── index.ts                         # Public exports
├── CustomerDetailView.tsx           # Main component (React)
├── CustomerDetailView.component.ts  # Main component (Angular)
├── CustomerDetailView.module.scss   # Styles
├── CustomerDetailView.test.tsx      # Unit tests
├── components/
│   ├── CustomerHeader/              # Header with avatar, badges, actions
│   ├── ContactInfoCard/             # Contact information section
│   ├── OwnershipCard/               # Ownership details section
│   ├── VehiclesCard/                # Vehicle list section
│   ├── ReferralHistoryCard/         # Referral history section
│   └── ActivityTimeline/            # Activity timeline section
└── hooks/
    └── useCustomerDetail.ts         # Data fetching hook
```

## Props / Inputs

### React

```typescript
interface CustomerDetailViewProps {
  /** Customer ID to display */
  customerId: string;

  /** Callback when edit is clicked */
  onEdit?: (customerId: string) => void;

  /** Callback when refer is clicked */
  onRefer?: (customerId: string) => void;

  /** Callback when message is clicked */
  onMessage?: (customerId: string) => void;

  /** Enable editing features */
  canEdit?: boolean;

  /** Enable referral features */
  canRefer?: boolean;
}
```

### Angular

```typescript
@Component({
  selector: 'app-customer-detail-view',
  templateUrl: './customer-detail-view.component.html',
  styleUrls: ['./customer-detail-view.component.scss']
})
export class CustomerDetailViewComponent {
  @Input() customerId!: string;
  @Input() canEdit = true;
  @Input() canRefer = true;

  @Output() edit = new EventEmitter<string>();
  @Output() refer = new EventEmitter<string>();
  @Output() message = new EventEmitter<string>();
}
```

## Data Models

### Customer Detail

```typescript
interface CustomerDetail {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: Address;
  preferredContact: 'email' | 'phone' | 'sms';
  avatar?: string;
  ownership: OwnershipInfo;
  status: 'active' | 'inactive';
  customerSince: Date;
  vehicles: Vehicle[];
  referralHistory: ReferralHistoryItem[];
  recentActivity: ActivityItem[];
  tags?: string[];
  notes?: string;
}

interface OwnershipInfo {
  owner: {
    id: string;
    name: string;
    avatar?: string;
  };
  type: 'primary' | 'referred' | 'shared';
  since: Date;
  source: string;
}

interface Vehicle {
  id: string;
  year: number;
  make: string;
  model: string;
  vin?: string;
  mileage?: number;
  isPrimary: boolean;
}

interface ReferralHistoryItem {
  id: string;
  type: 'sent' | 'received' | 'customer-referred';
  targetName: string;
  description: string;
  reward?: number;
  date: Date;
  status: 'pending' | 'completed' | 'expired';
}

interface ActivityItem {
  id: string;
  type: 'service' | 'inquiry' | 'referral' | 'message' | 'status-change';
  title: string;
  description: string;
  date: Date;
}
```

## Visual Specifications

### Layout

- **Grid System**: 3-column layout on desktop
- **Main Content**: 2 columns for primary cards
- **Side Content**: 1 column for secondary cards
- **Full Width**: Activity timeline spans all columns

### Card Sections

| Section | Grid Position | Content |
|---------|---------------|---------|
| Contact Info | Main (2 cols) | Email, phone, address, preferences |
| Ownership | Side (1 col) | Owner info, since date, source |
| Vehicles | Main (2 cols) | Vehicle list with primary badge |
| Referral History | Side (1 col) | Recent referrals sent/received |
| Activity Timeline | Full width | Chronological activity feed |

### Header Actions

| Action | Icon | Variant | Behavior |
|--------|------|---------|----------|
| Message | Email | Secondary | Opens message modal |
| Refer | Share | Secondary | Opens referral modal |
| Edit | Pencil | Primary | Navigates to edit form |

### Badges

| Badge Type | Background | Text |
|------------|------------|------|
| My Customer | --color-success-light | --color-success |
| Referred | --color-info-light | --color-info |
| Shared | --color-warning-light | --color-warning |
| Active | --color-success-light | --color-success |
| Inactive | --color-gray-100 | --color-gray-600 |

## Activity Timeline

### Icon Colors by Type

| Activity Type | Icon | Color |
|---------------|------|-------|
| service | Checkmark | --color-success |
| inquiry | Chat | --color-info |
| referral | Share | --color-primary |
| message | Email | --color-gray-600 |
| status-change | Flag | --color-warning |

### Timeline Display

- Maximum 5 items shown initially
- "View All" link loads full history
- Relative dates for recent (Today, Yesterday)
- Absolute dates for older items

## API Integration

### Get Customer Detail

```typescript
// GET /api/v1/customers/:id
interface CustomerDetailResponse {
  data: CustomerDetail;
}

// GET /api/v1/customers/:id/activity
interface ActivityResponse {
  data: ActivityItem[];
  meta: {
    page: number;
    totalItems: number;
  };
}
```

## Quick Actions

### Click-to-Call/Email

```typescript
const handlePhoneClick = () => {
  window.location.href = `tel:${customer.phone}`;
};

const handleEmailClick = () => {
  window.location.href = `mailto:${customer.email}`;
};
```

### Copy to Clipboard

```typescript
const copyToClipboard = async (text: string, label: string) => {
  await navigator.clipboard.writeText(text);
  toast.success(`${label} copied to clipboard`);
};
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Semantic heading hierarchy (h1 for name, h2 for sections)
- [ ] Interactive elements have accessible labels
- [ ] Timeline has proper ARIA roles
- [ ] Links have descriptive text
- [ ] Images have alt text

### Keyboard Navigation

- Tab through all interactive elements
- Enter to activate buttons/links
- Arrow keys for timeline navigation (optional)

## Responsive Behavior

### Desktop (≥1200px)
- 3-column grid layout
- All sections visible

### Tablet (768px - 1199px)
- 2-column grid layout
- Cards stack naturally

### Mobile (<768px)
- Single column layout
- Collapsible sections (optional)
- Sticky action bar at bottom

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerDetailView', () => {
  it('renders customer information', () => {});
  it('displays ownership information', () => {});
  it('lists vehicles', () => {});
  it('shows referral history', () => {});
  it('displays activity timeline', () => {});
  it('handles edit action', () => {});
  it('handles refer action', () => {});
  it('handles message action', () => {});
  it('handles loading state', () => {});
  it('handles error state', () => {});
  it('is accessible (axe audit)', () => {});
});
```

## Dependencies

- Card component
- Badge component
- Avatar component
- Button component
- Timeline component
- Breadcrumb component

## Related Components

- `CustomerListView` - Customer list page
- `CustomerForm` - Edit customer
- `ReferralModal` - Send referral
- `MessageModal` - Send message

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
