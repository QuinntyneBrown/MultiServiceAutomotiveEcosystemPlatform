# Referral System - Backend Requirements

## Feature Overview

The referral system is the **core MVP feature** enabling two types of referrals:
1. **Customer Referrals**: Customers refer friends/family to any professional (loyalty reward)
2. **Professional Referrals**: Professionals refer customers to other professionals (B2B tracking)

This system tracks all referral relationships, enables discount programs, and provides the foundation for the loyalty program.

---

## Data Model Requirements

### REQ-RF-B001: Customer Referral Entity [Phase 1] [P0]
**Description**: Track customer-to-customer referrals for the loyalty program.

**Acceptance Criteria**:
- Referrer (existing customer) identified
- Referee (new customer) tracked
- Referral code/link generation
- Status workflow (pending → converted → rewarded)
- Professional association
- Reward tracking

**Data Fields**:
```
CustomerReferral {
  id: UUID (PK)
  tenant_id: UUID (FK)

  // Referrer Information
  referrer_customer_id: UUID (FK)
  referrer_code: string (unique)

  // Referee Information
  referee_customer_id: UUID (FK, nullable) // Set when converted
  referee_email: string (nullable)
  referee_phone: string (nullable)
  referee_name: string (nullable)

  // Target
  target_professional_id: UUID (FK, nullable) // Specific professional or any
  target_service_type: string (nullable)

  // Status
  status: enum (pending, contacted, converted, expired, cancelled)
  converted_at: timestamp (nullable)

  // Reward
  reward_status: enum (pending, approved, paid, cancelled)
  reward_amount: decimal (nullable)
  reward_type: enum (cash, credit, discount)
  reward_paid_at: timestamp (nullable)

  // Tracking
  referral_source: string (nullable) // email, sms, social, direct
  utm_campaign: string (nullable)
  utm_source: string (nullable)

  // Timestamps
  created_at: timestamp
  updated_at: timestamp
  expires_at: timestamp (nullable)
}
```

---

### REQ-RF-B002: Professional Referral Entity [Phase 1] [P0]
**Description**: Track professional-to-professional referrals.

**Acceptance Criteria**:
- Source professional identified
- Target professional identified
- Customer being referred tracked
- Referral reason/notes
- Discount information
- Conversion tracking

**Data Fields**:
```
ProfessionalReferral {
  id: UUID (PK)
  tenant_id: UUID (FK)

  // Parties
  source_professional_id: UUID (FK) // Who is referring
  target_professional_id: UUID (FK) // Who receives the referral
  customer_id: UUID (FK)            // Customer being referred

  // Context
  reason: text (nullable)           // Why the referral
  service_needed: string (nullable) // What service the customer needs
  notes: text (nullable)            // Internal notes
  priority: enum (normal, high, urgent)

  // Status
  status: enum (pending, accepted, declined, completed, expired)
  accepted_at: timestamp (nullable)
  completed_at: timestamp (nullable)
  declined_reason: string (nullable)

  // Discount
  discount_offered: boolean (default: false)
  discount_type: enum (percentage, fixed, none)
  discount_value: decimal (nullable)
  discount_code: string (nullable)
  discount_used: boolean (default: false)

  // Follow-up
  follow_up_required: boolean (default: false)
  follow_up_date: date (nullable)
  follow_up_notes: text (nullable)

  // Timestamps
  created_at: timestamp
  updated_at: timestamp
  expires_at: timestamp (nullable)
}
```

---

### REQ-RF-B003: Referral Code Entity [Phase 1] [P0]
**Description**: Unique referral codes for tracking.

**Acceptance Criteria**:
- Unique codes per customer
- QR code generation support
- Short, memorable codes
- Code expiration support
- Usage tracking

**Data Fields**:
```
ReferralCode {
  id: UUID (PK)
  tenant_id: UUID (FK)

  code: string (unique)
  code_type: enum (customer, professional, campaign)

  // Owner
  customer_id: UUID (FK, nullable)
  professional_id: UUID (FK, nullable)
  campaign_id: UUID (FK, nullable)

  // Configuration
  max_uses: integer (nullable) // null = unlimited
  current_uses: integer (default: 0)
  reward_amount: decimal (nullable)
  discount_percentage: decimal (nullable)

  // Status
  is_active: boolean (default: true)
  expires_at: timestamp (nullable)

  // Timestamps
  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-RF-B004: Referral Statistics Cache [Phase 1] [P0]
**Description**: Pre-calculated referral statistics.

**Data Fields**:
```
ReferralStats {
  id: UUID (PK)
  tenant_id: UUID (FK)

  entity_type: enum (customer, professional)
  entity_id: UUID

  // Customer Stats
  total_referrals_sent: integer
  successful_referrals: integer
  pending_referrals: integer
  total_rewards_earned: decimal
  rewards_pending: decimal

  // Professional Stats (for professional entities)
  referrals_received: integer
  referrals_given: integer
  referral_conversion_rate: decimal
  avg_discount_given: decimal

  // Timestamps
  calculated_at: timestamp
}
```

---

## API Requirements

### REQ-RF-B005: Customer Referral API [Phase 1] [P0]
**Description**: API for customer referral operations.

**Endpoints**:
```
POST   /api/referrals/customer                    - Create customer referral
GET    /api/referrals/customer                    - List my referrals (customer)
GET    /api/referrals/customer/{id}               - Get referral details
PUT    /api/referrals/customer/{id}               - Update referral
DELETE /api/referrals/customer/{id}               - Cancel referral

