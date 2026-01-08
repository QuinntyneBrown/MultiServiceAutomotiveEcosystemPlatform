# Customer Referral Dashboard Specification

## Overview

**Requirement IDs:** REQ-RF-F001, REQ-RF-F002, REQ-RF-F003, REQ-RF-F004
**Phase:** 1
**Priority:** P0

The Customer Referral Dashboard enables customers to view, manage, and share their referrals, track referral status, and monitor rewards earned through the referral program.

---

## Page Purpose

- Display customer's unique referral code prominently
- Provide multiple sharing methods (Copy Link, Email, SMS, Social, QR Code)
- Show referral statistics and performance metrics
- List all referrals with detailed status tracking
- Display rewards earned and pending
- Enable direct referral invitations
- Track referral conversion through visual timeline
- Motivate customers to refer more through clear rewards display

---

## Component Architecture

```
CustomerReferralDashboard/
├── customer-referral-dashboard.ts           # Main component
├── customer-referral-dashboard.html         # Template
├── customer-referral-dashboard.scss         # Styles
├── components/
│   ├── referral-code-card/                 # Code display & sharing
│   ├── referral-stats-cards/               # Statistics grid
│   ├── referral-list/                      # Referral items list
│   │   └── referral-item/                  # Individual referral
│   │       └── status-timeline/            # Visual status tracker
│   ├── rewards-summary/                    # Rewards display
│   ├── invitation-form/                    # Direct invite form
│   └── share-modal/                        # Share options modal
└── index.ts                                # Barrel export
```

---

## Page Sections

### 1. Referral Code Card (REQ-RF-F002)

Large, prominent card displaying the customer's unique referral information.

**Elements:**
- **Referral Code Display**
  - Large, monospace font for easy reading
  - Unique code (e.g., "JDOE2026")
  - Full referral URL below code
  - Dashed border for visual emphasis
  - Copy button with success feedback

- **QR Code**
  - Scannable QR code for mobile sharing
  - Download option
  - Print-friendly version

- **Share Buttons**
  - Copy Link (primary action - yellow accent button)
  - Email Share
  - SMS Share
  - Social Media Share (Facebook, Twitter, WhatsApp, LinkedIn)
  - Each button with appropriate icon
  - Toast notification on action

