// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('message', event => {
    if (event.data?.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }
});
self.addEventListener('fetch', event => {
    if (event.request.method !== 'GET') {
        return;
    }

    event.respondWith(onFetch(event));
});

const cacheVersion = 'v1.1.0';
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${cacheVersion}-${self.assetsManifest.version}`;
const applicationAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.webcil$/, /\.html$/, /\.js$/, /\.json$/, /\.css$/, /\.woff2?$/, /\.blat$/, /\.dat$/ ];
const applicationAssetsExclude = [
    /^service-worker(?:-assets|-registrator)?\.js$/,
    /(^|\/)appsettings(?:\.[^.]+)?\.json$/i
];

// Replace with your base path if you are hosting on a subfolder. Ensure there is a trailing '/'.
const base = "/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

async function onInstall(event) {
    console.info('Service worker: Install');

    // Cache the complete startup bundle before this worker can become available. Images use the
    // browser's normal HTTP cache so a new application deployment does not re-download them.
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => applicationAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !applicationAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { cache: 'no-cache' }));
    const cache = await caches.open(cacheName);

    try {
        await cache.addAll(assetsRequests);
    } catch (error) {
        await caches.delete(cacheName);
        throw error;
    }
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Take control of any already-open clients right away.
    await self.clients.claim();

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    const requestUrl = new URL(event.request.url);
    if (requestUrl.origin !== self.location.origin) {
        return fetch(event.request);
    }

    if (/\/appsettings(?:\.[^.]+)?\.json$/i.test(requestUrl.pathname)) {
        return fetch(event.request, { cache: 'no-store' });
    }

    const cache = await caches.open(cacheName);

    // For navigation requests, prefer network to avoid serving stale app shell and
    // use cached index.html only as an offline fallback.
    const shouldServeIndexHtml = event.request.mode === 'navigate'
        && !manifestUrlList.some(url => url === event.request.url)
        && !event.request.url.includes('/account/');

    if (shouldServeIndexHtml) {
        try {
            return await fetch(event.request, { cache: 'no-store' });
        } catch {
            const offlineResponse = await cache.match('index.html');
            if (offlineResponse) {
                return offlineResponse;
            }

            return new Response('Offline', { status: 503, statusText: 'Offline' });
        }
    }

    const cachedResponse = await cache.match(event.request);
    if (cachedResponse) {
        return cachedResponse;
    }

    try {
        return await fetch(event.request);
    } catch {
        return new Response('Resource unavailable', { status: 503, statusText: 'Offline' });
    }
}
