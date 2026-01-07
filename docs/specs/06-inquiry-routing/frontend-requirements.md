# Inquiry & Routing - Frontend Requirements

## Feature Overview

User interfaces for submitting service inquiries, tracking inquiry status, and managing inquiries as a professional.

---

## Customer Inquiry Interface

### REQ-IR-F001: Inquiry Submission Form [Phase 2] [P0]
**Description**: Form for customers to submit service inquiries.

**Acceptance Criteria**:
- Multi-step form wizard
- Service type selection
- Vehicle information
- Problem description
- Preferred contact method
- File upload capability

**Form Steps**:
```
Step 1: Service Type
- Visual cards for each service type
- Icon and description for each
- "Not sure" option

Step 2: Vehicle Information
- Make (dropdown with common + other)
- Model (conditional on make)
- Year (dropdown)
- VIN (optional)
- Mileage (optional)

Step 3: Describe the Issue
- Subject line
- Description (textarea)
- Photo upload (optional)
- Voice note (Phase 5)

Step 4: Preferences
- Urgency selection
- Preferred date (date picker)
- Preferred contact method
- Best time to contact

Step 5: Review & Submit
- Summary of inquiry
- Edit buttons for each section
- Terms acceptance
- Submit button
```

---

### REQ-IR-F002: Professional Selection (Optional) [Phase 2] [P1]
**Description**: Allow customers to select specific professional.

**Acceptance Criteria**:
- Toggle: "Let us match you" vs "Choose a professional"
- Professional cards with filters
- Selected professional highlighted
- Proceed without selection allowed

---

### REQ-IR-F003: Inquiry Confirmation Page [Phase 2] [P0]
**Description**: Confirmation after inquiry submission.

**Acceptance Criteria**:
- Confirmation number display
- Summary of inquiry
- Expected response time
- Next steps explanation
- Create account prompt (guest)

---

### REQ-IR-F004: Inquiry Tracking Page [Phase 2] [P0]
**Description**: Track status of submitted inquiries.

**Acceptance Criteria**:
- List of all inquiries
- Status badge for each
- Click for details
- Filter by status

**Inquiry Card**:
```
┌─────────────────────────────────────────────────┐
│ INQ-2024-001234                    [In Progress]│
│ BMW 535i - Check Engine Light                   │
│ Submitted: Jan 5, 2024                          │
│ Assigned to: German Auto Specialists            │
│ Last Update: "We can see you tomorrow..."       │
│                                       [View] →  │
└─────────────────────────────────────────────────┘
```

---

### REQ-IR-F005: Inquiry Detail Page [Phase 2] [P0]
**Description**: Detailed view of inquiry with conversation.

**Acceptance Criteria**:
- Inquiry summary header
- Status timeline
- Message thread
- Reply capability
- Quote display (if sent)
- Accept/decline quote

**Layout**:
```
┌─────────────────────────────────────────────────┐
│ Inquiry #INQ-2024-001234        [In Progress]   │
│ BMW 535i 2018 - Check Engine Light              │
├─────────────────────────────────────────────────┤
│ Status Timeline:                                │
│ ○ Submitted (Jan 5, 10:00 AM)                  │
│ ○ Assigned to Professional (Jan 5, 10:15 AM)   │
│ ● Response Received (Jan 5, 2:30 PM)           │
├─────────────────────────────────────────────────┤
│ Messages:                                       │
│ ┌─ Professional (Jan 5, 2:30 PM)               │
│ │ "Hi, we can diagnose your BMW..."            │
│ └──────────────────────────────────────────────│
│ ┌─ You (Jan 5, 3:00 PM)                        │
│ │ "That works, what time?"                     │
│ └──────────────────────────────────────────────│
├─────────────────────────────────────────────────┤
│ [Reply to Professional]                         │
└─────────────────────────────────────────────────┘
```

---

### REQ-IR-F006: Quote Display and Acceptance [Phase 2] [P1]
**Description**: Display quotes and allow acceptance.

