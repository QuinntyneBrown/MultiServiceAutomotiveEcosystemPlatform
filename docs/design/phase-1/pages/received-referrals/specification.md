# Received Referrals Management - Page Specification

## Overview

The Received Referrals Management page allows service professionals to view, manage, accept, and decline referrals received from other professionals in the network. This page implements requirements REQ-RF-F007, REQ-RF-F008, and REQ-RF-F009 from the Referral System frontend requirements.

---

## Requirements Mapping

### REQ-RF-F007: Received Referrals Management [Phase 1] [P0]
- List of received referrals with card-based layout
- Accept/decline action buttons on each referral
- View detailed referral information
- Customer contact information display
- Direct contact options (call/email)

### REQ-RF-F008: Referral Acceptance Flow [Phase 1] [P0]
- Review referral summary
- Optional message to source professional
- Discount honor confirmation checkbox
- Expected follow-up date selection
- Notifications sent to source professional and customer

### REQ-RF-F009: Referral Decline Flow [Phase 1] [P0]
- Required decline reason selection
- Predefined reason options
- Optional additional details
- Alternative professional suggestion
- Notification to source professional

---

## Page Layout

### Header
- **Page Title**: "Received Referrals"
- **Subtitle**: "Manage customer referrals from other professionals"
- Top navigation bar with search and notifications

### Sidebar Navigation
- Dashboard (home)
- Customers
- Referrals section:
  - Send Referrals
  - **Received** (active)
  - Analytics
- My Profile

### Filter Bar
**Filter Tabs:**
- All (count)
- Pending (count)
- Accepted (count)
- Declined (count)

**Filter Controls:**
- Priority dropdown (All Priorities, Urgent, High, Normal)
- Sort dropdown (Newest First, Oldest First, Priority)

---

## Components

### 1. Referral Card

