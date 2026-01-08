# Share Referral Interface Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-RF-F002` |
| **Component Name** | ShareReferralInterface |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Modal Component |

## Description

A comprehensive modal interface that enables users to share their referral links through multiple channels including direct link copying, email, SMS, social media platforms, and QR codes. The component provides a unified, tab-based interface for all sharing methods with real-time previews and success feedback.

## User Stories

1. **As a customer**, I want to easily share my referral link, so I can invite friends to the platform.
2. **As a professional**, I want to share my referral code via email, so I can reach multiple colleagues at once.
3. **As a mobile user**, I want to generate a QR code, so others can scan it instantly.
4. **As a social media user**, I want one-click social sharing, so I can promote the platform on my networks.
5. **As a user**, I want to see a preview of my message, so I know what recipients will receive.

## Component Structure

```
ShareReferralInterface/
├── index.ts                           # Public exports
├── ShareReferralInterface.tsx         # Main modal component (React)
├── ShareReferralInterface.component.ts # Main component (Angular)
├── ShareReferralInterface.module.scss # Styles
├── ShareReferralInterface.test.tsx    # Unit tests
├── hooks/
│   ├── useShareReferral.ts            # Share state management
│   ├── useQRCode.ts                   # QR code generation
│   └── useClipboard.ts                # Clipboard operations
├── components/
│   ├── CopyLinkTab/                   # Copy link section
│   ├── EmailShareTab/                 # Email sharing form
│   ├── SMSShareTab/                   # SMS sharing form
│   ├── SocialShareTab/                # Social media buttons
│   ├── QRCodeTab/                     # QR code display
│   ├── MessagePreview/                # Message preview component
│   └── SuccessToast/                  # Success notification
├── services/
│   ├── shareService.ts                # Share API calls
│   ├── qrcodeService.ts               # QR code generation service
│   └── analyticsService.ts            # Share tracking
└── utils/
    ├── shareUrlBuilder.ts             # URL construction
    └── validators.ts                  # Email/phone validation
```

## Props / Inputs

### React

```typescript
interface ShareReferralInterfaceProps {
  /** User's unique referral code */
  referralCode: string;

  /** Full referral URL (auto-generated if not provided) */
  referralUrl?: string;

  /** User information for personalization */
  user: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
  };

  /** Default tab to display */
  defaultTab?: 'copy-link' | 'email' | 'sms' | 'social' | 'qr-code';

  /** Available share methods */
  enabledMethods?: ShareMethod[];

  /** Pre-filled message template */
  defaultMessage?: string;

  /** Callback when modal is closed */
  onClose: () => void;

  /** Callback when share is successful */
  onShareSuccess?: (method: ShareMethod, metadata: ShareMetadata) => void;

  /** Callback when share fails */
  onShareError?: (method: ShareMethod, error: ShareError) => void;

  /** Custom branding/styling */
  branding?: {
    logo?: string;
    accentColor?: string;
    discount?: string;
  };

  /** Analytics tracking enabled */
  enableAnalytics?: boolean;
}

type ShareMethod = 'copy-link' | 'email' | 'sms' | 'facebook' | 'twitter' | 'whatsapp' | 'linkedin' | 'qr-code';

interface ShareMetadata {
  timestamp: Date;
  recipientCount?: number;
  platform?: string;
}

interface ShareError {
  code: 'NETWORK_ERROR' | 'INVALID_EMAIL' | 'INVALID_PHONE' | 'RATE_LIMIT' | 'SERVICE_UNAVAILABLE';
  message: string;
}
```

### Angular

