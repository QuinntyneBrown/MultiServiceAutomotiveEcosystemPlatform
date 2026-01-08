# Tenant Not Found Page Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-MT-F009` |
| **Component Name** | TenantNotFoundPage |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A dedicated error page displayed when a user attempts to access a tenant subdomain that doesn't exist or is no longer active. This page provides a friendly user experience while guiding users to the main platform.

## User Stories

1. **As a visitor**, I want to see a clear message when I visit an invalid network URL, so I understand what went wrong.
2. **As a visitor**, I want easy navigation to the main platform, so I can find legitimate services.
3. **As a platform admin**, I want invalid tenant access logged, so I can monitor potential issues.

## Component Structure

```
TenantNotFoundPage/
├── index.ts                    # Public exports
├── TenantNotFoundPage.tsx      # Main component (React)
├── TenantNotFoundPage.component.ts   # Main component (Angular)
├── TenantNotFoundPage.module.scss    # Styles
├── TenantNotFoundPage.test.tsx       # Unit tests
└── components/
    ├── ErrorIcon/              # Animated question mark icon
    └── HelpSection/            # Help tips section
```

## Props / Inputs

### React

```typescript
interface TenantNotFoundPageProps {
  /** The invalid subdomain that was accessed */
  attemptedSubdomain?: string;

  /** URL to redirect to main platform */
  mainPlatformUrl?: string;

  /** URL for support contact */
  supportUrl?: string;

  /** Custom error message (optional) */
  customMessage?: string;

  /** Callback when user clicks main platform button */
  onNavigateToMain?: () => void;

  /** Callback when user clicks support button */
  onContactSupport?: () => void;
}
```

### Angular

```typescript
@Component({
  selector: 'app-tenant-not-found-page',
  templateUrl: './tenant-not-found-page.component.html',
  styleUrls: ['./tenant-not-found-page.component.scss']
})
export class TenantNotFoundPageComponent {
  @Input() attemptedSubdomain?: string;
  @Input() mainPlatformUrl: string = 'https://www.platform.com';
  @Input() supportUrl: string = 'https://www.platform.com/contact';
  @Input() customMessage?: string;

  @Output() navigateToMain = new EventEmitter<void>();
  @Output() contactSupport = new EventEmitter<void>();
}
```

## State Management

This component is primarily presentational with minimal state:

```typescript
interface TenantNotFoundState {
  isLoading: boolean;  // For any async operations (logging)
}
```

## Visual Specifications

### Layout

- **Container**: Centered vertically and horizontally within viewport
- **Max Width**: 560px
- **Padding**: 48px (desktop), 32px (mobile)
- **Border Radius**: 12px (--radius-lg)

### Colors (from Style Guide)

| Element | Color Token | Hex Value |
|---------|-------------|-----------|
| Background | --color-gray-50 | #FAFAFA |
| Card Background | --color-white | #FFFFFF |
| Error Icon Background | --color-info-light | #E1F5FE |
| Error Icon | --color-info | #0288D1 |
| Title Text | --color-gray-900 | #1A1A1A |
| Description Text | --color-gray-600 | #666666 |
| Primary Button | --color-primary | #00529F |
| Primary Button Hover | --color-primary-dark | #003366 |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Error Code | Montserrat | 12px | 600 |
| Title | Montserrat | 32px (desktop) / 24px (mobile) | 700 |
| Description | Lato | 16px | 400 |
| Button Text | Lato | 16px | 600 |
| Help Title | Montserrat | 14px | 600 |
| Help List | Lato | 14px | 400 |

### Spacing

| Element | Spacing |
|---------|---------|
| Icon to Error Code | 32px |
| Error Code to Title | 12px |
| Title to Description | 16px |
| Description to Buttons | 32px |
| Button Gap | 16px |
| Help Section Top | 32px |

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] All text meets 4.5:1 contrast ratio
- [ ] Focus indicators visible on all interactive elements
- [ ] Page title accurately describes the error
- [ ] Semantic HTML structure (header, main, footer)
- [ ] ARIA labels on icon elements

### Keyboard Navigation

- Tab order: Primary Button → Secondary Button → Footer Links
- Enter/Space activates buttons
- Focus trap not needed (not a modal)

### Screen Reader Support

```html
<!-- Example ARIA implementation -->
<main role="main" aria-labelledby="error-title">
  <div role="img" aria-label="Question mark icon indicating page not found">
    <!-- Icon SVG -->
  </div>
  <p id="error-code" aria-live="polite">Error 404</p>
  <h1 id="error-title">Network Not Found</h1>
  <p id="error-description">...</p>
</main>
```