GET    /api/referrals/customer/stats              - Get my referral stats
GET    /api/referrals/customer/rewards            - Get my rewards
POST   /api/referrals/customer/share              - Generate share link
```

**Create Referral Request**:
```json
{
  "referee_email": "friend@example.com",
  "referee_name": "John Doe",
  "referee_phone": "+1234567890",
  "target_professional_id": "uuid",
  "message": "Personal message to friend"
}
```

**Acceptance Criteria**:
- Customers can only view their own referrals
- Duplicate referee prevention (same email/phone)
- Email/SMS notification to referee
- Referral link generation

---

### REQ-RF-B006: Professional Referral API [Phase 1] [P0]
**Description**: API for professional-to-professional referrals.

**Endpoints**:
```
POST   /api/referrals/professional                - Create professional referral
GET    /api/referrals/professional/sent           - Referrals I've sent
GET    /api/referrals/professional/received       - Referrals I've received
GET    /api/referrals/professional/{id}           - Get referral details
PUT    /api/referrals/professional/{id}           - Update referral
POST   /api/referrals/professional/{id}/accept    - Accept referral
POST   /api/referrals/professional/{id}/decline   - Decline referral
POST   /api/referrals/professional/{id}/complete  - Mark as completed
```

**Create Referral Request**:
```json
{
  "target_professional_id": "uuid",
  "customer_id": "uuid",
  "reason": "Customer needs German specialist for BMW repair",
  "service_needed": "Engine diagnostic",
  "priority": "high",
  "discount_type": "percentage",
  "discount_value": 10,
  "notes": "Customer is a VIP - please prioritize"
}
```

**Acceptance Criteria**:
- Source professional sees customer as "referred by" sender
- Target professional notified immediately
- Customer notified of referral
- Discount automatically applied if configured

---

### REQ-RF-B007: Referral Code API [Phase 1] [P0]
**Description**: API for referral code management.

**Endpoints**:
```
GET    /api/referral-codes/my-code                - Get my referral code
POST   /api/referral-codes/generate               - Generate new code
POST   /api/referral-codes/validate               - Validate a code
GET    /api/referral-codes/{code}/info            - Get code info (public)
```

**Acceptance Criteria**:
- Each customer gets one default code
- Codes are short and memorable (e.g., JOHN2024)
- QR code generation included
- Public validation endpoint (no auth)

---

### REQ-RF-B008: Referral Conversion API [Phase 1] [P0]
**Description**: API for handling referral conversions.

**Endpoints**:
```
POST   /api/referrals/convert                     - Convert referral (internal)
POST   /api/referrals/claim                       - Claim referral by code
```

**Conversion Flow**:
1. New customer signs up with referral code
2. System validates code
3. System links referee to referrer
4. System credits reward to referrer (pending approval)
5. Notifications sent to both parties

**Acceptance Criteria**:
- Conversion only happens once per referee
- Reward pending until first service completed (Phase 3)
- Automatic status updates
- Event published for downstream processing

---

### REQ-RF-B009: Referral Reporting API [Phase 3] [P1]
**Description**: API for referral analytics and reporting.

**Endpoints**:
```
GET    /api/referrals/reports/summary             - Summary statistics
GET    /api/referrals/reports/by-professional     - Stats by professional
GET    /api/referrals/reports/by-period           - Stats by time period
GET    /api/referrals/reports/top-referrers       - Leaderboard
GET    /api/referrals/reports/conversion-funnel   - Funnel analysis
```

**Acceptance Criteria**:
- Role-based access (admin sees all, professional sees own)
- Date range filtering
- Export to CSV/PDF
- Cached for performance

---

## Business Logic Requirements

### REQ-RF-B010: Referral Code Generation [Phase 1] [P0]
**Description**: Generate unique, memorable referral codes.

**Algorithm**:
- Format: {NAME_PREFIX}{RANDOM_SUFFIX}
- Example: JOHN7K2M, SMITH4X9P
- Length: 6-8 characters
- Characters: Uppercase letters + numbers (no ambiguous: 0, O, I, 1, L)

**Acceptance Criteria**:
- Collision detection and retry
- Blacklist offensive combinations
- Customizable codes for VIP (Phase 3)

---

### REQ-RF-B011: Referral Attribution Logic [Phase 1] [P0]
**Description**: Determine referral attribution for new customers.

**Attribution Rules**:
1. Explicit referral code used → attribute to code owner
2. Referral link with tracking → attribute to link owner
3. Email match with pending referral → attribute to referrer
4. Phone match with pending referral → attribute to referrer
5. No match → organic acquisition

**Acceptance Criteria**:
- First attribution wins (no double credit)
- Attribution window configurable (default 30 days)
- Manual attribution by admin (Phase 3)

---

### REQ-RF-B012: Referral Expiration [Phase 1] [P0]
**Description**: Handle referral expiration.

**Acceptance Criteria**:
- Pending referrals expire after configurable period (default 90 days)
- Expired referrals marked as such
- No reward for expired referrals
- Batch job runs daily to expire referrals

---

### REQ-RF-B013: Referral Notification Triggers [Phase 1] [P0]
**Description**: Trigger notifications for referral events.

**Events**:
| Event | Notify |
|-------|--------|
| Customer referral created | Referee (email/SMS) |
| Professional referral created | Target professional, Customer |
| Referral accepted | Source professional, Customer |
| Referral declined | Source professional |
| Referral converted | Referrer |
| Reward approved | Referrer |

**Acceptance Criteria**:
- Notifications queued for async delivery
- Template-based messages
- Opt-out respect

---

### REQ-RF-B014: Discount Code Generation for Referrals [Phase 1] [P0]
**Description**: Generate discount codes for referred customers.

**Acceptance Criteria**:
- Unique discount code per referral
- Discount type configurable (% or fixed)
- Single use enforcement
- Expiration date
- Tracking of usage

---

### REQ-RF-B015: Referral Network Tracking [Phase 1] [P0]
**Description**: Track the network of referral relationships.

**Acceptance Criteria**:
- Source professional sees customer's owner
- Target professional knows referral source
- Customer ownership not transferred on referral
- Referral chain visible (who referred whom)
- Network visualization (Phase 5)

---

### REQ-RF-B016: Anti-Fraud Measures [Phase 2] [P1]
**Description**: Prevent referral fraud.

**Checks**:
- Same IP address for referrer/referee
- Same device fingerprint
- Same household address
- Velocity limits (max referrals per day/week)
- Email domain restrictions

**Acceptance Criteria**:
- Suspicious referrals flagged for review
- Admin approval required for flagged
- Automatic rejection of obvious fraud
- Fraud score calculation (Phase 5)

---

## Integration Requirements

### REQ-RF-B017: Referral Event Publishing [Phase 1] [P0]
**Description**: Publish referral events.

**Events**:
```
referral.customer.created
referral.customer.converted
referral.customer.expired
referral.professional.created
referral.professional.accepted
referral.professional.declined
referral.professional.completed
referral.reward.approved
referral.reward.paid
```

**Acceptance Criteria**:
- Events include full context
- Events trigger notification service
- Events trigger loyalty service (Phase 3)

---

### REQ-RF-B018: SMS/Email Integration [Phase 1] [P0]
**Description**: Send referral invitations.

**Acceptance Criteria**:
- Email invitation with referral link
- SMS invitation with short link
- Personalized messages
- Tracking pixels for open rates (email)
- Short URL service integration

---

## Security Requirements

### REQ-RF-B019: Referral Code Security [Phase 1] [P0]
**Description**: Secure referral code handling.

**Acceptance Criteria**:
- Rate limiting on code validation
- No enumeration of valid codes
- Codes not guessable
- Audit log for code usage

---

### REQ-RF-B020: Referral Data Privacy [Phase 2] [P1]
**Description**: Protect referral data privacy.

**Acceptance Criteria**:
- Referee email/phone visible only to admin
- Referrer sees status, not referee details
- GDPR compliance for referral data
- Right to deletion includes referral data

---

## Performance Requirements

### REQ-RF-B021: Referral Statistics Caching [Phase 1] [P0]
**Description**: Cache referral statistics for performance.

**Acceptance Criteria**:
- Statistics updated on write
- Batch recalculation daily
- API response < 100ms for stats
- Cache invalidation on relevant events
