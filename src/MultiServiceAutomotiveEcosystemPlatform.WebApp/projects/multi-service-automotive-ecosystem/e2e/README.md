# End-to-End Tests

This directory contains Playwright end-to-end tests for the Multi-Service Automotive Ecosystem Platform.

## Running Tests

### Run all tests
```bash
npm run test:e2e
```

### Run tests in UI mode
```bash
npm run test:e2e:ui
```

### Run tests in headed mode (visible browser)
```bash
npm run test:e2e:headed
```

## Test Structure

Tests are organized by feature area:

- `home.spec.ts` - Homepage tests
- `professionals.spec.ts` - Professional directory and profile tests
- `referrals.spec.ts` - Referral dashboard tests

## Writing Tests

All tests follow the Playwright testing framework conventions:

```typescript
import { test, expect } from '@playwright/test';

test.describe('Feature Name', () => {
  test('should do something', async ({ page }) => {
    await page.goto('/path');
    await expect(page.locator('selector')).toBeVisible();
  });
});
```

## Coverage

E2E tests cover:
- Navigation flows
- Page rendering
- User interactions
- Component visibility
- Data display

## Configuration

Tests are configured in `playwright.config.ts` at the workspace root.

The test server is automatically started before running tests.
