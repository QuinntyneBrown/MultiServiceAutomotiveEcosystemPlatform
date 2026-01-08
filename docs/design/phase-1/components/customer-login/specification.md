# Customer Login Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-CM-F002` |
| **Component Name** | CustomerLogin |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A secure login page allowing customers to authenticate using email/password or social login providers. Includes remember me functionality, forgot password flow, and account lockout handling.

## User Stories

1. **As a returning customer**, I want to sign in quickly, so I can access my account.
2. **As a forgetful user**, I want to reset my password, so I can regain access.
3. **As a busy user**, I want to stay logged in, so I don't have to sign in every time.
4. **As a security-conscious user**, I want to see if my account is locked, so I know if there's an issue.

## Component Structure

```
CustomerLogin/
├── index.ts                      # Public exports
├── CustomerLogin.tsx             # Main component (React)
├── CustomerLogin.component.ts    # Main component (Angular)
├── CustomerLogin.module.scss     # Styles
├── CustomerLogin.test.tsx        # Unit tests
├── hooks/
│   └── useLoginForm.ts           # Form state management
├── components/
│   ├── LoginForm/                # Email/password form
│   ├── PasswordField/            # Password with show/hide toggle
│   ├── RememberMe/               # Remember me checkbox
│   ├── SocialLoginButtons/       # OAuth buttons
│   └── AlertBanner/              # Error/warning messages
└── services/
    └── authService.ts            # Authentication API calls
```

## Props / Inputs

### React

```typescript
interface CustomerLoginProps {
  /** Redirect URL after successful login */
  redirectUrl?: string;

  /** Available social login providers */
  socialProviders?: ('google' | 'facebook' | 'apple')[];

  /** Enable/disable social login */
  enableSocialLogin?: boolean;

  /** Enable/disable remember me */
  enableRememberMe?: boolean;

  /** Custom login success handler */
  onSuccess?: (user: AuthenticatedUser) => void;

  /** Custom login error handler */
  onError?: (error: AuthError) => void;

  /** Pre-filled email (from registration or password reset) */
  prefillEmail?: string;

  /** Show success message (e.g., after password reset) */
  successMessage?: string;
}

interface AuthenticatedUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'customer' | 'professional' | 'admin';
  token: string;
}

interface AuthError {
  code: 'INVALID_CREDENTIALS' | 'ACCOUNT_LOCKED' | 'EMAIL_NOT_VERIFIED' | 'SERVER_ERROR';
  message: string;
  lockoutMinutes?: number;
  attemptsRemaining?: number;
}
```

### Angular

```typescript
@Component({
  selector: 'app-customer-login',
  templateUrl: './customer-login.component.html',
  styleUrls: ['./customer-login.component.scss']
})
export class CustomerLoginComponent {
  @Input() redirectUrl?: string;
  @Input() socialProviders: string[] = ['google', 'facebook'];
  @Input() enableSocialLogin = true;
  @Input() enableRememberMe = true;
  @Input() prefillEmail?: string;
  @Input() successMessage?: string;

  @Output() loginSuccess = new EventEmitter<AuthenticatedUser>();
  @Output() loginError = new EventEmitter<AuthError>();
}
```

## Form Fields

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| email | email | Yes | Valid email format |
| password | password | Yes | Non-empty |
| rememberMe | checkbox | No | Boolean |

## State Management

```typescript
interface LoginFormState {
  // Form values
  values: {
    email: string;
    password: string;
    rememberMe: boolean;
  };

  // Validation errors
  errors: {
    email?: string;
    password?: string;
    form?: string;
  };

  // UI state
  isSubmitting: boolean;
  showPassword: boolean;

  // Security state
  failedAttempts: number;
  isLocked: boolean;
  lockoutEndTime?: Date;

  // Success message (e.g., from password reset)
  successMessage?: string;
}
```

## Visual Specifications

### Layout

