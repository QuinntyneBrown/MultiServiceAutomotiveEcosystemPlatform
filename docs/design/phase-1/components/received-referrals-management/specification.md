# Received Referrals Management Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-RF-F007` |
| **Component Name** | ReceivedReferralsManagement |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive management interface for professionals to view, accept, decline, and track referrals received from other professionals in the network. Includes filtering, status tracking, timeline visualization, and action workflows.

## User Stories

1. **As a professional**, I want to see all referrals I've received, so I can manage incoming business opportunities.
2. **As a professional**, I want to accept or decline referrals, so I can manage my workload.
3. **As a professional**, I want to track referral progress, so I can keep sending professionals informed.
4. **As a professional**, I want to see urgent referrals highlighted, so I can prioritize appropriately.

## Component Structure

```
ReceivedReferralsManagement/
├── index.ts                              # Public exports
├── ReceivedReferralsManagement.tsx       # Main component (React)
├── ReceivedReferralsManagement.component.ts  # Main component (Angular)
├── ReceivedReferralsManagement.module.scss   # Styles
├── ReceivedReferralsManagement.test.tsx      # Unit tests
├── components/
│   ├── ReferralStatsCards/               # Summary statistics
│   ├── ReferralFilters/                  # Filter tabs and search
│   ├── ReferralCard/                     # Individual referral display
│   ├── ReferralTimeline/                 # Progress timeline
│   ├── AcceptDeclineActions/             # Accept/decline workflow
│   ├── CustomerPreview/                  # Embedded customer info
│   └── EmptyState/                       # No referrals state
├── hooks/
│   ├── useReceivedReferrals.ts           # Data fetching
│   ├── useReferralFilters.ts             # Filter state
│   └── useReferralActions.ts             # Accept/decline logic
└── types/
    └── received-referral.types.ts        # Type definitions
```

## Props / Inputs

### React

```typescript
interface ReceivedReferralsManagementProps {
  /** Initial filter status */
  initialStatus?: ReferralStatus | 'all';

  /** Callback when referral is accepted */
  onAccept?: (referralId: string) => void;

  /** Callback when referral is declined */
  onDecline?: (referralId: string, reason: string) => void;

  /** Callback when referral is marked complete */
  onComplete?: (referralId: string) => void;

  /** Callback when viewing referral details */
  onViewDetails?: (referralId: string) => void;

  /** Enable/disable stats display */
  showStats?: boolean;

  /** Page size for pagination */
  pageSize?: number;
}

type ReferralStatus = 'pending' | 'accepted' | 'in-progress' | 'completed' | 'declined';
```

### Angular

```typescript
@Component({
  selector: 'app-received-referrals-management',
  templateUrl: './received-referrals-management.component.html',
  styleUrls: ['./received-referrals-management.component.scss']
})
export class ReceivedReferralsManagementComponent {
  @Input() initialStatus: ReferralStatus | 'all' = 'all';
  @Input() showStats = true;
  @Input() pageSize = 10;

  @Output() accept = new EventEmitter<string>();
  @Output() decline = new EventEmitter<{id: string; reason: string}>();
  @Output() complete = new EventEmitter<string>();
  @Output() viewDetails = new EventEmitter<string>();
}
```

## Data Models

### Received Referral

```typescript
interface ReceivedReferral {
  id: string;
  sendingProfessional: {
    id: string;
    businessName: string;
    ownerName: string;
    avatar?: string;
    location: {
      city: string;
      state: string;
    };
  };
  customer: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    avatar?: string;
  };
  vehicle?: {
    id: string;
    year: number;
    make: string;
    model: string;
    vin?: string;
  };
  serviceType: string;
  estimatedValue: number;
  finalValue?: number;
  priority: 'normal' | 'high' | 'urgent';
  status: ReferralStatus;
  reason: string;
  notes?: string;
  timeline: ReferralTimelineEvent[];
  createdAt: Date;
  acceptedAt?: Date;
  startedAt?: Date;
  completedAt?: Date;
  expectedCompletionDate?: Date;
  declinedAt?: Date;
  declineReason?: string;
}

interface ReferralTimelineEvent {
  id: string;
  status: ReferralStatus;
  date: Date;
  note?: string;
}

interface ReceivedReferralStats {
  total: number;
  totalChange: number;
  pending: number;
  pendingUrgent: number;
  inProgress: number;
  inProgressNearCompletion: number;
  completed: number;
  completedValue: number;
  declined: number;
  acceptanceRate: number;
}
```

## State Management

```typescript
interface ReceivedReferralsState {
  // Data
  referrals: ReceivedReferral[];
  stats: ReceivedReferralStats;

  // Loading
  isLoading: boolean;
  isLoadingMore: boolean;

  // Filters
  statusFilter: ReferralStatus | 'all';
  searchQuery: string;
  sortBy: 'date' | 'value' | 'priority';
  sortDirection: 'asc' | 'desc';

  // Pagination
  page: number;
  pageSize: number;
  totalItems: number;
  hasMore: boolean;

  // Actions
  acceptingId: string | null;
  decliningId: string | null;
  completingId: string | null;

  // Modals
  showDeclineModal: boolean;
  declineReferralId: string | null;
  declineReason: string;

  // Error
  error: string | null;
}
```

