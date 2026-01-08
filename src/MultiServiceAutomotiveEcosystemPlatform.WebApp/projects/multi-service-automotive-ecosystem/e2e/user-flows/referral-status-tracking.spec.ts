import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Referral Status Tracking', () => {
  test('shows statuses for sent referrals', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/referrals');

    await expect(page.locator('h1')).toContainText('Your Referral Dashboard');
    await expect(page.locator('h2:has-text("Your Referrals")')).toBeVisible();

    const janeRow = page.locator('.referral-dashboard__table-row', { hasText: 'Jane Smith' });
    await expect(janeRow).toBeVisible();
    await expect(janeRow.locator('.referral-dashboard__status')).toContainText('Pending');
  });
});
