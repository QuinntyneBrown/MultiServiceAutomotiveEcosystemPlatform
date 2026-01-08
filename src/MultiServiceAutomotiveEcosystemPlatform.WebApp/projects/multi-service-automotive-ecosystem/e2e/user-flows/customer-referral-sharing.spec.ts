import { test, expect } from '@playwright/test';
import { setupApiMocks, grantClipboardPermissions } from './_flow-utils';

test.describe('User Flow: Customer Referral Sharing', () => {
  test('opens the share modal and copies the referral link', async ({ page, context }) => {
    await setupApiMocks(page);
    await grantClipboardPermissions(context);

    await page.goto('/referrals');
    await expect(page.locator('h1')).toContainText('Your Referral Dashboard');

    await page.click('button:has-text("Share Referral")');

    await expect(page.locator('#share-modal-title')).toContainText('Share Your Referral');

    await page.click('button:has-text("Copy")');
    await expect(page.locator('text=Link copied to clipboard!')).toBeVisible();

    await page.click('button[aria-label="Close modal"]');
    await expect(page.locator('#share-modal-title')).toBeHidden();
  });
});
