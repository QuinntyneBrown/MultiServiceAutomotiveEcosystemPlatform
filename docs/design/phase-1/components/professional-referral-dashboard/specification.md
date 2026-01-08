# Professional Referral Dashboard Component Specification

## Component Overview

**Component ID:** REQ-RF-F005
**Component Name:** Professional Referral Dashboard
**Phase:** Phase 1
**Priority:** P0
**Status:** Design Complete

### Purpose

The Professional Referral Dashboard provides automotive service professionals with a comprehensive interface to manage both sent and received referrals. This component enables professionals to track referral status, respond to incoming referrals, follow up on sent referrals, and monitor referral performance metrics.

### User Stories

- As a service professional, I want to see all my sent referrals in one place so I can track their status
- As a service professional, I want to quickly accept or decline received referrals so I can manage my workload
- As a service professional, I want to see referral statistics to understand my network performance
- As a service professional, I want to follow up on pending referrals to ensure customer satisfaction
- As a service professional, I want to view conversion rates to measure the success of my referrals

---

## Component Architecture

### Technology Stack

**React Implementation:**
- React 18+
- TypeScript
- React Hooks (useState, useEffect, useCallback, useMemo)
- Context API for state management
- React Query for data fetching

**Angular Implementation:**
- Angular 16+
- TypeScript
- RxJS for reactive programming
- NgRx for state management
- Angular Services for API integration

---

## Props/Inputs

### React Props

```typescript
interface ProfessionalReferralDashboardProps {
  // Professional Identity
  professionalId: string;
  professionalName: string;
  businessName: string;

  // Data Configuration
  maxSentReferrals?: number; // Default: 10
  maxReceivedReferrals?: number; // Default: 10
  autoRefreshInterval?: number; // Default: 30000 (30 seconds)

  // Feature Flags
  enableRealTimeUpdates?: boolean; // Default: true
  enableNotifications?: boolean; // Default: true
  showConversionMetrics?: boolean; // Default: true

  // Event Handlers
  onSendReferral?: () => void;
  onAcceptReferral?: (referralId: string) => Promise<void>;
  onDeclineReferral?: (referralId: string, reason: string) => Promise<void>;
  onViewReferralDetails?: (referralId: string) => void;
  onFollowUp?: (referralId: string) => void;
  onContactCustomer?: (referralId: string, customerId: string) => void;
  onViewAllSent?: () => void;
  onViewAllReceived?: () => void;

  // Styling
  className?: string;
  theme?: 'light' | 'dark';
  compactMode?: boolean;
}
```

### Angular Inputs

```typescript
@Component({
  selector: 'app-professional-referral-dashboard',
  templateUrl: './professional-referral-dashboard.component.html',
  styleUrls: ['./professional-referral-dashboard.component.scss']
})
export class ProfessionalReferralDashboardComponent implements OnInit, OnDestroy {
  // Professional Identity
  @Input() professionalId!: string;
  @Input() professionalName!: string;
  @Input() businessName!: string;

  // Data Configuration
  @Input() maxSentReferrals: number = 10;
  @Input() maxReceivedReferrals: number = 10;
  @Input() autoRefreshInterval: number = 30000;

  // Feature Flags
  @Input() enableRealTimeUpdates: boolean = true;
  @Input() enableNotifications: boolean = true;
  @Input() showConversionMetrics: boolean = true;

  // Event Emitters
  @Output() sendReferral = new EventEmitter<void>();
  @Output() acceptReferral = new EventEmitter<string>();
  @Output() declineReferral = new EventEmitter<ReferralDeclineEvent>();
  @Output() viewReferralDetails = new EventEmitter<string>();
  @Output() followUp = new EventEmitter<string>();
  @Output() contactCustomer = new EventEmitter<CustomerContactEvent>();
  @Output() viewAllSent = new EventEmitter<void>();
  @Output() viewAllReceived = new EventEmitter<void>();

  // Styling
  @Input() className?: string;
  @Input() theme: 'light' | 'dark' = 'light';
  @Input() compactMode: boolean = false;
}
```

---

## Data Models

### Sent Referral Model

