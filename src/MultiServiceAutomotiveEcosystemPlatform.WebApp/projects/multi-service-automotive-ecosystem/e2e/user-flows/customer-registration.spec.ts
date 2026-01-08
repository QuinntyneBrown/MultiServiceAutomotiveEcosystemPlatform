import { test, expect } from '@playwright/test';
import { setupApiMocks, uniqueEmail } from './_flow-utils';

test.describe('User Flow: Customer Registration', () => {
  test('registers a new customer and routes to login', async ({ page }) => {
    await setupApiMocks(page);

    await page.goto('/customer/register');

    await expect(page.locator('h1')).toContainText('Create Account');

    await page.fill('#firstName', 'Test');
    await page.fill('#lastName', 'User');
    await page.fill('#email', uniqueEmail('customer'));

    // Uses the component formatter to produce (555) 123-4567
    await page.fill('#phone', '5551234567');

    await page.fill('#password', 'Password1');
    await page.fill('#confirmPassword', 'Password1');
    await page.check('#termsAccept');

    await page.click('button:has-text("Create Account")');

    await page.waitForURL('**/customer/login');
    await expect(page.locator('h1')).toContainText('Welcome Back');
  });
});
