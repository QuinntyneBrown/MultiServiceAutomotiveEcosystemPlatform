# Professional Referral Dashboard - Technical Specification

**Page**: Professional Referral Dashboard
**Version**: 1.0.0
**Last Updated**: January 2026
**Requirements**: REQ-RF-F005, REQ-RF-F006, REQ-RF-F011

---

## Overview

The Professional Referral Dashboard is a comprehensive interface that allows service professionals to manage, track, and send customer referrals to other professionals within the platform ecosystem. This page serves as the central hub for all professional-to-professional referral activities.

---

## Requirements Mapping

### REQ-RF-F005: Professional Referral Dashboard [Phase 1] [P0]

**Status**: ✅ Implemented

**Acceptance Criteria**:
- ✅ Referrals sent summary
- ✅ Referrals received summary
- ✅ Quick action to send referral
- ⏸️ Network visualization (Phase 5 - deferred)

**Dashboard Sections Implemented**:
1. ✅ Quick Stats
   - Referrals sent this month
   - Referrals received this month
   - Conversion rate
   - Active referrals

2. ✅ Sent Referrals Panel
   - Recent sent referrals
   - Status of each
   - Quick follow-up actions

3. ✅ Received Referrals Panel
   - Pending acceptance
   - Recently accepted
   - Quick accept/decline

4. ✅ Quick Actions
   - "Send New Referral" button
   - "View All Sent" link
   - "View All Received" link

### REQ-RF-F006: Send Professional Referral Form [Phase 1] [P0]

**Status**: ✅ Implemented

**Acceptance Criteria**:
- ✅ Select target professional
- ✅ Select customer from list
- ✅ Add referral details
- ✅ Optional discount offer
- ✅ Confirmation before send

**Form Fields Implemented**:

1. **Target Professional**
   - Searchable dropdown
   - Filter by specialty/type
   - Shows professional specialty in dropdown

2. **Customer Selection**
   - Search my customers
   - Customer summary with vehicle info
   - Easy identification

3. **Referral Details**
   - Service Needed (required)
   - Reason for Referral (required)
   - Priority (normal/high/urgent)
   - Internal Notes (optional)

4. **Discount Options**
   - Enable discount toggle
   - Discount type (percentage/fixed)
   - Discount value input
   - Discount expiration date

5. **Preview & Confirm**
   - Form validation
   - Submit button
   - Cancel option

### REQ-RF-F011: Sent Referrals Tracking [Phase 1] [P0]

**Status**: ✅ Implemented

**Acceptance Criteria**:
- ✅ List of sent referrals
- ✅ Status tracking
- ✅ Follow-up reminders (UI ready)
- ✅ Resend option for declined

**Status Tracking Implemented**:
- ✅ Pending acceptance (Yellow badge)
- ✅ Accepted (Green badge with date)
- ✅ Declined (Red badge with reason UI)
- ⏸️ Completed (Phase 2 - with feedback)

---

## Page Structure

### Layout

```
┌─────────────────────────────────────────────────────────────┐
│                     Top Navigation Bar                      │
│  [☰] Referral Dashboard                          [🔔] [👤] │
├──────────┬──────────────────────────────────────────────────┤
│          │                                                  │
│          │  Page Header                                     │
│          │  ┌────────────────────────────────────────────┐  │
│ Sidebar  │  │ Subtitle + [Send New Referral Button]     │  │
│          │  └────────────────────────────────────────────┘  │
│ Nav      │                                                  │
│          │  Quick Stats (4 cards)                          │
│ - Main   │  ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐              │
│ - Refer  │  │Sent │ │Recv │ │Conv │ │Actv │              │
│   - Dash │  └─────┘ └─────┘ └─────┘ └─────┘              │
│   - Recv │                                                  │
│ - Acct   │  Referral Panels (2 columns on desktop)        │
│          │  ┌───────────────┐ ┌───────────────┐           │
│ [User]   │  │ Sent          │ │ Received      │           │
│          │  │ Referrals     │ │ Referrals     │           │
│          │  │               │ │               │           │
│          │  │ [List items]  │ │ [List items]  │           │
│          │  └───────────────┘ └───────────────┘           │
│          │                                                  │
│          │  Quick Actions Panel                            │
│          │  ┌──────────────────────────────────────────┐   │
│          │  │ [Send] [View All Sent] [View All Recv]  │   │
│          │  └──────────────────────────────────────────┘   │
└──────────┴──────────────────────────────────────────────────┘
```

