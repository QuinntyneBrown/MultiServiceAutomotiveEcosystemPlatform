# Customer Login Page Specification

## Overview

**Requirement ID:** REQ-CM-F002
**Phase:** 1
**Priority:** P0

The Customer Login page enables existing customers to authenticate and access their accounts securely.

---

## Page Purpose

- Authenticate existing customers
- Provide password reset functionality
- Support "Remember me" functionality
- Handle account lockout scenarios
- Provide alternative authentication methods (social login)
- Redirect to appropriate page after login

---

## Component Architecture

```
CustomerLoginPage/
├── customer-login.ts            # Component class
├── customer-login.html          # Template
├── customer-login.scss          # Styles
├── components/
│   └── social-login-buttons/    # Shared with registration
└── index.ts                     # Barrel export
```

---

## Form Fields

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| Email | email | Yes | Valid email format |
| Password | password | Yes | Not empty |
| Remember Me | checkbox | No | - |

---

## Component Structure

### Angular Component

```typescript
@Component({
  selector: 'app-customer-login',
  templateUrl: './customer-login.html',
  styleUrls: ['./customer-login.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    SocialLoginButtonsComponent
  ]
})
export class CustomerLogin {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    rememberMe: new FormControl(false)
  });

  isSubmitting = false;
  showPassword = false;
  loginError: string | null = null;
  isAccountLocked = false;
  lockoutMinutes = 0;

  private returnUrl: string = '/dashboard';

  ngOnInit(): void {
    // Get return URL from route parameters or default to dashboard
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';

    // Pre-fill email if coming from registration
    const email = this.route.snapshot.queryParams['email'];
    if (email) {
      this.loginForm.patchValue({ email });
    }
  }

  onSubmit(): void {
    if (this.loginForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      this.loginError = null;

      const { email, password, rememberMe } = this.loginForm.value;

      this.authService.login(email!, password!, rememberMe!).subscribe({
        next: () => {
          this.router.navigateByUrl(this.returnUrl);
        },
        error: (error) => {
          this.isSubmitting = false;

          if (error.status === 423) {
            // Account locked
            this.isAccountLocked = true;
            this.lockoutMinutes = error.error.lockoutMinutes || 15;
          } else if (error.status === 401) {
            this.loginError = 'Invalid email or password. Please try again.';
          } else {
            this.loginError = 'An error occurred. Please try again.';
          }
        }
      });
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onForgotPassword(): void {
    const email = this.loginForm.value.email;
    this.router.navigate(['/forgot-password'], {
      queryParams: email ? { email } : {}
    });
  }
}
```

---

## UI Sections

### 1. Sidebar (Desktop Only)
- **Logo:** Platform branding
- **Welcome message:** "Welcome back!"
- **Description:** Brief value proposition
- **Visible:** Only on screens >= 992px

### 2. Mobile Header
- **Logo:** Platform branding (smaller)
- **Sign Up link:** Navigate to registration page
- **Visible:** Only on screens < 992px

### 3. Login Card
- **Title:** "Sign in"
- **Subtitle:** "Enter your credentials..."
- **Error alerts:** Shown on failed login
- **Form:** Email, password, remember me
- **Forgot password link:** Navigate to reset flow
- **Submit button:** "Sign In"
- **Social login:** Google/Facebook options
- **Footer:** Link to sign up

---

## Error States

### Field-Level Errors
- Email: "Email is required" / "Please enter a valid email"
- Password: "Password is required"

### Alert Types

#### Invalid Credentials
```html
<div class="alert alert--error">
  <icon>warning</icon>
  <span>Invalid email or password. Please try again.</span>
</div>
```

#### Account Locked
```html
<div class="alert alert--warning">
  <icon>lock</icon>
  <span>Account temporarily locked. Please try again in {minutes} minutes or reset your password.</span>
</div>
```

#### Network Error
```html
<div class="alert alert--error">
  <icon>warning</icon>
  <span>Unable to connect. Please check your internet connection.</span>
</div>
```

---

## Responsive Behavior

