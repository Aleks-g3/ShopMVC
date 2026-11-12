# ShopMVC visual tests

Playwright-based visual and accessibility checks for the main user journeys.

## Prerequisites

- .NET 10 SDK
- Node.js 20+
- SQL Server with the Development database available
- Running application (`dotnet run --project ShopMVC --urls http://localhost:5069`)
- Admin credentials in `ADMIN_EMAIL` and `ADMIN_PASSWORD` environment variables
  (the same values used for `SeedAdmin` in user secrets)

## Run

```powershell
cd ShopMVC.VisualTests
npm install
npx playwright install chromium
$env:BASE_URL="http://localhost:5069"
$env:ADMIN_EMAIL="admin@admin.com"
$env:ADMIN_PASSWORD="<local admin password>"
npm run test:visual
```

The suite:

- logs in as admin,
- creates a product, adds it to the cart, and places an order,
- captures desktop and mobile screenshots for home, product, cart, checkout, order, and admin pages,
- runs an axe-core WCAG 2.1 A/AA audit on each page,
- fails on browser console errors and serious/critical accessibility violations.

Artifacts are written to `visual-artifacts/` (screenshots and raw axe reports) and
`visual-report/` (HTML report). Both directories are ignored by Git.