### Responsive Behavior

| Breakpoint | Layout | Sidebar | Panels |
|------------|--------|---------|--------|
| Mobile (< 768px) | Single column | Hidden (hamburger) | Stacked |
| Tablet (768-991px) | Single column | Hidden (hamburger) | Stacked |
| Desktop (≥ 992px) | Two column | Visible | Side-by-side |

---

## Component Specifications

### 1. Page Header

**Location**: Top of page content area
**Components**:
- Title: "Referral Dashboard"
- Subtitle: "Manage and track your professional referrals"
- Primary CTA: "Send New Referral" button (accent yellow)

**Behavior**:
- Button opens Send Referral Modal
- Mobile: Title smaller, button stacks below on very small screens

---

### 2. Quick Stats Cards

**Layout**: 4 cards in responsive grid (auto-fit, min 240px)

#### Card 1: Sent This Month
- **Icon**: Send arrow (blue)
- **Value**: Number of referrals sent
- **Trend**: Percentage vs last month with arrow indicator
- **Color**: Blue theme

#### Card 2: Received This Month
- **Icon**: Receive/download arrow (green)
- **Value**: Number of referrals received
- **Trend**: Percentage vs last month
- **Color**: Green theme

#### Card 3: Conversion Rate
- **Icon**: Chart/graph (yellow)
- **Value**: Percentage of accepted referrals
- **Trend**: Change vs last month
- **Color**: Yellow/gold theme

#### Card 4: Active Referrals
- **Icon**: Clock (purple)
- **Value**: Currently pending count
- **Footer**: "Currently pending response"
- **Color**: Purple theme

**Interactions**:
- Hover: Elevate with shadow increase
- Click: Navigate to detailed stats (future)

---

### 3. Sent Referrals Panel

**Layout**: Card with header, scrollable list, footer

**Header**:
- Title: "Sent Referrals"
- Count badge (blue)
- Filter tabs: All | Pending | Accepted

**List Items**:
Each referral displays:
- Customer name (title)
- Target professional (subtitle)
- Status badge (color-coded)
- Service type with icon
- Discount badge (if applicable)
- Date sent
- Action buttons (View Details, Follow Up, etc.)

**Status Indicators**:
- 🟢 **Accepted**: Green left border, green badge
- 🟡 **Pending**: Yellow left border, yellow badge
- 🔴 **Declined**: Red left border, red badge, opacity reduced

**Interactions**:
- Hover: Background color change
- Click item: Navigate to detail view
- Filter tabs: Show/hide items by status
- Action buttons: Context-specific actions

**Footer**:
- Link: "View All Sent Referrals →"

**Scroll Behavior**:
- Max height: 600px
- Overflow-y: auto
- Smooth scrolling

---

### 4. Received Referrals Panel

**Layout**: Card with header, scrollable list, footer

**Header**:
- Title: "Received Referrals"
- Count badge (blue)
- Filter tabs: Pending | Accepted

**List Items**:
Each referral displays:
- Customer name (title)
- Source professional (subtitle)
- Priority badge
- Service type with icon
- Discount badge (if offered)
- Date received
- Action buttons (Accept, Decline, View Details, etc.)

**Priority Indicators**:
- 🔴 **High Priority**: Primary blue badge
- ⚪ **Normal**: Secondary gray badge
- 🔴 **Urgent**: Red badge (future)

**Interactions**:
- Hover: Background color change
- Accept button: Primary action, show confirmation
- Decline button: Ghost style, show decline modal
- View Details: Navigate to detail view
- Contact Customer: Open communication interface

**Footer**:
- Link: "View All Received Referrals →"

---

### 5. Quick Actions Panel

**Layout**: Card with title and action grid

**Actions**:

1. **Send New Referral**
   - Icon: Send arrow (accent yellow background)
   - Title: "Send New Referral"
   - Description: "Refer a customer to another professional"
   - Action: Opens Send Referral Modal

2. **View All Sent**
   - Icon: Document/list (primary blue background)
   - Title: "View All Sent"
   - Description: "See complete history of sent referrals"
   - Action: Navigate to sent referrals list page

3. **View All Received**
   - Icon: Download/receive (primary blue background)
   - Title: "View All Received"
   - Description: "Manage all incoming referrals"
   - Action: Navigate to received referrals list page

**Interactions**:
- Hover: Border appears, background lightens
- Click: Execute associated action

---

### 6. Send Referral Modal

