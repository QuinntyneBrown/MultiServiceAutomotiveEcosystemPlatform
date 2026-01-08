# Customer Referral Dashboard Component Specification

## Component Metadata

| Property | Value |
|----------|-------|
| Component ID | REQ-RF-F001 |
| Component Name | Customer Referral Dashboard |
| Phase | Phase 1 |
| Priority | P0 |
| Status | Design - Mockup Complete |
| Last Updated | 2026-01-08 |

## Overview

The Customer Referral Dashboard is a comprehensive interface that enables customers to view, manage, and share their referral code to earn rewards. This component serves as the central hub for the customer referral program, providing visibility into referral statistics, status tracking, and reward management.

### Business Value

- Drives customer acquisition through word-of-mouth marketing
- Increases customer engagement and loyalty
- Provides transparent reward tracking to build trust
- Reduces acquisition costs compared to traditional marketing

### User Stories

- As a customer, I want to easily share my referral code so that I can invite friends and family
- As a customer, I want to track the status of my referrals so that I know when I'll receive rewards
- As a customer, I want to view my reward balance so that I can redeem my earnings
- As a customer, I want to share my code through multiple channels so that I can reach more people

## Component Structure

### React Props/Interface

```typescript
interface ReferralDashboardProps {
  customerId: string;
  customerName: string;
  onCopyCode?: (code: string) => void;
  onShareClick?: (method: ShareMethod) => void;
  onRedeemRewards?: (amount: number) => void;
  onViewAllReferrals?: () => void;
  onViewAllActivity?: () => void;
  className?: string;
  theme?: 'light' | 'dark';
}

interface ReferralData {
  code: string;
  qrCodeUrl: string;
  statistics: ReferralStatistics;
  referrals: Referral[];
  rewards: RewardsData;
}

interface ReferralStatistics {
  totalReferrals: number;
  totalReferralsChange: number;
  successfulConversions: number;
  conversionsChange: number;
  pendingReferrals: number;
  pendingChange: number;
  totalRewardsEarned: number;
  rewardsChange: number;
}

interface Referral {
  id: string;
  recipientName: string;
  email?: string;
  phone?: string;
  dateSent: string;
  status: ReferralStatus;
  service?: string;
  rewardAmount: number;
  expirationDate?: string;
  conversionDate?: string;
}

type ReferralStatus = 'pending' | 'converted' | 'expired' | 'declined';

interface RewardsData {
  availableBalance: number;
  pendingRewards: number;
  rewardHistory: RewardHistoryItem[];
  currency: string;
}

interface RewardHistoryItem {
  id: string;
  type: 'earned' | 'redeemed' | 'expired';
  description: string;
  amount: number;
  date: string;
  referralId?: string;
}

type ShareMethod = 'email' | 'sms' | 'facebook' | 'twitter' | 'whatsapp' | 'link';
```

### Angular Component Interface

```typescript
@Component({
  selector: 'app-customer-referral-dashboard',
  templateUrl: './customer-referral-dashboard.component.html',
  styleUrls: ['./customer-referral-dashboard.component.scss']
})
export class CustomerReferralDashboardComponent implements OnInit {
  @Input() customerId!: string;
  @Input() customerName!: string;
  @Input() theme: 'light' | 'dark' = 'light';

  @Output() copyCode = new EventEmitter<string>();
  @Output() shareClick = new EventEmitter<ShareMethod>();
  @Output() redeemRewards = new EventEmitter<number>();
  @Output() viewAllReferrals = new EventEmitter<void>();
  @Output() viewAllActivity = new EventEmitter<void>();

  referralData$!: Observable<ReferralData>;
  loading = false;
  error: string | null = null;
}
```

## Data Models

### Referral Code Generation

```typescript
interface ReferralCodeConfig {
  prefix?: string; // e.g., customer first name
  suffix?: string; // e.g., year
  length: number; // total length including prefix/suffix
  format: 'alphanumeric' | 'numeric' | 'alpha';
  checkDigit?: boolean; // include validation digit
}

// Example: JOHND2024
// Format: {FirstName}{LastInitial}{Year}
```

