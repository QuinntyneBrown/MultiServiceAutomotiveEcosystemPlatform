import { test, expect } from '@playwright/test';

test.describe('Professional Directory', () => {
  test('should display professional directory page', async ({ page }) => {
    await page.goto('/professionals');
    
    // Check page title
    await expect(page.locator('h1')).toContainText('Find Automotive Professionals');
    
    // Check that professional cards are displayed
    const cards = page.locator('.professional-card');
    await expect(cards.first()).toBeVisible();
  });

  test('should display professional information in cards', async ({ page }) => {
    await page.goto('/professionals');
    
    const firstCard = page.locator('.professional-card').first();
    
    // Check for business name
    await expect(firstCard.locator('.professional-card__business-name')).toBeVisible();
    
    // Check for professional name
    await expect(firstCard.locator('.professional-card__professional-name')).toBeVisible();
    
    // Check for business type
    await expect(firstCard.locator('.professional-card__type')).toBeVisible();
    
    // Check for rating
    await expect(firstCard.locator('.professional-card__rating')).toBeVisible();
    
    // Check for View Profile link
    await expect(firstCard.locator('a:has-text("View Profile")')).toBeVisible();
  });

  test('should navigate to professional profile when clicking View Profile', async ({ page }) => {
    await page.goto('/professionals');
    
    // Click the first View Profile button
    await page.locator('.professional-card').first().locator('a:has-text("View Profile")').click();
    
    // Wait for navigation to profile page
    await page.waitForURL('**/professionals/*');
    
    // Verify we're on a profile page
    await expect(page.locator('.professional-profile')).toBeVisible();
  });
});
