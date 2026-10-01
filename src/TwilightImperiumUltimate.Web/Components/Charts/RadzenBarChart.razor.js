const observers = new WeakMap();

function updatePrivateCategories(chartElement, labels) {
    const privateLabels = new Set(labels ?? []);
    const ticks = chartElement.querySelectorAll('.rz-value-axis .rz-tick-text');

    for (const tick of ticks) {
        const label = tick.textContent?.trim() ?? '';
        tick.classList.toggle('rz-private-category', label === 'Private Profile' || privateLabels.has(label));

        if (tick.querySelector('.rz-player-stat-value')) {
            continue;
        }

        const match = label.match(/^(.*) \((\d+)\)$/);
        if (!match) {
            continue;
        }

        tick.textContent = '';
        const playerName = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
        playerName.textContent = `${match[1]} (`;
        const playerStatValue = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
        playerStatValue.classList.add('rz-player-stat-value');
        playerStatValue.textContent = `${match[2]})`;
        tick.append(playerName, playerStatValue);
    }
}

function keepValueAxisLabelsVisible(chartElement) {
    const svg = chartElement.querySelector('.rz-chart > svg');
    const plotGroup = svg?.firstElementChild;
    const labels = chartElement.querySelectorAll('.rz-value-axis .rz-tick-text');
    if (!svg || !plotGroup || labels.length === 0) {
        return;
    }

    const chartLeft = svg.getBoundingClientRect().left;
    const leftmostLabel = [...labels].reduce((leftmost, label) => {
        const labelLeft = label.getBoundingClientRect().left;
        return leftmost === null || labelLeft < leftmost ? labelLeft : leftmost;
    }, null);
    const requiredOffset = leftmostLabel === null ? 0 : chartLeft - leftmostLabel + 4;
    if (requiredOffset <= 0) {
        return;
    }

    const transform = plotGroup.getAttribute('transform') ?? '';
    const match = transform.match(/^translate\(([-\d.]+),\s*([-\d.]+)\)$/);
    if (!match) {
        return;
    }

    plotGroup.setAttribute(
        'transform',
        `translate(${Number(match[1]) + requiredOffset}, ${match[2]})`);
}

export function observePrivateCategories(chartElement, labels) {
    if (!chartElement) {
        return;
    }

    let observer = observers.get(chartElement);
    if (!observer) {
        observer = new MutationObserver(() => {
            const currentLabels = chartElement.dataset.privateCategoryLabels
                ? JSON.parse(chartElement.dataset.privateCategoryLabels)
                : [];
            updatePrivateCategories(chartElement, currentLabels);
            keepValueAxisLabelsVisible(chartElement);
        });
        observer.observe(chartElement, { childList: true, subtree: true, characterData: true });
        observers.set(chartElement, observer);
    }

    chartElement.dataset.privateCategoryLabels = JSON.stringify(labels ?? []);
    updatePrivateCategories(chartElement, labels);
    keepValueAxisLabelsVisible(chartElement);
}

export function disposePrivateCategories(chartElement) {
    const observer = observers.get(chartElement);
    observer?.disconnect();
    observers.delete(chartElement);
}