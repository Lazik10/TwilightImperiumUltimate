// Development (localhost) uses no service worker at all. A worker provides no benefit locally
// and is actively harmful: after a rebuild it can intercept requests for fingerprinted
// framework files (_framework/*.wasm|.pdb) and serve stale/empty cached responses, which
// surfaces as 404s + "SRI integrity" failures even though the dev server serves the files
// fine. So in development we proactively remove any existing worker + caches and never
// register one. Production keeps the real service worker (offline support + update prompt).
const isDevelopmentHost = ['localhost', '127.0.0.1', '[::1]', '::1'].includes(window.location.hostname);

if (isDevelopmentHost) {
    // Provide no-op stubs so the Blazor update-notification interop calls don't throw in dev.
    window.updateAvailable = Promise.resolve(false);
    window.registerForUpdateAvailableNotification = () => { };

    (async () => {
        let removedSomething = false;

        try {
            if ('serviceWorker' in navigator) {
                const registrations = await navigator.serviceWorker.getRegistrations();
                for (const registration of registrations) {
                    const unregistered = await registration.unregister();
                    removedSomething = removedSomething || unregistered;
                }
            }

            if ('caches' in window) {
                const cacheKeys = await caches.keys();
                if (cacheKeys.length > 0) {
                    removedSomething = true;
                }
                await Promise.all(cacheKeys.map(key => caches.delete(key)));
            }
        } catch (error) {
            console.error('Development service worker cleanup failed:', error);
        }

        // If a worker was still controlling this page, its interception/stale cache already
        // affected the current load. Reload once (guarded against loops) so the next load runs
        // with no controller and fetches framework files straight from the dev server.
        const alreadyCleared = sessionStorage.getItem('ti-dev-sw-cleared') === '1';
        if (removedSomething && navigator.serviceWorker && navigator.serviceWorker.controller && !alreadyCleared) {
            sessionStorage.setItem('ti-dev-sw-cleared', '1');
            console.warn('Removed a stale development service worker/cache. Reloading once to recover.');
            window.location.reload();
        }
    })();
} else {
    window.updateAvailable = new Promise((resolve, reject) => {
        if (!('serviceWorker' in navigator)) {
            const errorMessage = `This browser doesn't support service workers`;
            console.error(errorMessage);
            reject(errorMessage);
            return;
        }

        navigator.serviceWorker.register('service-worker.js')
            .then(registration => {
                console.info(`Service worker registration successful (scope: ${registration.scope})`);

                setInterval(() => {
                    console.log('Service worker check for updates.');
                    registration.update();
                }, 60000); // check each minute

                registration.onupdatefound = () => {
                    const installingServiceWorker = registration.installing;
                    installingServiceWorker.onstatechange = () => {
                        if (installingServiceWorker.state === 'installed') {
                            resolve(true);
                        }
                    }
                };
            })
            .catch(error => {
                console.error('Service worker registration failed with error:', error);
                reject(error);
            });
    });

    window.registerForUpdateAvailableNotification = (caller, methodName) => {
        window.updateAvailable.then(isUpdateAvailable => {
            if (isUpdateAvailable) {
                console.log('Service worker update available.');
                caller.invokeMethodAsync(methodName).then();
            }
        });
    };
}
