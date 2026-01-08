# Received Referrals Management - UI Screenshots & States

## Overview
This document provides visual documentation of all UI states for the Received Referrals Management page.

---

## 1. Main Page - Default View

**File:** `mockup.html` (default state)

**Description:**
The main landing page showing the list of received referrals in card format.

**Key Elements:**
- Page header with title "Received Referrals"
- Subtitle: "Manage customer referrals from other professionals"
- Filter bar with tabs (All, Pending, Accepted, Declined) and dropdowns
- Grid of referral cards showing pending referrals
- Sidebar navigation with "Received" highlighted
- Badge showing count of pending referrals (5)

**Visible Referrals:**
1. Michael Rodriguez - URGENT priority (red border)
2. Sarah Thompson - HIGH PRIORITY (orange border)
3. David Chen - NORMAL priority (blue border)
4. Jennifer Martinez - NORMAL priority (blue border)
5. Robert Anderson - HIGH PRIORITY (orange border)

**Screenshot Elements to Capture:**
```
┌─────────────────────────────────────────────────────────────────┐
│ [AutoPro Logo]  Received Referrals               🔔(5)  [Avatar]│
├─────────────────────────────────────────────────────────────────┤
│ [Sidebar]  │  Received Referrals                                │
│ Dashboard  │  Manage customer referrals from other professionals│
│ Customers  │                                                     │
│            │  [All(5)] [Pending(5)] [Accepted(0)] [Declined(0)] │
│ Referrals: │  [All Priorities ▼]  [Sort: Newest First ▼]       │
│ • Send     │                                                     │
│ • Received │  ┌─────────────┐  ┌─────────────┐  ┌────────────┐ │
│   (active) │  │ URGENT      │  │HIGH PRIORITY│  │ NORMAL     │ │
│ • Analytics│  │ Michael R.  │  │ Sarah T.    │  │ David C.   │ │
│            │  │ Elite Motors│  │Quick Oil Chg│  │Premium Auto│ │
│ My Profile │  │ Body repair │  │Paint scratch│  │Dent removal│ │
│            │  │ 15% discount│  │ 10% discount│  │            │ │
│            │  │[View][Accept]  │[View][Accept]  │[View][Accept│
│            │  └─────────────┘  └─────────────┘  └────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. Referral Card - Hover State

**Description:**
Visual feedback when hovering over a referral card.

**Changes:**
- Card elevates 2px upward (translateY -2px)
- Shadow increases from `shadow-sm` to `shadow-md`
- Smooth transition (200ms ease)
- Cursor changes to pointer

**Visual Indicator:**
```
Before Hover:
┌─────────────────────────┐
│ [URGENT]                │  ← shadow-sm
│ Michael Rodriguez       │
│ Elite Motors            │
└─────────────────────────┘