```typescript
interface SentReferral {
  id: string;
  customerName: string;
  customerId: string;
  customerPhone: string;
  customerEmail: string;

  // Referral Details
  serviceType: string;
  serviceDescription: string;
  urgency: 'ASAP' | 'THIS_WEEK' | 'THIS_MONTH' | 'FLEXIBLE';

  // Recipient Information
  recipientProfessionalId: string;
  recipientBusinessName: string;
  recipientName: string;
  recipientSpecialty: string;

  // Status Tracking
  status: 'PENDING' | 'ACCEPTED' | 'DECLINED' | 'COMPLETED' | 'CANCELLED';
  sentDate: string; // ISO 8601 format
  respondedDate?: string;
  completedDate?: string;

  // Additional Information
  notes?: string;
  declineReason?: string;
  estimatedValue?: number;
  actualValue?: number;

  // Metadata
  createdAt: string;
  updatedAt: string;
  lastFollowUpDate?: string;
}
```

### Received Referral Model

```typescript
interface ReceivedReferral {
  id: string;
  customerName: string;
  customerId: string;
  customerPhone: string;
  customerEmail: string;

  // Referral Details
  serviceType: string;
  serviceDescription: string;
  urgency: 'ASAP' | 'THIS_WEEK' | 'THIS_MONTH' | 'FLEXIBLE';
  estimatedValue?: number;

  // Sender Information
  senderProfessionalId: string;
  senderBusinessName: string;
  senderName: string;
  senderSpecialty: string;

  // Status Tracking
  status: 'NEW' | 'ACCEPTED' | 'DECLINED' | 'IN_PROGRESS' | 'COMPLETED';
  receivedDate: string; // ISO 8601 format
  respondedDate?: string;
  appointmentDate?: string;
  completedDate?: string;

  // Response Information
  acceptedBy?: string; // User ID who accepted
  declineReason?: string;

  // Additional Information
  senderNotes?: string;
  internalNotes?: string;

  // Metadata
  createdAt: string;
  updatedAt: string;
  isRead: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
}
```

### Referral Statistics Model

```typescript
interface ReferralStatistics {
  currentMonth: {
    sentCount: number;
    receivedCount: number;
    acceptedCount: number;
    completedCount: number;
    declinedCount: number;
    activeCount: number;
    conversionRate: number; // Percentage (0-100)
    averageResponseTime: number; // Hours
    totalEstimatedValue: number;
  };

  previousMonth: {
    sentCount: number;
    receivedCount: number;
    conversionRate: number;
  };

  trends: {
    sentChange: number; // Percentage change
    receivedChange: number; // Percentage change
    conversionChange: number; // Percentage change
  };
}
```

### Decline Event Model

```typescript
interface ReferralDeclineEvent {
  referralId: string;
  reason: 'FULLY_BOOKED' | 'OUT_OF_SCOPE' | 'NO_CAPACITY' | 'LOCATION' | 'OTHER';
  customReason?: string;
  suggestAlternative?: boolean;
  alternativeProfessionalId?: string;
}

interface CustomerContactEvent {
  referralId: string;
  customerId: string;
  contactMethod: 'PHONE' | 'EMAIL' | 'SMS';
}
```

---

## Panel Configurations

### Sent Referrals Panel

**Display Settings:**
- Maximum visible referrals: 4-8 (configurable)
- Sort order: Most recent first
- Filter options: All, Pending, Accepted, Completed, Declined
- Pagination: "View All Sent" link for full list

