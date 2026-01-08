# Referral Landing Page Specification

**Page:** Referral Landing Page
**Route:** `/referral/:code` or `/join/:code`
**Access:** Public (No Authentication Required)
**Requirements:** REQ-RF-F015, REQ-RF-F016
**Priority:** P0
**Phase:** 1

---

## Overview

The Referral Landing Page is a public-facing page designed to welcome new customers who have been invited to join the AutoPro Network platform through a referral link. The page showcases the platform benefits, highlights the referral discount, displays information about the referring professional or customer, and provides a streamlined signup process with the referral code pre-applied.

---

## Page Objectives

1. **Convert Referrals**: Maximize conversion of referred visitors into registered customers
2. **Build Trust**: Showcase platform credibility through professional profiles and testimonials
3. **Highlight Value**: Clearly communicate the referral discount and platform benefits
4. **Streamline Signup**: Make registration quick and easy with pre-filled referral codes
5. **Engage Visitors**: Use compelling visuals and copy to motivate action

---

## Page URL Structure

### URL Patterns

```
/referral/:referralCode
/join/:referralCode
/ref/:referralCode

Examples:
https://autopro.network/referral/SARAH2024
https://autopro.network/join/MIKE2024
https://autopro.network/ref/PROMO10
```

### URL Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `referralCode` | string | Yes | Unique referral code identifying the referrer |

### Query Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `utm_source` | string | Marketing attribution source |
| `utm_medium` | string | Marketing attribution medium |
| `utm_campaign` | string | Marketing campaign identifier |
| `professional` | string | Professional ID (if professional-specific referral) |

---

## Page Sections

### 1. Header (Public Version)

**Purpose**: Provide navigation and branding for non-authenticated visitors.

**Components**:
- Platform logo and name
- "Sign In" button (for existing users)
- No authenticated user menu

**Styling**:
- Background: White (`--color-bg-primary`)
- Sticky positioning
- Box shadow for elevation
- CarMax blue branding (`#00529F`)

---

### 2. Hero Section

**Purpose**: Immediately capture attention and communicate the referral offer.

**Content**:
- **Badge**: "Special Invitation" with star icon
- **Headline**: "[Referrer Name] invited you to join AutoPro Network!"
  - Referrer name highlighted in accent yellow (`#FFD520`)
- **Subtitle**: Brief platform description (1-2 sentences)
- **Discount Highlight**: Large, eye-catching display of the offer
  - Example: "Get 10% OFF your first service!"
  - Rotated design element for visual interest
- **Call-to-Action Buttons**:
  - Primary: "Claim Your Discount" (scrolls to signup)
  - Secondary: "Learn More" (scrolls to how-it-works)

**Styling**:
- Background: Gradient from primary blue to darker blue (`#00529F` to `#001F3F`)
- White text for contrast
- Decorative background elements (circles, patterns)
- Large typography for impact

**Responsive Behavior**:
- Desktop: Full-width hero with side decorations
- Tablet: Reduce padding, smaller typography
- Mobile: Stack elements vertically, reduce font sizes

---

### 3. About Section

**Purpose**: Explain what AutoPro Network is and why it's valuable.

**Content**:
- **Overline**: "ABOUT AUTOPRO NETWORK" (small caps, blue)
- **Title**: "Your Trusted Automotive Service Platform"
- **Description**: 2-3 paragraph explanation of the platform
- **Benefits Grid**: 4 benefit cards with icons

**Benefits to Highlight**:

1. **Verified Professionals**
   - Icon: Checkmark shield
   - Description: Thoroughly vetted and certified service providers

2. **Convenient Scheduling**
   - Icon: Calendar/clock
   - Description: Flexible booking options that fit your schedule

3. **Quality Guaranteed**
   - Icon: Star/badge
   - Description: Satisfaction guarantee and transparent pricing

4. **Rewards Program**
   - Icon: Gift/rewards
   - Description: Earn points and refer friends for discounts

**Styling**:
- Background: White
- Grid layout (4 columns desktop, 2 tablet, 1 mobile)
- Icons with blue gradient backgrounds
- Clean, readable typography

---

### 4. Professional Spotlight Section

**Purpose**: Build trust by showcasing the referring professional or featured professionals.

**Two Variants**:

#### A. Specific Professional Referral (Featured Card)

Display when referral code is linked to a specific professional.

**Content**:
- **Header**:
  - Professional avatar/photo
  - "Top Rated" badge
  - Name and title
  - Specialty
  - Location with pin icon