**Trigger**: "Send New Referral" button/link
**Size**: 600px max width, responsive
**Overlay**: Dark semi-transparent backdrop

**Modal Structure**:

#### Header
- Title: "Send New Referral"
- Close button (X icon)

#### Body (Form Fields)

1. **Target Professional** (Required)
   - Type: Searchable select dropdown
   - Options: Professional name + specialty
   - Helper text: "Search by name or specialty"
   - Validation: Required field

2. **Customer** (Required)
   - Type: Select dropdown
   - Options: Customer name + vehicle info
   - Helper text: "Customer's vehicle info will be included"
   - Validation: Required field

3. **Service Needed** (Required)
   - Type: Text input
   - Placeholder: "e.g., Paint & Body Work"
   - Validation: Required, min 3 characters

4. **Reason for Referral** (Required)
   - Type: Textarea
   - Placeholder: "Explain why you're referring this customer..."
   - Helper text: "Help the receiving professional understand the customer's needs"
   - Validation: Required, min 10 characters

5. **Priority** (Required)
   - Type: Select dropdown
   - Options: Normal | High Priority | Urgent
   - Default: Normal

6. **Internal Notes** (Optional)
   - Type: Textarea
   - Placeholder: "Private notes for your records only..."
   - Helper text: "These notes will not be shared with the professional or customer"

7. **Offer Discount** (Optional)
   - Type: Toggle switch
   - Label: "Offer Discount"
   - Description: "Provide a discount to incentivize the customer"
   - Default: Off

8. **Discount Options** (Conditional - shown when toggle enabled)
   - **Discount Type**: Percentage (%) | Fixed Amount ($)
   - **Discount Value**: Number input
   - **Discount Expiration**: Date picker

#### Footer
- Cancel button (ghost style)
- Send Referral button (primary blue)

**Behavior**:
- Form validation on submit
- Close on backdrop click
- Close on ESC key
- Close on X button click
- Clear form on successful submit
- Show success toast notification
- Animate discount options in/out

**Validation Rules**:
- All required fields must be filled
- Discount value must be > 0 if enabled
- Expiration date must be future date if set

---

## Design Tokens Used

### Colors

| Element | Token | Value |
|---------|-------|-------|
| Primary brand | `--color-blue-700` | #00529F |
| Accent | `--color-yellow-700` | #FFD520 |
| Success | `--color-success` | #2E7D32 |
| Warning | `--color-warning` | #ED6C02 |
| Error | `--color-error` | #D32F2F |
| Background primary | `--color-bg-primary` | #FFFFFF |
| Background secondary | `--color-bg-secondary` | #FAFAFA |
| Text primary | `--color-gray-900` | #1A1A1A |
| Text secondary | `--color-gray-600` | #666666 |
| Border | `--color-border-light` | #E5E5E5 |

### Typography

| Element | Font Family | Size | Weight |
|---------|-------------|------|--------|
| Page title | Montserrat | 28px (1.75rem) | 700 |
| Card titles | Montserrat | 20px (1.25rem) | 600 |
| Stat values | Montserrat | 36px (2.25rem) | 700 |
| Body text | Lato | 16px (1rem) | 400 |
| Small text | Lato | 14px (0.875rem) | 400 |
| Captions | Lato | 12px (0.75rem) | 400 |

### Spacing

| Element | Token | Value |
|---------|-------|-------|
| Section margin | `--spacing-6` | 24px |
| Card padding | `--spacing-6` | 24px |
| Item padding | `--spacing-4` | 16px |
| Gap between elements | `--spacing-3` | 12px |
| Button padding | `--spacing-3` `--spacing-6` | 12px 24px |

### Border Radius

| Element | Token | Value |
|---------|-------|-------|
| Cards | `--radius-lg` | 12px |
| Buttons | `--radius-md` | 8px |
| Badges | `--radius-full` | 9999px |
| Inputs | `--radius-md` | 8px |

### Shadows

| Element | Token | Value |
|---------|-------|-------|
| Cards default | `--shadow-sm` | 0 1px 3px rgba(0,0,0,0.1) |
| Cards hover | `--shadow-md` | 0 4px 6px rgba(0,0,0,0.1) |
| Modal | `--shadow-xl` | 0 20px 25px rgba(0,0,0,0.1) |
| Focus ring | `--shadow-focus` | 0 0 0 3px rgba(0,82,159,0.4) |

---

## Interactive States

### Buttons