```typescript
@Component({
  selector: 'app-share-referral-interface',
  templateUrl: './share-referral-interface.component.html',
  styleUrls: ['./share-referral-interface.component.scss']
})
export class ShareReferralInterfaceComponent {
  @Input() referralCode!: string;
  @Input() referralUrl?: string;
  @Input() user!: UserInfo;
  @Input() defaultTab: ShareMethod = 'copy-link';
  @Input() enabledMethods: ShareMethod[] = ['copy-link', 'email', 'sms', 'social', 'qr-code'];
  @Input() defaultMessage?: string;
  @Input() branding?: BrandingOptions;
  @Input() enableAnalytics = true;

  @Output() closeModal = new EventEmitter<void>();
  @Output() shareSuccess = new EventEmitter<ShareSuccessEvent>();
  @Output() shareError = new EventEmitter<ShareErrorEvent>();
}
```

## Tab Configurations

### 1. Copy Link Tab

**Purpose**: Quick one-click link copying with visual feedback

```typescript
interface CopyLinkTabState {
  referralUrl: string;
  isCopied: boolean;
  showToast: boolean;
}

interface CopyLinkTabProps {
  referralUrl: string;
  onCopy: () => void;
}
```

**Features**:
- Read-only URL input field
- One-click copy button
- Success toast notification (3-second duration)
- Auto-select URL on input click

**Behavior**:
1. User clicks "Copy Link" button
2. URL copied to clipboard
3. Success toast appears
4. Analytics event fired
5. Toast auto-dismisses after 3 seconds

---

### 2. Email Share Tab

**Purpose**: Send referral link via email with personalized message

```typescript
interface EmailShareTabState {
  recipients: string[];
  subject: string;
  message: string;
  isValid: boolean;
  isSending: boolean;
  errors: {
    recipients?: string;
    subject?: string;
    message?: string;
  };
}

interface EmailShareTabProps {
  defaultMessage: string;
  referralUrl: string;
  onSend: (data: EmailShareData) => Promise<void>;
}

interface EmailShareData {
  recipients: string[];
  subject: string;
  message: string;
  referralUrl: string;
}
```

**Form Fields**:

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| recipients | text | Yes | Valid email format, comma-separated |
| subject | text | No | Auto-generated from template |
| message | textarea | No | Max 2000 characters |

**Features**:
- Multi-recipient support (comma-separated)
- Email validation with inline errors
- Character counter for message
- Live message preview
- Auto-append referral link to message
- Pre-filled default message

**Default Message Template**:
```
Hey! I've been using AutoService Platform for all my automotive needs and thought you might be interested. Use my referral link to get [DISCOUNT]% off your first service!

[REFERRAL_URL]
```

---

### 3. SMS Share Tab

**Purpose**: Send referral link via SMS with character counting

```typescript
interface SMSShareTabState {
  phoneNumber: string;
  message: string;
  characterCount: number;
  isValid: boolean;
  isSending: boolean;
  errors: {
    phoneNumber?: string;
    message?: string;
  };
}

interface SMSShareTabProps {
  defaultMessage: string;
  referralUrl: string;
  maxLength?: number;
  onSend: (data: SMSShareData) => Promise<void>;
}

interface SMSShareData {
  phoneNumber: string;
  message: string;
}
```

**Form Fields**:

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| phoneNumber | tel | Yes | Valid phone format (E.164) |
| message | textarea | Yes | Max 160 characters (SMS limit) |

**Features**:
- Phone number formatting (auto-format as user types)
- Real-time character counter
- SMS length indicator (160/320 char limits)
- Multi-segment warning (if > 160 chars)
- Live message preview
- International format support

**Default SMS Template**:
```
Check out AutoService Platform! Get [DISCOUNT]% off: [SHORT_URL]
```

---

### 4. Social Share Tab

**Purpose**: One-click sharing to social media platforms

```typescript
interface SocialShareTabProps {
  referralUrl: string;
  message: string;
  platforms: SocialPlatform[];
  onShare: (platform: SocialPlatform) => void;
}

type SocialPlatform = 'facebook' | 'twitter' | 'whatsapp' | 'linkedin';

interface SocialShareConfig {
  facebook: {
    url: string;
    quote: string;
  };
  twitter: {
    url: string;
    text: string;
    hashtags?: string[];
  };
  whatsapp: {
    text: string;
  };
  linkedin: {
    url: string;
    title: string;
    summary: string;
  };
}
```

