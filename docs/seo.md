# SEO operations

## Implemented client behavior

The WebAssembly client adds route-aware metadata through `SeoMetadata` in the main layout:

- A single canonical URL without query strings or fragments.
- Curated titles and descriptions, Open Graph metadata with dimensions, Twitter cards, breadcrumbs, and `WebPage` or `CollectionPage` JSON-LD for indexable public routes.
- Visible breadcrumb navigation and matching `BreadcrumbList` markup for public content routes.
- `Article` list markup for the news feed and `CreativeWork` markup for public map and slice details after their data loads.
- `noindex, follow` for account, action, query-string, legacy, and unknown routes.
- Page-type Open Graph preview images. The `social-*.png` assets are `1200×630` screenshots captured from representative faction, tools, TIGL, rules, map archive, and slice archive pages. Other pages use `resources/images/shared/Twilight_imperium_ultimate_background.webp`.

`sitemap.xml` is generated before publish from `SitemapPaths` and the `FactionName` enum. It cannot drift when factions are added. To include public database-backed map and slice details, set `SitemapApiBaseUrl` to the public API origin during the release publish; the generator queries the existing public archive endpoints and adds their canonical detail URLs.

## Production host requirements

The standalone Blazor WebAssembly client cannot control the initial HTTP response. Configure the production host or CDN to:

1. Redirect HTTP to HTTPS and all non-canonical hostnames to `https://ti4ultimate.com` with a permanent redirect.
2. Return HTTP `404` for unknown routes instead of returning the SPA fallback with `200 OK`.
3. Return permanent redirects for retired routes, including the legacy Discordant Stars faction route.
4. Return `X-Robots-Tag: noindex, follow` for `/account/` and other private/action route prefixes.
5. Serve compressed static assets and long-lived cache headers for fingerprinted framework files.

## Content and metadata maintenance

- Add every new stable, public route to `SitemapPaths.PublicPagePaths`; publish regenerates `sitemap.xml`.
- Do not add query-string URLs, authenticated routes, write/action pages, or temporary share URLs to the sitemap.
- Add a stable detail URL to the sitemap only when its content is public, unique, and useful without authentication.
- Publish production assets with the public archive API available to the sitemap generator, for example `dotnet publish src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj -c Release -p:SitemapApiBaseUrl=https://<public-api-origin>/`. Omit the property for offline builds; only stable routes and faction pages will be included.
- Use a real modification date before adding `lastmod`; do not invent dates or add `priority`/`changefreq` values.
- Keep the displayed `h1`, page title, description, canonical URL, and social-preview image consistent with the actual page content.
- Only add structured data for facts visible on the matching page. Validate it with Google's Rich Results Test.

## Monitoring

After deployment, submit `https://ti4ultimate.com/sitemap.xml` to Google Search Console and Bing Webmaster Tools. Monitor index coverage, Core Web Vitals, rich-result errors, sitemap processing, crawl errors, and rendered HTML through Search Console's URL Inspection tool.

## Rendering roadmap

Google can render this client-side app, but public reference pages still require JavaScript before their body content is available. The highest-impact future SEO improvement is server-side rendering or prerendering for the public reference, rules, faction, and archive pages. Preserve the current metadata contract when making that hosting-model change.