- **Stats Row**:
  - Years of experience
  - Star rating
  - Number of customers
- **Biography**: 2-3 sentences about the professional
- **Specialties**: Chip list of services offered

**Styling**:
- Large featured card (600px max-width, centered)
- Blue gradient header with white text
- White card body
- Box shadow for elevation
- Rounded corners

#### B. General Platform Referral (Professional Grid)

Display when referral code is for general platform signup (customer-to-customer referral).

**Content**:
- Grid of 3-6 featured professionals
- Each card shows:
  - Avatar
  - Name
  - Specialty
  - Star rating
  - Location

**Styling**:
- Responsive grid (3 columns desktop, 2 tablet, 1 mobile)
- Hover effects (lift on hover)
- Consistent card design

---

### 5. How It Works Section

**Purpose**: Clearly explain the signup and service process in simple steps.

**Content**:

**Step 1: Sign Up**
- Icon: User/people icon
- Title: "Sign Up"
- Description: "Create your free account using the referral code to unlock your exclusive discount."

**Step 2: Get Service**
- Icon: Calendar icon
- Title: "Get Service"
- Description: "Browse professionals, book your first service, and enjoy quality automotive care."

**Step 3: Both Earn Rewards**
- Icon: Star/trophy icon
- Title: "Both Earn Rewards"
- Description: "You save on your service, and your referrer earns rewards. Everyone wins!"

**Styling**:
- White background
- Horizontal layout with connecting line between steps (desktop)
- Vertical stacked layout (mobile)
- Numbered circles or icon badges
- Yellow accent color for icons
- Blue for step numbers

---

### 6. Sign Up Section

**Purpose**: Convert visitors into registered users with streamlined signup form.

**Content**:

#### A. Referral Code Display (When from Referral Link)

Pre-filled referral information display:
- Label: "Your Referral Code"
- Code: Display in monospace font (e.g., "SARAH2024")
- Referrer: "Referred by [Name]"
- Discount Badge: "10% OFF First Service"

**Styling**:
- Blue gradient background
- Yellow accent for code
- Read-only display (not editable)
- Prominent placement at top of form

#### B. Registration Form Fields

**Required Fields**:

| Field | Type | Validation | Placeholder |
|-------|------|------------|-------------|
| First Name | text | Required, 2-50 chars | "John" |
| Last Name | text | Required, 2-50 chars | "Doe" |
| Email | email | Required, valid email format | "john.doe@example.com" |
| Phone | tel | Required, valid phone format | "(555) 123-4567" |
| Password | password | Required, min 8 chars, complexity | "Minimum 8 characters" |
| Confirm Password | password | Required, must match password | "Re-enter your password" |

**Password Requirements**:
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 number
- Optional: 1 special character

**Checkboxes**:
- [ ] **Required**: "I agree to the Terms of Service and Privacy Policy"
- [ ] **Optional**: "Send me updates about special offers and promotions"

**Submit Button**:
- Text: "Create Account & Claim Discount"
- Full-width button
- Blue gradient background
- Large, prominent design

**Footer Links**:
- "Already have an account? Sign in"

#### C. Alternative: Referral Code Entry (When No Code in URL)

Display when user arrives without a referral code in URL.

**Content**:
- Section title: "Have a Referral Code?"
- Subtitle: "Enter it below to unlock exclusive discounts"
- Input field: Large text input for code entry
- Real-time validation with visual feedback

**Validation States**:

1. **Default State**:
   - Gray border
   - Placeholder: "ENTER CODE"

2. **Validating State** (during API check):
   - Show loading spinner
   - Border color unchanged

3. **Valid State**:
   - Green border
   - Success icon (checkmark)
   - Success message: "Code applied! Referred by [Name] - Get [X]% off your first service"
   - Light green background

4. **Invalid State**:
   - Red border
   - Error icon (X)
   - Error message: "Invalid referral code. Please check and try again."
   - Light red background

**Validation Logic**:
- Trigger validation after 4+ characters entered
- Debounce validation (1000ms delay)
- Show spinner during validation
- Display result with appropriate styling
- Auto-uppercase input

**Styling**:
- Separate section with top border
- Monospace font for code input
- Letter-spacing for readability
- Prominent validation messages

---

### 7. Footer (Public Version)

**Purpose**: Provide navigation, legal links, and social media connections.

**Content**:

