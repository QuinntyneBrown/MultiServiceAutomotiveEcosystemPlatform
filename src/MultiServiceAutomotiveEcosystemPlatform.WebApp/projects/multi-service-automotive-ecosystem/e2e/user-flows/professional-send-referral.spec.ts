import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Professional Send Referral', () => {
  test('fills and submits a professional referral', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/professional/referrals');
    await expect(page.locator('h1')).toContainText('Professional Referrals');

    await page.click('a:has-text("Send Referral")');
    await page.waitForURL('**/professional/referrals/send');

    await expect(page.locator('h1')).toContainText('Send Professional Referral');

    await page.selectOption('#targetProfessional', { index: 1 });
    await page.selectOption('#customer', { index: 1 });
    await page.fill('#serviceNeeded', 'Tire replacement');
    await page.fill('#reasonForReferral', 'Customer needs tire work and I am fully booked.');

    await page.click('button:has-text("Send Referral")');

    await page.waitForURL('**/professional/referrals');
    await expect(page.locator('h1')).toContainText('Professional Referrals');
  });
});