**Supported Platforms**:

| Platform | Share Method | Parameters |
|----------|--------------|------------|
| **Facebook** | Share Dialog | url, quote |
| **Twitter/X** | Web Intent | text, url, hashtags |
| **WhatsApp** | URL Scheme | text (includes URL) |
| **LinkedIn** | Share URL | url, title, summary |

**Share URL Generation**:

```typescript
const socialShareUrls = {
  facebook: (url: string, quote: string) =>
    `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}&quote=${encodeURIComponent(quote)}`,

  twitter: (url: string, text: string, hashtags?: string[]) =>
    `https://twitter.com/intent/tweet?url=${encodeURIComponent(url)}&text=${encodeURIComponent(text)}${hashtags ? '&hashtags=' + hashtags.join(',') : ''}`,

  whatsapp: (text: string) =>
    `https://wa.me/?text=${encodeURIComponent(text)}`,

  linkedin: (url: string, title: string, summary: string) =>
    `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}&title=${encodeURIComponent(title)}&summary=${encodeURIComponent(summary)}`
};
```

**Behavior**:
1. User clicks social platform button
2. New window opens with share dialog
3. Analytics event fired
4. Success callback triggered when window closes

---

### 5. QR Code Tab

**Purpose**: Generate and download QR code for offline sharing

```typescript
interface QRCodeTabState {
  qrCodeDataUrl: string;
  isGenerating: boolean;
  downloadFormat: 'png' | 'svg';
}

interface QRCodeTabProps {
  referralUrl: string;
  size?: number;
  errorCorrectionLevel?: 'L' | 'M' | 'Q' | 'H';
  onDownload: (format: 'png' | 'svg') => void;
}

interface QRCodeGenerationOptions {
  data: string;
  size: number;
  errorCorrectionLevel: 'L' | 'M' | 'Q' | 'H';
  margin: number;
  color: {
    dark: string;
    light: string;
  };
}
```

**Features**:
- Auto-generate QR code on tab open
- QR code size: 240x240px (display), 1024x1024px (download)
- Error correction level: M (15% recovery)
- Download formats: PNG, SVG
- Optional: Print functionality
- Responsive sizing

**QR Code Generation**:

```typescript
const generateQRCode = async (url: string): Promise<string> => {
  const qrCode = new QRCodeGenerator({
    data: url,
    size: 1024,
    errorCorrectionLevel: 'M',
    margin: 4,
    color: {
      dark: '#000000',
      light: '#FFFFFF'
    }
  });

  return qrCode.toDataURL();
};
```

---

## Message Preview Component

**Purpose**: Show real-time preview of share message

```typescript
interface MessagePreviewProps {
  message: string;
  referralUrl: string;
  platform?: ShareMethod;
}
```

**Features**:
- Live update as user types
- URL highlighting
- Platform-specific formatting
- Character limit warnings
- Link truncation preview (for SMS)

---

## State Management

```typescript
interface ShareReferralState {
  // UI State
  isOpen: boolean;
  activeTab: ShareMethod;
  isLoading: boolean;

  // Referral Data
  referralCode: string;
  referralUrl: string;
  shortUrl?: string;

  // Share Status
  shareHistory: ShareHistoryItem[];
  lastSharedMethod?: ShareMethod;
  lastSharedTimestamp?: Date;

  // Copy Link State
  isCopied: boolean;
  showToast: boolean;

  // Email State
  emailForm: EmailShareTabState;

  // SMS State
  smsForm: SMSShareTabState;

  // QR Code State
  qrCode: QRCodeTabState;

  // Analytics
  shareCount: {
    [key in ShareMethod]?: number;
  };
}