## Visual Specifications

### Layout

- **Stats Row**: 4 cards across top
- **Filter Bar**: Tabs + search + sort
- **Referral Cards**: Full-width stacked cards
- **Card Sections**: Header, body with details, footer with actions

### Stats Cards

| Stat | Style | Highlight Condition |
|------|-------|---------------------|
| Total Received | Default | - |
| Pending Response | Warning border | When count > 0 |
| In Progress | Default | Show "near completion" count |
| Completed | Default | Show total value |

### Referral Card Anatomy

```
┌─────────────────────────────────────────────────────────────────┐
│ [Warning Bar for Pending/Urgent]                                │
├─────────────────────────────────────────────────────────────────┤
│ HEADER (Gray background)                                        │
│ ┌──────┐  Business Name          [Urgent] [Pending]  Mar 20     │
│ │ Logo │  Location                                              │
│ └──────┘                                                        │
├─────────────────────────────────────────────────────────────────┤
│ BODY                                                            │
│ Service Type    Estimated Value    Vehicle                      │
│ Collision       $4,500             2022 Toyota Camry            │
│                                                                 │
│ ┌─────────────────────────────────────────────────────────┐     │
│ │ Customer Card                                           │     │
│ │ [Avatar] Name | Email | Phone                           │     │
│ └─────────────────────────────────────────────────────────┘     │
│                                                                 │
│ Reason for Referral                                             │
│ Description text...                                             │
│                                                                 │
│ [Notes Section - if present]                                    │
│                                                                 │
│ [Timeline - for in-progress/completed]                          │
├─────────────────────────────────────────────────────────────────┤
│ FOOTER (Gray background)                                        │
│ Status info                    [View Details] [Decline] [Accept]│
└─────────────────────────────────────────────────────────────────┘
```

### Status Indicators

| Status | Left Border | Badge Color |
|--------|-------------|-------------|
| Pending | --color-warning | --color-warning-light |
| Accepted/In Progress | --color-info | --color-info-light |
| Completed | --color-success | --color-success-light |
| Declined | --color-gray-400 | --color-gray-100 |

### Priority Badges

| Priority | Background | Text |
|----------|------------|------|
| Normal | None shown | - |
| High | --color-warning-light | --color-warning |
| Urgent | --color-error-light | --color-error |

### Timeline Visualization

```
○─────○─────●─────○
Received   Accepted   In Progress   Completed

○ = Completed step (green check)
● = Current step (blue filled)
○ = Pending step (gray empty)
```

## Action Workflows

### Accept Referral

```typescript
const handleAccept = async (referralId: string) => {
  setAcceptingId(referralId);
  try {
    await api.acceptReferral(referralId);
    toast.success('Referral accepted! Customer has been notified.');
    refetch();
  } catch (error) {
    toast.error('Failed to accept referral');
  } finally {
    setAcceptingId(null);
  }
};
```

### Decline Referral

```typescript
const handleDecline = async (referralId: string, reason: string) => {
  setDecliningId(referralId);
  try {
    await api.declineReferral(referralId, { reason });
    toast.info('Referral declined. Sending professional notified.');
    closeDeclineModal();
    refetch();
  } catch (error) {
    toast.error('Failed to decline referral');
  } finally {
    setDecliningId(null);
  }
};
```

### Mark Complete

```typescript
const handleComplete = async (referralId: string, finalValue?: number) => {
  setCompletingId(referralId);
  try {
    await api.completeReferral(referralId, { finalValue });
    toast.success('Referral marked complete! Great job!');
    refetch();
  } catch (error) {
    toast.error('Failed to complete referral');
  } finally {
    setCompletingId(null);
  }
};
```

## API Integration

### List Received Referrals

```typescript
// GET /api/v1/referrals/received
interface ListReceivedReferralsRequest {
  status?: ReferralStatus | 'all';
  search?: string;
  sortBy?: 'date' | 'value' | 'priority';
  sortDirection?: 'asc' | 'desc';
  page?: number;
  pageSize?: number;
}

interface ListReceivedReferralsResponse {
  data: ReceivedReferral[];
  meta: {
    page: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
  };
  stats: ReceivedReferralStats;
}
```

### Accept Referral

```typescript
// POST /api/v1/referrals/:id/accept
interface AcceptReferralRequest {
  estimatedCompletionDate?: Date;
  note?: string;
}

interface AcceptReferralResponse {
  data: ReceivedReferral;
  message: string;
}
```

### Decline Referral

```typescript
// POST /api/v1/referrals/:id/decline
interface DeclineReferralRequest {
  reason: string;
  recommendAlternative?: string;
}

interface DeclineReferralResponse {
  data: ReceivedReferral;
  message: string;
}
```

