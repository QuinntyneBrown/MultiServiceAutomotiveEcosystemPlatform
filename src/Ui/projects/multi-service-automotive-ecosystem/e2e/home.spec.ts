import { test, expect } from '@playwright/test';

test.describe('Home Page', () => {
  test('should display the home page with hero section', async ({ page }) => {
    await page.goto('/');
    
    // Check that the home page title is visible
    await expect(page.locator('h1').first()).toBeVisible();
    await expect(page.locator('h1').first()).toContainText('Welcome');
    
    // Check for action buttons - use more specific selectors to avoid matching multiple elements
    await expect(page.locator('a.action-button:has-text("Find Professionals")')).toBeVisible();
    await expect(page.locator('a:has-text("Get Started")').first()).toBeVisible();
  });

  test('should navigate to professionals page when clicking Find Professionals', async ({ page }) => {
    await page.goto('/');
    
    // Click the action button specifically (not the nav link)
    await page.click('a.action-button:has-text("Find Professionals")');
    
    // Wait for navigation
    await page.waitForURL('**/professionals');
    
    // Verify we're on the professionals page
    await expect(page.locator('h1')).toContainText('Find Automotive Professionals');
  });

  test('should navigate to registration page when clicking Get Started', async ({ page }) => {
    await page.goto('/');
    
    await page.click('a:has-text("Get Started")');
    
    // Wait for navigation
    await page.waitForURL('**/customer/register');
    
    // Verify we're on the registration page - actual text is "Create Account"
    await expect(page.locator('h1')).toContainText('Create Account');
  });

  test('should display features section', async ({ page }) => {
    await page.goto('/');
    
    // Check for features - use heading selector to be more specific
    await expect(page.locator('h3:has-text("Find Services")')).toBeVisible();
    await expect(page.locator('h3:has-text("Get Referrals")')).toBeVisible();
    await expect(page.locator('h3:has-text("Earn Rewards")')).toBeVisible();
  });
});