| State | Background | Border | Text | Shadow |
|-------|------------|--------|------|--------|
| Default | Blue 700 | None | White | None |
| Hover | Blue 800 | None | White | None |
| Active | Blue 900 | None | White | None |
| Focus | Blue 700 | None | White | Focus ring |
| Disabled | Gray 300 | None | Gray 500 | None |

### Cards

| State | Background | Border | Shadow | Transform |
|-------|------------|--------|--------|-----------|
| Default | White | None | sm | None |
| Hover | White | None | md | translateY(-2px) |

### Form Inputs

| State | Border | Background | Shadow |
|-------|--------|------------|--------|
| Default | Gray 400 | White | None |
| Focus | Blue 700 | White | Focus ring |
| Error | Error | White | Error ring |
| Disabled | Gray 300 | Gray 100 | None |

---

## Accessibility

### WCAG 2.1 AA Compliance

**Color Contrast**:
- ✅ All text meets 4.5:1 ratio minimum
- ✅ Large text meets 3:1 ratio minimum
- ✅ UI components meet 3:1 ratio

**Keyboard Navigation**:
- ✅ All interactive elements accessible via keyboard
- ✅ Logical tab order
- ✅ Focus indicators visible
- ✅ Modal traps focus
- ✅ ESC closes modal

**Screen Reader Support**:
- ✅ Semantic HTML structure
- ✅ ARIA labels on icon buttons
- ✅ Form labels properly associated
- ✅ Error messages announced
- ✅ Status changes announced (future)

**Focus Management**:
- ✅ Focus visible on all interactive elements
- ✅ Focus ring: 3px blue outline
- ✅ Skip links provided (in full implementation)
- ✅ Modal auto-focuses first input

---

## Data Requirements

### API Endpoints (Future Implementation)

#### GET /api/professionals/referrals/stats
Returns quick stats for dashboard

**Response**:
```json
{
  "sentThisMonth": 18,
  "receivedThisMonth": 12,
  "conversionRate": 78,
  "activeReferrals": 23,
  "trends": {
    "sent": 22,
    "received": 15,
    "conversion": 5
  }
}
```

#### GET /api/professionals/referrals/sent
Returns paginated list of sent referrals

**Query Parameters**:
- `page` (number)
- `limit` (number)
- `status` (pending|accepted|declined|all)
- `sortBy` (date|status|customer)
- `order` (asc|desc)

**Response**:
```json
{
  "data": [
    {
      "id": "ref_123",
      "customer": {
        "id": "cust_456",
        "name": "Sarah Williams",
        "vehicle": "2020 Honda Accord"
      },
      "targetProfessional": {
        "id": "pro_789",
        "name": "Elite Auto Body",
        "specialty": "Paint & Body Work"
      },
      "service": "Paint & Body Work",
      "status": "accepted",
      "priority": "normal",
      "discount": {
        "type": "percentage",
        "value": 10,
        "expiration": "2026-02-15"
      },
      "createdAt": "2026-01-06T10:30:00Z",
      "updatedAt": "2026-01-06T14:20:00Z"
    }
  ],
  "pagination": {
    "total": 18,
    "page": 1,
    "limit": 10,
    "pages": 2
  }
}
```

#### GET /api/professionals/referrals/received
Returns paginated list of received referrals

**Query Parameters**:
- `page` (number)
- `limit` (number)
- `status` (pending|accepted|declined|all)
- `priority` (normal|high|urgent|all)

**Response**:
```json
{
  "data": [
    {
      "id": "ref_321",
      "customer": {
        "id": "cust_654",
        "name": "Emma Davis",
        "phone": "(555) 123-4567",
        "email": "emma.davis@email.com",
        "vehicle": {
          "make": "Toyota",
          "model": "Camry",
          "year": 2019,
          "vin": "1HGBH41JXMN109186"
        }
      },
      "sourceProfessional": {
        "id": "pro_987",
        "name": "Joe's Auto Repair",
        "businessName": "Joe's Auto Repair",
        "phone": "(555) 987-6543"
      },
      "service": "Brake Inspection",
      "reason": "Customer needs brake inspection and possible replacement",
      "status": "pending",
      "priority": "high",
      "discount": {
        "type": "percentage",
        "value": 10,
        "expiration": "2026-02-01"
      },
      "createdAt": "2026-01-08T08:00:00Z"
    }
  ],
  "pagination": {
    "total": 5,
    "page": 1,
    "limit": 10,
    "pages": 1
  }
}
```

