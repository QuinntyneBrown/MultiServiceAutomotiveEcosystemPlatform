# Referral System - Frontend Requirements

## Feature Overview

User interfaces for the referral system, including customer referral sharing tools, professional referral management, and referral tracking dashboards.

---

## Customer Referral Interface

### REQ-RF-F001: Referral Dashboard (Customer) [Phase 1] [P0]
**Description**: Customer's view of their referral activity and rewards.

**Acceptance Criteria**:
- Display unique referral code prominently
- Show referral statistics
- List all referrals with status
- Display rewards earned/pending
- Quick share actions

**Dashboard Sections**:
```
1. Referral Code Card
   - Large display of code
   - "Copy Code" button
   - QR code display
   - Share buttons (Email, SMS, Social)

2. Statistics Cards
   - Total referrals sent
   - Successful conversions
   - Pending referrals
   - Total rewards earned
   - Pending rewards

3. Referral List
   - Table/cards of all referrals
   - Status badges (pending, converted, expired)
   - Date sent
   - Reward amount

4. Rewards Summary
   - Available balance
   - Pending rewards
   - Reward history
   - Redemption options (Phase 3)
```

---

### REQ-RF-F002: Share Referral Interface [Phase 1] [P0]
**Description**: Interface for customers to share referrals.

**Acceptance Criteria**:
- Multiple share methods
- Personalized message option
- Preview before sending
- Track share method used

**Share Methods**:
```
1. Copy Link
   - One-click copy
   - Toast confirmation
   - Short URL display

2. Email Share
   - Email input field
   - Pre-filled message (editable)
   - Send via platform or open mail client
   - Bulk email option (Phase 3)

3. SMS Share
   - Phone number input
   - Character count
   - Send via platform
   - US phone number format

4. Social Share
   - Facebook share button
   - Twitter/X share button
   - WhatsApp share button
   - LinkedIn share button

5. QR Code
   - Display QR code
   - Download QR code
   - Print-friendly version
```

**UI Components**:
- Share modal/drawer
- Tab navigation for methods
- Message preview
- Success/error feedback

---

### REQ-RF-F003: Referral Invitation Form [Phase 1] [P0]
**Description**: Direct referral invitation form.

**Acceptance Criteria**:
- Friend's contact information
- Personal message
- Select specific professional (optional)
- Send invitation

**Form Fields**:
```
- Friend's Name (required)
- Friend's Email (required)
- Friend's Phone (optional)
- Select Professional (optional dropdown)
- Personal Message (optional textarea)
```

---

### REQ-RF-F004: Referral Status Tracking [Phase 1] [P0]
**Description**: Track status of sent referrals.

**Acceptance Criteria**:
- Visual status timeline
- Status explanations
- Estimated reward display
- Expiration warning

**Status Display**:
```
Timeline visualization:
○ Sent → ○ Opened → ○ Signed Up → ○ First Service → ● Reward Paid

Status Cards:
- Pending: "Waiting for [Name] to sign up"
- Converted: "Great! [Name] signed up on [Date]"
- Rewarded: "You earned $[Amount]!"
- Expired: "This referral expired on [Date]"
```

---

## Professional Referral Interface

### REQ-RF-F005: Professional Referral Dashboard [Phase 1] [P0]
**Description**: Professional's view of referral activity.

**Acceptance Criteria**:
- Referrals sent summary
- Referrals received summary
- Quick action to send referral
- Network visualization (Phase 5)

**Dashboard Sections**:
```
1. Quick Stats
   - Referrals sent this month
   - Referrals received this month
   - Conversion rate
   - Active referrals

2. Sent Referrals Panel
   - Recent sent referrals
   - Status of each
   - Quick follow-up actions

3. Received Referrals Panel
   - Pending acceptance
   - Recently accepted
   - Quick accept/decline

4. Quick Actions
   - "Send New Referral" button
   - "View All Sent" link
   - "View All Received" link
```

---

### REQ-RF-F006: Send Professional Referral Form [Phase 1] [P0]
**Description**: Form for professionals to refer customers to other professionals.

**Acceptance Criteria**:
- Select target professional
- Select customer from list
- Add referral details
- Optional discount offer
- Confirmation before send

**Form Fields**:
```
1. Target Professional
   - Searchable dropdown
   - Filter by specialty/type
   - Show recent collaborators

2. Customer Selection
   - Search my customers
   - Customer summary card
   - Vehicle info display

3. Referral Details
   - Service Needed (required)
   - Reason for Referral (required)
   - Priority (normal/high/urgent)
   - Internal Notes (optional)

4. Discount Options
   - Enable discount toggle
   - Discount type (percentage/fixed)
   - Discount value input
   - Discount expiration

5. Preview & Confirm
   - Summary of referral
   - What customer will see
   - What target will see
   - Send button
```

---

### REQ-RF-F007: Received Referrals Management [Phase 1] [P0]
**Description**: Manage referrals received from other professionals.

**Acceptance Criteria**:
- List of received referrals
- Accept/decline actions
- View referral details
- Customer information display
- Contact customer directly

**List View**:
```
Referral Card:
┌─────────────────────────────────────────────┐
│ [Priority Badge] Customer Name              │
│ Referred by: [Professional Name]            │
│ Service Needed: [Service Description]       │
│ Discount: [Yes/No - Amount]                 │
│ Received: [Date]                            │
│                                             │
│ [Accept] [Decline] [View Details]           │
└─────────────────────────────────────────────┘
```

**Detail View**:
- Full customer information
- Referrer notes
- Service requested
- Discount details
- Customer's vehicle info
- Contact options (call, email)
- Accept/decline with notes

