import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Professional Receive Referral', () => {
  test('opens a received referral and accepts it', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/professional/referrals');
    await expect(page.locator('h1')).toContainText('Professional Referrals');

    // Open a received referral detail.
    await page.click('a:has-text("View")');
    await page.waitForURL('**/professional/referrals/received/**');

    await expect(page.locator('h1')).toContainText('Referral Details');

    // Click the action button
    await page.click('button.referral-action__btn:has-text("Accept Referral")');
    // Wait for modal to appear - use heading selector
    await expect(page.locator('h2:has-text("Accept Referral")')).toBeVisible();

    await page.click('button:has-text("Confirm Accept")');

    await page.waitForURL('**/professional/referrals');
    await expect(page.locator('h1')).toContainText('Professional Referrals');
  });
});