On Hover:
┌─────────────────────────┐
│ [URGENT]                │  ← shadow-md, lifted
│ Michael Rodriguez       │
│ Elite Motors            │
└─────────────────────────┘
```

---

## 3. Referral Detail Modal

**Trigger:** Click "View Details" button on any referral card

**Description:**
Full-screen modal overlay displaying comprehensive referral information.

**Modal Structure:**
```
┌────────────────────────────────────────────────────────────────┐
│ Referral Details                                          [X]  │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│ CUSTOMER INFORMATION                                           │
│ ════════════════════════════════════════════════════════════   │
│ Customer Name: Michael Rodriguez    Phone: (555) 123-4567     │
│ Email: michael.rodriguez@email.com  Preferred: Phone          │
│ [📞 Call Customer]  [✉️ Send Email]                           │
│                                                                │
│ VEHICLE INFORMATION                                            │
│ ════════════════════════════════════════════════════════════   │
│ Make & Model: 2021 Tesla Model 3    VIN: 5YJ3E1EA1MF123456    │
│ Color: Midnight Silver Metallic     Mileage: 15,230 miles     │
│                                                                │
│ REFERRAL INFORMATION                                           │
│ ════════════════════════════════════════════════════════════   │
│ Referred By: Elite Motors           Contact: James Wilson     │
│ Priority: [URGENT]                  Received: Jan 8, 10:30 AM │
│ Service: Complete body repair after collision                 │
│ Discount: 15% off total service cost                          │
│                                                                │
│ NOTES FROM REFERRER                                            │
│ ════════════════════════════════════════════════════════════   │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ Michael was involved in a minor collision last week.     │  │
│ │ The front bumper, hood, and right fender need repair and │  │
│ │ repainting. He's a long-time customer of ours and always │  │
│ │ pays promptly. Very easy to work with. He needs the work │  │
│ │ done ASAP as he has a business trip next week...         │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
├────────────────────────────────────────────────────────────────┤
│                           [Close] [Decline] [Accept Referral] │
└────────────────────────────────────────────────────────────────┘
```

**Key Features:**
- Customer contact information with action buttons
- Complete vehicle details including VIN
- Referrer information and notes
- Visual hierarchy with section titles
- Blue left border on notes section
- Three action buttons in footer

---

## 4. Accept Referral Modal

**Trigger:** Click "Accept" button from card or detail modal

**Description:**
Confirmation modal for accepting a referral with optional details.

**Modal Structure:**
```
┌────────────────────────────────────────────────────────────────┐
│ Accept Referral                                           [X]  │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ Customer: Michael Rodriguez                              │  │
│ │ Service: Complete body repair after collision            │  │
│ │ Referred by: Elite Motors (James Wilson)                 │  │
│ │ Discount Offered: 15% off total service                  │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ☑ I agree to honor the 15% discount offered by the      │  │
│ │   referring professional                                  │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ Message to Referrer (Optional)                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ Thank you for the referral. I'll contact Michael        │  │
│ │ today and get him scheduled...                           │  │
│ └──────────────────────────────────────────────────────────┘  │
│ This message will be sent to Elite Motors to confirm          │
│ acceptance                                                     │
│                                                                │
│ Expected Follow-up Date (Optional)                            │
│ [January 9, 2026        ▼]                                    │
│ When do you plan to contact or schedule this customer?        │
│                                                                │
├────────────────────────────────────────────────────────────────┤
│                                    [Cancel] [Confirm Acceptance│
└────────────────────────────────────────────────────────────────┘
```

**Key Features:**
- Summary section with green background (#E8F5E9)
- Discount honor checkbox with yellow highlight (#FFF2CC)
- Pre-checked by default
- Optional message textarea
- Optional date picker
- Helper text under each field
- Two action buttons: Cancel (ghost) and Confirm (primary)

---

## 5. Decline Referral Modal

**Trigger:** Click "Decline" button from card or detail modal

**Description:**
Modal requiring reason selection for declining a referral.

**Modal Structure:**
```
┌────────────────────────────────────────────────────────────────┐
│ Decline Referral                                          [X]  │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│ Please select a reason for declining this referral. This      │
│ information will be shared with the referring professional.   │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ○ Too busy / No capacity                                 │  │
│ │   Currently at full capacity and unable to take new      │  │
│ │   customers                                               │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ○ Outside service area                                   │  │
│ │   Customer location is outside my service area           │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ○ Not my specialty                                       │  │
│ │   This type of work is outside my area of expertise      │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ○ Customer already known                                 │  │
│ │   This customer is already in my system                  │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ○ Other reason                                           │  │
│ │   Please specify below                                    │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ Additional Details (Optional)                                 │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ Provide additional context or suggest an alternative... │  │
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ Suggest Alternative Professional (Optional)                   │
│ [Select a professional to suggest...              ▼]          │
│ Help the customer by suggesting another professional          │
│                                                                │
├────────────────────────────────────────────────────────────────┤
│                                      [Cancel] [Confirm Decline]│
└────────────────────────────────────────────────────────────────┘
```

**Key Features:**
- Required reason selection (radio buttons)
- Five predefined reason options
- Each option has title and description
- Optional additional details textarea
- Optional alternative professional dropdown
- Helper text for alternative suggestion
- Cancel (ghost) and Confirm Decline (destructive/red) buttons

**Validation:**
- Shows error if user tries to submit without selecting reason
- Additional fields remain optional

---

## 6. Filter States

### 6.1 All Referrals Tab (Active)
**Filter Bar State:**
```
[All (5)] [Pending (5)] [Accepted (0)] [Declined (0)]
  ^^^^
 Active
```
- Shows all referrals regardless of status
- Count displays total number

### 6.2 Pending Tab (Active)
**Filter Bar State:**
```
[All (5)] [Pending (5)] [Accepted (0)] [Declined (0)]
          ^^^^^^^^^^^^^
            Active
```
- Shows only pending referrals
- Default view on page load

### 6.3 Accepted Tab (Active - Empty State)
**Filter Bar State:**
```
[All (5)] [Pending (5)] [Accepted (0)] [Declined (0)]
                        ^^^^^^^^^^^^^^^
                          Active
```

**Empty State Display:**
```
┌────────────────────────────────────────┐
│                                        │
│         ┌───────────────┐              │
│         │   📋         │              │
│         └───────────────┘              │
│                                        │
│   No Accepted Referrals Yet           │
│                                        │
│   Accepted referrals will appear here │
│   once you accept them from the        │
│   pending list.                        │
│                                        │
│   [View Pending Referrals]             │
│                                        │
└────────────────────────────────────────┘
```

### 6.4 Declined Tab (Active - Empty State)
Similar to Accepted empty state with appropriate messaging.

---

## 7. Priority Filter Dropdown

**Filter Bar Dropdown States:**

**Default State:**
```
[All Priorities ▼]
```

**Expanded State:**
```
┌─────────────────┐
│ All Priorities  │ ← Selected
│ Urgent          │
│ High            │
│ Normal          │
└─────────────────┘
```

**After Selecting "Urgent":**
```
[Urgent ▼]  ← Shows only urgent referrals
```

---

## 8. Sort Dropdown

**Default State:**
```
[Sort: Newest First ▼]
```

**Expanded State:**
```
┌─────────────────────┐
│ Sort: Newest First  │ ← Selected
│ Sort: Oldest First  │
│ Sort: Priority      │
└─────────────────────┘
```

---

## 9. Mobile View (< 768px)

**Changes from Desktop:**
- Sidebar hidden (hamburger menu visible)
- Referral cards stack in single column
- Filter bar stacks vertically
- Modals take 95% width
- Reduced padding throughout

**Mobile Layout:**
```
┌─────────────────────────┐
│ ☰  Received Referrals 🔔│
├─────────────────────────┤
│ [Search box            ]│
├─────────────────────────┤
│ [All] [Pending]         │
│ [Accepted] [Declined]   │
│                         │
│ [All Priorities      ▼] │
│ [Sort: Newest First  ▼] │
├─────────────────────────┤
│ ┌─────────────────────┐ │
│ │ [URGENT]            │ │
│ │ Michael Rodriguez   │ │
│ │ Elite Motors        │ │
│ │ Body repair         │ │
│ │ 15% discount        │ │
│ │ [View] [Accept]     │ │
│ └─────────────────────┘ │
│                         │
│ ┌─────────────────────┐ │
│ │ [HIGH]              │ │
│ │ Sarah Thompson      │ │
│ │ Quick Oil Change    │ │
│ └─────────────────────┘ │
└─────────────────────────┘
```

---

## 10. Loading States

### 10.1 Initial Page Load
**Display:**
```
┌────────────────────────────────────┐
│  Received Referrals                │
│                                    │
│  [Filter bar visible but disabled] │
│                                    │
│  ┌────────┐  ┌────────┐  ┌────────┐│
│  │        │  │        │  │        ││  ← Skeleton cards
│  │ ▒▒▒▒   │  │ ▒▒▒▒   │  │ ▒▒▒▒   ││
│  │ ▒▒▒    │  │ ▒▒▒    │  │ ▒▒▒    ││
│  └────────┘  └────────┘  └────────┘│
└────────────────────────────────────┘
```

### 10.2 Detail Modal Loading
**Display:**
```
┌────────────────────────────────────┐
│ Referral Details              [X]  │
├────────────────────────────────────┤
│                                    │
│          ⟳ Loading...              │
│                                    │
└────────────────────────────────────┘
```

### 10.3 Action Processing
**Display:** Button shows spinner while processing
```
[⟳ Accepting...]  ← Disabled during processing
```

---

## 11. Success States

### 11.1 Acceptance Success
**Toast Notification:**
```
┌────────────────────────────────────┐
│ ✓ Referral accepted successfully!  │
│   Elite Motors and Michael         │
│   Rodriguez have been notified.    │
└────────────────────────────────────┘
```
- Green background (#E8F5E9)
- Check icon
- Auto-dismiss after 5 seconds
- Position: Top-right corner

### 11.2 Decline Success
**Toast Notification:**
```
┌────────────────────────────────────┐
│ ✓ Referral declined                │
│   Elite Motors has been notified   │
│   of your decision.                │
└────────────────────────────────────┘
```
- Yellow background (#FFF3E0)
- Position: Top-right corner

---

## 12. Error States

### 12.1 Network Error
**Alert Display:**
```
┌────────────────────────────────────┐
│ ⚠ Unable to load referrals         │
│   Please check your connection     │
│   and try again.                   │
│                                    │
│   [Retry]                          │
└────────────────────────────────────┘
```
- Red background (#FFEBEE)
- Warning icon
- Retry button

### 12.2 Validation Error (Decline without reason)
**Modal Error State:**
```
┌────────────────────────────────────┐
│ Decline Referral              [X]  │
├────────────────────────────────────┤
│ ⚠ Please select a reason for      │
│   declining this referral.         │
│                                    │
│ [Reason options below...]          │
└────────────────────────────────────┘
```
- Error message at top
- Red border on reason section
- Focus moves to first radio button

---

## 13. Notification Badge

**Sidebar Navigation:**
```
Received
  (5)  ← Yellow badge with count
```

**Updates:**
- Real-time via WebSocket
- Increases when new referral received
- Decreases when referral accepted/declined
- Yellow background (#FFD520)
- Dark text for contrast

---

## 14. Priority Badge Variations

### Urgent Badge
```
[🔴 URGENT]
```
- Background: #FFEBEE (light red)
- Text: #D32F2F (red)
- Border-left on card: #D32F2F

### High Priority Badge
```
[🟠 HIGH PRIORITY]
```
- Background: #FFF3E0 (light orange)
- Text: #ED6C02 (orange)
- Border-left on card: #ED6C02

### Normal Badge
```
[🔵 NORMAL]
```
- Background: #E3F2FD (light blue)
- Text: #00529F (blue)
- Border-left on card: #00529F

---

## 15. Discount Badge Variations

### Percentage Discount
```
┌────────────────────────────────┐
│ ⓘ 15% discount included        │
└────────────────────────────────┘
```
- Background: #FFF2CC (yellow)
- Icon: Info circle
- Text: #B8860B (dark yellow)

### Fixed Amount Discount
```
┌────────────────────────────────┐
│ ⓘ $50 discount included        │
└────────────────────────────────┘
```
- Same styling as percentage

### No Discount
```
(Badge not displayed)
```

---

## UI Component Interaction Matrix

| Component | Hover | Click | Focus | Disabled |
|-----------|-------|-------|-------|----------|
| **Referral Card** | Elevate + Shadow | View Details Modal | Blue outline | Gray overlay |
| **Accept Button** | Darker blue | Accept Modal | Blue outline | Gray, no cursor |
| **Decline Button** | Light gray bg | Decline Modal | Blue outline | Gray, no cursor |
| **View Details** | Darker blue | Detail Modal | Blue outline | Gray, no cursor |
| **Filter Tab** | Light blue bg | Activate filter | Blue outline | N/A |
| **Modal Close (X)** | Gray bg | Close modal | Blue outline | N/A |
| **Checkbox** | Border glow | Toggle | Blue outline | Gray, no cursor |
| **Radio Button** | Border glow | Select | Blue outline | Gray, no cursor |

---

## Accessibility Features Visible in UI

1. **Focus Indicators**
   - 3px blue outline on all focusable elements
   - Visible keyboard navigation path

2. **Color Contrast**
   - All text meets WCAG AA standards
   - Priority badges have sufficient contrast

3. **Touch Targets**
   - All buttons minimum 40px height
   - Adequate spacing between interactive elements

4. **Screen Reader Text**
   - Icon-only buttons have aria-labels
   - Status updates use live regions

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | January 8, 2026 | Initial screenshot documentation |
