import { test, expect } from '@playwright/test';

test.describe('Home Page', () => {
  test('should display the home page with hero section', async ({ page }) => {
    await page.goto('/');
    
    // Check that the home page title is visible
    await expect(page.locator('h1').first()).toBeVisible();
    await expect(page.locator('h1').first()).toContainText('Welcome');
    
    // Check for action buttons
    await expect(page.locator('a:has-text("Find Professionals")')).toBeVisible();
    await expect(page.locator('a:has-text("Get Started")')).toBeVisible();
  });

  test('should navigate to professionals page when clicking Find Professionals', async ({ page }) => {
    await page.goto('/');
    
    await page.click('a:has-text("Find Professionals")');
    
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
    
    // Verify we're on the registration page
    await expect(page.locator('h1')).toContainText('Create Your Account');
  });

  test('should display features section', async ({ page }) => {
    await page.goto('/');
    
    // Check for features
    await expect(page.locator('text=Find Services')).toBeVisible();
    await expect(page.locator('text=Get Referrals')).toBeVisible();
    await expect(page.locator('text=Earn Rewards')).toBeVisible();
  });
});
