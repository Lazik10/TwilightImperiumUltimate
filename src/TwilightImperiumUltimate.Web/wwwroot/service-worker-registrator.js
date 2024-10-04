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