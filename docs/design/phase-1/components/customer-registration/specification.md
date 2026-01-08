# Customer Registration Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-CM-F001` |
| **Component Name** | CustomerRegistration |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive registration form allowing customers to create accounts on the platform. Supports standard email/password registration, social login options, and referral code integration.

## User Stories

1. **As a new visitor**, I want to create an account quickly, so I can access automotive services.
2. **As a referred user**, I want my referral discount applied automatically, so I can benefit from my friend's invitation.
3. **As a mobile user**, I want to register easily on my phone, so I can sign up on-the-go.
4. **As a privacy-conscious user**, I want to control my marketing preferences, so I only receive communications I want.

## Component Structure

```
CustomerRegistration/
├── index.ts                         # Public exports
├── CustomerRegistration.tsx         # Main component (React)
├── CustomerRegistration.component.ts    # Main component (Angular)
├── CustomerRegistration.module.scss     # Styles
├── CustomerRegistration.test.tsx        # Unit tests
├── hooks/
│   ├── useRegistrationForm.ts       # Form state management
│   └── usePasswordStrength.ts       # Password validation
├── components/
│   ├── RegistrationForm/            # Form fields component
│   ├── PasswordStrengthIndicator/   # Password strength display
│   ├── ReferralBanner/              # Referral notification
│   ├── SocialLoginButtons/          # OAuth buttons
│   └── TermsCheckbox/               # Terms acceptance
└── validation/
    └── registrationSchema.ts        # Form validation rules
```

## Props / Inputs

### React

```typescript
interface CustomerRegistrationProps {
  /** Pre-filled referral code from URL */
  referralCode?: string;

  /** Referrer information to display */
  referrer?: {
    name: string;
    discount?: string;
  };

  /** Available social login providers */
  socialProviders?: ('google' | 'facebook' | 'apple')[];

  /** Callback on successful registration */
  onSuccess?: (user: RegisteredUser) => void;

  /** Callback on registration error */
  onError?: (error: RegistrationError) => void;

  /** Custom redirect URL after registration */
  redirectUrl?: string;

  /** Enable/disable social login */
  enableSocialLogin?: boolean;
}

interface RegisteredUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  referralCode?: string;
}

interface RegistrationError {
  field?: string;
  code: string;
  message: string;
}
```

### Angular

```typescript
@Component({
  selector: 'app-customer-registration',
  templateUrl: './customer-registration.component.html',
  styleUrls: ['./customer-registration.component.scss']
})
export class CustomerRegistrationComponent {
  @Input() referralCode?: string;
  @Input() referrer?: { name: string; discount?: string };
  @Input() socialProviders: string[] = ['google', 'facebook'];
  @Input() redirectUrl?: string;
  @Input() enableSocialLogin = true;

  @Output() registrationSuccess = new EventEmitter<RegisteredUser>();
  @Output() registrationError = new EventEmitter<RegistrationError>();
}
```

## Form Fields

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| firstName | text | Yes | 2-50 characters, letters only |
| lastName | text | Yes | 2-50 characters, letters only |
| email | email | Yes | Valid email format, unique |
| phone | tel | Yes | Valid US phone format |
| password | password | Yes | Min 8 chars, 1 upper, 1 lower, 1 number |
| confirmPassword | password | Yes | Must match password |
| marketingConsent | checkbox | No | Boolean |
| termsAccept | checkbox | Yes | Must be checked |

## State Management

```typescript
interface RegistrationFormState {
  // Form values
  values: {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    password: string;
    confirmPassword: string;
    marketingConsent: boolean;
    termsAccept: boolean;
  };

  // Validation errors
  errors: {
    [key: string]: string | undefined;
  };

  // Touched fields
  touched: {
    [key: string]: boolean;
  };

  // Form state
  isSubmitting: boolean;
  isValid: boolean;
  submitError: string | null;

  // Password strength
  passwordStrength: 'weak' | 'fair' | 'good' | 'strong';
}
```

## Visual Specifications

### Layout

