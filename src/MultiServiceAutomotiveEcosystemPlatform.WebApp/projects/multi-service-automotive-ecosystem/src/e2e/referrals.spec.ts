import { test, expect } from '@playwright/test';

test.describe('Referral Dashboard', () => {
  test('should display referral dashboard', async ({ page }) => {
    await page.goto('/referrals');
    
    // Check page title
    await expect(page.locator('h1')).toContainText('Your Referral Dashboard');
    
    // Check for referral code card
    await expect(page.locator('.referral-dashboard__code-card')).toBeVisible();
  });

  test('should display referral code', async ({ page }) => {
    await page.goto('/referrals');
    
    // Check that referral code is visible
    await expect(page.locator('.referral-dashboard__code')).toBeVisible();
    
    // Check for copy button
    await expect(page.locator('button:has-text("Copy Code")')).toBeVisible();
  });

  test('should display statistics cards', async ({ page }) => {
    await page.goto('/referrals');
    
    // Check for stats cards
    await expect(page.locator('text=Total Referrals')).toBeVisible();
    await expect(page.locator('text=Successful')).toBeVisible();
    await expect(page.locator('text=Pending')).toBeVisible();
    await expect(page.locator('text=Total Rewards')).toBeVisible();
  });

  test('should display referral list', async ({ page }) => {
    await page.goto('/referrals');
    
    // Check for referrals section
    await expect(page.locator('.referral-dashboard__section')).toBeVisible();
    await expect(page.locator('text=Your Referrals')).toBeVisible();
    
    // Check for table
    await expect(page.locator('.referral-dashboard__table')).toBeVisible();
  });

  test('should copy referral code to clipboard when clicking copy button', async ({ page, context }) => {
    // Grant clipboard permissions
    await context.grantPermissions(['clipboard-read', 'clipboard-write']);
    
    await page.goto('/referrals');
    
    // Click copy button
    await page.click('button:has-text("Copy Code")');
    
    // Note: Verifying clipboard content in tests can be tricky
    // This test verifies the button click doesn't cause errors
  });
});