**Referral Card Elements:**
- Customer name and service type
- Recipient business name
- Status badge with color coding:
  - Pending: Orange background (#FFF3E0), orange text (#ED6C02)
  - Accepted: Green background (#E8F5E9), green text (#2E7D32)
  - Completed: Blue background (#E3F2FD), blue text (#00529F)
  - Declined: Red background (#FFEBEE), red text (#D32F2F)
- Date sent and time elapsed
- Quick action buttons:
  - View Details (always available)
  - Send Message (for accepted/in-progress)
  - Follow Up (for pending > 24 hours)
  - Send to Another (for declined)

### Received Referrals Panel

**Display Settings:**
- Maximum visible referrals: 4-8 (configurable)
- Sort order: New/Pending first, then most recent
- Priority highlighting: High priority referrals displayed at top
- Filter options: All, New, Pending, Accepted

**Referral Card Elements:**
- Customer name and service needed
- Sender business name
- Status badge with color coding:
  - New: Info blue background (#E1F5FE), info text (#0288D1)
  - Accepted: Green background (#E8F5E9), green text (#2E7D32)
- Urgency indicator (ASAP highlighted)
- Customer contact information
- Date received and time elapsed
- Quick action buttons:
  - Accept (green button, for new referrals)
  - Decline (red button, for new referrals)
  - View Details (always available)
  - Contact Customer (for accepted referrals)

---

## Quick Stats Configuration

### Metrics Displayed

1. **Referrals Sent This Month**
   - Value: Count of sent referrals in current month
   - Icon: Send/Upload icon (📤)
   - Color: Primary blue (#00529F)
   - Trend: Percentage change from previous month

2. **Referrals Received This Month**
   - Value: Count of received referrals in current month
   - Icon: Inbox/Download icon (📥)
   - Color: Info blue (#0288D1)
   - Trend: Percentage change from previous month

3. **Conversion Rate**
   - Value: Percentage of sent referrals that were accepted
   - Calculation: (Accepted + Completed) / Total Sent × 100
   - Icon: Chart icon (📊)
   - Color: Success green (#2E7D32)
   - Trend: Percentage point change from previous month

4. **Active Referrals**
   - Value: Count of referrals with status Pending or In Progress
   - Icon: Activity/Lightning icon (⚡)
   - Color: Warning orange (#ED6C02)
   - Subtext: "Currently pending" (neutral text, no trend)

---

## Quick Action Handlers

### Send New Referral

```typescript
const handleSendReferral = useCallback(() => {
  // Navigate to referral creation form or open modal
  onSendReferral?.();
}, [onSendReferral]);
```

**Implementation:**
- Opens referral creation modal or navigates to form
- Pre-fills professional information
- Validates required fields
- Submits to API endpoint: `POST /api/v1/referrals`

### Accept Referral

```typescript
const handleAcceptReferral = useCallback(async (referralId: string) => {
  try {
    setLoading(true);
    await onAcceptReferral?.(referralId);

    // Update local state
    setReceivedReferrals(prev =>
      prev.map(ref =>
        ref.id === referralId
          ? { ...ref, status: 'ACCEPTED', respondedDate: new Date().toISOString() }
          : ref
      )
    );

    // Show success notification
    showNotification('Referral accepted successfully', 'success');

    // Trigger refresh of statistics
    refreshStatistics();
  } catch (error) {
    showNotification('Failed to accept referral', 'error');
  } finally {
    setLoading(false);
  }
}, [onAcceptReferral]);
```

**API Call:**
- Endpoint: `PUT /api/v1/referrals/{referralId}/accept`
- Method: PUT
- Body: `{ acceptedBy: professionalId, acceptedDate: timestamp }`

### Decline Referral

```typescript
const handleDeclineReferral = useCallback(async (
  referralId: string,
  reason: string,
  customReason?: string
) => {
  try {
    setLoading(true);
    await onDeclineReferral?.(referralId, reason);

    // Update local state
    setReceivedReferrals(prev =>
      prev.filter(ref => ref.id !== referralId)
    );

    // Show notification
    showNotification('Referral declined', 'info');

    // Refresh statistics
    refreshStatistics();
  } catch (error) {
    showNotification('Failed to decline referral', 'error');
  } finally {
    setLoading(false);
  }
}, [onDeclineReferral]);
```

**Implementation:**
- Opens decline reason modal
- Validates reason selection
- API Call: `PUT /api/v1/referrals/{referralId}/decline`
- Body includes: `{ reason, customReason, declinedBy, declinedDate }`

### Follow Up

```typescript
const handleFollowUp = useCallback((referralId: string) => {
  // Open communication modal or initiate contact
  onFollowUp?.(referralId);

  // Update last follow-up date
  setSentReferrals(prev =>
    prev.map(ref =>
      ref.id === referralId
        ? { ...ref, lastFollowUpDate: new Date().toISOString() }
        : ref
    )
  );
}, [onFollowUp]);
```

### View All Sent/Received

```typescript
const handleViewAllSent = useCallback(() => {
  // Navigate to full sent referrals page
  onViewAllSent?.();
}, [onViewAllSent]);

const handleViewAllReceived = useCallback(() => {
  // Navigate to full received referrals page
  onViewAllReceived?.();
}, [onViewAllReceived]);
```

---

## Real-Time Updates

### WebSocket Integration

```typescript
useEffect(() => {
  if (!enableRealTimeUpdates) return;

  const ws = new WebSocket(`wss://api.platform.com/ws/referrals/${professionalId}`);

  ws.onmessage = (event) => {
    const update = JSON.parse(event.data);

    switch (update.type) {
      case 'NEW_REFERRAL_RECEIVED':
        handleNewReferralReceived(update.data);
        break;

      case 'REFERRAL_STATUS_CHANGED':
        handleReferralStatusChanged(update.data);
        break;

      case 'REFERRAL_ACCEPTED':
        handleReferralAccepted(update.data);
        break;

      case 'REFERRAL_DECLINED':
        handleReferralDeclined(update.data);
        break;

      case 'STATISTICS_UPDATED':
        handleStatisticsUpdated(update.data);
        break;
    }
  };

  return () => {
    ws.close();
  };
}, [professionalId, enableRealTimeUpdates]);
```

### Polling Fallback

```typescript
useEffect(() => {
  if (enableRealTimeUpdates) return; // Use WebSocket instead

  const interval = setInterval(() => {
    refreshReferrals();
    refreshStatistics();
  }, autoRefreshInterval);

  return () => clearInterval(interval);
}, [enableRealTimeUpdates, autoRefreshInterval]);
```

### Notification Handling

```typescript
const handleNewReferralReceived = useCallback((referral: ReceivedReferral) => {
  // Add to received referrals list
  setReceivedReferrals(prev => [referral, ...prev]);

  // Update statistics
  setStatistics(prev => ({
    ...prev,
    currentMonth: {
      ...prev.currentMonth,
      receivedCount: prev.currentMonth.receivedCount + 1
    }
  }));

  // Show browser notification if enabled
  if (enableNotifications && 'Notification' in window && Notification.permission === 'granted') {
    new Notification('New Referral Received', {
      body: `${referral.customerName} - ${referral.serviceType}`,
      icon: '/assets/icons/referral-icon.png',
      tag: referral.id
    });
  }

  // Show in-app notification
  showNotification(
    `New referral from ${referral.senderBusinessName}`,
    'info'
  );
}, [enableNotifications]);
```

---

## API Integration

### Endpoints

#### Get Sent Referrals

```
GET /api/v1/professionals/{professionalId}/referrals/sent
Query Parameters:
  - limit: number (default: 10)
  - offset: number (default: 0)
  - status: string[] (optional)
  - startDate: string (optional)
  - endDate: string (optional)

Response: {
  referrals: SentReferral[];
  total: number;
  hasMore: boolean;
}
```

#### Get Received Referrals

```
GET /api/v1/professionals/{professionalId}/referrals/received
Query Parameters:
  - limit: number (default: 10)
  - offset: number (default: 0)
  - status: string[] (optional)
  - unreadOnly: boolean (default: false)

Response: {
  referrals: ReceivedReferral[];
  total: number;
  unreadCount: number;
  hasMore: boolean;
}
```

#### Get Referral Statistics

```
GET /api/v1/professionals/{professionalId}/referrals/statistics
Query Parameters:
  - period: 'current_month' | 'last_30_days' | 'custom'
  - startDate: string (optional, for custom period)
  - endDate: string (optional, for custom period)

Response: ReferralStatistics
```

#### Accept Referral

```
PUT /api/v1/referrals/{referralId}/accept
Body: {
  acceptedBy: string;
  acceptedDate: string;
  estimatedCompletionDate?: string;
  notes?: string;
}

Response: {
  success: boolean;
  referral: ReceivedReferral;
}
```

#### Decline Referral

```
PUT /api/v1/referrals/{referralId}/decline
Body: {
  declinedBy: string;
  declinedDate: string;
  reason: string;
  customReason?: string;
  suggestAlternative?: boolean;
  alternativeProfessionalId?: string;
}

Response: {
  success: boolean;
  message: string;
}
```

#### Send Follow-Up

```
POST /api/v1/referrals/{referralId}/follow-up
Body: {
  professionalId: string;
  message: string;
  followUpDate: string;
  followUpType: 'MESSAGE' | 'PHONE' | 'EMAIL';
}

Response: {
  success: boolean;
  followUpId: string;
}
```

---

## State Management

### React State Structure

```typescript
interface DashboardState {
  // Data
  sentReferrals: SentReferral[];
  receivedReferrals: ReceivedReferral[];
  statistics: ReferralStatistics | null;

  // UI State
  loading: boolean;
  error: string | null;
  selectedReferral: string | null;

  // Filters
  sentFilter: 'ALL' | 'PENDING' | 'ACCEPTED' | 'COMPLETED' | 'DECLINED';
  receivedFilter: 'ALL' | 'NEW' | 'ACCEPTED';

  // Modals
  showDeclineModal: boolean;
  showDetailsModal: boolean;
  declineReferralId: string | null;
}
```

### Angular State (NgRx)

```typescript
export interface ReferralDashboardState {
  sentReferrals: {
    data: SentReferral[];
    loading: boolean;
    error: string | null;
  };
  receivedReferrals: {
    data: ReceivedReferral[];
    loading: boolean;
    error: string | null;
  };
  statistics: {
    data: ReferralStatistics | null;
    loading: boolean;
    error: string | null;
  };
  ui: {
    selectedReferralId: string | null;
    showDeclineModal: boolean;
    showDetailsModal: boolean;
    sentFilter: ReferralStatusFilter;
    receivedFilter: ReferralStatusFilter;
  };
}
```

---

## Accessibility Requirements

### WCAG 2.1 AA Compliance

1. **Keyboard Navigation**
   - All interactive elements accessible via Tab/Shift+Tab
   - Enter/Space to activate buttons
   - Escape to close modals
   - Arrow keys for navigating between referral cards (optional)

2. **Screen Reader Support**
   - Semantic HTML elements (header, main, section, article)
   - ARIA labels for all interactive elements
   - ARIA live regions for real-time updates
   - Status announcements for actions (accept, decline, etc.)

3. **Color Contrast**
   - All text meets 4.5:1 contrast ratio (AA standard)
   - Status badges meet 3:1 contrast ratio
   - Interactive elements have visible focus indicators

4. **Focus Management**
   - Focus trap in modals
   - Focus returns to trigger element when modal closes
   - Visible focus indicators on all interactive elements
   - Skip links for keyboard users

### ARIA Implementation

```html
<!-- Quick Stats -->
<div role="region" aria-label="Referral Statistics">
  <div class="stat-card" role="group" aria-labelledby="sent-label">
    <h3 id="sent-label">Referrals Sent This Month</h3>
    <div class="stat-value" aria-label="23 referrals sent">23</div>
  </div>
</div>

<!-- Referral List -->
<div role="region" aria-label="Sent Referrals">
  <div class="referral-item" role="article" aria-labelledby="ref-1-title">
    <h4 id="ref-1-title">Sarah Johnson - Brake Repair</h4>
    <span role="status" aria-label="Status: Accepted">Accepted</span>
    <button aria-label="View details for Sarah Johnson's referral">
      View Details
    </button>
  </div>
</div>

<!-- Live Updates -->
<div aria-live="polite" aria-atomic="true" class="sr-only">
  <!-- Announcements inserted here for screen readers -->
</div>
```

---

## Testing Requirements

### Unit Tests

**React Testing (Jest + React Testing Library):**

```typescript
describe('ProfessionalReferralDashboard', () => {
  it('renders statistics correctly', () => {
    const { getByText } = render(
      <ProfessionalReferralDashboard
        professionalId="prof-123"
        statistics={mockStatistics}
      />
    );
    expect(getByText('23')).toBeInTheDocument();
    expect(getByText('Referrals Sent This Month')).toBeInTheDocument();
  });

  it('handles accept referral action', async () => {
    const handleAccept = jest.fn();
    const { getByText } = render(
      <ProfessionalReferralDashboard
        onAcceptReferral={handleAccept}
        receivedReferrals={mockReceivedReferrals}
      />
    );

    fireEvent.click(getByText('Accept'));
    await waitFor(() => {
      expect(handleAccept).toHaveBeenCalledWith('ref-123');
    });
  });

  it('updates UI on real-time referral received', () => {
    const { container } = render(
      <ProfessionalReferralDashboard
        enableRealTimeUpdates={true}
      />
    );

    // Simulate WebSocket message
    act(() => {
      mockWebSocket.simulateMessage({
        type: 'NEW_REFERRAL_RECEIVED',
        data: mockNewReferral
      });
    });

    expect(container.querySelector('.referral-item')).toBeInTheDocument();
  });
});
```

**Angular Testing (Jasmine + Karma):**

```typescript
describe('ProfessionalReferralDashboardComponent', () => {
  let component: ProfessionalReferralDashboardComponent;
  let fixture: ComponentFixture<ProfessionalReferralDashboardComponent>;

  it('should display sent referrals count', () => {
    component.statistics = mockStatistics;
    fixture.detectChanges();

    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.stat-value').textContent).toBe('23');
  });

  it('should emit acceptReferral event', () => {
    spyOn(component.acceptReferral, 'emit');

    component.handleAcceptReferral('ref-123');

    expect(component.acceptReferral.emit).toHaveBeenCalledWith('ref-123');
  });
});
```

### Integration Tests

1. **API Integration:**
   - Test data fetching from all endpoints
   - Test error handling for failed requests
   - Test pagination and filtering
   - Test real-time WebSocket connections

2. **User Flows:**
   - Accept referral flow (end-to-end)
   - Decline referral flow with reason
   - Send new referral from dashboard
   - Filter and view all referrals

### E2E Tests (Cypress/Playwright)

```typescript
describe('Professional Referral Dashboard E2E', () => {
  it('should allow professional to accept a referral', () => {
    cy.visit('/dashboard/referrals');
    cy.wait('@loadReferrals');

    // Find first pending referral
    cy.get('[data-testid="received-referrals"]')
      .find('[data-status="NEW"]')
      .first()
      .within(() => {
        cy.get('[data-testid="accept-button"]').click();
      });

    // Verify success notification
    cy.get('[data-testid="notification"]')
      .should('contain', 'Referral accepted successfully');

    // Verify statistics updated
    cy.get('[data-testid="active-referrals"]')
      .should('contain', '13'); // Increased by 1
  });

  it('should decline referral with reason', () => {
    cy.visit('/dashboard/referrals');

    cy.get('[data-testid="decline-button"]').first().click();
    cy.get('[data-testid="decline-modal"]').should('be.visible');
    cy.get('[data-testid="decline-reason"]').select('FULLY_BOOKED');
    cy.get('[data-testid="confirm-decline"]').click();

    cy.get('[data-testid="notification"]')
      .should('contain', 'Referral declined');
  });
});
```

### Accessibility Tests

```typescript
describe('Accessibility', () => {
  it('should have no accessibility violations', async () => {
    const { container } = render(<ProfessionalReferralDashboard />);
    const results = await axe(container);
    expect(results).toHaveNoViolations();
  });

  it('should be keyboard navigable', () => {
    const { getByText } = render(<ProfessionalReferralDashboard />);

    // Tab to first accept button
    userEvent.tab();
    expect(getByText('Accept')).toHaveFocus();

    // Press Enter to activate
    userEvent.keyboard('{Enter}');
    // Assert action occurred
  });
});
```

### Performance Tests

1. **Rendering Performance:**
   - Dashboard should render within 300ms
   - List updates should occur within 100ms
   - Real-time updates should not cause re-render of entire list

2. **Load Tests:**
   - Handle up to 100 referrals without performance degradation
   - WebSocket should handle 10 updates per second
   - Pagination should load additional pages within 200ms

---

## Error Handling

### Error States

```typescript
interface ErrorState {
  type: 'NETWORK' | 'VALIDATION' | 'AUTHORIZATION' | 'SERVER' | 'UNKNOWN';
  message: string;
  recoverable: boolean;
  retryAction?: () => void;
}
```

### Error Scenarios

1. **Network Errors:**
   - Display retry button
   - Show cached data if available
   - Indicate offline mode

2. **API Errors:**
   - 401 Unauthorized: Redirect to login
   - 403 Forbidden: Show permission denied message
   - 404 Not Found: Show empty state
   - 500 Server Error: Show error message with retry

3. **Validation Errors:**
   - Inline validation messages
   - Prevent form submission until resolved

### Error UI Components

```typescript
const ErrorBoundary: React.FC = ({ children }) => {
  return (
    <div className="error-state">
      <div className="error-icon">⚠️</div>
      <h3>Something went wrong</h3>
      <p>{error.message}</p>
      {error.recoverable && (
        <button onClick={error.retryAction}>Try Again</button>
      )}
    </div>
  );
};
```

---

## Performance Optimization

### Memoization

```typescript
// Memoize computed values
const activeReferralsCount = useMemo(() => {
  return sentReferrals.filter(
    ref => ref.status === 'PENDING' || ref.status === 'ACCEPTED'
  ).length;
}, [sentReferrals]);

// Memoize callbacks
const handleAcceptReferral = useCallback(async (id: string) => {
  // Implementation
}, [onAcceptReferral, refreshStatistics]);
```

### Virtual Scrolling

For large lists (> 50 items), implement virtual scrolling:

```typescript
import { FixedSizeList } from 'react-window';

const ReferralList: React.FC = ({ referrals }) => {
  return (
    <FixedSizeList
      height={600}
      itemCount={referrals.length}
      itemSize={120}
      width="100%"
    >
      {({ index, style }) => (
        <div style={style}>
          <ReferralCard referral={referrals[index]} />
        </div>
      )}
    </FixedSizeList>
  );
};
```

### Lazy Loading

```typescript
// Load referrals on demand
const loadMoreReferrals = useCallback(async () => {
  if (isLoading || !hasMore) return;

  setIsLoading(true);
  const newReferrals = await fetchReferrals({
    offset: sentReferrals.length,
    limit: 10
  });

  setSentReferrals(prev => [...prev, ...newReferrals]);
  setIsLoading(false);
}, [sentReferrals, isLoading, hasMore]);
```

---

## Responsive Design

### Breakpoints

- **Desktop (≥ 1200px):** Two-column layout, all stats visible
- **Tablet (768px - 1199px):** Two-column layout, stats in 2x2 grid
- **Mobile (< 768px):** Single column layout, stats stacked, actions stacked

### Mobile Optimizations

1. **Touch Targets:**
   - Minimum 44x44px for all interactive elements
   - Increased spacing between buttons
   - Swipe gestures for accept/decline

2. **Simplified View:**
   - Collapse referral details by default
   - Show only essential information
   - Sticky action buttons

3. **Performance:**
   - Reduce initial render count (5 items on mobile vs 8 on desktop)
   - Optimize images and icons
   - Minimize animations

---

## Security Considerations

1. **Data Protection:**
   - Customer PII (phone, email) only visible to authorized professionals
   - Encrypt sensitive data in transit (HTTPS)
   - Implement rate limiting on API endpoints

2. **Authorization:**
   - Verify professional has permission to view/modify referrals
   - Validate ownership before accept/decline actions
   - Implement CSRF protection

3. **Input Validation:**
   - Sanitize all user inputs
   - Validate referral IDs server-side
   - Prevent XSS attacks in notes/messages

---

## Future Enhancements

1. **Advanced Filtering:**
   - Filter by date range
   - Filter by service type
   - Filter by recipient/sender

2. **Bulk Actions:**
   - Accept/decline multiple referrals
   - Export referrals to CSV
   - Print referral summaries

3. **Analytics:**
   - Revenue tracking from referrals
   - Referral source attribution
   - Network strength indicators

4. **Communication:**
   - In-app messaging with recipients
   - Automated follow-up reminders
   - SMS/Email notifications

---

## Dependencies

### React Version

```json
{
  "react": "^18.2.0",
  "react-dom": "^18.2.0",
  "react-query": "^3.39.0",
  "date-fns": "^2.30.0",
  "axios": "^1.4.0"
}
```

### Angular Version

```json
{
  "@angular/core": "^16.0.0",
  "@angular/common": "^16.0.0",
  "@ngrx/store": "^16.0.0",
  "@ngrx/effects": "^16.0.0",
  "rxjs": "^7.8.0",
  "date-fns": "^2.30.0"
}
```

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | January 8, 2026 | Initial specification |

---

## Related Components

- **REQ-RF-F003:** Send Professional Referral Form
- **REQ-RF-F004:** Received Referrals List
- **REQ-RF-F006:** Referral Status Tracking
- **REQ-RF-F007:** Professional Dashboard (Main)

---

## Design Assets

- **Mockup:** `/docs/design/phase-1/components/professional-referral-dashboard/mockup.html`
- **Screenshot:** `/docs/design/phase-1/components/professional-referral-dashboard/screenshot.svg`
- **Figma:** [Link to Figma design when available]

---

## Approval

**Design Approved By:** [Name]
**Technical Approved By:** [Name]
**Date:** January 8, 2026
**Status:** Ready for Development
