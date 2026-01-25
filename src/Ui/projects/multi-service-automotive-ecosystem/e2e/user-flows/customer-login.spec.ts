import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Customer Login', () => {
  test('signs in and navigates to home', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/customer/login');

    await expect(page.locator('h1')).toContainText('Welcome Back');

    await page.fill('#email', 'test.user@example.com');
    await page.fill('#password', 'Password1');

    await page.click('button:has-text("Sign In")');

    await page.waitForURL('**/');
    await expect(page.locator('h1').first()).toBeVisible();
  });
});
