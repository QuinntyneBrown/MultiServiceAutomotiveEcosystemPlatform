# Multi-Service Automotive Ecosystem Components

A component library for the Multi-Service Automotive Ecosystem Platform, built with Angular 21 and following strict design system guidelines.

## Overview

This library contains reusable components for the automotive platform, implementing Phase 1 component specifications with:
- **Design Tokens**: Consistent colors, spacing, and typography
- **Storybook**: Interactive component documentation
- **Unit Tests**: Comprehensive test coverage (>80%)
- **Accessibility**: WCAG 2.1 AA compliant
- **BEM Methodology**: Consistent CSS naming conventions

## Getting Started

### Installation

```bash
npm install
```

### Development

```bash
# Build the library
npm run build

# Run unit tests
npm run test

# Run tests with coverage
npm run test:coverage

# Launch Storybook
npm run storybook

# Build Storybook for deployment
npm run build-storybook
```

## Project Structure

```
src/lib/
├── styles/
│   └── _design-tokens.scss      # Platform design tokens
├── customer-login/               # ✅ Complete
│   ├── customer-login.ts         # Component logic
│   ├── customer-login.html       # Template
│   ├── customer-login.scss       # Styles (BEM)
│   ├── customer-login.spec.ts   # Unit tests (30 tests)
│   └── customer-login.stories.ts # Storybook stories (7 stories)
└── [other components...]         # To be implemented
```

## Design Tokens

The component library uses CSS custom properties for consistent design:

### Colors
- Primary Brand: `--color-primary` (#00529F)
- Accent: `--color-accent` (#FFD520)
- Semantic colors for success, error, warning, info
- Full grayscale and blue/yellow palettes

### Typography
- Primary Font: Sharp Sans / Montserrat
- Secondary Font: Lato / Roboto
- Responsive font sizes from 10px to 72px

### Spacing
- Base unit: 8px (`--spacing-2`)
- Scale from 0 to 128px
- Consistent padding and margins

### Component Styling
- Border radius: 4px - 24px
- Shadows: 6 levels of elevation
- Transitions: 150ms - 500ms

## Components

### Completed Components

#### customer-login (CM-F002) ✅
A secure login form with:
- Email/password authentication
- Form validation with error messages
- Password visibility toggle
- Remember me functionality
- Optional social login
- Loading states
- Responsive design
- **30 unit tests** with excellent coverage
- **7 Storybook stories** demonstrating all states

### Remaining Components (To be implemented)

Priority P0 (Critical):
- `customer-registration` (CM-F001)
- `tenant-not-found` (MT-F009)

Priority P1 (High):
- `customer-list-view` (CM-F006)
- `customer-detail-view` (CM-F007)
- `customer-form` (CM-F008)
- `professional-dashboard`
- `professional-directory`
- `professional-profile`
- `profile-management`
- `specialty-management`

Priority P2 (Medium):
- `customer-referral-dashboard`
- `professional-referral-dashboard`
- `share-referral`

## Component Development Guide

Each component should follow this structure:

### 1. Generate Component
```bash
npx ng generate component [component-name] --project=multi-service-automotive-ecosystem-components --standalone --skip-tests
```

### 2. Rename CSS to SCSS
```bash
mv component-name.css component-name.scss
```

### 3. Implement Component
- Add inputs and outputs per specification
- Use Angular signals for state management
- Follow standalone component pattern
- Use ReactiveFormsModule for forms

### 4. Style with BEM
```scss
@use '../styles/design-tokens';

.component-name {
  // Block styles
  
  &__element {
    // Element styles
  }
  
  &__element--modifier {
    // Modifier styles
  }
}
```

### 5. Write Unit Tests
- Create `.spec.ts` file
- Use vitest (`vi.spyOn` for mocking)
- Aim for >80% coverage
- Test all inputs, outputs, and interactions

### 6. Create Storybook Stories
```typescript
import type { Meta, StoryObj } from '@storybook/angular';

const meta: Meta<ComponentName> = {
  title: 'Components/Category/ComponentName',
  component: ComponentName,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<ComponentName>;

export const Default: Story = {
  args: {},
};
```

### 7. Export from Public API
```typescript
// src/public-api.ts
export * from './lib/component-name/component-name';
```

## Testing

### Running Tests
```bash
# Run all tests
npm run test

# Run tests in watch mode
npm run test -- --watch

# Run tests with coverage
npm run test -- --coverage
```

### Test Structure
- **Initialization**: Component creation and input binding
- **Validation**: Form validation and error handling
- **Interactions**: User actions and event emissions
- **State Management**: Signals and form state
- **Accessibility**: ARIA attributes and keyboard navigation

## Storybook

### Running Storybook
```bash
npm run storybook
```

Access at: http://localhost:6006

### Story Types
Each component should have stories for:
- **Default**: Standard state
- **With Data**: Populated with sample data
- **Error State**: Showing validation errors
- **Loading State**: During async operations
- **Mobile**: Responsive mobile view
- **Accessibility**: Testing with screen readers

## Code Quality Standards

### TypeScript
- Strict mode enabled
- No `any` types
- Proper interfaces for all data structures

### Styling
- BEM methodology required
- Design tokens for all values
- Mobile-first responsive design
- SCSS with `@use` (not `@import`)

### Accessibility
- WCAG 2.1 AA compliance
- Semantic HTML
- ARIA labels where needed
- Keyboard navigation
- Focus management

### Testing
- Minimum 80% code coverage
- Test all public APIs
- Test error conditions
- Test accessibility features

## Build and Deployment

### Build Library
```bash
npm run build
```

Output: `dist/multi-service-automotive-ecosystem-components`

### Build Storybook
```bash
npm run build-storybook
```

Output: `storybook-static/`

## Contributing

### Component Checklist
- [ ] Component implements specification
- [ ] Follows Angular standalone pattern
- [ ] Uses design tokens exclusively
- [ ] BEM methodology in SCSS
- [ ] Responsive design (mobile-first)
- [ ] Unit tests (>80% coverage)
- [ ] Storybook stories (min 3)
- [ ] Accessibility compliant
- [ ] Exported from public API
- [ ] Documentation comments

### Commit Message Format
```
[Component] Brief description

- Detailed change 1
- Detailed change 2
```

## Resources

- [Component Specifications](../../../docs/design/phase-1/components/)
- [Style Guide](../../../docs/specs/style-guide.md)
- [Implementation Specs](../../../docs/specs/implementation-specs.md)
- [Angular Documentation](https://angular.dev)
- [Storybook Documentation](https://storybook.js.org)

## License

Copyright © 2026 Multi-Service Automotive Ecosystem Platform