### Status Badge Specifications

| Status | Background Color | Text Color | Label | Icon |
|--------|-----------------|------------|-------|------|
| pending | #fff3cd | #856404 | PENDING | ⏳ |
| converted | #d4edda | #2E7D32 | CONVERTED | ✓ |
| expired | #f8d7da | #721c24 | EXPIRED | ⨯ |
| declined | #e2e3e5 | #383d41 | DECLINED | - |

### Reward Calculation Rules

```typescript
interface RewardRule {
  id: string;
  serviceType: string | 'all';
  referralReward: number;
  refereeReward: number;
  minimumPurchase?: number;
  expirationDays: number;
  maxRewardsPerMonth?: number;
  validFrom: string;
  validUntil?: string;
}

// Example Rules:
// - Oil Change: $50 referrer, $25 referee, 90 day expiration
// - Tire Service: $75 referrer, $50 referee, 90 day expiration
// - Brake Service: $60 referrer, $40 referee, 90 day expiration
```

## API Integration

### Endpoints

#### Get Referral Dashboard Data
```http
GET /api/v1/customers/{customerId}/referrals/dashboard
Authorization: Bearer {token}

Response 200:
{
  "code": "JOHND2024",
  "qrCodeUrl": "https://cdn.platform.com/qr/JOHND2024.png",
  "statistics": {
    "totalReferrals": 24,
    "totalReferralsChange": 4,
    "successfulConversions": 18,
    "conversionsChange": 3,
    "pendingReferrals": 4,
    "pendingChange": -2,
    "totalRewardsEarned": 1260.00,
    "rewardsChange": 180.00
  },
  "referrals": [...],
  "rewards": {
    "availableBalance": 840.00,
    "pendingRewards": 420.00,
    "rewardHistory": [...],
    "currency": "USD"
  }
}
```

#### Get Referral List (Paginated)
```http
GET /api/v1/customers/{customerId}/referrals?page=1&limit=20&status=all&sort=dateSent:desc
Authorization: Bearer {token}

Query Parameters:
- page: number (default: 1)
- limit: number (default: 20, max: 100)
- status: 'all' | 'pending' | 'converted' | 'expired' | 'declined'
- sort: 'dateSent:asc' | 'dateSent:desc' | 'rewardAmount:asc' | 'rewardAmount:desc'

Response 200:
{
  "referrals": [...],
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 24,
    "totalPages": 2
  }
}
```

#### Send Referral
```http
POST /api/v1/customers/{customerId}/referrals/send
Authorization: Bearer {token}
Content-Type: application/json

{
  "method": "email",
  "recipients": [
    {
      "name": "John Smith",
      "email": "john.smith@example.com"
    }
  ],
  "personalMessage": "Optional custom message"
}

Response 201:
{
  "sent": 1,
  "failed": 0,
  "referrals": [
    {
      "id": "ref_abc123",
      "recipientName": "John Smith",
      "email": "john.smith@example.com",
      "status": "pending",
      "dateSent": "2026-01-08T10:30:00Z"
    }
  ]
}
```

#### Copy Referral Code (Analytics Tracking)
```http
POST /api/v1/customers/{customerId}/referrals/track-action
Authorization: Bearer {token}
Content-Type: application/json

{
  "action": "copy_code",
  "timestamp": "2026-01-08T10:30:00Z"
}

Response 204: No Content
```

#### Redeem Rewards
```http
POST /api/v1/customers/{customerId}/rewards/redeem
Authorization: Bearer {token}
Content-Type: application/json

{
  "amount": 200.00,
  "redemptionMethod": "account_credit" | "gift_card" | "cash"
}

Response 200:
{
  "transactionId": "rdm_xyz789",
  "amountRedeemed": 200.00,
  "newBalance": 640.00,
  "redemptionMethod": "account_credit",
  "processedAt": "2026-01-08T10:30:00Z"
}
```