**Design:**
- Gradient blue background (#00529F to #003366)
- White text for contrast
- Responsive: Stack QR code below on mobile
- Semi-transparent button backgrounds

---

### 2. Statistics Cards (REQ-RF-F001)

Four metric cards showing key referral performance indicators.

**Cards:**

1. **Total Referrals Sent**
   - Count of all referrals sent
   - Month-over-month change
   - Blue icon (users group)

2. **Successful Conversions**
   - Count of completed referrals
   - Conversion rate percentage
   - Green icon (checkmark)

3. **Pending Referrals**
   - Count of active but not yet converted
   - Status indicator
   - Gray icon (clock)

4. **Total Rewards Earned**
   - Dollar amount of all earned rewards
   - Month-over-month earnings
   - Yellow icon (dollar sign)

**Design:**
- White cards with subtle shadow
- Hover effect: lift and increase shadow
- Icon in colored background circle
- Large number display (36px)
- Small trend indicator with arrow

**Grid:**
- 4 columns on desktop (≥992px)
- 2 columns on tablet (768-991px)
- 1 column on mobile (<768px)
- Minimum card width: 240px

---

### 3. Referral List (REQ-RF-F001, REQ-RF-F004)

Scrollable list of all referrals with detailed status.

**Referral Item Components:**

- **Header**
  - Friend's name (bold)
  - Email address (gray, smaller)
  - Status badge (colored pill)

- **Metadata**
  - Date sent/joined
  - Reward amount (earned or pending)
  - Icons for each metadata item

- **Status Timeline** (REQ-RF-F004)
  - 5-step visual timeline:
    1. Sent (initial)
    2. Opened (link clicked)
    3. Signed Up (account created)
    4. First Service (service completed)
    5. Rewarded (reward paid)
  - Completed steps: Green circle with checkmark
  - Active step: Blue circle with glow effect
  - Pending steps: Gray circle
  - Connected by line (green for completed, gray for pending)

**Status Badges:**
- **Pending:** Orange background, "Waiting for signup"
- **Converted:** Green background, "Signed up"
- **Rewarded:** Blue background, "Reward earned"
- **Expired:** Gray background, "Expired"

**Interactions:**
- Hover: Border color changes to blue, subtle shadow
- Click: Expand to show full details (future enhancement)
- Responsive: Timeline scrolls horizontally on mobile

---

### 4. Rewards Summary (REQ-RF-F001)

Sidebar card showing reward balance and breakdown.

**Components:**

- **Available Balance**
  - Large dollar amount
  - Yellow gradient background
  - "Ready to use" subtitle
  - Most prominent display

- **Reward Breakdown**
  - Total Earned (with checkmark icon)
  - Pending (with clock icon)
  - Redeemed (with download icon)
  - Each row: label + amount
  - Border between rows

- **Redemption CTA**
  - "Redeem Rewards" button
  - Disabled state for Phase 3 placeholder
  - Blue background when active
  - Full width

**Design:**
- White card background
- Yellow gradient for balance section
- Icons in gray
- Values right-aligned

---

### 5. Invitation Form (REQ-RF-F003)

Direct invitation form for sending referrals.

**Form Fields:**

1. **Friend's Name** (required)
   - Text input
   - Validation: Not empty

2. **Friend's Email** (required)
   - Email input
   - Validation: Valid email format

3. **Friend's Phone** (optional)
   - Phone input
   - Placeholder: +1 (555) 123-4567
   - Format: US phone number

4. **Personal Message** (optional)
   - Textarea (3 rows)
   - Placeholder: "Add a personal note..."
   - Character limit: 500

**Submission:**
- "Send Invitation" button (full width, blue)
- Form validation before submit
- Success toast notification
- Form reset on success
- Error handling for failed sends

**Design:**
- Follows platform text field styles
- Proper spacing (24px) between fields
- Accessible labels
- Focus states with blue ring

---

## Data Requirements

### Referral Object
```typescript
interface Referral {
  id: string;
  referrerId: string;
  referralCode: string;
  friendName: string;
  friendEmail: string;
  friendPhone?: string;
  personalMessage?: string;
  status: 'pending' | 'opened' | 'signed_up' | 'converted' | 'rewarded' | 'expired';
  dateSent: Date;
  dateOpened?: Date;
  dateSignedUp?: Date;
  dateFirstService?: Date;
  dateRewarded?: Date;
  expiresAt: Date;
  rewardAmount: number;
  rewardStatus: 'pending' | 'earned' | 'paid';
  shareMethod?: 'link' | 'email' | 'sms' | 'social';
}
```

### Referral Statistics
```typescript
interface ReferralStatistics {
  totalSent: number;
  totalConverted: number;
  totalPending: number;
  totalExpired: number;
  totalRewardsEarned: number;
  pendingRewards: number;
  availableBalance: number;
  redeemedAmount: number;
  conversionRate: number;
  monthlyChange: {
    sent: number;
    converted: number;
    earnings: number;
  };
}
```

### Customer Referral Info
```typescript
interface CustomerReferralInfo {
  customerId: string;
  referralCode: string;
  referralUrl: string;
  qrCodeUrl: string;
  statistics: ReferralStatistics;
  referrals: Referral[];
}
```

---

## API Integration

### GET /api/customers/me/referrals

Retrieve customer's referral dashboard data.

**Response:**
```json
{
  "customerId": "cust_123",
  "referralCode": "JDOE2026",
  "referralUrl": "https://autopro.network/join/JDOE2026",
  "qrCodeUrl": "https://api.autopro.network/qr/JDOE2026",
  "statistics": {
    "totalSent": 12,
    "totalConverted": 7,
    "totalPending": 3,
    "totalExpired": 2,
    "totalRewardsEarned": 350,
    "pendingRewards": 150,
    "availableBalance": 200,
    "redeemedAmount": 150,
    "conversionRate": 58,
    "monthlyChange": {
      "sent": 3,
      "converted": 2,
      "earnings": 100
    }
  },
  "referrals": [...]
}
```

### POST /api/referrals/invite

Send a direct referral invitation.

**Request:**
```json
{
  "friendName": "John Smith",
  "friendEmail": "john@email.com",
  "friendPhone": "+15551234567",
  "personalMessage": "You should try this service!",
  "shareMethod": "email"
}
```

**Response:**
```json
{
  "success": true,
  "referralId": "ref_456",
  "message": "Invitation sent successfully"
}
```

### POST /api/referrals/share-track

Track referral share action.

**Request:**
```json
{
  "referralCode": "JDOE2026",
  "shareMethod": "link",
  "shareTarget": "copy_link"
}
```

**Response:**
```json
{
  "success": true,
  "tracked": true
}
```

---

## State Management

### Component State

```typescript
@Component({
  selector: 'app-customer-referral-dashboard',
  templateUrl: './customer-referral-dashboard.html',
  styleUrls: ['./customer-referral-dashboard.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ReferralCodeCardComponent,
    ReferralStatsCardsComponent,
    ReferralListComponent,
    RewardsSummaryComponent,
    InvitationFormComponent,
    ShareModalComponent,
    ToastComponent
  ]
})
export class CustomerReferralDashboard implements OnInit {
  private readonly referralService = inject(ReferralService);
  private readonly toastService = inject(ToastService);
  private readonly clipboardService = inject(ClipboardService);

  // Data
  referralInfo$ = signal<CustomerReferralInfo | null>(null);
  isLoading$ = signal<boolean>(true);
  error$ = signal<string | null>(null);

  // UI State
  showShareModal$ = signal<boolean>(false);
  selectedReferral$ = signal<Referral | null>(null);

  // Invitation Form
  invitationForm = new FormGroup({
    friendName: new FormControl('', [Validators.required]),
    friendEmail: new FormControl('', [Validators.required, Validators.email]),
    friendPhone: new FormControl('', [Validators.pattern(/^\+?1?\d{10,14}$/)]),
    personalMessage: new FormControl('', [Validators.maxLength(500)])
  });

  isSubmittingInvite$ = signal<boolean>(false);

  ngOnInit(): void {
    this.loadReferralData();
  }

  loadReferralData(): void {
    this.isLoading$.set(true);
    this.referralService.getCustomerReferrals()
      .subscribe({
        next: (data) => {
          this.referralInfo$.set(data);
          this.isLoading$.set(false);
        },
        error: (err) => {
          this.error$.set('Failed to load referral data');
          this.isLoading$.set(false);
        }
      });
  }

  copyReferralLink(): void {
    const url = this.referralInfo$()?.referralUrl;
    if (url) {
      this.clipboardService.copy(url);
      this.toastService.success('Referral link copied to clipboard!');
      this.trackShare('copy_link');
    }
  }

  shareViaEmail(): void {
    this.showShareModal$.set(true);
    // Open share modal with email tab
  }

  shareViaSMS(): void {
    this.showShareModal$.set(true);
    // Open share modal with SMS tab
  }

  shareViaSocial(platform: string): void {
    const url = this.referralInfo$()?.referralUrl;
    // Open social share dialog
    this.trackShare(platform);
  }

  trackShare(method: string): void {
    const code = this.referralInfo$()?.referralCode;
    if (code) {
      this.referralService.trackShare(code, method).subscribe();
    }
  }

  sendInvitation(): void {
    if (this.invitationForm.valid && !this.isSubmittingInvite$()) {
      this.isSubmittingInvite$.set(true);

      const formData = this.invitationForm.value;
      this.referralService.sendInvitation(formData)
        .subscribe({
          next: (response) => {
            this.toastService.success('Invitation sent successfully!');
            this.invitationForm.reset();
            this.isSubmittingInvite$.set(false);
            this.loadReferralData(); // Refresh data
          },
          error: (err) => {
            this.toastService.error('Failed to send invitation. Please try again.');
            this.isSubmittingInvite$.set(false);
          }
        });
    }
  }

  viewReferralDetails(referral: Referral): void {
    this.selectedReferral$.set(referral);
    // Navigate or open modal with full details
  }

  downloadQRCode(): void {
    const qrUrl = this.referralInfo$()?.qrCodeUrl;
    // Trigger download
  }
}
```

---

## Responsive Behavior

### Desktop (≥992px)
- Two-column layout for main content
- Referral list (2/3 width) + Rewards/Invite sidebar (1/3 width)
- 4-column statistics grid
- QR code displayed inline with referral code
- Full navigation visible

### Tablet (768-991px)
- Single column layout
- 2-column statistics grid
- Referral list full width
- Rewards and Invite sections stacked below
- Full navigation visible

### Mobile (<768px)
- Single column layout
- 1-column statistics grid
- QR code centered below referral code
- Share buttons may wrap
- Hamburger menu for navigation
- Status timeline scrolls horizontally
- Form fields stack vertically

---

## Accessibility (WCAG 2.1 AA)

### Color Contrast
- All text meets 4.5:1 ratio minimum
- Status badges use sufficient contrast
- Icons have text labels
- Color not sole indicator of status

### Keyboard Navigation
- All interactive elements keyboard accessible
- Logical tab order
- Focus indicators visible (blue ring)
- Share buttons in logical sequence
- Form fields properly labeled

### Screen Reader Support
- Semantic HTML structure
- ARIA labels for icons
- Status announcements for dynamic updates
- Form validation messages announced
- Progress indicators for timeline

### Implementation
```html
<!-- Referral code with SR support -->
<div class="referral-code-display" role="region" aria-label="Your referral code">
  <span class="sr-only">Your referral code is</span>
  <div class="referral-code-display__code" aria-label="JDOE2026">JDOE2026</div>
</div>

<!-- Share button with proper labels -->
<button
  class="share-btn"
  aria-label="Copy referral link to clipboard"
  onclick="copyLink()">
  <svg aria-hidden="true">...</svg>
  Copy Link
</button>

<!-- Status timeline with ARIA -->
<div class="status-timeline" role="list" aria-label="Referral progress">
  <div class="status-timeline__step status-timeline__step--completed" role="listitem">
    <div class="status-timeline__circle" aria-label="Completed: Sent">
      <svg aria-hidden="true">...</svg>
    </div>
    <div class="status-timeline__label">Sent</div>
  </div>
  <!-- More steps -->
</div>

<!-- Form with proper labels and validation -->
<form class="invitation-form" aria-label="Send referral invitation">
  <div class="text-field">
    <label for="friend-name" class="text-field__label">Friend's Name *</label>
    <input
      id="friend-name"
      type="text"
      class="text-field__input"
      required
      aria-required="true"
      aria-describedby="name-error">
    <span id="name-error" class="text-field__error" role="alert"></span>
  </div>
</form>
```

---

## Performance Considerations

### Initial Load
- Lazy load referral list (virtual scrolling for >50 items)
- Preload critical statistics
- Defer QR code generation
- Optimize images and icons

### Data Updates
- Real-time updates via WebSocket for status changes
- Debounce form inputs
- Cache referral data (5 minutes)
- Optimistic UI updates

### Bundle Size
- Code splitting for share modal
- Lazy load QR code library
- Tree-shake unused components
- Optimize SVG icons

---

## Error Handling

### Network Errors
- Retry mechanism for failed loads
- Offline state detection
- Cached data fallback
- User-friendly error messages

### Validation Errors
- Inline field validation
- Clear error messages
- Form-level validation summary
- Disabled submit on invalid

### Edge Cases
- No referrals sent yet (empty state)
- Expired referral codes
- Duplicate invitations
- Invalid email/phone formats
- Share method not supported

---

## Testing Requirements

### Unit Tests
- Statistics calculations
- Status timeline rendering
- Form validation logic
- Share method tracking
- Date formatting

### Integration Tests
- API data loading
- Form submission flow
- Share modal interactions
- Toast notifications
- Clipboard operations

### E2E Tests
- Complete referral flow
- Copy link and verify
- Send invitation and verify
- View referral details
- Mobile responsive behavior

---

## Security Considerations

### Data Protection
- Referral codes properly authenticated
- Friend email/phone encrypted in transit
- Rate limiting on invitation sends
- CSRF protection on forms
- XSS prevention in personal messages

### Privacy
- Friend data not shared before acceptance
- Referral history private to customer
- Secure QR code generation
- No PII in URLs
- Opt-out mechanism for referred friends

---

## Future Enhancements (Post-Phase 1)

### Phase 2
- Bulk email invitations
- Email templates customization
- Referral analytics dashboard
- Export referral list

### Phase 3
- Reward redemption interface
- Gamification elements (badges, leaderboards)
- Social media preview customization
- Advanced sharing options

### Phase 4
- Referral campaigns
- Tiered reward structures
- Team referral challenges
- API for third-party integrations

---

## Related Requirements

- **REQ-RF-B001:** Referral System Backend (referral code generation)
- **REQ-RF-B002:** Share Tracking Backend (analytics)
- **REQ-RF-B003:** Reward Calculation Backend (reward amounts)
- **REQ-CM-F001:** Customer Dashboard (navigation integration)
- **REQ-LP-F001:** Loyalty Program (rewards display)
- **REQ-NO-F001:** Notifications (referral status updates)

---

## Design Assets

### Icons Required
- Users group (referrals sent)
- Checkmark (conversions)
- Clock (pending)
- Dollar sign (rewards)
- Calendar (dates)
- Email (share)
- Phone (SMS)
- Social media logos
- QR code placeholder
- Copy icon

### Color Usage
- Primary Blue (#00529F): Headers, CTAs, active states
- Accent Yellow (#FFD520): Primary buttons, rewards
- Success Green (#2E7D32): Converted status, checkmarks
- Warning Orange (#ED6C02): Pending status
- Gray (#666666): Disabled, secondary text

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | January 2026 | Initial specification |