- **Form Container**: Max width 420px, centered
- **Card Padding**: 32px (desktop), 24px (mobile)
- **Form Gap**: 24px between field groups
- **Border Radius**: 12px (card), 8px (inputs)

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Card Background | --color-white | #FFFFFF |
| Input Border | --color-gray-300 | #CCCCCC |
| Input Border Focus | --color-primary | #00529F |
| Error Alert Background | --color-error-light | #FFEBEE |
| Error Alert Border | --color-error | #D32F2F |
| Warning Alert Background | --color-warning-light | #FFF3E0 |
| Warning Alert Border | --color-warning | #ED6C02 |
| Link Color | --color-primary | #00529F |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Page Title | Montserrat | 28px | 700 |
| Subtitle | Lato | 16px | 400 |
| Form Labels | Lato | 14px | 600 |
| Input Text | Lato | 16px | 400 |
| Remember Me | Lato | 14px | 400 |
| Links | Lato | 14px | 500 |
| Alert Title | Lato | 14px | 600 |
| Alert Message | Lato | 14px | 400 |

## Alert Banners

### Error States

```typescript
const errorMessages = {
  INVALID_CREDENTIALS: {
    title: 'Invalid credentials',
    message: 'The email or password you entered is incorrect. Please try again.'
  },
  ACCOUNT_LOCKED: {
    title: 'Account temporarily locked',
    message: 'Too many failed attempts. Please try again in {minutes} minutes or reset your password.'
  },
  EMAIL_NOT_VERIFIED: {
    title: 'Email not verified',
    message: 'Please check your email and click the verification link to continue.'
  },
  SERVER_ERROR: {
    title: 'Something went wrong',
    message: 'We\'re having trouble signing you in. Please try again later.'
  }
};
```

### Success States

```typescript
const successMessages = {
  PASSWORD_RESET: 'Your password has been reset successfully. Please sign in with your new password.',
  EMAIL_VERIFIED: 'Your email has been verified. You can now sign in.',
  ACCOUNT_CREATED: 'Your account has been created. Please sign in.'
};
```

## Password Field Features

### Show/Hide Toggle

```typescript
interface PasswordFieldProps {
  value: string;
  onChange: (value: string) => void;
  showPassword: boolean;
  onToggleVisibility: () => void;
  error?: string;
  placeholder?: string;
}
```

### Visual States

- **Hidden**: Password masked with dots
- **Visible**: Password shown as plain text
- **Toggle Icon**: Eye (show) / Eye-slash (hide)

## Remember Me Functionality

### Behavior

1. When checked, session persists for 30 days
2. When unchecked, session expires after browser close
3. State persisted in localStorage for pre-fill

### Implementation

```typescript
const handleRememberMe = (checked: boolean) => {
  if (checked) {
    localStorage.setItem('remember_email', email);
    // Set longer session expiration
  } else {
    localStorage.removeItem('remember_email');
    // Set session-only cookie
  }
};
```

## Account Lockout Logic

### Thresholds

| Attempts | Action |
|----------|--------|
| 1-3 | Normal login attempts |
| 4 | Warning: "2 attempts remaining" |
| 5 | Lockout for 15 minutes |
| After unlock + 2 more | Lockout for 1 hour |
| After unlock + 2 more | Lockout for 24 hours |

### Display

```html
<div class="alert alert--warning" role="alert">
  <WarningIcon />
  <div class="alert__content">
    <p class="alert__title">Account temporarily locked</p>
    <p class="alert__message">
      Too many failed attempts. Please try again in
      <span id="countdown">14:59</span> or
      <a href="/forgot-password">reset your password</a>.
    </p>
  </div>
</div>
```

## API Integration

### Login Endpoint

```typescript
// POST /api/v1/auth/login
interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
}

interface LoginResponse {
  success: boolean;
  data?: {
    user: AuthenticatedUser;
    token: string;
    refreshToken: string;
    expiresAt: string;
  };
  error?: {
    code: string;
    message: string;
    attemptsRemaining?: number;
    lockoutMinutes?: number;
  };
}
```

