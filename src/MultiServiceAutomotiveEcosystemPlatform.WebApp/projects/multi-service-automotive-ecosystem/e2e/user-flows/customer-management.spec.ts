import { test, expect } from '@playwright/test';
import { setupApiMocks, uniqueEmail } from './_flow-utils';

test.describe('User Flow: Customer Management', () => {
  test('adds a customer and views a customer record', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/customers');
    await expect(page.locator('text=Customers')).toBeVisible();

    await page.click('a:has-text("+ Add Customer")');
    await page.waitForURL('**/customers/new');

    await expect(page.locator('h1')).toContainText('Add New Customer');

    await page.fill('#firstName', 'E2E');
    await page.fill('#lastName', 'Customer');
    await page.fill('#email', uniqueEmail('cust'));
    await page.fill('#phone', '(555) 123-4567');

    await page.click('button:has-text("Add Customer")');
    await page.waitForURL('**/customers');

    await page.click('.customer-list__name-link');
    await page.waitForURL('**/customers/**');
    await expect(page.locator('h1')).toBeVisible();
  });
});
