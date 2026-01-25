import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Professional Directory Browse', () => {
  test('browses professionals and opens a profile', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/professionals');

    await expect(page.locator('text=Find Automotive Professionals')).toBeVisible();
    await expect(page.locator('.professional-card').first()).toBeVisible();

    await page.click('.professional-card a:has-text("View Profile")');
    await page.waitForURL('**/professionals/**');

    await expect(page.locator('h1')).toBeVisible();
    await expect(page.locator('h2:has-text("About")')).toBeVisible();
  });
});