interface ShareHistoryItem {
  method: ShareMethod;
  timestamp: Date;
  success: boolean;
  recipientCount?: number;
  error?: string;
}
```

## API Integration

### Get Referral Link

```typescript
// GET /api/v1/referrals/link
interface GetReferralLinkResponse {
  success: boolean;
  data: {
    code: string;
    url: string;
    shortUrl: string;
    expiresAt?: string;
    usageLimit?: number;
    usageCount: number;
  };
}
```

### Send Email Referral

```typescript
// POST /api/v1/referrals/share/email
interface SendEmailReferralRequest {
  recipients: string[];
  subject?: string;
  message: string;
  referralCode: string;
}

interface SendEmailReferralResponse {
  success: boolean;
  data: {
    sentCount: number;
    failedCount: number;
    failedRecipients?: string[];
  };
  error?: {
    code: string;
    message: string;
  };
}
```

### Send SMS Referral

```typescript
// POST /api/v1/referrals/share/sms
interface SendSMSReferralRequest {
  phoneNumber: string;
  message: string;
  referralCode: string;
}

interface SendSMSReferralResponse {
  success: boolean;
  data: {
    messageId: string;
    status: 'sent' | 'queued' | 'failed';
  };
  error?: {
    code: string;
    message: string;
  };
}
```

### Track Share Event

```typescript
// POST /api/v1/referrals/track/share
interface TrackShareEventRequest {
  referralCode: string;
  method: ShareMethod;
  platform?: string;
  recipientCount?: number;
  metadata?: Record<string, any>;
}

interface TrackShareEventResponse {
  success: boolean;
  data: {
    eventId: string;
    timestamp: string;
  };
}
```

## Visual Specifications

### Layout

- **Modal Width**: 600px (desktop), 100% (mobile)
- **Modal Max Height**: 90vh
- **Content Padding**: 24px
- **Tab Bar Height**: 48px
- **Border Radius**: 12px (modal), 8px (inputs/buttons)

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Modal Background | --color-white | #FFFFFF |
| Modal Overlay | rgba(0, 0, 0, 0.5) | - |
| Active Tab Text | --color-primary | #00529F |
| Active Tab Border | --color-primary | #00529F |
| Inactive Tab Text | --color-gray-600 | #666666 |
| Input Border | --color-gray-300 | #CCCCCC |
| Input Border Focus | --color-primary | #00529F |
| Success Toast BG | --color-success-light | #E8F5E9 |
| Success Toast Border | --color-success | #2E7D32 |
| Preview Background | --color-gray-50 | #FAFAFA |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Modal Title | Montserrat | 24px | 700 |
| Tab Labels | Lato | 14px | 500/600 (active) |
| Section Description | Lato | 14px | 400 |
| Form Labels | Lato | 14px | 600 |
| Input Text | Lato | 14px | 400 |
| Button Text | Lato | 14px | 600 |
| Toast Message | Lato | 14px | 400 |
| Character Count | Lato | 12px | 400 |
| Preview Header | Lato | 10px | 600 |

### Spacing

| Component | Padding | Margin | Gap |
|-----------|---------|--------|-----|
| Modal Header | 24px | - | - |
| Modal Body | 24px | - | - |
| Form Group | - | 0 0 20px | - |
| Tab Buttons | 16px 12px | - | - |
| Button Group | - | - | 12px |
| Toast | 12px 16px | - | 12px |

## Share Method Configurations

### URL Shortening

```typescript
interface URLShortenerConfig {
  enabled: boolean;
  service: 'internal' | 'bitly' | 'tinyurl';
  customDomain?: string;
  expirationDays?: number;
}

const shortenUrl = async (longUrl: string): Promise<string> => {
  const response = await fetch('/api/v1/urls/shorten', {
    method: 'POST',
    body: JSON.stringify({ url: longUrl })
  });

  const data = await response.json();
  return data.shortUrl;
};
```

### Email Templates

```typescript
interface EmailTemplate {
  subject: string;
  body: string;
  variables: {
    senderName: string;
    recipientName?: string;
    referralUrl: string;
    discount: string;
    platformName: string;
  };
}

