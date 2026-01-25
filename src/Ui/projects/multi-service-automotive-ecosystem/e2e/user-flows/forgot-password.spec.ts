import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Forgot Password', () => {
  test('exposes a forgot password action from login', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/customer/login');
    await expect(page.locator('h1')).toContainText('Welcome Back');

    await page.click('button:has-text("Forgot password?")');

    // Current implementation logs to console and stays on the same page.
    await page.waitForURL('**/customer/login');
    await expect(page.locator('h1')).toContainText('Welcome Back');
  });
});