#### POST /api/professionals/referrals
Create new referral

**Request Body**:
```json
{
  "targetProfessionalId": "pro_789",
  "customerId": "cust_456",
  "service": "Paint & Body Work",
  "reason": "Customer needs front bumper repaired after minor collision",
  "priority": "normal",
  "internalNotes": "Customer is very particular about quality",
  "discount": {
    "enabled": true,
    "type": "percentage",
    "value": 10,
    "expiration": "2026-02-15"
  }
}
```

**Response**:
```json
{
  "id": "ref_123",
  "status": "pending",
  "createdAt": "2026-01-08T10:00:00Z",
  "message": "Referral sent successfully"
}
```

#### POST /api/professionals/referrals/:id/accept
Accept a received referral

**Response**:
```json
{
  "id": "ref_321",
  "status": "accepted",
  "acceptedAt": "2026-01-08T11:00:00Z",
  "message": "Referral accepted successfully"
}
```

#### POST /api/professionals/referrals/:id/decline
Decline a received referral

**Request Body**:
```json
{
  "reason": "too-busy",
  "message": "Currently at capacity, unable to take new customers"
}
```

**Response**:
```json
{
  "id": "ref_321",
  "status": "declined",
  "declinedAt": "2026-01-08T11:00:00Z",
  "message": "Referral declined"
}
```

---

## Performance Considerations

### Loading Strategy
1. **Initial Load**: Load dashboard stats and first 5 items of each panel
2. **Lazy Load**: Load remaining items on scroll
3. **Caching**: Cache stats for 5 minutes
4. **Optimistic Updates**: Update UI immediately, sync with server

### Optimization
- Use pagination for large lists
- Implement virtual scrolling for 100+ items
- Debounce search inputs
- Compress images and icons
- Lazy load modal content

### Target Metrics
- **First Contentful Paint**: < 1.5s
- **Time to Interactive**: < 3.5s
- **Total Page Size**: < 500KB
- **API Response Time**: < 200ms

---

## Error Handling

### API Errors

| Error Code | User Message | Action |
|------------|--------------|--------|
| 400 | "Please check your input and try again" | Show field errors |
| 401 | "Please log in to continue" | Redirect to login |
| 403 | "You don't have permission to perform this action" | Show error message |
| 404 | "Referral not found" | Show error state |
| 500 | "Something went wrong. Please try again" | Show retry option |

### Validation Errors

- Show inline error messages below fields
- Highlight invalid fields with red border
- Prevent form submission until valid
- Clear errors on input change

### Network Errors

- Show offline indicator
- Queue actions for retry
- Notify user when back online
- Don't lose form data

---

## Browser Support

### Desktop
- ✅ Chrome 90+
- ✅ Firefox 88+
- ✅ Safari 14+
- ✅ Edge 90+

### Mobile
- ✅ iOS Safari 14+
- ✅ Chrome Android 90+
- ✅ Samsung Internet 14+

### Graceful Degradation
- CSS Grid with flexbox fallback
- Modern features with polyfills
- Progressive enhancement approach

---

## Testing Requirements

### Unit Tests
- ✅ Form validation logic
- ✅ Data filtering functions
- ✅ Status badge rendering
- ✅ Discount calculation

### Integration Tests
- ✅ Modal open/close behavior
- ✅ Form submission flow
- ✅ Filter tab switching
- ✅ Pagination

### E2E Tests
- ✅ Send new referral flow
- ✅ Accept referral flow
- ✅ Decline referral flow
- ✅ Filter and search

### Accessibility Tests
- ✅ Keyboard navigation
- ✅ Screen reader compatibility
- ✅ Color contrast
- ✅ Focus management

---

## Future Enhancements (Post Phase 1)

### Phase 2
- [ ] Referral completion tracking
- [ ] Feedback system
- [ ] Performance analytics
- [ ] Export referral reports

### Phase 3
- [ ] Referral templates
- [ ] Bulk referral sending
- [ ] Automated follow-ups
- [ ] Integration with calendar

### Phase 4
- [ ] Real-time notifications
- [ ] Chat with professionals
- [ ] Video call integration
- [ ] Mobile app parity

### Phase 5
- [ ] Network visualization
- [ ] AI-powered recommendations
- [ ] Predictive analytics
- [ ] Advanced reporting

---

## Version History

| Version | Date | Changes | Author |
|---------|------|---------|--------|
| 1.0.0 | 2026-01-08 | Initial specification | Design Team |