## SEO Requirements

- **Meta robots**: `noindex, nofollow` (error pages should not be indexed)
- **HTTP Status**: Should return 404 status code
- **Canonical**: None (error page)
- **Title**: "Network Not Found - [Platform Name]"

## Error Logging

The component should log invalid tenant access attempts:

```typescript
interface TenantAccessLog {
  timestamp: Date;
  attemptedSubdomain: string;
  userAgent: string;
  referrer: string | null;
  ipAddress: string;  // Server-side only
}
```

## Responsive Behavior

### Desktop (≥992px)
- Full-width card with 48px padding
- Side-by-side footer links

### Tablet (768px - 991px)
- Card width: 90%
- Maintain horizontal button layout

### Mobile (<768px)
- Card width: 100% (with 16px margins)
- Stack buttons vertically
- Reduce icon size (100px → 80px)
- Reduce title size (32px → 24px)

## Animation Specifications

### Error Icon
- Subtle pulse animation on load
- Duration: 2s
- Iteration: 2

```css
@keyframes pulse {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); }
}
```

### Page Load
- Fade in animation
- Duration: 300ms
- Timing: ease-out

## Testing Requirements

### Unit Tests

```typescript
describe('TenantNotFoundPage', () => {
  it('should render error message', () => {});
  it('should display attempted subdomain if provided', () => {});
  it('should navigate to main platform on button click', () => {});
  it('should navigate to support on secondary button click', () => {});
  it('should be accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Tenant Not Found', () => {
  it('should display 404 page for invalid subdomain', () => {});
  it('should redirect to main platform from button', () => {});
  it('should log invalid access attempt', () => {});
});
```

## Dependencies

- None (standalone page component)

## Related Components

- `Header` - Platform header component
- `Footer` - Platform footer component
- `Button` - Shared button component

## Implementation Notes

### React Implementation

```tsx
import React from 'react';
import styles from './TenantNotFoundPage.module.scss';

export const TenantNotFoundPage: React.FC<TenantNotFoundPageProps> = ({
  attemptedSubdomain,
  mainPlatformUrl = 'https://www.platform.com',
  supportUrl = 'https://www.platform.com/contact',
  customMessage,
  onNavigateToMain,
  onContactSupport,
}) => {
  useEffect(() => {
    // Log invalid access attempt
    logInvalidTenantAccess(attemptedSubdomain);
  }, [attemptedSubdomain]);

  return (
    <div className={styles.container}>
      <header className={styles.header}>
        {/* Logo */}
      </header>

      <main className={styles.main} role="main">
        <div className={styles.errorCard}>
          <div className={styles.errorIcon} role="img" aria-label="Page not found">
            <QuestionIcon />
          </div>

          <p className={styles.errorCode}>Error 404</p>
          <h1 className={styles.errorTitle}>Network Not Found</h1>
          <p className={styles.errorDescription}>
            {customMessage || 'The service network you\'re looking for doesn\'t exist...'}
          </p>

          <div className={styles.actions}>
            <Button
              variant="primary"
              href={mainPlatformUrl}
              onClick={onNavigateToMain}
            >
              Go to Main Platform
            </Button>
            <Button
              variant="secondary"
              href={supportUrl}
              onClick={onContactSupport}
            >
              Contact Support
            </Button>
          </div>

          <HelpSection />
        </div>
      </main>

      <footer className={styles.footer}>
        {/* Footer links */}
      </footer>
    </div>
  );
};
```

### Angular Implementation

```typescript
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { LoggingService } from '@core/services/logging.service';

@Component({
  selector: 'app-tenant-not-found-page',
  templateUrl: './tenant-not-found-page.component.html',
  styleUrls: ['./tenant-not-found-page.component.scss']
})
export class TenantNotFoundPageComponent implements OnInit {
  @Input() attemptedSubdomain?: string;
  @Input() mainPlatformUrl = 'https://www.platform.com';
  @Input() supportUrl = 'https://www.platform.com/contact';
  @Input() customMessage?: string;

  @Output() navigateToMain = new EventEmitter<void>();
  @Output() contactSupport = new EventEmitter<void>();

  constructor(private loggingService: LoggingService) {}

  ngOnInit(): void {
    this.loggingService.logInvalidTenantAccess(this.attemptedSubdomain);
  }

  onMainClick(): void {
    this.navigateToMain.emit();
    window.location.href = this.mainPlatformUrl;
  }

  onSupportClick(): void {
    this.contactSupport.emit();
    window.location.href = this.supportUrl;
  }
}
```

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