**Visual Design:**
- Card with colored left border indicating priority
  - Red (#D32F2F): Urgent
  - Orange (#ED6C02): High Priority
  - Blue (#00529F): Normal
- Rounded corners (12px border-radius)
- Box shadow with hover elevation
- Three sections: Header, Body, Actions

**Header Section:**
- Priority badge (colored pill)
- Customer name (h3, bold, 20px)
- "Referred by: [Professional Name]" (14px, blue link)
- Received timestamp (12px, gray)

**Body Section:**
- Service needed (with gear icon)
- Vehicle information (with calendar icon)
- Discount badge (yellow background) - if applicable

**Actions Section:**
- "View Details" button (primary)
- "Accept" button (secondary)
- "Decline" button (ghost/tertiary)

**Card Data Fields:**
```typescript
interface ReferralCard {
  id: string;
  priority: 'urgent' | 'high' | 'normal';
  customerName: string;
  referredBy: {
    businessName: string;
    contactPerson: string;
    businessId: string;
  };
  serviceNeeded: string;
  vehicle: {
    year: number;
    make: string;
    model: string;
  };
  discount?: {
    type: 'percentage' | 'fixed';
    value: number;
  };
  receivedDate: Date;
  status: 'pending' | 'accepted' | 'declined';
}
```

### 2. Detail Modal

**Purpose:** Display comprehensive referral information

**Sections:**

1. **Customer Information**
   - Customer Name
   - Phone Number (with "Call" action)
   - Email Address (with "Email" action)
   - Preferred Contact Method
   - Contact Action Buttons:
     - "Call Customer" (primary, with phone icon)
     - "Send Email" (secondary, with email icon)

2. **Vehicle Information**
   - Make & Model
   - VIN (Vehicle Identification Number)
   - Color
   - Mileage

3. **Referral Information**
   - Referred By (business name)
   - Contact Person
   - Priority (with badge)
   - Received Date & Time
   - Service Requested (full description)
   - Discount Offered (if applicable)

4. **Notes from Referrer**
   - Full text notes in highlighted box
   - Blue left border
   - Light blue background (#E3F2FD)

**Footer Actions:**
- "Close" (ghost)
- "Decline" (destructive/red)
- "Accept Referral" (primary/blue)

**Modal Specifications:**
- Max width: 800px
- Max height: 90vh (scrollable)
- Backdrop: rgba(0, 0, 0, 0.5)
- Close on backdrop click
- Close button (X) in header

### 3. Accept Referral Modal

**Purpose:** Confirm acceptance with optional details

**Components:**

1. **Summary Section** (green background #E8F5E9)
   - Customer name
   - Service requested
   - Referred by (with contact person)
   - Discount offered

2. **Discount Confirmation** (yellow highlight #FFF2CC)
   - Checkbox: "I agree to honor the [X]% discount offered"
   - Pre-checked by default
   - Visual emphasis with yellow background

3. **Message to Referrer** (Optional)
   - Textarea input
   - Placeholder: "Thank you for the referral..."
   - Helper text: "This message will be sent to [Business Name] to confirm acceptance"

4. **Expected Follow-up Date** (Optional)
   - Date picker input
   - Helper text: "When do you plan to contact or schedule this customer?"

**Footer Actions:**
- "Cancel" (ghost)
- "Confirm Acceptance" (primary)

**Validation:**
- No required fields (all optional except implicit acceptance)
- Success feedback on confirmation

### 4. Decline Referral Modal

**Purpose:** Capture decline reason and optional details

**Components:**

1. **Instructions**
   - Text: "Please select a reason for declining this referral. This information will be shared with the referring professional."

2. **Decline Reason Options** (Radio buttons)
   - **Too busy / No capacity**
     - Description: "Currently at full capacity and unable to take new customers"
   - **Outside service area**
     - Description: "Customer location is outside my service area"
   - **Not my specialty**
     - Description: "This type of work is outside my area of expertise"
   - **Customer already known**
     - Description: "This customer is already in my system"
   - **Other reason**
     - Description: "Please specify below"

3. **Additional Details** (Optional)
   - Textarea input
   - Placeholder: "Provide additional context or suggest an alternative professional..."

4. **Suggest Alternative Professional** (Optional)
   - Dropdown/select input
   - Options populated from professional directory
   - Helper text: "Help the customer by suggesting another professional"

**Footer Actions:**
- "Cancel" (ghost)
- "Confirm Decline" (destructive/red)

**Validation:**
- Reason selection is REQUIRED
- Show error if attempting to submit without reason
- Additional details and alternative suggestion are optional

---

## Interactions & States

### Referral Card Interactions

**Hover State:**
- Elevate card (translateY -2px)
- Increase shadow (shadow-md)
- Transition: 200ms ease

**Click Actions:**
- "View Details" → Opens Detail Modal
- "Accept" → Opens Accept Modal
- "Decline" → Opens Decline Modal

### Modal Interactions

**Opening:**
- Fade in backdrop (opacity 0 → 0.5)
- Scale modal (0.95 → 1)
- Duration: 200ms

**Closing:**
- Click backdrop
- Click X button
- Click "Cancel" or "Close"
- After successful submission

**Keyboard:**
- ESC key closes modal
- Tab navigation within modal (focus trap)
- Enter key submits form (when applicable)

### Accept Flow

1. User clicks "Accept" button
2. Accept Modal opens with pre-filled summary
3. User optionally enters message and follow-up date
4. User clicks "Confirm Acceptance"
5. System processes acceptance:
   - Creates customer record (if not exists)
   - Updates referral status to "accepted"
   - Sends notification to referring professional
   - Sends notification to customer
   - Optionally sends message to referrer
6. Success feedback shown (toast/alert)
7. Modal closes
8. Referral card updates or moves to "Accepted" tab

### Decline Flow

1. User clicks "Decline" button
2. Decline Modal opens
3. User selects decline reason (REQUIRED)
4. User optionally enters additional details
5. User optionally selects alternative professional
6. User clicks "Confirm Decline"
7. Validation: Check that reason is selected
8. System processes decline:
   - Updates referral status to "declined"
   - Records decline reason
   - Sends notification to referring professional
   - Optionally notifies alternative professional
   - Does NOT notify customer (referrer handles that)
9. Success feedback shown
10. Modal closes
11. Referral card updates or moves to "Declined" tab

---

## Data Requirements

### API Endpoints Needed

```typescript
// Get received referrals
GET /api/professionals/{professionalId}/referrals/received
Query Parameters:
  - status?: 'pending' | 'accepted' | 'declined'
  - priority?: 'urgent' | 'high' | 'normal'
  - sortBy?: 'newest' | 'oldest' | 'priority'
  - page?: number
  - limit?: number

Response: {
  referrals: ReferralSummary[];
  totalCount: number;
  pagination: {
    page: number;
    limit: number;
    totalPages: number;
  };
}

// Get referral details
GET /api/referrals/{referralId}
Response: ReferralDetail

// Accept referral
POST /api/referrals/{referralId}/accept
Body: {
  honorDiscount: boolean;
  messageToReferrer?: string;
  expectedFollowUpDate?: Date;
}
Response: {
  success: boolean;
  referral: ReferralDetail;
}

// Decline referral
POST /api/referrals/{referralId}/decline
Body: {
  reason: 'busy' | 'location' | 'specialty' | 'known' | 'other';
  additionalDetails?: string;
  suggestedAlternativeId?: string;
}
Response: {
  success: boolean;
  referral: ReferralDetail;
}

// Get professional directory (for alternatives)
GET /api/professionals/directory
Query Parameters:
  - specialty?: string
  - location?: string
Response: {
  professionals: ProfessionalSummary[];
}
```

### Data Models

```typescript
interface ReferralSummary {
  id: string;
  priority: 'urgent' | 'high' | 'normal';
  status: 'pending' | 'accepted' | 'declined';
  customer: {
    id: string;
    name: string;
  };
  referredBy: {
    businessId: string;
    businessName: string;
    contactPerson: string;
  };
  serviceNeeded: string;
  vehicle: {
    year: number;
    make: string;
    model: string;
  };
  discount?: {
    type: 'percentage' | 'fixed';
    value: number;
  };
  receivedAt: Date;
}

interface ReferralDetail extends ReferralSummary {
  customer: {
    id: string;
    name: string;
    phone: string;
    email: string;
    preferredContact: 'phone' | 'email' | 'sms';
  };
  vehicle: {
    year: number;
    make: string;
    model: string;
    vin: string;
    color: string;
    mileage: number;
  };
  referrerNotes: string;
  internalNotes?: string;
}

interface ProfessionalSummary {
  id: string;
  businessName: string;
  specialty: string;
  location: string;
}
```

---

## Responsive Design

### Desktop (≥992px)
- Sidebar visible
- Referral cards: 2-3 columns grid
- Full detail modal width (800px)

### Tablet (768px - 991px)
- Sidebar hidden (hamburger menu)
- Referral cards: 2 columns grid
- Detail modal: 90% width

### Mobile (< 768px)
- Sidebar hidden (hamburger menu)
- Referral cards: 1 column (full width)
- Filter bar stacks vertically
- Detail modal: 95% width
- Modal sections may adjust layout

---

## Accessibility

### WCAG 2.1 AA Compliance

**Keyboard Navigation:**
- All interactive elements focusable
- Logical tab order
- Modal focus trap
- ESC closes modals
- Enter submits forms

**Screen Readers:**
- Semantic HTML (header, main, nav, article)
- ARIA labels for icons
- ARIA live regions for status updates
- Form labels properly associated

**Color Contrast:**
- All text meets 4.5:1 ratio
- Priority badges have sufficient contrast
- Disabled states clearly indicated

**Focus Indicators:**
- Visible focus ring (3px, blue, 0.4 opacity)
- Never remove focus styles
- Enhanced focus on modals

---

## Error Handling

### Network Errors
- Show error toast/alert
- Retry option for failed actions
- Maintain form data on failure

### Validation Errors
- Inline error messages
- Error summary at top of form
- Red border on invalid fields
- Focus first error field

### Empty States
- "No referrals received yet" message
- Illustration/icon
- Suggested action: "Expand your network"

---

## Performance Considerations

### Initial Load
- Load first page of referrals (10-20 items)
- Lazy load additional pages on scroll
- Cache referral list in memory

### Modal Loading
- Detail modal loads full data on open
- Show loading spinner if data fetch needed
- Cache loaded referral details

### Real-time Updates
- WebSocket connection for new referrals
- Badge count updates in real-time
- Toast notification for new urgent referrals

---

## Analytics & Tracking

### Events to Track
- `received_referrals_viewed` - Page view
- `referral_detail_opened` - Detail modal opened
- `referral_accepted` - Acceptance confirmed
- `referral_declined` - Decline confirmed
- `customer_contacted` - Call/email button clicked
- `filter_changed` - Filter or sort applied

### Metrics
- Acceptance rate by priority
- Time to respond to referral
- Decline reasons distribution
- Customer contact conversion rate

---

## Security & Privacy

### Authorization
- Verify user has permission to view referrals
- Validate referral belongs to logged-in professional
- Mask sensitive customer data until accepted

### Data Protection
- HTTPS for all API calls
- Sanitize user input in messages
- Rate limit accept/decline actions
- Log all referral status changes

---

## Future Enhancements (Post Phase 1)

### Phase 2
- Bulk accept/decline actions
- Automated decline rules
- Referral scoring/ranking
- Mobile app notifications

### Phase 3
- AI-powered acceptance suggestions
- Customer matching algorithm
- Referral analytics dashboard
- Automated follow-up reminders

---

## Design System Reference

### Colors
- **Primary Blue**: #00529F
- **Accent Yellow**: #FFD520
- **Success Green**: #2E7D32
- **Warning Orange**: #ED6C02
- **Error Red**: #D32F2F
- **Gray Scale**: #1A1A1A to #FAFAFA

### Typography
- **Font Family Primary**: Montserrat (headings)
- **Font Family Secondary**: Lato (body)
- **H2 (Page Title)**: 28px, Bold
- **H3 (Modal Title)**: 24px, Semibold
- **H4 (Card Title)**: 20px, Semibold
- **Body**: 16px, Regular
- **Small**: 14px, Regular
- **Caption**: 12px, Regular

### Spacing
- Base unit: 8px
- Card padding: 24px
- Modal padding: 24px
- Section margin: 24px
- Button gap: 12px

### Shadows
- Card: `0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06)`
- Card hover: `0 4px 6px rgba(0, 0, 0, 0.1), 0 2px 4px rgba(0, 0, 0, 0.06)`
- Modal: `0 20px 25px rgba(0, 0, 0, 0.1), 0 10px 10px rgba(0, 0, 0, 0.04)`

### Border Radius
- Card: 12px
- Button: 8px
- Modal: 12px
- Badge: 9999px (pill)

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | January 8, 2026 | Initial specification |
