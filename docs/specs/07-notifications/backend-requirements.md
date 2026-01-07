# Notifications - Backend Requirements

## Feature Overview

Comprehensive notification system supporting email, SMS, push notifications, and in-app notifications. Also includes newsletter and marketing communication management.

---

## Data Model Requirements

### REQ-NT-B001: Notification Entity [Phase 4] [P0]
**Description**: Core notification data model.

**Data Fields**:
```
Notification {
  id: UUID (PK)
  tenant_id: UUID (FK)

  // Recipient
  recipient_type: enum (customer, professional, admin)
  recipient_id: UUID (FK)

  // Content
  type: enum (see NotificationType)
  title: string
  body: text
  data: JSON (nullable) // Additional structured data

  // Delivery
  channels: string[] // ['email', 'sms', 'push', 'in_app']
  priority: enum (low, normal, high, urgent)

  // Status
  status: enum (pending, sent, delivered, read, failed)
  read_at: timestamp (nullable)

  // Related Entity
  related_entity_type: string (nullable)
  related_entity_id: UUID (nullable)
  action_url: string (nullable)

  created_at: timestamp
  expires_at: timestamp (nullable)
}

// Notification Types
NotificationType {
  // Inquiry
  INQUIRY_RECEIVED
  INQUIRY_ASSIGNED
  INQUIRY_RESPONSE
  INQUIRY_QUOTE

  // Referral
  REFERRAL_RECEIVED
  REFERRAL_SENT
  REFERRAL_ACCEPTED
  REFERRAL_DECLINED
  REFERRAL_CONVERTED
  REFERRAL_REWARD

  // Customer
  CUSTOMER_WELCOME
  CUSTOMER_VERIFICATION

  // Professional
  PROFESSIONAL_WELCOME
  PROFESSIONAL_VERIFIED

  // System
  SYSTEM_ANNOUNCEMENT
  SYSTEM_MAINTENANCE
}
```

---

### REQ-NT-B002: Notification Delivery Log [Phase 4] [P0]
**Description**: Track delivery attempts per channel.

**Data Fields**:
```
NotificationDelivery {
  id: UUID (PK)
  notification_id: UUID (FK)

  channel: enum (email, sms, push, in_app)
  status: enum (pending, sent, delivered, bounced, failed)

  // Delivery Details
  provider: string // sendgrid, twilio, firebase
  provider_message_id: string (nullable)
  sent_at: timestamp (nullable)
  delivered_at: timestamp (nullable)
  failed_at: timestamp (nullable)
  failure_reason: string (nullable)

  // Engagement (email)
  opened_at: timestamp (nullable)
  clicked_at: timestamp (nullable)

  created_at: timestamp
}
```

---

### REQ-NT-B003: Notification Template Entity [Phase 4] [P0]
**Description**: Templates for notification content.

**Data Fields**:
```
NotificationTemplate {
  id: UUID (PK)
  tenant_id: UUID (FK, nullable) // null = platform default

  type: enum (NotificationType)
  channel: enum (email, sms, push)
  locale: string (default: 'en')

  // Content
  subject: string (nullable) // for email
  title: string (nullable) // for push
  body: text // supports {{variables}}
  html_body: text (nullable) // for email

  // Settings
  is_active: boolean (default: true)
  version: integer (default: 1)

  created_at: timestamp
  updated_at: timestamp
}
```

---

### REQ-NT-B004: Notification Preference Entity [Phase 4] [P0]
**Description**: User notification preferences.

**Data Fields**:
```
NotificationPreference {
  id: UUID (PK)
  tenant_id: UUID (FK)

  user_type: enum (customer, professional)
  user_id: UUID (FK)

  // Channel Preferences
  email_enabled: boolean (default: true)
  sms_enabled: boolean (default: true)
  push_enabled: boolean (default: true)

  // Type Preferences (JSON map of type -> enabled)
  type_preferences: JSON

  // Quiet Hours
  quiet_hours_enabled: boolean (default: false)
  quiet_hours_start: time (nullable) // e.g., 22:00
  quiet_hours_end: time (nullable) // e.g., 08:00
  timezone: string (default: 'America/New_York')

  // Marketing
  marketing_email_enabled: boolean (default: false)
  newsletter_enabled: boolean (default: false)

  updated_at: timestamp
}
```

---

### REQ-NT-B005: Newsletter Entity [Phase 4] [P1]
**Description**: Newsletter campaigns.

**Data Fields**:
```
Newsletter {
  id: UUID (PK)
  tenant_id: UUID (FK)

  name: string
  subject: string
  content_html: text
  content_text: text

  // Audience
  audience: enum (all_customers, all_professionals, segment)
  segment_criteria: JSON (nullable)

  // Scheduling
  status: enum (draft, scheduled, sending, sent, cancelled)
  scheduled_at: timestamp (nullable)
  sent_at: timestamp (nullable)

  // Statistics
  total_recipients: integer (default: 0)
  total_sent: integer (default: 0)
  total_opened: integer (default: 0)
  total_clicked: integer (default: 0)

  created_at: timestamp
  created_by: UUID (FK)
  updated_at: timestamp
}
```

---

## API Requirements

### REQ-NT-B006: Notification API [Phase 4] [P0]
**Description**: User notification management.

**Endpoints**:
```
GET    /api/notifications                   - List my notifications
GET    /api/notifications/unread-count      - Get unread count
PUT    /api/notifications/{id}/read         - Mark as read
PUT    /api/notifications/read-all          - Mark all as read
DELETE /api/notifications/{id}              - Dismiss notification

GET    /api/notifications/preferences       - Get preferences
PUT    /api/notifications/preferences       - Update preferences
```

**Acceptance Criteria**:
- Paginated notification list
- Real-time unread count updates
- Respect user preferences
- Soft delete (dismiss)