**Column 1: Brand**
- Logo/brand name
- Tagline: "Connecting customers with trusted automotive professionals across the nation."

**Column 2: Services**
- Find Professionals
- Book Service
- Pricing
- Referral Program

**Column 3: Company**
- About Us
- Careers
- Blog
- Contact

**Column 4: Support**
- Help Center
- Safety
- Terms of Service
- Privacy Policy

**Bottom Row**:
- Copyright notice
- Social media links (Facebook, Twitter, Instagram)

**Styling**:
- Dark blue background (`#001F3F`)
- White text
- Grid layout (4 columns desktop, 2 tablet, 1 mobile)
- Divider line above bottom row

---

## User Flows

### Flow 1: Successful Referral Signup

1. User clicks referral link from email/text/social media
2. Lands on referral landing page with code in URL
3. Sees hero with referrer name and discount offer
4. Scrolls through benefits and professional spotlight
5. Reads "How It Works" section
6. Scrolls to signup form
7. Sees pre-filled referral code
8. Fills out registration form
9. Accepts terms and conditions
10. Clicks "Create Account & Claim Discount"
11. Account created, referral tracked
12. Redirected to customer dashboard
13. Sees welcome message and discount applied

### Flow 2: Signup Without Referral Code

1. User lands on page without code in URL (direct visit)
2. Sees general platform marketing (no specific referrer)
3. Scrolls to signup section
4. Sees "Have a Referral Code?" section
5. Enters referral code manually
6. System validates code in real-time
7. If valid: Shows success message with referrer name
8. Completes registration form
9. Account created with referral applied

### Flow 3: Invalid Referral Code Handling

1. User clicks referral link with invalid/expired code
2. Page loads with error notification
3. Referral code section shows error state
4. User can continue signup without code
5. Option to enter different code manually

### Flow 4: Existing User Click Referral Link

1. Authenticated user clicks referral link
2. Lands on referral page
3. Sees message: "You're already a member!"
4. Options:
   - "Go to Dashboard"
   - "Share this referral with a friend"
5. Referral code remains in URL for sharing

---

## State Management

### URL State

| State | Value | Source |
|-------|-------|--------|
| Referral Code | String | URL parameter `:code` |
| Professional ID | String | Query param `?professional=` |
| Campaign Info | Object | UTM query params |

### Component State

```javascript
{
  referralCode: string,           // Code from URL or user input
  referralData: {                 // Data fetched from API
    code: string,
    referrerName: string,
    referrerType: 'customer' | 'professional',
    professionalId: string | null,
    discountType: 'percentage' | 'fixed',
    discountValue: number,
    expiresAt: Date | null,
    isValid: boolean
  },
  validationState: 'idle' | 'validating' | 'valid' | 'invalid',
  formData: {
    firstName: string,
    lastName: string,
    email: string,
    phone: string,
    password: string,
    confirmPassword: string,
    agreeToTerms: boolean,
    marketingOptIn: boolean,
    manualReferralCode: string
  },
  errors: {
    [fieldName]: string | null
  },
  isSubmitting: boolean
}
```

---

## API Integration

### Validate Referral Code

**Endpoint**: `GET /api/referrals/validate/:code`

**Request**:
```javascript
GET /api/referrals/validate/SARAH2024
```

**Response (Success)**:
```json
{
  "isValid": true,
  "code": "SARAH2024",
  "referrer": {
    "id": "user_abc123",
    "name": "Sarah Johnson",
    "type": "professional",
    "professionalId": "prof_xyz789"
  },
  "discount": {
    "type": "percentage",
    "value": 10,
    "maxAmount": 50,
    "description": "10% off first service (up to $50)"
  },
  "expiresAt": "2026-12-31T23:59:59Z",
  "usageCount": 42,
  "maxUsages": null
}
```

**Response (Invalid)**:
```json
{
  "isValid": false,
  "code": "INVALID123",
  "error": "INVALID_CODE",
  "message": "This referral code is not valid or has expired."
}
```

### Get Professional Profile

**Endpoint**: `GET /api/professionals/:id/public`

**Response**:
```json
{
  "id": "prof_xyz789",
  "firstName": "Sarah",
  "lastName": "Johnson",
  "businessName": "Sarah's Auto Care",
  "specialty": "Certified Master Mechanic",
  "bio": "With over 15 years of experience...",
  "location": {
    "city": "Charlotte",
    "state": "NC",
    "zipCode": "28202"
  },
  "stats": {
    "yearsExperience": 15,
    "rating": 4.9,
    "reviewCount": 234,
    "completedJobs": 500
  },
  "specialties": [
    "Engine Repair",
    "Diagnostics",
    "Brake Service",
    "Oil Changes"
  ],
  "certifications": [
    "ASE Master Technician",
    "EPA 609 Certified"
  ],
  "badges": ["Top Rated", "Verified"]
}
```

