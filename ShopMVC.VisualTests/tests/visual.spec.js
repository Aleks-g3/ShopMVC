const fs = require('fs');
const path = require('path');
const { test, expect } = require('@playwright/test');
const AxeBuilder = require('@axe-core/playwright').default;

const artifactDir = path.join(__dirname, '..', 'visual-artifacts');
const adminEmail = process.env.ADMIN_EMAIL;
const adminPassword = process.env.ADMIN_PASSWORD;
const runId = Date.now();
const productName = `Visual test product ${runId}`;

test('captures key pages and audits accessibility', async ({ page }, testInfo) => {
  test.skip(!adminEmail || !adminPassword, 'ADMIN_EMAIL and ADMIN_PASSWORD must be set');
  fs.mkdirSync(artifactDir, { recursive: true });

  const project = testInfo.project.name;
  const consoleErrors = [];
  page.on('console', message => {
    if (message.type() === 'error')
      consoleErrors.push(message.text());
  });
  page.on('pageerror', error => consoleErrors.push(error.message));

  const violations = [];

  async function capture(name, url) {
    if (url)
      await page.goto(url);
    await page.waitForLoadState('networkidle');
    await page.screenshot({
      path: path.join(artifactDir, `${project}-${name}.png`),
      fullPage: true
    });

    const hasHorizontalOverflow = await page.evaluate(() =>
      document.body.scrollWidth > document.body.clientWidth + 1);
    expect(hasHorizontalOverflow, `Horizontal overflow on ${name}`).toBe(false);

    const results = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze();

    fs.writeFileSync(
      path.join(artifactDir, `${project}-${name}.axe.json`),
      JSON.stringify(results.violations, null, 2)
    );

    for (const violation of results.violations)
      violations.push({ page: name, ...violation });
  }

  try {
    await capture('login', '/Identity/Account/Login');
    await capture('register', '/Identity/Account/Register');

    await page.goto('/Identity/Account/Login');
    await page.locator('input[name="Input.Email"]').fill(adminEmail);
    await page.locator('input[name="Input.Password"]').fill(adminPassword);
    await page.getByRole('button', { name: 'Log in' }).click();
    await expect(page).toHaveURL('/');

    await capture('product-create', '/Product/Create');
    await page.getByRole('button', { name: 'Create' }).click();
    await expect(page.locator('#Name')).toBeFocused();
    await capture('product-create-validation');
    await page.locator('#Name').fill(productName);
    await page.locator('#Description').fill('Product created by the visual test suite.');
    await page.locator('#Category').selectOption('1');
    await page.locator('#Price').fill('149.99');
    await page.locator('#AvailableQuantity').fill('5');
    await page.getByRole('button', { name: 'Create' }).click();

    await capture('product-manage', '/Product/Manage');
    const editLink = page.getByRole('link', { name: `Edit ${productName}` });
    const productId = (await editLink.getAttribute('href')).match(/product\/edit\/(\d+)/)[1];

    await capture('home', '/');
    await capture('product-details', `/product/${productId}/details`);

    await page.locator('#amount').fill('2');
    await page.getByRole('button', { name: 'Add to cart' }).click();
    await expect(page.getByText('Product added to cart.')).toBeVisible();

    await capture('cart', '/Cart');
    await capture('checkout', '/Order/Checkout');

    await page.locator('#Form_City').fill('Warsaw');
    await page.locator('#Form_StreetName').fill('Main');
    await page.locator('#Form_StreetNumber').fill('12');
    await page.locator('#Form_ReceiverFullName').fill('Jan Kowalski');
    await page.locator('#Form_ShipmentMethod').selectOption('2');
    await page.locator('#Form_PaymentMethod').selectOption('1');
    await page.getByRole('button', { name: 'Place order' }).click();
    await expect(page).toHaveURL(/\/Order\/Details\/\d+/);

    await capture('order-details');
    await capture('orders', '/Order');
    await capture('cart-empty', '/Cart');
    await capture('not-found', '/product/999999/details');

    await capture('product-edit', `/product/edit/${productId}`);
  } finally {
    await page.goto('/Product/Manage');
    const row = page.getByRole('row', { name: new RegExp(productName) });
    if (await row.count()) {
      page.once('dialog', dialog => dialog.accept());
      await row.locator('form').evaluate(form => form.requestSubmit());
      await page.waitForLoadState('networkidle');
    }
  }

  const seriousViolations = violations.filter(violation =>
    violation.impact === 'serious' || violation.impact === 'critical');

  fs.writeFileSync(
    path.join(artifactDir, `${project}-axe-summary.json`),
    JSON.stringify({
      total: violations.length,
      serious: seriousViolations.length,
      violations
    }, null, 2)
  );

  expect(consoleErrors, 'Browser console errors').toEqual([]);
  expect(seriousViolations, 'Serious or critical accessibility violations').toEqual([]);
});
