# Referral Landing Page Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-RF-F015` |
| **Component Name** | ReferralLandingPage |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A conversion-optimized landing page displayed when visitors click on a referral link. Features the referring customer's personalized message, the professional's profile and services, special offer details, and a streamlined registration form to convert visitors into customers.

## User Stories

1. **As a referred visitor**, I want to see who referred me, so I trust the recommendation.
2. **As a referred visitor**, I want to see the professional's details, so I can evaluate their services.
3. **As a referred visitor**, I want to easily claim my discount, so I'm motivated to sign up.
4. **As a referred visitor**, I want quick registration options, so the process is frictionless.

## Component Structure

```
ReferralLandingPage/
├── index.ts                          # Public exports
├── ReferralLandingPage.tsx           # Main component (React)
├── ReferralLandingPage.component.ts  # Main component (Angular)
├── ReferralLandingPage.module.scss   # Styles
├── ReferralLandingPage.test.tsx      # Unit tests
├── components/
│   ├── LandingHeader/                # Minimal header with logo
│   ├── HeroSection/                  # Hero with referral info and offer
│   ├── ProfessionalCard/             # Professional details and contact
│   ├── RegistrationForm/             # Sign-up form with validation
│   ├── SocialLogin/                  # OAuth buttons
│   ├── TrustBadges/                  # Security/trust indicators
│   ├── CountdownTimer/               # Offer expiration timer
│   └── ReferralInfoBox/              # Referrer benefit display
├── hooks/
│   ├── useReferralData.ts            # Fetch referral details
│   ├── useRegistration.ts            # Registration logic
│   └── useCountdown.ts               # Countdown timer logic
└── types/
    └── referral-landing.types.ts     # Type definitions
```

## Props / Inputs

### React

```typescript
interface ReferralLandingPageProps {
  /** Referral code from URL */
  referralCode: string;

  /** Callback when registration is successful */
  onRegistrationSuccess?: (user: User) => void;

  /** Callback when visitor signs in instead */
  onSignIn?: () => void;

  /** Custom offer override */
  customOffer?: ReferralOffer;

  /** Enable social login */
  enableSocialLogin?: boolean;
}
```

### Angular

```typescript
@Component({
  selector: 'app-referral-landing-page',
  templateUrl: './referral-landing-page.component.html',
  styleUrls: ['./referral-landing-page.component.scss']
})
export class ReferralLandingPageComponent {
  @Input() referralCode!: string;
  @Input() customOffer?: ReferralOffer;
  @Input() enableSocialLogin = true;

  @Output() registrationSuccess = new EventEmitter<User>();
  @Output() signIn = new EventEmitter<void>();
}
```

## Data Models

### Referral Data

```typescript
interface ReferralLandingData {
  referralCode: string;
  referrer: {
    id: string;
    firstName: string;
    lastName: string;
    avatar?: string;
  };
  professional: {
    id: string;
    businessName: string;
    avatar?: string;
    address: Address;
    phone: string;
    email: string;
    website?: string;
    hours: BusinessHours;
    rating: number;
    reviewCount: number;
    specialties: string[];
    description: string;
    yearsInBusiness?: number;
    certifications?: string[];
  };
  offer: ReferralOffer;
  isValid: boolean;
  expiresAt: Date;
}

interface ReferralOffer {
  type: 'percentage' | 'fixed' | 'credit';
  value: number;
  description: string;
  minimumPurchase?: number;
  referrerReward: {
    type: 'credit' | 'cash' | 'points';
    value: number;
  };
  terms?: string;
}

interface BusinessHours {
  monday?: { open: string; close: string };
  tuesday?: { open: string; close: string };
  wednesday?: { open: string; close: string };
  thursday?: { open: string; close: string };
  friday?: { open: string; close: string };
  saturday?: { open: string; close: string };
  sunday?: { open: string; close: string };
}

interface RegistrationFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
  referralCode: string;
  marketingConsent?: boolean;
}
```

## State Management

```typescript
interface ReferralLandingState {
  // Referral data
  referralData: ReferralLandingData | null;
  isLoading: boolean;
  error: string | null;

  // Form data
  formData: RegistrationFormData;
  formErrors: Record<string, string>;
  touchedFields: Record<string, boolean>;

  // Registration
  isSubmitting: boolean;
  registrationError: string | null;

  // Countdown
  timeRemaining: {
    days: number;
    hours: number;
    minutes: number;
    seconds: number;
  };
  isExpired: boolean;

  // Social login
  socialLoginProvider: 'google' | 'facebook' | null;
  isSocialLoginLoading: boolean;
}
```

## Visual Specifications

### Layout

- **Header**: Minimal, fixed, 65px height
- **Hero**: Full-width gradient, centered content
- **Content**: 2-column grid (professional card | registration form)
- **Max Width**: 1100px for content area

### Hero Section