**Quote Card**:
```
┌─────────────────────────────────────────────────┐
│ 💰 QUOTE FROM GERMAN AUTO SPECIALISTS          │
├─────────────────────────────────────────────────┤
│ Diagnostic Fee:                        $150.00  │
│ Estimated Repair (if needed):  $300 - $500.00  │
│                                                 │
│ Valid Until: January 15, 2024                   │
│                                                 │
│ Notes: "Price includes OBD scan and..."        │
│                                                 │
│ [Accept Quote]  [Decline]  [Ask Question]       │
└─────────────────────────────────────────────────┘
```

---

## Professional Interface

### REQ-IR-F007: Inquiry Queue [Phase 2] [P0]
**Description**: Professional's view of assigned and available inquiries.

**Acceptance Criteria**:
- Tabs: Assigned / Available / All
- Priority sorting
- Quick response action
- Claim action for available

**Queue Interface**:
```
Tabs: [My Inquiries (3)] [Available (5)] [History]

┌─────────────────────────────────────────────────┐
│ 🔴 URGENT                     INQ-2024-001235  │
│ Mercedes C300 - Won't Start                     │
│ Customer: John D. | Zip: 12345                  │
│ Submitted: 2 hours ago                          │
│                      [Respond] [View Details]   │
└─────────────────────────────────────────────────┘
```

---

### REQ-IR-F008: Inquiry Detail (Professional View) [Phase 2] [P0]
**Description**: Detailed inquiry view for professionals.

**Acceptance Criteria**:
- Full customer details
- Vehicle information
- Attached photos
- Conversation history
- Response/quote tools
- Refer to colleague option

**Actions Available**:
- Respond (message)
- Send Quote
- Update Status
- Refer to Colleague
- Add Internal Note
- Close Inquiry

---

### REQ-IR-F009: Response Composer [Phase 2] [P0]
**Description**: Interface for composing responses.

**Acceptance Criteria**:
- Rich text editor
- Template insertion
- Attachment support
- Quote builder
- Preview before send

---

### REQ-IR-F010: Quote Builder [Phase 2] [P1]
**Description**: Interface for building and sending quotes.

**Quote Form**:
```
Line Items:
┌─────────────────────────────────────────────────┐
│ Description           │ Quantity │ Price        │
├───────────────────────┼──────────┼──────────────┤
│ Diagnostic Fee        │ 1        │ $150.00      │
│ [+ Add Line Item]                               │
└─────────────────────────────────────────────────┘
Subtotal: $150.00
Tax (if applicable): $12.00
Total: $162.00

Valid Until: [Date Picker]
Notes: [Textarea]

[Save Draft] [Send Quote]
```

---

### REQ-IR-F011: Refer to Colleague Modal [Phase 2] [P1]
**Description**: Interface for referring inquiry to another professional.

**Acceptance Criteria**:
- Search/select colleague
- Add referral notes
- Indicate if discount offered
- Confirm and transfer

---

## Admin Interface

### REQ-IR-F012: Inquiry Admin Dashboard [Phase 2] [P1]
**Description**: Admin overview of all inquiries.

**Dashboard Metrics**:
- Total inquiries today
- Pending assignment
- Average response time
- Conversion rate

**Features**:
- List all inquiries
- Filter by status/professional
- Manual assignment
- Escalation queue

---

### REQ-IR-F013: Routing Rules Manager [Phase 2] [P1]
**Description**: Admin interface for routing configuration.

**Acceptance Criteria**:
- List all routing rules
- Create/edit rules
- Rule priority ordering (drag-drop)
- Test rule matching
- Enable/disable rules

---

## Guest Inquiry Flow

### REQ-IR-F014: Guest Inquiry Form [Phase 2] [P0]
**Description**: Inquiry form for non-logged-in visitors.

**Additional Fields**:
- Name (required)
- Email (required)
- Phone (required)

**Post-Submit**:
- Create account prompt
- Track via email link
- Benefits of account creation

---

## Mobile Requirements

### REQ-IR-F015: Mobile Inquiry Submission [Phase 2] [P0]
**Description**: Mobile-optimized inquiry form.

**Acceptance Criteria**:
- Camera integration for photos
- Voice-to-text for description
- Simplified steps
- Touch-friendly inputs

---

### REQ-IR-F016: Mobile Inquiry Management (Professional) [Phase 2] [P1]
**Description**: Mobile professional inquiry management.

**Acceptance Criteria**:
- Push notifications for new
- Quick response templates
- Swipe actions
- Click-to-call customer