### Complete Referral

```typescript
// POST /api/v1/referrals/:id/complete
interface CompleteReferralRequest {
  finalValue?: number;
  note?: string;
  customerRating?: number;
}

interface CompleteReferralResponse {
  data: ReceivedReferral;
  message: string;
}
```

### Get Stats

```typescript
// GET /api/v1/referrals/received/stats
interface ReceivedReferralStatsResponse {
  data: ReceivedReferralStats;
}
```

## Decline Modal

### Fields

```typescript
interface DeclineModalProps {
  referralId: string;
  businessName: string;
  onConfirm: (reason: string) => void;
  onCancel: () => void;
  isLoading: boolean;
}

const declineReasons = [
  'Outside my service area',
  'Service not offered',
  'At capacity / booked out',
  'Customer requirements unclear',
  'Other (please specify)',
];
```

### Validation

- Reason is required
- If "Other" selected, custom reason required
- Minimum 10 characters for custom reason

## Real-time Updates

### WebSocket Events

```typescript
// Subscribe to referral updates
useEffect(() => {
  const socket = connectWebSocket();

  socket.on('referral:received', (referral: ReceivedReferral) => {
    addReferral(referral);
    showNotification('New referral received!');
  });

  socket.on('referral:updated', (referral: ReceivedReferral) => {
    updateReferral(referral);
  });

  socket.on('referral:reminder', (referral: ReceivedReferral) => {
    showNotification(`Pending referral requires response: ${referral.customer.firstName}`);
  });

  return () => socket.disconnect();
}, []);
```

## Notifications

### Pending Response Reminder

- Show after 12 hours of no response
- Escalate reminder after 24 hours
- Display countdown in card footer

### New Referral Notification

- Browser notification (if permitted)
- Badge on sidebar menu item
- Sound notification (optional setting)

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Cards have proper landmark roles
- [ ] Status badges have aria-label
- [ ] Timeline has proper ARIA for progress
- [ ] Action buttons have descriptive labels
- [ ] Modal follows focus trap pattern
- [ ] Loading states are announced

### Keyboard Navigation

- Tab through cards and actions
- Enter to expand/interact
- Escape to close modals
- Arrow keys for filter tabs (optional)

### Screen Reader Support

```html
<article role="article" aria-labelledby="referral-title-123">
  <header>
    <h3 id="referral-title-123">
      Referral from Mike's Auto Repair
      <span class="sr-only">Status: Pending, Priority: Urgent</span>
    </h3>
  </header>
  <div role="region" aria-label="Referral details">
    <!-- Details content -->
  </div>
  <footer>
    <div role="group" aria-label="Referral actions">
      <button aria-describedby="accept-hint">Accept Referral</button>
    </div>
    <p id="accept-hint" class="sr-only">
      Accepting will notify the customer and sending professional
    </p>
  </footer>
</article>
```

## Responsive Behavior

### Desktop (≥1200px)
- 4-column stats grid
- Full card details visible
- Timeline horizontal

### Tablet (768px - 1199px)
- 2-column stats grid
- Cards stack naturally
- Timeline may wrap

### Mobile (<768px)
- Single column stats
- Condensed card view
- Timeline vertical
- Sticky action bar

## Empty States

### No Referrals

```typescript
{
  icon: 'inbox',
  title: 'No referrals yet',
  description: 'Referrals from other professionals will appear here.',
  tip: 'Expand your network to receive more referrals!'
}
```

### No Results (Filtered)

```typescript
{
  icon: 'filter',
  title: 'No matching referrals',
  description: 'Try adjusting your filters or search terms.',
  action: {
    label: 'Clear Filters',
    onClick: clearFilters
  }
}
```

## Testing Requirements

### Unit Tests

```typescript
describe('ReceivedReferralsManagement', () => {
  it('renders referral list', () => {});
  it('displays stats cards', () => {});
  it('filters by status', () => {});
  it('searches referrals', () => {});
  it('sorts referrals', () => {});
  it('handles accept action', () => {});
  it('handles decline with reason', () => {});
  it('handles mark complete', () => {});
  it('shows timeline for in-progress', () => {});
  it('highlights urgent referrals', () => {});
  it('handles loading state', () => {});
  it('handles error state', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### Integration Tests

```typescript
describe('Received Referrals Flow', () => {
  it('accepts referral and updates list', () => {});
  it('declines referral with reason', () => {});
  it('completes referral workflow', () => {});
  it('receives real-time updates', () => {});
});
```

## Dependencies

- Card component
- Badge component
- Avatar component
- Timeline component
- Modal component
- Filter tabs component
- Search input component
- Toast notifications
- WebSocket client

## Related Components

- `ProfessionalReferralDashboard` - Overview of all referrals
- `SendProfessionalReferralForm` - Send referrals
- `ReferralDetailView` - Full referral details
- `CustomerPreviewCard` - Embedded customer info

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
