import type { BrowserContext, Page } from '@playwright/test';

export async function setupApiMocks(page: Page) {
  await page.route('**/api/**', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({}),
    });
  });
}

export function uniqueEmail(prefix = 'e2e') {
  const nonce = `${Date.now()}-${Math.random().toString(16).slice(2)}`;
  return `${prefix}-${nonce}@example.com`;
}

export async function grantClipboardPermissions(
  context: BrowserContext,
  browserName: string,
  origin = 'http://localhost:4200'
) {
  // WebKit (Safari) doesn't support clipboard-write permission
  if (browserName !== 'webkit') {
    await context.grantPermissions(['clipboard-read', 'clipboard-write'], { origin });
  }
}

export async function acceptNextDialog(page: Page) {
  page.once('dialog', async (dialog) => {
    await dialog.accept();
  });
}