```
┌─────────────────────────────────────────────────────────────────────┐
│                     [Gradient Background]                           │
│                                                                     │
│             ┌──────────────────────────────┐                        │
│             │ [Avatar] Referred by John D. │                        │
│             └──────────────────────────────┘                        │
│                                                                     │
│     You've Been Referred to Mike's Auto Repair!                     │
│                                                                     │
│   John thought you'd love our service. Sign up and earn rewards!    │
│                                                                     │
│           ┌─────────────────────────────────┐                       │
│           │  15% OFF  │ Your First Service  │                       │
│           │           │ Plus $25 for John   │                       │
│           └─────────────────────────────────┘                       │
└─────────────────────────────────────────────────────────────────────┘
```

### Color Scheme

| Element | Color |
|---------|-------|
| Hero gradient | Primary → Primary Dark |
| Offer banner | Accent (yellow) |
| CTA button | Primary |
| Trust badges | Success (green check) |
| Countdown | Error (red) if < 24h |

### Offer Display

| Offer Type | Display Format |
|------------|----------------|
| Percentage | "15% OFF" |
| Fixed | "$50 OFF" |
| Credit | "$25 CREDIT" |

### Countdown Timer

```typescript
const formatCountdown = (remaining: TimeRemaining): string => {
  if (remaining.days > 0) {
    return `${remaining.days} days, ${remaining.hours} hours`;
  }
  if (remaining.hours > 0) {
    return `${remaining.hours}h ${remaining.minutes}m`;
  }
  return `${remaining.minutes}m ${remaining.seconds}s`;
};
```

## Form Validation

### Validation Rules

```typescript
const validationRules = {
  firstName: {
    required: 'First name is required',
    minLength: { value: 2, message: 'At least 2 characters' },
    pattern: { value: /^[a-zA-Z\s'-]+$/, message: 'Invalid characters' },
  },
  lastName: {
    required: 'Last name is required',
    minLength: { value: 2, message: 'At least 2 characters' },
    pattern: { value: /^[a-zA-Z\s'-]+$/, message: 'Invalid characters' },
  },
  email: {
    required: 'Email is required',
    pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Invalid email' },
  },
  phone: {
    required: 'Phone number is required',
    pattern: { value: /^\(\d{3}\) \d{3}-\d{4}$/, message: 'Format: (555) 123-4567' },
  },
  password: {
    required: 'Password is required',
    minLength: { value: 8, message: 'At least 8 characters' },
    pattern: {
      value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
      message: 'Must include uppercase, lowercase, and number',
    },
  },
};
```

### Phone Number Formatting

```typescript
const formatPhoneNumber = (value: string): string => {
  const cleaned = value.replace(/\D/g, '');
  if (cleaned.length <= 3) return cleaned;
  if (cleaned.length <= 6) return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3)}`;
  return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6, 10)}`;
};
```

## API Integration

### Validate Referral Code

```typescript
// GET /api/v1/referrals/validate/:code
interface ValidateReferralResponse {
  data: ReferralLandingData;
  isValid: boolean;
  message?: string;
}
```

### Register with Referral

```typescript
// POST /api/v1/auth/register
interface RegisterWithReferralRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
  referralCode: string;
  marketingConsent?: boolean;
}

interface RegisterWithReferralResponse {
  data: {
    user: User;
    token: string;
    referralApplied: boolean;
    discount: ReferralOffer;
  };
  message: string;
}
```

### Social Login with Referral

```typescript
// POST /api/v1/auth/social/:provider
interface SocialLoginRequest {
  token: string;
  referralCode: string;
}

interface SocialLoginResponse {
  data: {
    user: User;
    token: string;
    isNewUser: boolean;
    referralApplied: boolean;
  };
}
```

### Track Visit

```typescript
// POST /api/v1/referrals/:code/visit
interface TrackVisitRequest {
  userAgent: string;
  referrer?: string;
  utmParams?: Record<string, string>;
}
```

## Countdown Hook

```typescript
const useCountdown = (expiresAt: Date) => {
  const [timeRemaining, setTimeRemaining] = useState<TimeRemaining | null>(null);
  const [isExpired, setIsExpired] = useState(false);

  useEffect(() => {
    const calculateRemaining = () => {
      const now = new Date();
      const diff = expiresAt.getTime() - now.getTime();

      if (diff <= 0) {
        setIsExpired(true);
        return null;
      }

      return {
        days: Math.floor(diff / (1000 * 60 * 60 * 24)),
        hours: Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
        minutes: Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60)),
        seconds: Math.floor((diff % (1000 * 60)) / 1000),
      };
    };

    const interval = setInterval(() => {
      const remaining = calculateRemaining();
      setTimeRemaining(remaining);
    }, 1000);

    return () => clearInterval(interval);
  }, [expiresAt]);

  return { timeRemaining, isExpired };
};
```

## Social Login Integration

### Google OAuth

```typescript
const handleGoogleLogin = async () => {
  setSocialLoginProvider('google');
  setIsSocialLoginLoading(true);

  try {
    const googleUser = await googleAuth.signIn();
    const response = await api.socialLogin('google', {
      token: googleUser.getAuthResponse().id_token,
      referralCode,
    });

    onRegistrationSuccess(response.data.user);
  } catch (error) {
    setRegistrationError('Google login failed. Please try again.');
  } finally {
    setIsSocialLoginLoading(false);
    setSocialLoginProvider(null);
  }
};
```

