# Tenant Not Found Page Specification

## Overview

**Requirement ID:** REQ-MT-F009
**Phase:** 1
**Priority:** P0

The Tenant Not Found page is displayed when a user attempts to access an invalid or non-existent tenant subdomain. This page provides a friendly error message and helps users find their intended destination.

---

## Page Purpose

- Display a clear, friendly "network not found" message
- Provide navigation to the main platform
- Allow users to enter a network code to access their intended network
- Maintain brand consistency while being clearly an error state
- Ensure the page is not indexed by search engines (noindex)

---

## Component Architecture

```
TenantNotFoundPage/
├── tenant-not-found.ts          # Component class
├── tenant-not-found.html        # Template
├── tenant-not-found.scss        # Styles
└── index.ts                     # Barrel export
```

---

## Component Structure

### Angular Component

```typescript
@Component({
  selector: 'app-tenant-not-found',
  templateUrl: './tenant-not-found.html',
  styleUrls: ['./tenant-not-found.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class TenantNotFound {
  private readonly router = inject(Router);
  private readonly analyticsService = inject(AnalyticsService);

  networkCodeForm = new FormGroup({
    code: new FormControl('', [Validators.required, Validators.minLength(3)])
  });

  isSubmitting = false;

  ngOnInit(): void {
    // Log invalid tenant access attempt
    this.analyticsService.trackEvent('tenant_not_found', {
      attemptedUrl: window.location.href,
      referrer: document.referrer
    });
  }

  onSubmitNetworkCode(): void {
    if (this.networkCodeForm.valid) {
      const code = this.networkCodeForm.value.code?.trim().toLowerCase();
      // Redirect to the network subdomain
      window.location.href = `https://${code}.platform.com`;
    }
  }

  navigateToMainSite(): void {
    window.location.href = 'https://www.platform.com';
  }

  navigateToSupport(): void {
    window.location.href = 'https://www.platform.com/support';
  }
}
```

---

## UI Sections

### 1. Header
- **Content:** Platform logo and name
- **Behavior:** Links to main platform site
- **Styling:** White background with subtle shadow

### 2. Error Content Area
- **Icon:** Search/magnifying glass with plus icon (indicating "not found")
- **Title:** "Network Not Found" (h1)
- **Subtitle:** Explanatory text about the issue
- **Actions:**
  - Primary button: "Visit Main Site"
  - Secondary button: "Contact Support"

### 3. Network Code Entry Card
- **Title:** "Know your network code?"
- **Description:** Explanatory text
- **Input:** Text field for network code
- **Action:** "Go" button

### 4. Footer
- **Content:** Support contact information
- **Links:** Support email/phone, copyright

---

## Responsive Behavior

### Mobile (< 576px)
- Single column layout
- Smaller icon (96px)
- Stacked buttons (full width)
- Stacked input and button in code entry
- Reduced padding

### Tablet (768px - 991px)
- Centered content with max-width
- Side-by-side buttons
- Inline code entry (input + button)

### Desktop (≥ 992px)
- Same as tablet
- Larger icon (120px)
- More generous spacing

---

## Accessibility Requirements

| Requirement | Implementation |
|-------------|----------------|
| Page title | Descriptive title for screen readers |
| Heading hierarchy | Proper h1 → h2 structure |
| Form labels | aria-label on input field |
| Focus management | Visible focus states on all interactive elements |
| Color contrast | Meets WCAG AA (4.5:1 for text) |
| Keyboard navigation | All actions accessible via keyboard |

---

## SEO Considerations

```html
<meta name="robots" content="noindex, nofollow">
```

- Page should NOT be indexed by search engines
- Canonical URL should not be set
- Return appropriate HTTP status code (404) from server

---

## Analytics Events

| Event Name | Trigger | Data |
|------------|---------|------|
| `tenant_not_found` | Page load | `attemptedUrl`, `referrer` |
| `network_code_submit` | Code form submission | `enteredCode` |
| `main_site_click` | Main site button click | - |
| `support_click` | Support button click | - |

---

## State Management

This page is stateless and does not require external state management. The only local state is:

- `networkCodeForm`: Reactive form for code input
- `isSubmitting`: Loading state for form submission

---

## Dependencies

| Dependency | Purpose |
|------------|---------|
| `@angular/common` | CommonModule for directives |
| `@angular/forms` | ReactiveFormsModule for form handling |
| `@angular/router` | RouterModule for navigation (if needed) |
| `AnalyticsService` | Custom service for event tracking |

---

## Error Handling

- Invalid network code format: Show validation error
- Network code submission failure: Show error toast
- Navigation failures: Fallback to window.location redirect

---

## Testing Checklist

### Unit Tests
- [ ] Component renders correctly
- [ ] Form validation works
- [ ] Analytics events are tracked
- [ ] Navigation functions work

### Integration Tests
- [ ] Links navigate to correct URLs
- [ ] Form submission redirects correctly

### Visual Tests
- [ ] Mobile layout renders correctly
- [ ] Tablet layout renders correctly
- [ ] Desktop layout renders correctly
- [ ] Focus states are visible
- [ ] Color contrast passes

### Accessibility Tests
- [ ] Screen reader announces content correctly
- [ ] Keyboard navigation works
- [ ] Focus order is logical

---

## Design Tokens Used

```scss
// Colors
$bg-gradient: linear-gradient(135deg, var(--color-blue-50), var(--color-bg-primary));
$icon-bg: var(--color-blue-100);
$icon-color: var(--color-blue-700);
$title-color: var(--color-gray-900);
$subtitle-color: var(--color-gray-600);
$footer-color: var(--color-gray-500);

// Spacing
$content-padding-mobile: var(--spacing-6);
$content-padding-desktop: var(--spacing-8);
$icon-size-mobile: 96px;
$icon-size-desktop: 120px;
$card-padding-mobile: var(--spacing-6);
$card-padding-desktop: var(--spacing-8);

// Typography
$title-size-mobile: var(--text-h2);
$title-size-desktop: var(--text-display-sm);
$subtitle-size: var(--text-body-lg);
```

---

## File Checklist

- [ ] `tenant-not-found.ts` - Component class
- [ ] `tenant-not-found.html` - Template
- [ ] `tenant-not-found.scss` - Styles
- [ ] `tenant-not-found.spec.ts` - Unit tests
- [ ] `index.ts` - Barrel export