- **Form Container**: Max width 480px, centered
- **Card Padding**: 32px (desktop), 24px (mobile)
- **Form Gap**: 24px between field groups
- **Border Radius**: 12px (card), 8px (inputs)

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Card Background | --color-white | #FFFFFF |
| Input Border | --color-gray-300 | #CCCCCC |
| Input Border Focus | --color-primary | #00529F |
| Input Border Error | --color-error | #D32F2F |
| Label Text | --color-gray-800 | #333333 |
| Helper Text | --color-gray-600 | #666666 |
| Error Text | --color-error | #D32F2F |
| Success Banner | --color-success-light | #E8F5E9 |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Page Title | Montserrat | 28px | 700 |
| Subtitle | Lato | 16px | 400 |
| Form Labels | Lato | 14px | 600 |
| Input Text | Lato | 16px | 400 |
| Helper Text | Lato | 12px | 400 |
| Error Text | Lato | 12px | 400 |
| Button Text | Lato | 16px | 600 |

### Input Field States

```scss
// Default
.form-input {
  border: 1px solid var(--color-gray-300);
  background: var(--color-white);
}

// Focus
.form-input:focus {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(0, 82, 159, 0.4);
}

// Error
.form-input--error {
  border-color: var(--color-error);
}

.form-input--error:focus {
  box-shadow: 0 0 0 3px rgba(211, 47, 47, 0.4);
}

// Disabled
.form-input:disabled {
  background: var(--color-gray-100);
  cursor: not-allowed;
}
```

## Password Strength Indicator

### Strength Levels

| Level | Color | Width | Requirements |
|-------|-------|-------|--------------|
| Weak | #D32F2F | 25% | < 8 characters |
| Fair | #ED6C02 | 50% | 8+ chars, missing requirements |
| Good | #FFD520 | 75% | 8+ chars, 3 of 4 requirements |
| Strong | #2E7D32 | 100% | All requirements met |

### Requirements

1. Minimum 8 characters
2. At least 1 uppercase letter
3. At least 1 lowercase letter
4. At least 1 number
5. At least 1 special character (optional for "strong")

## Referral Banner

Displayed when a referral code is present:

```typescript
interface ReferralBannerProps {
  referrerName: string;
  discountAmount?: string;  // e.g., "10%" or "$20"
  expiresAt?: Date;
}
```

### Display Rules

- Show banner above form when referral code is valid
- Display referrer's first name + last initial
- Show discount amount prominently
- Green success theme indicates benefit

## Validation Rules

```typescript
const registrationSchema = {
  firstName: {
    required: 'First name is required',
    minLength: { value: 2, message: 'First name must be at least 2 characters' },
    maxLength: { value: 50, message: 'First name must be less than 50 characters' },
    pattern: { value: /^[A-Za-z\s\-']+$/, message: 'First name can only contain letters' }
  },
  lastName: {
    required: 'Last name is required',
    minLength: { value: 2, message: 'Last name must be at least 2 characters' },
    maxLength: { value: 50, message: 'Last name must be less than 50 characters' },
    pattern: { value: /^[A-Za-z\s\-']+$/, message: 'Last name can only contain letters' }
  },
  email: {
    required: 'Email is required',
    pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Please enter a valid email' }
  },
  phone: {
    required: 'Phone number is required',
    pattern: { value: /^\(\d{3}\) \d{3}-\d{4}$/, message: 'Please enter a valid phone number' }
  },
  password: {
    required: 'Password is required',
    minLength: { value: 8, message: 'Password must be at least 8 characters' },
    validate: {
      hasUppercase: (v) => /[A-Z]/.test(v) || 'Password must contain an uppercase letter',
      hasLowercase: (v) => /[a-z]/.test(v) || 'Password must contain a lowercase letter',
      hasNumber: (v) => /\d/.test(v) || 'Password must contain a number'
    }
  },
  confirmPassword: {
    required: 'Please confirm your password',
    validate: {
      matches: (v, { password }) => v === password || 'Passwords do not match'
    }
  },
  termsAccept: {
    required: 'You must accept the terms and conditions'
  }
};
```

## API Integration

### Registration Endpoint

```typescript
// POST /api/v1/customers/register
interface RegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
  marketingConsent: boolean;
  termsAcceptedAt: string;  // ISO timestamp
  referralCode?: string;
}

interface RegistrationResponse {
  success: boolean;
  data?: {
    customerId: string;
    email: string;
    verificationEmailSent: boolean;
    referralApplied: boolean;
    discountCode?: string;
  };
  error?: {
    code: string;
    message: string;
    field?: string;
  };
}
```

### Email Verification Flow

