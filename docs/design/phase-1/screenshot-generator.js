/**
 * Screenshot Generator for Phase 1 Mockups
 *
 * Usage: node screenshot-generator.js <page-folder-name>
 * Example: node screenshot-generator.js tenant-not-found
 *
 * This script generates screenshots at mobile (375px), tablet (768px), and desktop (1440px) viewports.
 */

const puppeteer = require('puppeteer');
const path = require('path');
const fs = require('fs');

const VIEWPORTS = {
  mobile: { width: 375, height: 812 },
  tablet: { width: 768, height: 1024 },
  desktop: { width: 1440, height: 900 }
};

async function generateScreenshots(pageFolder) {
  const pagesDir = path.join(__dirname, 'pages');
  const pagePath = path.join(pagesDir, pageFolder);

  if (!fs.existsSync(pagePath)) {
    console.error(`Page folder not found: ${pagePath}`);
    process.exit(1);
  }

  const htmlFile = path.join(pagePath, 'mockup.html');
  if (!fs.existsSync(htmlFile)) {
    console.error(`mockup.html not found in: ${pagePath}`);
    process.exit(1);
  }

  console.log(`Generating screenshots for: ${pageFolder}`);

  const browser = await puppeteer.launch({
    headless: 'new',
    args: ['--no-sandbox', '--disable-setuid-sandbox']
  });

  try {
    const page = await browser.newPage();

    for (const [viewportName, viewport] of Object.entries(VIEWPORTS)) {
      console.log(`  Capturing ${viewportName} (${viewport.width}x${viewport.height})...`);

      await page.setViewport(viewport);
      await page.goto(`file://${htmlFile}`, { waitUntil: 'networkidle0' });

      // Wait for any fonts/styles to load
      await page.waitForTimeout(500);

      const screenshotPath = path.join(pagePath, `screenshot-${viewportName}.png`);
      await page.screenshot({
        path: screenshotPath,
        fullPage: true
      });

      console.log(`    Saved: screenshot-${viewportName}.png`);
    }

    console.log('Done!');
  } finally {
    await browser.close();
  }
}

async function generateAllScreenshots() {
  const pagesDir = path.join(__dirname, 'pages');
  const pageFolders = fs.readdirSync(pagesDir).filter(f =>
    fs.statSync(path.join(pagesDir, f)).isDirectory()
  );

  for (const folder of pageFolders) {
    const htmlFile = path.join(pagesDir, folder, 'mockup.html');
    if (fs.existsSync(htmlFile)) {
      await generateScreenshots(folder);
    }
  }
}

// Main execution
const args = process.argv.slice(2);

if (args.length === 0 || args[0] === '--all') {
  generateAllScreenshots().catch(console.error);
} else {
  generateScreenshots(args[0]).catch(console.error);
}