### Facebook OAuth

```typescript
const handleFacebookLogin = async () => {
  setSocialLoginProvider('facebook');
  setIsSocialLoginLoading(true);

  try {
    const fbResponse = await FB.login({ scope: 'email,public_profile' });
    const response = await api.socialLogin('facebook', {
      token: fbResponse.authResponse.accessToken,
      referralCode,
    });

    onRegistrationSuccess(response.data.user);
  } catch (error) {
    setRegistrationError('Facebook login failed. Please try again.');
  } finally {
    setIsSocialLoginLoading(false);
    setSocialLoginProvider(null);
  }
};
```

## Invalid/Expired Referral States

### Invalid Code

```typescript
{
  icon: 'x-circle',
  title: 'Invalid Referral Link',
  description: 'This referral link is no longer valid or may have been entered incorrectly.',
  actions: [
    { label: 'Browse Professionals', href: '/directory' },
    { label: 'Sign Up Anyway', href: '/register' },
  ]
}
```

### Expired Offer

```typescript
{
  icon: 'clock',
  title: 'Offer Expired',
  description: 'This referral offer has expired, but you can still sign up and explore our services.',
  actions: [
    { label: 'Sign Up', href: '/register' },
    { label: 'View Professional', href: `/professionals/${professionalId}` },
  ]
}
```

## SEO & Meta Tags

```typescript
const generateMetaTags = (data: ReferralLandingData) => ({
  title: `Special Offer from ${data.professional.businessName} | AutoService`,
  description: `${data.referrer.firstName} referred you to ${data.professional.businessName}. Claim your ${formatOffer(data.offer)} discount now!`,
  ogImage: data.professional.avatar || '/default-og-image.png',
  ogType: 'website',
  canonical: `https://autoservice.com/r/${data.referralCode}`,
});
```

## Analytics Tracking

```typescript
// Track page view
analytics.track('referral_landing_view', {
  referralCode,
  referrerId: referralData.referrer.id,
  professionalId: referralData.professional.id,
  offerType: referralData.offer.type,
  offerValue: referralData.offer.value,
});

// Track registration start
analytics.track('referral_registration_start', {
  referralCode,
  method: 'form' | 'google' | 'facebook',
});

// Track registration complete
analytics.track('referral_registration_complete', {
  referralCode,
  userId: user.id,
  offerApplied: true,
});
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Form labels are properly associated
- [ ] Error messages use aria-describedby
- [ ] Countdown is announced to screen readers
- [ ] Social login buttons have accessible names
- [ ] Color contrast meets 4.5:1 minimum
- [ ] Focus states are visible

### Keyboard Navigation

- Tab through all form fields
- Enter to submit form
- Social buttons are focusable

### Screen Reader Support

```html
<form aria-labelledby="registration-heading">
  <h2 id="registration-heading">Claim Your Referral Discount</h2>

  <div role="timer" aria-live="polite" aria-label="Offer expires in">
    6 days, 23 hours remaining
  </div>

  <div role="status" aria-live="polite">
    <!-- Form submission status -->
  </div>
</form>
```

## Responsive Behavior

### Desktop (≥1024px)
- 2-column layout (professional card | form)
- Hero with full gradient
- Side-by-side social buttons

### Tablet (768px - 1023px)
- Single column, professional card above form
- Slightly reduced hero height
- Full-width elements

### Mobile (<768px)
- Stacked layout
- Condensed professional info
- Full-width form
- Sticky CTA button at bottom

## Performance Optimization

### Loading Strategy

1. SSR/SSG the page with referral data
2. Lazy load social login SDKs
3. Preload professional avatar
4. Inline critical CSS

### Image Optimization

```typescript
// Next.js example
<Image
  src={professional.avatar}
  alt={professional.businessName}
  width={80}
  height={80}
  priority
/>
```

## Testing Requirements

### Unit Tests

```typescript
describe('ReferralLandingPage', () => {
  it('renders referral data', () => {});
  it('displays professional information', () => {});
  it('shows countdown timer', () => {});
  it('validates form fields', () => {});
  it('handles form submission', () => {});
  it('handles social login', () => {});
  it('displays expired state', () => {});
  it('displays invalid code state', () => {});
  it('tracks analytics events', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Referral Landing Flow', () => {
  it('completes registration from referral link', () => {});
  it('applies discount after registration', () => {});
  it('handles social login with referral', () => {});
  it('shows error for expired referral', () => {});
});
```

## Dependencies

- Form validation library
- OAuth SDKs (Google, Facebook)
- Analytics library
- Countdown timer utility
- Phone number formatting
- SEO/meta tag management

## Related Components

- `CustomerRegistration` - Base registration form
- `SocialLoginButtons` - OAuth integration
- `ProfessionalProfileCard` - Professional display
- `CountdownTimer` - Reusable countdown

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
