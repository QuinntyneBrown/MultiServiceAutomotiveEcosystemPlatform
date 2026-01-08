import { test, expect } from '@playwright/test';
import { setupApiMocks } from './_flow-utils';

test.describe('User Flow: Professional Dashboard', () => {
  test('loads dashboard and navigates to profile management', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/professional/dashboard');

    await expect(page.locator('text=Welcome back')).toBeVisible();
    await expect(page.locator('text=Quick Actions')).toBeVisible();

    await page.click('a:has-text("Update Profile")');
    await page.waitForURL('**/professional/profile');

    await expect(page.locator('h1')).toContainText('Manage Your Profile');
  });
});
