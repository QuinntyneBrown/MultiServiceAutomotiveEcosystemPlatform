# Customer Registration Page Specification

## Overview

**Requirement ID:** REQ-CM-F001
**Phase:** 1
**Priority:** P0

The Customer Registration page enables new customers to create accounts on the platform. It supports direct registration and referral-based registration with automatic code application.

---

## Page Purpose

- Allow customers to self-register on the platform
- Collect required customer information
- Validate email and password requirements
- Handle referral code attribution
- Send confirmation email upon registration
- Provide alternative sign-up methods (social login)

---

## Component Architecture

```
CustomerRegistrationPage/
├── customer-registration.ts     # Component class
├── customer-registration.html   # Template
├── customer-registration.scss   # Styles
├── components/
│   ├── password-strength/       # Password strength indicator
│   ├── referral-banner/         # Referral attribution banner
│   └── social-login-buttons/    # Social authentication buttons
└── index.ts                     # Barrel export
```

---

## Form Fields

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| First Name | text | Yes | Min 2 chars, max 50 |
| Last Name | text | Yes | Min 2 chars, max 50 |
| Email | email | Yes | Valid email format, unique |
| Phone | tel | Yes | Valid US phone format |
| Password | password | Yes | Min 8 chars, 1 uppercase, 1 number |
| Confirm Password | password | Yes | Must match password |
| Marketing Consent | checkbox | No | - |
| Terms Acceptance | checkbox | Yes | Must be checked |

---

## Component Structure

### Angular Component

```typescript
@Component({
  selector: 'app-customer-registration',
  templateUrl: './customer-registration.html',
  styleUrls: ['./customer-registration.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    PasswordStrengthComponent,
    ReferralBannerComponent,
    SocialLoginButtonsComponent
  ]
})
export class CustomerRegistration {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);
  private readonly referralService = inject(ReferralService);

  registrationForm = new FormGroup({
    firstName: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50)
    ]),
    lastName: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50)
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
    phone: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\(\d{3}\) \d{3}-\d{4}$/)
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      this.passwordStrengthValidator
    ]),
    confirmPassword: new FormControl('', [Validators.required]),
    marketingConsent: new FormControl(false),
    termsAccepted: new FormControl(false, [Validators.requiredTrue])
  }, {
    validators: this.passwordMatchValidator
  });

  referralInfo$ = this.route.queryParams.pipe(
    map(params => params['ref']),
    filter(code => !!code),
    switchMap(code => this.referralService.validateCode(code))
  );

  isSubmitting = false;
  showPassword = false;

  passwordStrength$ = this.registrationForm.get('password')!.valueChanges.pipe(
    map(password => this.calculatePasswordStrength(password))
  );

  onSubmit(): void {
    if (this.registrationForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;

      const formValue = this.registrationForm.value;
      const referralCode = this.route.snapshot.queryParams['ref'];

      this.authService.register({
        firstName: formValue.firstName!,
        lastName: formValue.lastName!,
        email: formValue.email!,
        phone: formValue.phone!,
        password: formValue.password!,
        marketingConsent: formValue.marketingConsent!,
        referralCode
      }).subscribe({
        next: () => {
          this.router.navigate(['/registration-success']);
        },
        error: (error) => {
          this.isSubmitting = false;
          // Handle error (show toast, mark field as invalid)
        }
      });
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  private calculatePasswordStrength(password: string): PasswordStrength {
    // Implementation
  }

  private passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    // Implementation
  }

  private passwordMatchValidator(group: FormGroup): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}
```

---

## UI Sections

### 1. Sidebar (Desktop Only)
- **Logo:** Platform branding
- **Headline:** Value proposition
- **Benefits list:** Key features with checkmark icons
- **Visible:** Only on screens >= 992px

### 2. Mobile Header
- **Logo:** Platform branding (smaller)
- **Sign In link:** Navigate to login page
- **Visible:** Only on screens < 992px

### 3. Registration Card
- **Referral Banner:** Shows if arriving via referral link
- **Title:** "Create your account"
- **Form:** All registration fields
- **Submit Button:** "Create Account"
- **Social Login:** Google/Facebook options
- **Footer:** Link to sign in

---

## Responsive Behavior

### Mobile (< 576px)
- Single column layout
- Full-width form fields
- Stacked name fields
- Card without shadow (blends with background)
- Mobile header visible

### Tablet (768px - 991px)
- Centered card with max-width
- Two-column layout for name fields
- Card with shadow
- Mobile header visible

### Desktop (≥ 992px)
- Split layout: sidebar (480px) + main content
- Sidebar with benefits visible
- No mobile header
- Larger spacing

---

## Password Strength Indicator

### Strength Levels

