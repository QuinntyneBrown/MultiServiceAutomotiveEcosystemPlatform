import { test, expect } from '@playwright/test';
import { acceptNextDialog, setupApiMocks } from './_flow-utils';

test.describe('User Flow: Professional Profile Setup', () => {
  test('updates profile and visits specialty management', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/professional/dashboard');
    await expect(page.locator('text=Complete Your Profile')).toBeVisible();

    await page.click('a:has-text("Update Profile")');
    await page.waitForURL('**/professional/profile');

    await expect(page.locator('h1')).toContainText('Manage Your Profile');

    // Form is prefilled by mock data; save shows an alert.
    await acceptNextDialog(page);
    await page.click('button:has-text("Save Changes")');

    await page.goto('/professional/specialties');
    await expect(page.locator('h1')).toContainText('Manage Specialties');

    await page.click('button:has-text("+ Add Specialty")');
    await expect(page.locator('text=Add Specialty')).toBeVisible();

    await acceptNextDialog(page);
    await page.click('button.specialty-management__modal-item');

    await expect(page.locator('text=Add Specialty')).toBeHidden();
  });
});