### Create Customer Account

**Endpoint**: `POST /api/auth/register`

**Request**:
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "+15551234567",
  "password": "SecurePassword123!",
  "userType": "customer",
  "referralCode": "SARAH2024",
  "marketingOptIn": true,
  "utmParams": {
    "source": "email",
    "medium": "referral",
    "campaign": "spring2026"
  }
}
```

**Response (Success)**:
```json
{
  "success": true,
  "user": {
    "id": "user_new123",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "userType": "customer"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "referralApplied": {
    "code": "SARAH2024",
    "discount": {
      "type": "percentage",
      "value": 10
    }
  }
}
```

---

## Validation Rules

### Client-Side Validation

| Field | Rules | Error Messages |
|-------|-------|----------------|
| First Name | Required, 2-50 chars, letters only | "First name is required" / "First name must be 2-50 characters" |
| Last Name | Required, 2-50 chars, letters only | "Last name is required" / "Last name must be 2-50 characters" |
| Email | Required, valid email format | "Email is required" / "Please enter a valid email address" |
| Phone | Required, valid phone format | "Phone is required" / "Please enter a valid phone number" |
| Password | Required, min 8 chars, complexity | "Password is required" / "Password must be at least 8 characters with 1 uppercase, 1 lowercase, and 1 number" |
| Confirm Password | Required, matches password | "Please confirm your password" / "Passwords do not match" |
| Terms | Must be checked | "You must agree to the Terms of Service and Privacy Policy" |
| Referral Code | Optional, 4-20 chars, alphanumeric | "Invalid referral code format" |

### Server-Side Validation

- Email uniqueness check
- Phone number format validation
- Password strength verification
- Referral code validity check
- Rate limiting on signup attempts

---

## Accessibility Requirements

### WCAG 2.1 AA Compliance

**Keyboard Navigation**:
- All interactive elements must be keyboard accessible
- Logical tab order through form fields
- Visible focus indicators on all focusable elements
- Skip links to main content

**Screen Reader Support**:
- Proper ARIA labels on all form fields
- ARIA live regions for validation messages
- Semantic HTML structure (nav, main, section, footer)
- Alt text for all images and icons
- Form field labels properly associated

**Color Contrast**:
- Text on blue background: Minimum 4.5:1 ratio
- Form field labels: 4.5:1 ratio
- Error messages: 4.5:1 ratio
- Button text: 4.5:1 ratio

**Form Accessibility**:
- Label elements for all inputs
- Error messages linked to fields via aria-describedby
- Required fields marked with aria-required="true"
- Error states announced to screen readers

**Focus Management**:
- Focus ring on all interactive elements
- Focus trapped in modal (if any)
- Focus returned to trigger element after modal close

---

## Responsive Design

### Breakpoints

| Device | Breakpoint | Layout Changes |
|--------|------------|----------------|
| Mobile | < 576px | Single column, stacked elements, reduced padding |
| Tablet | 576px - 991px | 2-column grids, medium spacing |
| Desktop | ≥ 992px | Full layout, 3-4 column grids, optimal spacing |

### Mobile-Specific Considerations

**Hero Section**:
- Reduce headline font size (36px → 28px)
- Smaller discount badge
- Stack CTA buttons vertically
- Reduce vertical padding

**Benefits Grid**:
- Single column on mobile
- Smaller icons (48px → 40px)
- Reduced spacing

**Professional Card**:
- Full-width on mobile
- Larger tap targets (min 44x44px)
- Simplified layout

**Form**:
- Full-width inputs
- Larger form fields for touch (min 48px height)
- Stack first/last name on mobile
- Prominent submit button

**Footer**:
- Single column on mobile
- Collapsed sections with expand/collapse (optional)
- Stack social icons

---

## Performance Optimization

### Page Load Performance

**Critical Path**:
1. HTML + inline critical CSS
2. Referral code validation API call
3. Professional profile fetch (if applicable)
4. Form interaction ready

**Performance Targets**:
- First Contentful Paint (FCP): < 1.5s
- Largest Contentful Paint (LCP): < 2.5s
- Time to Interactive (TTI): < 3.5s
- Total Blocking Time (TBT): < 300ms

**Optimization Strategies**:
- Inline critical CSS for above-the-fold content
- Defer non-critical JavaScript
- Lazy load professional images
- Preconnect to API domain
- Use system fonts with web font fallbacks
- Compress images (WebP format)
- Minify HTML, CSS, JS
- Enable gzip/brotli compression
- Cache static assets (fonts, images)

### API Performance

- Cache referral validation response (5 minutes)
- Debounce manual code validation (1000ms)
- Implement request cancellation for rapid input changes
- Show loading states during API calls

---

## SEO Optimization

### Meta Tags

```html
<title>Join AutoPro Network - Exclusive Referral Discount | [Referrer Name] Invited You</title>
<meta name="description" content="You've been invited to join AutoPro Network! Get 10% off your first automotive service. Connect with trusted, certified professionals for all your vehicle needs.">
<meta name="robots" content="index, follow">
<link rel="canonical" href="https://autopro.network/referral/SARAH2024">

<!-- Open Graph -->
<meta property="og:type" content="website">
<meta property="og:title" content="Sarah Johnson invited you to AutoPro Network - Get 10% OFF!">
<meta property="og:description" content="Join thousands of satisfied customers getting quality automotive services from trusted professionals. Sign up now and claim your exclusive discount!">
<meta property="og:image" content="https://autopro.network/images/referral-share.jpg">
<meta property="og:url" content="https://autopro.network/referral/SARAH2024">

<!-- Twitter Card -->
<meta name="twitter:card" content="summary_large_image">
<meta name="twitter:title" content="Sarah Johnson invited you to AutoPro Network">
<meta name="twitter:description" content="Get 10% off your first service! Join AutoPro Network today.">
<meta name="twitter:image" content="https://autopro.network/images/referral-share.jpg">
```

### Structured Data

```json
{
  "@context": "https://schema.org",
  "@type": "WebPage",
  "name": "AutoPro Network Referral Program",
  "description": "Join AutoPro Network through a referral and get exclusive discounts on automotive services",
  "url": "https://autopro.network/referral/SARAH2024",
  "provider": {
    "@type": "Organization",
    "name": "AutoPro Network",
    "url": "https://autopro.network"
  },
  "offers": {
    "@type": "Offer",
    "description": "10% off first service",
    "priceSpecification": {
      "@type": "PriceSpecification",
      "valueAddedTaxIncluded": false
    }
  }
}
```

---

## Analytics Tracking

### Page Events

| Event | Trigger | Data |
|-------|---------|------|
| `referral_landing_view` | Page load | `referral_code`, `referrer_id`, `utm_params` |
| `referral_code_validated` | Code validation success | `code`, `referrer_name`, `discount_value` |
| `signup_form_started` | First field interaction | `referral_code` |
| `signup_form_field_completed` | Field blur with valid value | `field_name` |
| `signup_form_error` | Validation error | `field_name`, `error_type` |
| `referral_signup_completed` | Form submission success | `user_id`, `referral_code`, `discount_applied` |
| `cta_clicked` | CTA button click | `button_text`, `button_location`, `destination` |

### Conversion Tracking

**Conversion Goals**:
1. Referral page visit
2. Form started (first field interaction)
3. Form 50% complete
4. Form 100% complete
5. Account created

**Marketing Attribution**:
- Track UTM parameters from URL
- Associate with user account on signup
- Store in user profile for attribution reporting

---

## Security Considerations

### Input Sanitization

- Sanitize all form inputs on client and server
- Prevent XSS attacks through proper escaping
- Validate email format
- Sanitize phone number input

### CSRF Protection

- Include CSRF token in form
- Validate token on server-side

### Rate Limiting

- Limit signup attempts per IP (5 per hour)
- Limit referral code validation (10 per minute per IP)
- Implement captcha after failed attempts

### Password Security

- Enforce strong password requirements
- Hash passwords with bcrypt (cost factor 12+)
- Never log or display passwords
- Implement password strength indicator

### Referral Code Security

- Validate code format
- Check expiration dates
- Verify usage limits
- Prevent code abuse

---

## Error Handling

### Error States

| Error Type | Display | User Action |
|------------|---------|-------------|
| Invalid referral code in URL | Banner: "This referral code is invalid or expired" | Continue signup without code |
| Expired referral code | Banner: "This referral code has expired" | Continue signup without code |
| Email already exists | Inline error on email field | "Sign in" link provided |
| Weak password | Inline error with requirements | Update password |
| Network error | Toast notification: "Connection error" | Retry button |
| Server error | Modal: "We're experiencing technical difficulties" | Contact support link |
| Failed validation | Inline errors on fields | Fix and resubmit |

### Error Messages

**Style**: Friendly, helpful, actionable

**Examples**:
- "This email is already registered. Would you like to sign in instead?"
- "Please enter a valid phone number in the format (555) 123-4567"
- "Your password needs at least one uppercase letter"
- "We couldn't validate this referral code. You can continue signing up, or try a different code."

---

## Testing Requirements

### Functional Testing

**Scenarios**:
1. Valid referral code in URL
2. Invalid referral code in URL
3. No referral code in URL
4. Manual referral code entry (valid)
5. Manual referral code entry (invalid)
6. Form validation (all fields)
7. Successful signup
8. Email already exists
9. Password mismatch
10. Terms not accepted

### Browser Compatibility

**Supported Browsers**:
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile Safari (iOS 14+)
- Chrome Mobile (Android 10+)

### Device Testing

**Devices**:
- Desktop (1920x1080, 1366x768)
- Tablet (iPad, 768x1024)
- Mobile (iPhone 12, 390x844)
- Mobile (Pixel 5, 393x851)

### Accessibility Testing

- Keyboard navigation test
- Screen reader test (NVDA, JAWS, VoiceOver)
- Color contrast validation
- Focus indicator visibility
- ARIA attribute validation

---

## Success Metrics

### Key Performance Indicators (KPIs)

| Metric | Target | Measurement |
|--------|--------|-------------|
| Referral conversion rate | > 25% | Signups / Page visits |
| Form completion rate | > 60% | Form starts / Completions |
| Average time to signup | < 3 minutes | Timestamp tracking |
| Bounce rate | < 40% | Single-page sessions |
| Mobile conversion rate | > 20% | Mobile signups / Mobile visits |

### A/B Testing Opportunities

1. Hero headline variations
2. CTA button text ("Claim Discount" vs "Get Started" vs "Join Now")
3. Discount display format (badge vs banner vs callout)
4. Professional spotlight (featured card vs grid)
5. Form length (single page vs multi-step)

---

## Future Enhancements (Post-Phase 1)

1. **Video Testimonials**: Add video reviews from satisfied customers
2. **Trust Badges**: Display security certifications and guarantees
3. **Live Chat**: Implement chat widget for visitor questions
4. **Social Proof**: Show real-time signup notifications
5. **Multi-Language Support**: Translate page for non-English speakers
6. **Personalization**: Customize content based on referrer type
7. **Geolocation**: Show nearby professionals based on visitor location
8. **Progressive Web App**: Enable offline viewing and app-like experience
9. **Gamification**: Add progress bar for form completion
10. **Exit Intent**: Show retention popup on exit attempt

---

## Compliance Requirements

### Legal Requirements

**Terms of Service**:
- Link prominently in footer
- Require checkbox acceptance for signup
- Store acceptance timestamp

**Privacy Policy**:
- Link prominently in footer
- Explain data collection and usage
- GDPR/CCPA compliance

**Marketing Consent**:
- Separate checkbox for marketing emails
- Easy opt-out mechanism
- Honor unsubscribe requests

### Data Protection

**GDPR Compliance**:
- Clear consent for data processing
- Right to access data
- Right to deletion
- Data portability

**CCPA Compliance**:
- Privacy policy disclosure
- Opt-out option for data sale
- Consumer rights disclosure

---

## Maintenance & Updates

### Regular Updates

**Content Updates**:
- Refresh benefit descriptions quarterly
- Update featured professionals monthly
- Review and update copy based on user feedback

**Technical Updates**:
- Monitor and fix broken links
- Update dependencies and security patches
- Performance optimization based on metrics

**Design Updates**:
- A/B test variations quarterly
- Refresh visuals annually
- Update based on user research

---

## Documentation References

- [Platform Style Guide](/docs/specs/style-guide.md)
- [Referral System Frontend Requirements](/docs/specs/04-referral-system/frontend-requirements.md)
- [Customer Registration Specification](/docs/design/phase-1/pages/customer-registration/specification.md)
- [API Documentation](/docs/api/README.md)

---

**Version**: 1.0
**Last Updated**: January 2026
**Status**: Ready for Development