---

### REQ-NT-B007: Notification Send API (Internal) [Phase 4] [P0]
**Description**: Internal API for sending notifications.

**Endpoints**:
```
POST   /api/internal/notifications/send     - Send notification
POST   /api/internal/notifications/batch    - Send batch notifications
```

**Send Request**:
```json
{
  "recipient_type": "customer",
  "recipient_id": "uuid",
  "type": "REFERRAL_CONVERTED",
  "data": {
    "referrer_name": "John",
    "reward_amount": 25.00
  },
  "channels": ["email", "push", "in_app"]
}
```

---

### REQ-NT-B008: Newsletter API [Phase 4] [P1]
**Description**: Newsletter management.

**Endpoints**:
```
GET    /api/admin/newsletters               - List newsletters
POST   /api/admin/newsletters               - Create newsletter
GET    /api/admin/newsletters/{id}          - Get newsletter
PUT    /api/admin/newsletters/{id}          - Update newsletter
DELETE /api/admin/newsletters/{id}          - Delete draft
POST   /api/admin/newsletters/{id}/schedule - Schedule sending
POST   /api/admin/newsletters/{id}/send     - Send immediately
POST   /api/admin/newsletters/{id}/test     - Send test email
GET    /api/admin/newsletters/{id}/stats    - Get statistics
```

---

### REQ-NT-B009: Template API [Phase 4] [P1]
**Description**: Notification template management.

**Endpoints**:
```
GET    /api/admin/templates                 - List templates
GET    /api/admin/templates/{type}          - Get template by type
PUT    /api/admin/templates/{type}          - Update template
POST   /api/admin/templates/{type}/preview  - Preview with data
POST   /api/admin/templates/{type}/reset    - Reset to default
```

---

## Business Logic Requirements

### REQ-NT-B010: Notification Routing [Phase 4] [P0]
**Description**: Route notifications to appropriate channels.

**Logic**:
1. Check user preferences for enabled channels
2. Check notification type preferences
3. Check quiet hours
4. Apply channel-specific rules
5. Queue for delivery

**Acceptance Criteria**:
- Respect all user preferences
- Quiet hours pause non-urgent
- Urgent notifications bypass quiet hours
- At least one channel for critical notifications

---

### REQ-NT-B011: Template Rendering [Phase 4] [P0]
**Description**: Render notification templates.

**Template Variables**:
```
{{recipient.name}}
{{recipient.email}}
{{tenant.name}}
{{data.*}} - Custom data fields
{{action_url}}
{{unsubscribe_url}}
```

**Acceptance Criteria**:
- Variable substitution
- Fallback for missing variables
- HTML escaping where needed
- Tenant branding injection

---

### REQ-NT-B012: Delivery Queue Processing [Phase 4] [P0]
**Description**: Process notification delivery queue.

**Acceptance Criteria**:
- Async processing via queue
- Retry failed deliveries (max 3)
- Exponential backoff
- Dead letter queue for permanent failures
- Rate limiting per provider

---

### REQ-NT-B013: Email Delivery [Phase 4] [P0]
**Description**: Email notification delivery.

**Acceptance Criteria**:
- HTML and plain text versions
- Unsubscribe link in footer
- Tracking pixels for opens
- Link tracking for clicks
- Bounce handling

---

### REQ-NT-B014: SMS Delivery [Phase 4] [P0]
**Description**: SMS notification delivery.

**Acceptance Criteria**:
- Character limit handling
- Link shortening
- Opt-out handling (STOP)
- Delivery receipts
- Cost optimization (batching)

---

### REQ-NT-B015: Push Notification Delivery [Phase 4] [P0]
**Description**: Push notification delivery.

**Acceptance Criteria**:
- FCM (Firebase) integration
- APNS (Apple) integration
- Device token management
- Badge count updates
- Deep linking

---

### REQ-NT-B016: Real-time In-App Notifications [Phase 4] [P0]
**Description**: Real-time notification delivery.

**Acceptance Criteria**:
- WebSocket connection
- Event-based updates
- Connection state management
- Offline queue
- Reconnection handling

---

## Integration Requirements

### REQ-NT-B017: Email Provider Integration [Phase 4] [P0]
**Description**: Integrate with email service provider.

**Providers** (at least one):
- SendGrid
- AWS SES
- Mailgun
- Postmark

**Acceptance Criteria**:
- Webhook handling for events
- Template sync (optional)
- Reputation monitoring
- Suppression list sync

---

### REQ-NT-B018: SMS Provider Integration [Phase 4] [P0]
**Description**: Integrate with SMS provider.

**Providers** (at least one):
- Twilio
- AWS SNS
- Vonage

**Acceptance Criteria**:
- Two-way SMS support
- Delivery receipts
- Opt-out management
- Number verification

---

### REQ-NT-B019: Push Provider Integration [Phase 4] [P0]
**Description**: Integrate with push notification service.

**Providers**:
- Firebase Cloud Messaging (FCM)
- Apple Push Notification Service (APNS)

**Acceptance Criteria**:
- Token registration
- Topic subscriptions
- Silent push support
- Notification analytics

---

## Security Requirements

### REQ-NT-B020: Notification Security [Phase 4] [P0]
**Description**: Secure notification handling.

**Acceptance Criteria**:
- No PII in push payloads
- Signed unsubscribe links
- Rate limiting per recipient
- Audit logging
- GDPR compliance

---

## Performance Requirements

### REQ-NT-B021: Notification Performance [Phase 4] [P0]
**Description**: High-performance notification delivery.

**Acceptance Criteria**:
- Send 1000+ notifications/minute
- Real-time delivery < 5 seconds
- Queue processing SLA
- Horizontal scaling support
