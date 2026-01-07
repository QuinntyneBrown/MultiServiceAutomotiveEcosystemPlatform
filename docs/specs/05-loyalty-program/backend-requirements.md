# Loyalty Program - Backend Requirements

## Feature Overview

The loyalty program rewards customers for referrals, repeat business, and engagement with the platform. Customers earn rewards (cash, credits, discounts) for successful referrals.

---

## Data Model Requirements

### REQ-LP-B001: Loyalty Account Entity [Phase 3] [P0]
**Description**: Customer loyalty account for tracking rewards.

**Data Fields**:
```
LoyaltyAccount {
  id: UUID (PK)
  tenant_id: UUID (FK)
  customer_id: UUID (FK, unique per tenant)

  // Balance
  available_balance: decimal (default: 0)
  pending_balance: decimal (default: 0)
  lifetime_earned: decimal (default: 0)
  lifetime_redeemed: decimal (default: 0)

  // Tier (Phase 5)
  tier: enum (bronze, silver, gold, platinum)
  tier_points: integer (default: 0)
  tier_expiry: date (nullable)

  // Status
  status: enum (active, suspended, closed)

  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-LP-B002: Loyalty Transaction Entity [Phase 3] [P0]
**Description**: Track all loyalty point/reward transactions.

**Data Fields**:
```
LoyaltyTransaction {
  id: UUID (PK)
  tenant_id: UUID (FK)
  loyalty_account_id: UUID (FK)

  // Transaction Details
  type: enum (earn, redeem, expire, adjust, bonus)
  amount: decimal
  balance_after: decimal

  // Source
  source_type: enum (referral, service, promotion, manual, bonus)
  source_id: UUID (nullable) // referral_id, service_id, etc.

  description: string
  notes: text (nullable)

  // For pending transactions
  status: enum (pending, completed, cancelled, expired)
  pending_until: timestamp (nullable)
  completed_at: timestamp (nullable)

  created_at: timestamp
  created_by: UUID (FK, nullable)
}
```

---

### REQ-LP-B003: Reward Configuration Entity [Phase 3] [P0]
**Description**: Configure reward rules and amounts.

**Data Fields**:
```
RewardConfiguration {
  id: UUID (PK)
  tenant_id: UUID (FK)

  name: string
  type: enum (referral_reward, service_reward, bonus, promotion)

  // Reward Amount
  reward_type: enum (fixed, percentage)
  reward_value: decimal
  max_reward: decimal (nullable)
  min_threshold: decimal (nullable) // min service value to qualify

  // Conditions
  is_active: boolean (default: true)
  start_date: date (nullable)
  end_date: date (nullable)
  max_uses: integer (nullable)
  current_uses: integer (default: 0)

  // Eligibility
  customer_tier_required: enum (nullable) // null = all tiers
  first_time_only: boolean (default: false)

  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-LP-B004: Reward Redemption Entity [Phase 3] [P0]
**Description**: Track reward redemptions.

**Data Fields**:
```
RewardRedemption {
  id: UUID (PK)
  tenant_id: UUID (FK)
  loyalty_account_id: UUID (FK)

  amount: decimal
  redemption_type: enum (cash_out, service_credit, discount_code)

  // For cash out
  payout_method: enum (check, bank_transfer, paypal, venmo)
  payout_details: JSON (encrypted)

  // For discount code
  discount_code: string (nullable)
  discount_expires_at: timestamp (nullable)

  status: enum (pending, processing, completed, failed, cancelled)
  processed_at: timestamp (nullable)

  notes: text (nullable)
  created_at: timestamp
}
```

---

## API Requirements

### REQ-LP-B005: Loyalty Account API [Phase 3] [P0]
**Description**: Customer loyalty account operations.

**Endpoints**:
```
GET    /api/loyalty/account                    - Get my loyalty account
GET    /api/loyalty/balance                    - Get current balance
GET    /api/loyalty/transactions               - List transactions
GET    /api/loyalty/transactions/{id}          - Transaction details
```

**Acceptance Criteria**:
- Auto-create account on first referral reward
- Balance includes available and pending
- Transaction history paginated
- Filter transactions by type/date

---

### REQ-LP-B006: Reward Redemption API [Phase 3] [P0]
**Description**: Redeem loyalty rewards.

**Endpoints**:
```
POST   /api/loyalty/redeem                     - Request redemption
GET    /api/loyalty/redemptions                - List redemptions
GET    /api/loyalty/redemptions/{id}           - Redemption status
DELETE /api/loyalty/redemptions/{id}           - Cancel pending redemption
```

**Redemption Request**:
```json
{
  "amount": 50.00,
  "redemption_type": "cash_out",
  "payout_method": "paypal",
  "payout_details": {
    "email": "customer@example.com"
  }
}
```

**Acceptance Criteria**:
- Minimum redemption amount enforced
- Balance validation
- Payout processing queue
- Status tracking

---

### REQ-LP-B007: Admin Loyalty API [Phase 3] [P1]
**Description**: Admin management of loyalty program.

**Endpoints**:
```
GET    /api/admin/loyalty/accounts             - List all accounts
GET    /api/admin/loyalty/accounts/{id}        - Account details
POST   /api/admin/loyalty/adjust               - Manual adjustment
GET    /api/admin/loyalty/redemptions          - All redemption requests
PUT    /api/admin/loyalty/redemptions/{id}     - Process redemption
GET    /api/admin/loyalty/config               - Get reward config
PUT    /api/admin/loyalty/config               - Update reward config
```

---

## Business Logic Requirements

### REQ-LP-B008: Referral Reward Calculation [Phase 3] [P0]
**Description**: Calculate rewards for successful referrals.

**Rules**:
- Fixed amount per successful referral (configurable, default $25)
- Bonus for first referral (configurable, default $10 extra)
- Tier multipliers (Phase 5)
- Cap on monthly rewards (optional)

**Acceptance Criteria**:
- Reward credited as pending initially
- Reward confirmed after referee completes first service
- Configurable reward amounts per tenant

---

### REQ-LP-B009: Pending Reward Processing [Phase 3] [P0]
**Description**: Process pending rewards to available balance.

**Trigger Conditions**:
- Referee completes first service
- Configurable waiting period (default 30 days)
- No fraud flags

**Acceptance Criteria**:
- Batch processing job
- Notification on reward confirmation
- Audit trail

---

### REQ-LP-B010: Reward Expiration [Phase 5] [P2]
**Description**: Expire unused rewards.

**Acceptance Criteria**:
- Configurable expiration period (default 12 months)
- Warning notifications before expiry
- Expired rewards logged
- No expiration for tier status (optional)

---

## Integration Requirements

### REQ-LP-B011: Payment Provider Integration [Phase 3] [P1]
**Description**: Integrate with payment providers for payouts.

**Providers**:
- PayPal
- Venmo
- Bank transfer (ACH)
- Check printing service

**Acceptance Criteria**:
- Secure credential storage
- Transaction logging
- Failure handling and retry
- Reconciliation support

---

## Security Requirements

### REQ-LP-B012: Loyalty Data Security [Phase 3] [P0]
**Description**: Secure loyalty account data.

**Acceptance Criteria**:
- Payout details encrypted
- Balance modifications audited
- Manual adjustments require approval
- Rate limiting on redemptions