1. User submits registration
2. Backend creates unverified account
3. Verification email sent
4. User clicks verification link
5. Account marked as verified
6. User redirected to dashboard

## Social Login Integration

### Google OAuth

```typescript
const googleConfig = {
  clientId: process.env.GOOGLE_CLIENT_ID,
  scope: 'email profile',
  redirectUri: `${window.location.origin}/auth/google/callback`
};
```

### Facebook OAuth

```typescript
const facebookConfig = {
  appId: process.env.FACEBOOK_APP_ID,
  scope: 'email,public_profile',
  redirectUri: `${window.location.origin}/auth/facebook/callback`
};
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] All form fields have associated labels
- [ ] Error messages linked to fields via aria-describedby
- [ ] Focus visible on all interactive elements
- [ ] Color is not the only indicator of errors
- [ ] Form can be completed with keyboard only

### Screen Reader Support

```html
<div class="form-group" role="group" aria-labelledby="firstName-label">
  <label id="firstName-label" for="firstName" class="form-label">
    First Name <span aria-hidden="true">*</span>
    <span class="sr-only">(required)</span>
  </label>
  <input
    type="text"
    id="firstName"
    name="firstName"
    aria-required="true"
    aria-invalid="false"
    aria-describedby="firstName-error"
  />
  <span id="firstName-error" class="form-error" role="alert" aria-live="polite">
    <!-- Error message appears here -->
  </span>
</div>
```

### Focus Management

- Auto-focus first field on page load
- Move focus to first error on validation failure
- Announce form submission status

## Responsive Behavior

### Desktop (≥768px)

- Two-column layout for name fields
- Social login buttons side-by-side
- Full form visible without scrolling

### Mobile (<768px)

- Single-column layout for all fields
- Stacked social login buttons
- Larger touch targets (48px minimum)
- Fixed submit button at bottom

## Error Handling

### Client-Side Errors

| Error | Message | Behavior |
|-------|---------|----------|
| Validation | Field-specific message | Show below field |
| Empty required | "X is required" | Show on blur |
| Invalid format | Format-specific message | Show on blur |
| Password mismatch | "Passwords do not match" | Show on confirmPassword blur |

### Server-Side Errors

| Error Code | Message | Behavior |
|------------|---------|----------|
| EMAIL_EXISTS | "This email is already registered" | Show below email field |
| INVALID_REFERRAL | "Invalid referral code" | Show alert banner |
| RATE_LIMITED | "Too many attempts. Please try again later" | Show alert, disable form |
| SERVER_ERROR | "Something went wrong. Please try again" | Show alert banner |

## Analytics Events

```typescript
// Track registration funnel
trackEvent('registration_started');
trackEvent('registration_form_valid');
trackEvent('registration_submitted');
trackEvent('registration_success', { hasReferral: boolean });
trackEvent('registration_error', { errorCode: string });
trackEvent('social_login_clicked', { provider: string });
```

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerRegistration', () => {
  it('renders all form fields', () => {});
  it('validates required fields on blur', () => {});
  it('validates email format', () => {});
  it('validates phone format', () => {});
  it('validates password strength', () => {});
  it('validates password confirmation match', () => {});
  it('displays referral banner when code provided', () => {});
  it('submits form with valid data', () => {});
  it('displays server errors', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Registration Flow', () => {
  it('completes registration with email', () => {});
  it('completes registration with referral code', () => {});
  it('handles duplicate email error', () => {});
  it('completes social login registration', () => {});
  it('redirects to verification page on success', () => {});
});
```

## Dependencies

- Form validation library (react-hook-form / Angular Reactive Forms)
- Phone number formatting library
- OAuth libraries for social login
- Password strength library

## Related Components

- `Header` - Platform header
- `Footer` - Platform footer
- `Button` - Shared button component
- `TextField` - Shared input component
- `Checkbox` - Shared checkbox component
- `Alert` - Error/success messages
- `PasswordStrengthIndicator` - Password strength display

## Security Considerations

1. **Password Security**
   - Never log passwords
   - Hash with bcrypt (cost factor 12+)
   - Enforce complexity requirements

2. **Rate Limiting**
   - Max 5 registration attempts per IP per hour
   - Max 3 attempts per email per day

3. **CSRF Protection**
   - Include CSRF token in form
   - Validate on server

4. **Email Verification**
   - Require email verification before full access
   - Token expires after 24 hours

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
