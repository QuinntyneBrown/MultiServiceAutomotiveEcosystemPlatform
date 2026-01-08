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
    acceptNextDialog(page);
    await page.click('button:has-text("Save Changes")');
    
    // Wait a bit for dialog to be handled
    await page.waitForTimeout(500);

    await page.goto('/professional/specialties');
    await expect(page.locator('h1')).toContainText('Manage Specialties');

    await page.click('button:has-text("+ Add Specialty")');
    // Use more specific selector for the modal heading
    await expect(page.locator('h2:has-text("Add Specialty")')).toBeVisible();

    acceptNextDialog(page);
    await page.click('button.specialty-management__modal-item');
    
    // Wait a bit for dialog to be handled
    await page.waitForTimeout(500);

    // Verify modal is closed - check button again
    await expect(page.locator('h2:has-text("Add Specialty")')).toBeHidden();
  });
});