### Token Management

```typescript
const handleLoginSuccess = (response: LoginResponse) => {
  const { token, refreshToken, expiresAt } = response.data;

  // Store tokens
  if (rememberMe) {
    localStorage.setItem('access_token', token);
    localStorage.setItem('refresh_token', refreshToken);
  } else {
    sessionStorage.setItem('access_token', token);
    sessionStorage.setItem('refresh_token', refreshToken);
  }

  // Set auth header for API calls
  api.defaults.headers.common['Authorization'] = `Bearer ${token}`;

  // Redirect
  router.push(redirectUrl || '/dashboard');
};
```

## Forgot Password Flow

### Link Behavior

1. Click "Forgot password?"
2. Navigate to `/forgot-password`
3. Email entry form
4. Password reset email sent
5. Reset link in email
6. New password form
7. Redirect to login with success message

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Form labels properly associated with inputs
- [ ] Error messages linked via aria-describedby
- [ ] Focus management after form submission
- [ ] Color not sole indicator of errors
- [ ] Keyboard accessible password toggle

### Screen Reader Support

```html
<form aria-labelledby="login-title">
  <h1 id="login-title">Welcome Back</h1>

  <div role="group" aria-labelledby="email-label">
    <label id="email-label" for="email">Email Address</label>
    <input
      type="email"
      id="email"
      aria-required="true"
      aria-invalid="false"
      aria-describedby="email-error"
    />
    <span id="email-error" role="alert"></span>
  </div>

  <button
    type="button"
    aria-label="Show password"
    aria-pressed="false"
  >
    <EyeIcon />
  </button>
</form>
```

## Responsive Behavior

### Desktop (≥768px)

- Centered card layout
- Remember me and forgot password side-by-side
- Social buttons side-by-side

### Mobile (<768px)

- Full-width card with margins
- Remember me and forgot password stacked
- Social buttons stacked
- Larger touch targets (48px minimum)

## Security Considerations

1. **Brute Force Protection**
   - Account lockout after 5 failed attempts
   - Progressive lockout duration
   - CAPTCHA after 3 failed attempts (optional)

2. **CSRF Protection**
   - Include CSRF token in form
   - Validate on server

3. **Secure Token Storage**
   - HttpOnly cookies for refresh tokens
   - Short-lived access tokens
   - Secure flag in production

4. **Password Security**
   - Never log passwords
   - Constant-time comparison on server
   - No password hints

## Analytics Events

```typescript
trackEvent('login_page_viewed');
trackEvent('login_attempted', { method: 'email' });
trackEvent('login_success', { method: 'email' });
trackEvent('login_failed', { reason: errorCode });
trackEvent('social_login_clicked', { provider: string });
trackEvent('forgot_password_clicked');
trackEvent('remember_me_toggled', { enabled: boolean });
```

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerLogin', () => {
  it('renders login form', () => {});
  it('validates required email', () => {});
  it('validates email format', () => {});
  it('validates required password', () => {});
  it('toggles password visibility', () => {});
  it('submits form with valid data', () => {});
  it('displays error on invalid credentials', () => {});
  it('displays lockout warning', () => {});
  it('handles remember me checkbox', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Login Flow', () => {
  it('logs in with valid credentials', () => {});
  it('shows error for invalid credentials', () => {});
  it('locks account after 5 failed attempts', () => {});
  it('redirects to specified URL after login', () => {});
  it('persists session with remember me', () => {});
  it('completes social login flow', () => {});
});
```

## Dependencies

- Form validation library
- OAuth libraries for social login
- JWT token handling library

## Related Components

- `Header` - Platform header
- `Button` - Shared button component
- `TextField` - Shared input component
- `Checkbox` - Shared checkbox component
- `Alert` - Alert banner component
- `ForgotPassword` - Password reset flow

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