---

### REQ-RF-F008: Referral Acceptance Flow [Phase 1] [P0]
**Description**: Process for accepting a professional referral.

**Acceptance Criteria**:
- Review referral details
- Optional message to source
- Confirm acceptance
- Notification sent to source professional
- Customer notified

**Flow**:
```
1. Click "Accept"
2. Modal: Acceptance Confirmation
   - Summary of referral
   - Message to sender (optional)
   - Honor discount checkbox
   - Expected follow-up date (optional)
3. Confirm
4. Success notification
5. Redirect to customer detail
```

---

### REQ-RF-F009: Referral Decline Flow [Phase 1] [P0]
**Description**: Process for declining a professional referral.

**Acceptance Criteria**:
- Require decline reason
- Optional alternative suggestion
- Notification to source
- Confirm decline

**Decline Reasons** (predefined):
- Too busy / No capacity
- Outside service area
- Not my specialty
- Customer already known
- Other (free text)

---

### REQ-RF-F010: Referral Completion Flow [Phase 2] [P1]
**Description**: Mark a referral as completed.

**Acceptance Criteria**:
- Service completion confirmation
- Discount application confirmation
- Feedback to source professional
- Update referral status

---

### REQ-RF-F011: Sent Referrals Tracking [Phase 1] [P0]
**Description**: Track referrals sent to other professionals.

**Acceptance Criteria**:
- List of sent referrals
- Status tracking
- Follow-up reminders
- Resend option for declined

**Status Tracking**:
- Pending acceptance
- Accepted (with date)
- Declined (with reason)
- Completed (with feedback)

---

## Admin Interface

### REQ-RF-F012: Referral Admin Dashboard [Phase 2] [P1]
**Description**: Admin overview of all referral activity.

**Acceptance Criteria**:
- Platform-wide referral statistics
- Top referrers leaderboard
- Referral trends charts
- Flagged referrals for review

**Dashboard Metrics**:
- Total referrals (customer + professional)
- Conversion rate
- Average reward paid
- Referrals this month vs last
- Top referrers (customers)
- Top referring professionals
- Top receiving professionals

---

### REQ-RF-F013: Referral Review Queue [Phase 2] [P1]
**Description**: Admin queue for reviewing flagged referrals.

**Acceptance Criteria**:
- List of flagged referrals
- Fraud indicators display
- Approve/reject actions
- Add notes to referral

**Review Information**:
- Referrer details
- Referee details
- Fraud signals detected
- IP address info
- Device fingerprint
- Approval/rejection history

---

### REQ-RF-F014: Referral Reports [Phase 3] [P1]
**Description**: Detailed referral reporting interface.

**Reports**:
- Referral summary by period
- Referral breakdown by professional
- Reward payout report
- Conversion funnel analysis
- Fraud detection report

**Features**:
- Date range selection
- Filter by type/professional
- Export to CSV/PDF
- Schedule reports (Phase 4)

---

## Public Referral Interface

### REQ-RF-F015: Referral Landing Page [Phase 1] [P0]
**Description**: Landing page for referral links.

**Acceptance Criteria**:
- Display referrer info
- Welcome message
- Sign up prompt
- Discount highlight
- Professional showcase

**Page Sections**:
```
1. Hero Section
   - "[Name] invited you!"
   - Discount offer highlight
   - Sign up CTA

2. About Section
   - Platform description
   - Benefits for new customer

3. Professional Spotlight
   - If specific professional: their profile
   - If general: featured professionals

4. How It Works
   - Sign up
   - Get service
   - Both earn rewards

5. Sign Up Form
   - Pre-filled referral code
   - Registration form
```

---

### REQ-RF-F016: Referral Code Entry [Phase 1] [P0]
**Description**: Interface for entering referral code during signup.

**Acceptance Criteria**:
- Referral code input field
- Validation feedback
- Auto-apply from URL parameter
- "Don't have a code" option

**UI Behavior**:
- Real-time validation
- Show referrer name on valid
- Show discount on valid
- Error message on invalid

---

## Mobile Responsiveness

### REQ-RF-F017: Mobile Referral Sharing [Phase 1] [P0]
**Description**: Mobile-optimized referral sharing.

**Acceptance Criteria**:
- Native share sheet integration
- Touch-friendly share buttons
- QR code full screen display
- Copy to clipboard with haptic

---

### REQ-RF-F018: Mobile Referral Management [Phase 2] [P1]
**Description**: Mobile-optimized referral management for professionals.

**Acceptance Criteria**:
- Swipe actions (accept/decline)
- Quick send referral
- Push notification integration
- Offline referral viewing

---

## Notification Integration

### REQ-RF-F019: Referral Notification Display [Phase 1] [P0]
**Description**: In-app notifications for referral events.

**Notifications**:
- New referral received
- Referral accepted
- Referral declined
- Referral converted
- Reward earned

**Display**:
- Badge count on bell icon
- Notification dropdown list
- Click to view referral details
- Mark as read

---

## Gamification Elements (Phase 3)

### REQ-RF-F020: Referral Leaderboard [Phase 3] [P2]
**Description**: Gamified leaderboard for top referrers.

**Acceptance Criteria**:
- Monthly/all-time leaderboards
- Customer and professional boards
- Position indicator
- Progress to next rank
- Rewards/badges display

---

### REQ-RF-F021: Referral Milestones [Phase 3] [P2]
**Description**: Achievement milestones for referrals.

**Milestones**:
- First referral
- 5 successful referrals
- 10 successful referrals
- $100 in rewards
- etc.

**Display**:
- Achievement badges
- Celebration animation
- Progress bar to next milestone