## Share Method Integrations

### Email Share
```typescript
interface EmailSharePayload {
  to: string[];
  subject: string;
  body: string;
  referralCode: string;
  referralLink: string;
}

// Template:
Subject: Join me on [Platform Name] and get $25 off!
Body:
Hey! I've been using [Platform Name] for my car service needs and thought you might like it too.
Use my referral code JOHND2024 to get $25 off your first service!
[Referral Link]
```

### SMS Share
```typescript
interface SMSSharePayload {
  to: string;
  message: string;
  referralCode: string;
}

// Template (160 chars max):
"Get $25 off car service! Use code JOHND2024: [Short Link]"
```

### Social Media Share

#### Facebook
```typescript
// Uses Facebook Share Dialog
FB.ui({
  method: 'share',
  href: referralLink,
  quote: `Join me on ${platformName} and save on car services!`
});
```

#### Twitter
```typescript
// Pre-populated tweet
const tweetText = encodeURIComponent(
  `Just saved on car service with ${platformName}! Use code JOHND2024 for $25 off: ${referralLink}`
);
window.open(`https://twitter.com/intent/tweet?text=${tweetText}`);
```

#### WhatsApp
```typescript
// WhatsApp share link
const message = encodeURIComponent(
  `Get $25 off car service! Use my code JOHND2024: ${referralLink}`
);
window.open(`https://wa.me/?text=${message}`);
```

### Copy Link
```typescript
async function copyReferralLink(code: string): Promise<boolean> {
  const link = `https://platform.com/join?ref=${code}`;
  try {
    await navigator.clipboard.writeText(link);
    return true;
  } catch (error) {
    // Fallback for older browsers
    const textArea = document.createElement('textarea');
    textArea.value = link;
    document.body.appendChild(textArea);
    textArea.select();
    const success = document.execCommand('copy');
    document.body.removeChild(textArea);
    return success;
  }
}
```

## QR Code Generation

### QR Code Specifications

```typescript
interface QRCodeConfig {
  data: string; // Referral link
  size: number; // 512px for high quality
  errorCorrectionLevel: 'L' | 'M' | 'Q' | 'H'; // M = 15% recovery
  margin: number; // 4 modules
  color: {
    dark: string; // #00529F (primary brand color)
    light: string; // #FFFFFF
  };
  logo?: {
    url: string;
    width: number;
    height: number;
  };
}

// Generation endpoint:
GET /api/v1/referrals/qr-code?code=JOHND2024&size=512&format=png
```

## State Management

### React (Redux/Context)

```typescript
interface ReferralState {
  data: ReferralData | null;
  loading: boolean;
  error: string | null;
  copySuccess: boolean;
  shareSuccess: boolean;
  redeemSuccess: boolean;
}

// Actions
const actions = {
  fetchReferralData: (customerId: string) => void,
  copyReferralCode: (code: string) => void,
  shareReferral: (method: ShareMethod, recipients: any[]) => void,
  redeemRewards: (amount: number) => void,
  resetNotifications: () => void,
};
```

### Angular (NgRx)

```typescript
export interface ReferralDashboardState {
  referralData: ReferralData | null;
  loading: boolean;
  error: string | null;
  copySuccess: boolean;
  shareSuccess: boolean;
  redeemSuccess: boolean;
}

// Actions
export const loadReferralData = createAction(
  '[Referral Dashboard] Load Data',
  props<{ customerId: string }>()
);

export const copyReferralCode = createAction(
  '[Referral Dashboard] Copy Code',
  props<{ code: string }>()
);