| Level | Color | Criteria |
|-------|-------|----------|
| Weak | Red | < 8 chars or common pattern |
| Fair | Orange | 8+ chars, no variety |
| Medium | Yellow | 8+ chars, 2 types |
| Strong | Green | 8+ chars, 3+ types, no common patterns |

### Visual Display
- 4 horizontal bars
- Bars fill from left to right based on strength
- Text label describes current strength and suggestions

---

## Referral Banner

Displayed when URL contains `?ref=CODE` parameter:

```html
<div class="referral-banner">
  <icon>gift</icon>
  <div>
    <strong>Referred by {ReferrerName}</strong>
    <span>Complete registration to get {discount}% off your first service!</span>
  </div>
</div>
```

---

## Error States

### Field-Level Errors
- First Name: "First name is required" / "First name must be at least 2 characters"
- Last Name: "Last name is required" / "Last name must be at least 2 characters"
- Email: "Email is required" / "Please enter a valid email address" / "This email is already registered"
- Phone: "Phone is required" / "Please enter a valid phone number"
- Password: "Password is required" / "Password must be at least 8 characters"
- Confirm Password: "Passwords do not match"
- Terms: "You must accept the terms and conditions"

### Form-Level Errors
- Network error: Toast notification
- Server validation error: Show inline message

---

## Success Flow

1. User submits valid form
2. Show loading state on button
3. API creates account
4. Redirect to `/registration-success`
5. Show confirmation email message

---

## Accessibility Requirements

| Requirement | Implementation |
|-------------|----------------|
| Form labels | Associated with inputs via `for` attribute |
| Error messages | Linked with `aria-describedby` |
| Password visibility | Clear aria-label on toggle button |
| Focus management | Focus first error field on submission |
| Color contrast | Meets WCAG AA for all text |
| Screen reader | Announce password strength changes |

---

## Analytics Events

| Event Name | Trigger | Data |
|------------|---------|------|
| `registration_page_view` | Page load | `hasReferralCode`, `referralCode` |
| `registration_form_start` | First field interaction | - |
| `registration_submit` | Form submission | `hasReferralCode`, `marketingConsent` |
| `registration_success` | Successful registration | `referralApplied` |
| `registration_error` | Registration failed | `errorType`, `errorField` |
| `social_login_click` | Social button click | `provider` |

---

## API Integration

### Register Endpoint
```
POST /api/customers/register

Request:
{
  "firstName": string,
  "lastName": string,
  "email": string,
  "phone": string,
  "password": string,
  "marketingConsent": boolean,
  "referralCode": string | null
}

Response (201):
{
  "customerId": string,
  "email": string,
  "requiresEmailVerification": boolean
}

Error Response (400):
{
  "errors": {
    "email": ["Email already registered"]
  }
}
```

---

## Dependencies

| Dependency | Purpose |
|------------|---------|
| `@angular/common` | CommonModule |
| `@angular/forms` | ReactiveFormsModule |
| `@angular/router` | RouterModule, route params |
| `AuthService` | Registration API calls |
| `ReferralService` | Referral code validation |

---

## Testing Checklist

### Unit Tests
- [ ] Form validation rules work correctly
- [ ] Password strength calculator works
- [ ] Password match validator works
- [ ] Referral code is captured from URL
- [ ] Form submission calls API correctly
- [ ] Error handling works

### Integration Tests
- [ ] Full registration flow works
- [ ] Referral code application works
- [ ] Navigation to success page works

### Visual Tests
- [ ] Mobile layout renders correctly
- [ ] Tablet layout renders correctly
- [ ] Desktop layout renders correctly
- [ ] Password strength indicator displays correctly
- [ ] Error states display correctly

### Accessibility Tests
- [ ] Form can be completed via keyboard
- [ ] Screen reader announces all content
- [ ] Error messages are announced
- [ ] Focus order is logical

---

## Design Tokens Used

```scss
// Colors
$sidebar-gradient: linear-gradient(135deg, var(--color-blue-800), var(--color-blue-900));
$card-bg: var(--color-bg-primary);
$referral-gradient: linear-gradient(135deg, var(--color-blue-50), var(--color-yellow-100));

// Spacing
$sidebar-width: 480px;
$card-padding-mobile: var(--spacing-6);
$card-padding-desktop: var(--spacing-8);
$card-max-width: 480px;

// Password strength colors
$strength-weak: var(--color-error);
$strength-medium: var(--color-warning);
$strength-strong: var(--color-success);
```

---

## File Checklist

- [ ] `customer-registration.ts` - Component class
- [ ] `customer-registration.html` - Template
- [ ] `customer-registration.scss` - Styles
- [ ] `customer-registration.spec.ts` - Unit tests
- [ ] `components/password-strength/` - Strength indicator
- [ ] `components/referral-banner/` - Referral display
- [ ] `components/social-login-buttons/` - OAuth buttons
- [ ] `index.ts` - Barrel export