const emailTemplates = {
  default: {
    subject: '{{senderName}} invited you to {{platformName}}',
    body: `
      Hi{{recipientName ? ' ' + recipientName : ''}},

      {{senderName}} has invited you to join {{platformName}}!

      {{message}}

      Get started with {{discount}}% off your first service:
      {{referralUrl}}

      Looking forward to serving you!

      The {{platformName}} Team
    `
  }
};
```

### SMS Templates

```typescript
const smsTemplates = {
  short: '{{platformName}}: Get {{discount}}% off! {{shortUrl}}',

  default: 'Check out {{platformName}}! Get {{discount}}% off your first service: {{shortUrl}}',

  personal: '{{senderName}} recommends {{platformName}}. Get {{discount}}% off: {{shortUrl}}'
};
```

## Analytics Tracking

```typescript
interface ShareAnalyticsEvents {
  // Modal events
  'share_modal_opened': {
    source: string;
    referralCode: string;
  };

  'share_modal_closed': {
    duration: number;
    tabsViewed: ShareMethod[];
  };

  // Tab events
  'share_tab_switched': {
    from: ShareMethod;
    to: ShareMethod;
  };

  // Share events
  'share_link_copied': {
    referralCode: string;
    timestamp: Date;
  };

  'share_email_sent': {
    referralCode: string;
    recipientCount: number;
    success: boolean;
  };

  'share_sms_sent': {
    referralCode: string;
    messageLength: number;
    success: boolean;
  };

  'share_social_clicked': {
    platform: SocialPlatform;
    referralCode: string;
  };

  'share_qr_generated': {
    referralCode: string;
    size: number;
  };

  'share_qr_downloaded': {
    referralCode: string;
    format: 'png' | 'svg';
  };
}

// Analytics implementation
const trackShareEvent = (
  eventName: keyof ShareAnalyticsEvents,
  data: ShareAnalyticsEvents[typeof eventName]
) => {
  // Send to analytics service
  analyticsService.track(eventName, {
    ...data,
    timestamp: new Date().toISOString(),
    userId: user.id
  });
};
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Modal trap focus when open
- [ ] Escape key closes modal
- [ ] Tab navigation through all interactive elements
- [ ] ARIA labels for all buttons and inputs
- [ ] Screen reader announcements for success/error states
- [ ] Color contrast ratios meet 4.5:1 minimum
- [ ] Focus indicators visible on all elements
- [ ] Form validation errors announced

### Keyboard Navigation

```typescript
const keyboardHandlers = {
  Escape: () => closeModal(),
  Tab: (e) => handleTabNavigation(e),
  ArrowLeft: () => switchToPreviousTab(),
  ArrowRight: () => switchToNextTab(),
  Enter: (e) => {
    if (e.target.matches('button')) {
      e.target.click();
    }
  }
};
```

### ARIA Implementation

```html
<div
  role="dialog"
  aria-labelledby="modal-title"
  aria-describedby="modal-description"
  aria-modal="true"
>
  <h2 id="modal-title">Share Your Referral</h2>

  <div role="tablist" aria-label="Share methods">
    <button
      role="tab"
      aria-selected="true"
      aria-controls="copy-link-panel"
      id="copy-link-tab"
    >
      Copy Link
    </button>
  </div>

  <div
    role="tabpanel"
    id="copy-link-panel"
    aria-labelledby="copy-link-tab"
    tabindex="0"
  >
    <!-- Tab content -->
  </div>

  <div role="status" aria-live="polite" aria-atomic="true">
    <!-- Success/error messages -->
  </div>
</div>
```

## Responsive Behavior

### Desktop (≥768px)