### Mobile (< 576px)
- Single column layout
- Full-width form
- Card without shadow
- Mobile header visible

### Tablet (768px - 991px)
- Centered card with max-width
- Card with shadow
- Mobile header visible

### Desktop (≥ 992px)
- Split layout: sidebar + main content
- Sidebar with welcome message
- No mobile header

---

## Security Considerations

### Rate Limiting
- After 5 failed attempts, account is locked for 15 minutes
- Display lockout message with remaining time
- Provide password reset option

### Session Management
- "Remember me" extends session to 30 days
- Without "Remember me", session expires on browser close
- Store only refresh token when "Remember me" is checked

### Password Security
- Never log password values
- Use HTTPS only
- Implement CSRF protection

---

## Forgot Password Flow

Clicking "Forgot password?" navigates to `/forgot-password`:

1. User enters email address
2. System sends password reset email
3. User clicks link in email
4. User sets new password
5. Redirect to login with success message

---

## Accessibility Requirements

| Requirement | Implementation |
|-------------|----------------|
| Form labels | Associated with inputs |
| Error messages | aria-live regions |
| Password visibility | Clear aria-label |
| Focus management | Auto-focus email field |
| Keyboard navigation | All elements focusable |

---

## Analytics Events

| Event Name | Trigger | Data |
|------------|---------|------|
| `login_page_view` | Page load | `hasReturnUrl` |
| `login_submit` | Form submission | - |
| `login_success` | Successful login | `usedRememberMe` |
| `login_failure` | Failed login | `errorType` |
| `forgot_password_click` | Forgot password link | - |
| `social_login_click` | Social button click | `provider` |

---

## API Integration

### Login Endpoint
```
POST /api/auth/login

Request:
{
  "email": string,
  "password": string,
  "rememberMe": boolean
}

Response (200):
{
  "accessToken": string,
  "refreshToken": string,
  "expiresIn": number,
  "user": {
    "id": string,
    "email": string,
    "firstName": string,
    "lastName": string,
    "role": string
  }
}

Error Response (401):
{
  "error": "invalid_credentials",
  "message": "Invalid email or password"
}

Error Response (423):
{
  "error": "account_locked",
  "message": "Account temporarily locked",
  "lockoutMinutes": 15
}
```

---

## Dependencies

| Dependency | Purpose |
|------------|---------|
| `@angular/common` | CommonModule |
| `@angular/forms` | ReactiveFormsModule |
| `@angular/router` | RouterModule, route params |
| `AuthService` | Authentication API calls |

---

## Testing Checklist

### Unit Tests
- [ ] Form validation works correctly
- [ ] Login calls API with correct data
- [ ] Error handling displays correct messages
- [ ] Account lockout displays correctly
- [ ] Remember me is passed correctly
- [ ] Return URL is captured and used

### Integration Tests
- [ ] Full login flow works
- [ ] Redirect after login works
- [ ] Social login redirects correctly
- [ ] Forgot password navigation works

### Visual Tests
- [ ] Mobile layout renders correctly
- [ ] Tablet layout renders correctly
- [ ] Desktop layout renders correctly
- [ ] Error alerts display correctly
- [ ] Loading state displays correctly

### Accessibility Tests
- [ ] Form can be completed via keyboard
- [ ] Screen reader announces errors
- [ ] Focus order is logical

---

## Design Tokens Used

```scss
// Colors
$sidebar-gradient: linear-gradient(135deg, var(--color-blue-800), var(--color-blue-900));
$card-bg: var(--color-bg-primary);
$error-bg: var(--color-error-light);
$warning-bg: var(--color-warning-light);

// Spacing
$sidebar-width: 480px;
$card-max-width: 420px;
$card-padding-mobile: var(--spacing-6);
$card-padding-desktop: var(--spacing-8);
```

---

## File Checklist

- [ ] `customer-login.ts` - Component class
- [ ] `customer-login.html` - Template
- [ ] `customer-login.scss` - Styles
- [ ] `customer-login.spec.ts` - Unit tests
- [ ] `index.ts` - Barrel export
