import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Referral Landing Page', () => {
  test('navigates from referral landing to registration with ref code', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/r/ABC123');

    await expect(page.locator('text=invited you!')).toBeVisible();
    await expect(page.locator('text=Your referral code:')).toBeVisible();

    await page.click('button:has-text("Sign Up Now")');

    await page.waitForURL('**/customer/register?ref=ABC123');
    await expect(page.locator('h1')).toContainText('Create Account');
  });
});