- Modal centered on screen
- Width: 600px
- All tabs visible in tab bar
- Two-column social buttons
- QR code: 240x240px

### Tablet (≥576px, <768px)

- Modal fills width with margins
- Horizontal scrolling tabs if needed
- Two-column social buttons
- QR code: 200x200px

### Mobile (<576px)

- Modal fills entire screen
- Compact tab labels
- Single-column social buttons
- Stacked form buttons
- QR code: 200x200px
- Reduced padding (16px)

## Validation Rules

### Email Validation

```typescript
const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

const validateEmailList = (emails: string): string[] => {
  return emails
    .split(',')
    .map(email => email.trim())
    .filter(email => validateEmail(email));
};
```

### Phone Validation

```typescript
const validatePhoneNumber = (phone: string): boolean => {
  // E.164 format: +[country code][number]
  const phoneRegex = /^\+?[1-9]\d{1,14}$/;
  return phoneRegex.test(phone.replace(/[\s()-]/g, ''));
};

const formatPhoneNumber = (phone: string): string => {
  // Auto-format to (XXX) XXX-XXXX for US numbers
  const cleaned = phone.replace(/\D/g, '');
  const match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
  if (match) {
    return `(${match[1]}) ${match[2]}-${match[3]}`;
  }
  return phone;
};
```

### Message Validation

```typescript
const validateMessage = (message: string, maxLength: number): {
  isValid: boolean;
  error?: string;
} => {
  if (message.trim().length === 0) {
    return { isValid: false, error: 'Message cannot be empty' };
  }

  if (message.length > maxLength) {
    return {
      isValid: false,
      error: `Message exceeds ${maxLength} character limit`
    };
  }

  return { isValid: true };
};
```

## Error Handling

### Error States

```typescript
interface ErrorState {
  type: 'network' | 'validation' | 'rate-limit' | 'service';
  message: string;
  field?: string;
  retryable: boolean;
}

const errorMessages = {
  NETWORK_ERROR: 'Unable to connect. Please check your internet connection.',
  INVALID_EMAIL: 'Please enter a valid email address.',
  INVALID_PHONE: 'Please enter a valid phone number.',
  RATE_LIMIT: 'Too many requests. Please try again in a few minutes.',
  SERVICE_UNAVAILABLE: 'Service temporarily unavailable. Please try again later.',
  EMPTY_MESSAGE: 'Please enter a message.',
  MESSAGE_TOO_LONG: 'Message exceeds maximum length.',
  NO_RECIPIENTS: 'Please enter at least one recipient.'
};
```

### Retry Logic

```typescript
const retryWithBackoff = async (
  fn: () => Promise<any>,
  maxRetries = 3,
  delay = 1000
): Promise<any> => {
  for (let i = 0; i < maxRetries; i++) {
    try {
      return await fn();
    } catch (error) {
      if (i === maxRetries - 1) throw error;
      await new Promise(resolve => setTimeout(resolve, delay * Math.pow(2, i)));
    }
  }
};
```

## Testing Requirements

### Unit Tests