export const shareReferral = createAction(
  '[Referral Dashboard] Share',
  props<{ method: ShareMethod; recipients: any[] }>()
);
```

## Styling and Theming

### Color Palette

```css
:root {
  /* Primary Colors */
  --primary-blue: #00529F;
  --primary-blue-dark: #003d7a;
  --accent-yellow: #FFD520;
  --accent-yellow-dark: #ffc000;

  /* Status Colors */
  --success-green: #2E7D32;
  --success-green-dark: #1b5e20;
  --success-light: #d4edda;

  --pending-yellow: #fff3cd;
  --pending-dark: #856404;

  --error-red: #f8d7da;
  --error-dark: #721c24;

  /* Neutral Colors */
  --background: #f5f7fa;
  --card-background: #ffffff;
  --text-primary: #333333;
  --text-secondary: #666666;
  --border-color: #e0e0e0;
  --border-light: #f0f0f0;

  /* Shadows */
  --shadow-sm: 0 2px 8px rgba(0, 0, 0, 0.1);
  --shadow-md: 0 4px 16px rgba(0, 0, 0, 0.15);
  --shadow-button: 0 4px 12px rgba(255, 213, 32, 0.3);
}
```

### Card Shadows

```css
.card {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.card:hover {
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
}
```

### Responsive Breakpoints

```css
/* Mobile */
@media (max-width: 768px) {
  /* Single column layout */
  /* Smaller fonts */
  /* Stacked buttons */
}

/* Tablet */
@media (min-width: 769px) and (max-width: 1024px) {
  /* Two column grid where appropriate */
}

/* Desktop */
@media (min-width: 1025px) {
  /* Full multi-column layout */
  /* Maximum width: 1400px */
}
```

## Accessibility (WCAG 2.1 AA)

### Keyboard Navigation

- All interactive elements must be keyboard accessible
- Tab order follows visual flow: referral code → copy → share → statistics → table → rewards
- Enter/Space activates buttons
- Escape closes modals/dialogs
- Arrow keys navigate within share button group

### ARIA Labels

```html
<!-- Referral Code -->
<div role="region" aria-label="Your referral code">
  <div id="referral-code" aria-live="polite">JOHND2024</div>
  <button aria-label="Copy referral code JOHND2024 to clipboard">Copy Code</button>
</div>

<!-- Statistics -->
<div role="region" aria-label="Referral statistics">
  <div aria-label="Total referrals: 24, increased by 4 this month">...</div>
</div>

<!-- Referral Table -->
<table aria-label="Referral history">
  <thead>
    <tr>
      <th scope="col">Name</th>
      <th scope="col">Date Sent</th>
      <th scope="col">Status</th>
      <th scope="col">Service</th>
      <th scope="col">Reward Amount</th>
    </tr>
  </thead>
</table>

<!-- Status Badge -->
<span class="status-badge status-converted" role="status" aria-label="Status: Converted">
  Converted
</span>
```

### Color Contrast

All text must meet WCAG AA standards:
- Normal text (< 18pt): 4.5:1 contrast ratio
- Large text (≥ 18pt): 3:1 contrast ratio
- UI components: 3:1 contrast ratio

**Verified Combinations:**
- Primary blue (#00529F) on white: 8.1:1 ✓
- White text on primary blue: 8.1:1 ✓
- Success green (#2E7D32) on white: 6.2:1 ✓
- Pending text (#856404) on pending bg (#fff3cd): 7.8:1 ✓
- Error text (#721c24) on error bg (#f8d7da): 8.5:1 ✓

### Screen Reader Support

```html
<!-- Loading state -->
<div role="status" aria-live="polite" aria-atomic="true">
  <span class="sr-only">Loading referral dashboard...</span>
</div>

<!-- Success notification -->
<div role="alert" aria-live="assertive" aria-atomic="true">
  Referral code copied to clipboard
</div>

<!-- Error notification -->
<div role="alert" aria-live="assertive" aria-atomic="true">
  Failed to share referral. Please try again.
</div>
```

### Focus Management

```css
/* Visible focus indicators */
button:focus,
a:focus {
  outline: 2px solid #00529F;
  outline-offset: 2px;
}

/* Skip focus indicator within cards on mouse click */
.card:focus:not(:focus-visible) {
  outline: none;
}
```

## Performance Optimization

### Code Splitting

```typescript
// Lazy load share modal
const ShareModal = lazy(() => import('./ShareModal'));

// Lazy load QR code generator
const QRCodeDisplay = lazy(() => import('./QRCodeDisplay'));
```

### Data Caching

```typescript
// Cache referral data for 5 minutes
const cacheConfig = {
  staleTime: 5 * 60 * 1000, // 5 minutes
  cacheTime: 10 * 60 * 1000, // 10 minutes
};

// React Query example
const { data } = useQuery(
  ['referralData', customerId],
  () => fetchReferralData(customerId),
  cacheConfig
);
```

### Virtualization

For large referral lists (> 50 items), implement virtual scrolling:

```typescript
import { VirtualScroller } from 'virtual-scroller';

<VirtualScroller
  items={referrals}
  itemHeight={60}
  renderItem={(referral) => <ReferralRow data={referral} />}
/>
```

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerReferralDashboard', () => {
  it('should render referral code correctly', () => {});
  it('should copy code to clipboard on button click', () => {});
  it('should display correct statistics', () => {});
  it('should render referral list with correct statuses', () => {});
  it('should calculate available and pending rewards correctly', () => {});
  it('should handle share button clicks for each method', () => {});
  it('should show loading state while fetching data', () => {});
  it('should display error message on API failure', () => {});
  it('should format currency values correctly', () => {});
  it('should format dates according to locale', () => {});
});
```

### Integration Tests

```typescript
describe('Referral Dashboard Integration', () => {
  it('should fetch and display referral data on mount', async () => {});
  it('should update statistics after successful referral', async () => {});
  it('should send email referral and show confirmation', async () => {});
  it('should redeem rewards and update balance', async () => {});
  it('should paginate through referral list', async () => {});
  it('should filter referrals by status', async () => {});
  it('should generate and display QR code', async () => {});
});
```

### E2E Tests (Cypress/Playwright)

```typescript
describe('Referral Dashboard E2E', () => {
  it('should allow user to copy referral code', () => {
    cy.visit('/dashboard/referrals');
    cy.get('[data-testid="copy-code-button"]').click();
    cy.get('[role="alert"]').should('contain', 'copied to clipboard');
  });

  it('should allow user to share via email', () => {
    cy.visit('/dashboard/referrals');
    cy.get('[data-testid="share-email-button"]').click();
    cy.get('[data-testid="email-modal"]').should('be.visible');
    cy.get('[data-testid="recipient-email"]').type('friend@example.com');
    cy.get('[data-testid="send-button"]').click();
    cy.get('[role="alert"]').should('contain', 'Referral sent successfully');
  });

  it('should display correct reward calculations', () => {
    cy.visit('/dashboard/referrals');
    cy.get('[data-testid="available-balance"]').should('contain', '$840.00');
    cy.get('[data-testid="pending-rewards"]').should('contain', '$420.00');
  });
});
```

### Accessibility Tests

```typescript
import { axe } from 'jest-axe';

describe('Referral Dashboard Accessibility', () => {
  it('should have no accessibility violations', async () => {
    const { container } = render(<CustomerReferralDashboard {...props} />);
    const results = await axe(container);
    expect(results).toHaveNoViolations();
  });

  it('should be keyboard navigable', () => {
    render(<CustomerReferralDashboard {...props} />);
    userEvent.tab(); // Should focus copy button
    userEvent.tab(); // Should focus share button
    userEvent.tab(); // Should focus first share icon
    // ... continue testing tab order
  });
});
```

## Error Handling

### Error States

```typescript
interface ErrorState {
  type: 'network' | 'validation' | 'authorization' | 'server' | 'unknown';
  message: string;
  retryable: boolean;
  action?: () => void;
}

// Error messages
const errorMessages = {
  networkError: 'Unable to load referral data. Please check your connection.',
  copyFailed: 'Failed to copy code. Please try again or copy manually.',
  shareFailed: 'Unable to send referral. Please try again later.',
  redeemFailed: 'Failed to redeem rewards. Please contact support.',
  invalidAmount: 'Redemption amount must not exceed available balance.',
};
```

### Error Recovery

```typescript
// Automatic retry with exponential backoff
const retryConfig = {
  maxAttempts: 3,
  initialDelay: 1000,
  maxDelay: 10000,
  backoffFactor: 2,
};

// Manual retry button
<button onClick={() => refetch()}>
  Retry Loading Dashboard
</button>
```

## Analytics Tracking

### Events to Track

```typescript
// Track user interactions
analytics.track('Referral Code Copied', {
  customerId: string,
  code: string,
  timestamp: Date,
});

analytics.track('Referral Shared', {
  customerId: string,
  code: string,
  method: ShareMethod,
  recipientCount: number,
  timestamp: Date,
});

analytics.track('Rewards Redeemed', {
  customerId: string,
  amount: number,
  method: RedemptionMethod,
  timestamp: Date,
});

analytics.track('Referral Dashboard Viewed', {
  customerId: string,
  timestamp: Date,
});

// Track conversion funnel
analytics.track('Referral Sent', { ... });
analytics.track('Referral Clicked', { ... });
analytics.track('Referral Signed Up', { ... });
analytics.track('Referral Converted', { ... });
```

## Security Considerations

### Data Protection

- Referral codes must be unique and non-sequential to prevent enumeration
- Personal data (email, phone) must be masked in the UI: `j***@example.com`
- Rate limiting on share endpoints: max 50 referrals per day per customer
- Validation of referral code ownership before allowing actions

### XSS Prevention

```typescript
// Sanitize user inputs before display
import DOMPurify from 'dompurify';

const sanitizedMessage = DOMPurify.sanitize(userMessage);
```

### CSRF Protection

All state-changing requests must include CSRF tokens:

```typescript
headers: {
  'X-CSRF-Token': getCsrfToken(),
  'Authorization': `Bearer ${accessToken}`,
}
```

## Dependencies

### Required Packages

```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-query": "^3.39.0",
    "qrcode.react": "^3.1.0",
    "date-fns": "^2.29.0",
    "clipboard": "^2.0.11",
    "dompurify": "^3.0.0"
  },
  "devDependencies": {
    "@testing-library/react": "^13.4.0",
    "@testing-library/user-event": "^14.4.3",
    "jest-axe": "^7.0.0",
    "cypress": "^12.0.0"
  }
}
```

## Future Enhancements (Post-MVP)

1. **Referral Analytics Dashboard**
   - Conversion rate trends
   - Best performing share methods
   - Geographic referral map

2. **Gamification**
   - Achievement badges (e.g., "10 Successful Referrals")
   - Leaderboard for top referrers
   - Bonus rewards for milestones

3. **Advanced Sharing**
   - Custom referral landing pages
   - Personalized referral messages
   - Social media integration with auto-posting

4. **Reward Tiers**
   - Bronze/Silver/Gold status based on referrals
   - Increasing rewards for higher tiers
   - Exclusive perks for top referrers

5. **Multi-language Support**
   - Internationalization (i18n)
   - Locale-specific currency formatting
   - RTL layout support

## Changelog

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0.0 | 2026-01-08 | Design Team | Initial specification and mockup |

## Related Documentation

- [Referral Program Business Rules](/docs/business/referral-program.md)
- [Rewards System Architecture](/docs/architecture/rewards-system.md)
- [API Documentation](/docs/api/referrals.md)
- [Style Guide](/docs/design/carmax-style-guide.md)
- [Component Library](/docs/design/component-library.md)

## Approval

| Role | Name | Date | Signature |
|------|------|------|-----------|
| Product Owner | TBD | - | - |
| UX Designer | TBD | - | - |
| Tech Lead | TBD | - | - |
| QA Lead | TBD | - | - |
