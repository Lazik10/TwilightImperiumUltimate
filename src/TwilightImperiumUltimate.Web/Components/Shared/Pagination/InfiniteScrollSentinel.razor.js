const observers = new Map();

export function observe(element, dotNetRef) {
    if (!element || observers.has(element)) {
        return;
    }

    const observer = new IntersectionObserver((entries) => {
        for (const entry of entries) {
            if (entry.isIntersecting) {
                dotNetRef.invokeMethodAsync('HandleIntersectAsync');
            }
        }
    }, { rootMargin: '200px' });

    observer.observe(element);
    observers.set(element, observer);
}

export function unobserve(element) {
    if (!element) {
        return;
    }

    const observer = observers.get(element);
    if (observer) {
        observer.disconnect();
        observers.delete(element);
    }
}