```typescript
describe('ShareReferralInterface', () => {
  describe('Copy Link Tab', () => {
    it('copies link to clipboard on button click', () => {});
    it('shows success toast after copy', () => {});
    it('hides toast after 3 seconds', () => {});
    it('selects URL on input click', () => {});
  });

  describe('Email Share Tab', () => {
    it('validates email format', () => {});
    it('accepts multiple comma-separated emails', () => {});
    it('shows error for invalid email', () => {});
    it('sends email with correct data', () => {});
    it('displays message preview', () => {});
    it('handles send failure gracefully', () => {});
  });

  describe('SMS Share Tab', () => {
    it('validates phone number format', () => {});
    it('formats phone number as user types', () => {});
    it('shows character count', () => {});
    it('warns when exceeding 160 characters', () => {});
    it('sends SMS with correct data', () => {});
  });

  describe('Social Share Tab', () => {
    it('generates correct Facebook share URL', () => {});
    it('generates correct Twitter share URL', () => {});
    it('generates correct WhatsApp share URL', () => {});
    it('generates correct LinkedIn share URL', () => {});
    it('opens share dialog in new window', () => {});
  });

  describe('QR Code Tab', () => {
    it('generates QR code on mount', () => {});
    it('downloads QR code as PNG', () => {});
    it('downloads QR code as SVG', () => {});
    it('shows loading state while generating', () => {});
  });

  describe('Accessibility', () => {
    it('traps focus within modal', () => {});
    it('closes on Escape key', () => {});
    it('switches tabs with arrow keys', () => {});
    it('has proper ARIA labels', () => {});
    it('passes axe accessibility audit', () => {});
  });

  describe('Analytics', () => {
    it('tracks modal open event', () => {});
    it('tracks tab switch events', () => {});
    it('tracks share success events', () => {});
    it('tracks share failure events', () => {});
  });
});
```

### E2E Tests

```typescript
describe('Share Referral Flow', () => {
  it('opens share modal from dashboard', () => {});
  it('copies referral link successfully', () => {});
  it('sends email to multiple recipients', () => {});
  it('sends SMS to valid phone number', () => {});
  it('shares to Facebook', () => {});
  it('downloads QR code', () => {});
  it('switches between tabs', () => {});
  it('closes modal with close button', () => {});
  it('closes modal with Escape key', () => {});
});
```

### Integration Tests

```typescript
describe('API Integration', () => {
  it('fetches referral link on mount', () => {});
  it('sends email via API', () => {});
  it('sends SMS via API', () => {});
  it('tracks share events', () => {});
  it('handles API errors gracefully', () => {});
  it('retries failed requests', () => {});
});
```

## Performance Considerations

### Optimization Strategies

1. **Lazy Load QR Code Library**: Only load QR code generation library when QR tab is opened
2. **Debounce Input**: Debounce message preview updates (300ms)
3. **Memoize Share URLs**: Cache generated social share URLs
4. **Virtual Scrolling**: For large recipient lists (if implemented)
5. **Image Optimization**: Use WebP format for QR code downloads when supported

```typescript
const optimizations = {
  // Lazy load QR library
  qrCodeLibrary: () => import('qrcode'),

  // Debounce preview updates
  debouncedPreview: debounce((message: string) => {
    updatePreview(message);
  }, 300),

  // Memoize share URLs
  memoizedShareUrls: useMemo(() => ({
    facebook: generateFacebookUrl(url, message),
    twitter: generateTwitterUrl(url, message),
    whatsapp: generateWhatsAppUrl(url, message),
    linkedin: generateLinkedInUrl(url, message)
  }), [url, message])
};
```

## Dependencies

```json
{
  "dependencies": {
    "qrcode": "^1.5.0",
    "copy-to-clipboard": "^3.3.3",
    "libphonenumber-js": "^1.10.0"
  },
  "devDependencies": {
    "@testing-library/react": "^13.4.0",
    "@testing-library/user-event": "^14.4.3",
    "axe-core": "^4.6.0"
  }
}
```

## Related Components

- `ReferralDashboard` - Parent component showing referral stats
- `Button` - Shared button component
- `TextField` - Shared input component
- `TextArea` - Shared textarea component
- `Modal` - Base modal component
- `Toast` - Toast notification component
- `Tabs` - Tab navigation component

## Security Considerations

1. **Rate Limiting**: Limit share attempts per user/IP
2. **Input Sanitization**: Sanitize all user inputs before sending
3. **CSRF Protection**: Include CSRF tokens in API requests
4. **XSS Prevention**: Escape all user-generated content in previews
5. **Spam Prevention**: Implement CAPTCHA for high-volume sharing
6. **URL Validation**: Validate referral URLs before sharing

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-08 | Initial specification |
