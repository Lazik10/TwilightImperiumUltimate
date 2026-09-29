export function saveMapAsImage(elementId, imageName) {
    const element = document.getElementById(elementId);
    html2canvas(element, { backgroundColor: null }).then((canvas) => {
        const imgData = canvas.toDataURL('image/png');
        const a = document.createElement('a');
        a.href = imgData;
        a.download = imageName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    });
}

export function copyToClipboard(text) {
    console.log('copyToClipboard called with text:', text);
    if (!navigator.clipboard) {
        console.error('Clipboard API not supported. Using fallback method.');
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(() => {
        console.log('Text copied to clipboard');
    }).catch(err => {
        console.error('Could not copy text: ', err);
    });
}

export function fallbackCopyTextToClipboard(text) {
    const textArea = document.createElement('textarea');
    textArea.value = text;
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();
    try {
        document.execCommand('copy');
        console.log('Fallback: Text copied to clipboard');
    } catch (err) {
        console.error('Fallback: Could not copy text:', err);
    }
    document.body.removeChild(textArea);
}

const minimumMapZoom = 0.5;
const maximumMapZoom = 3;
const mapZoomStates = new WeakMap();

export function initializeMapZoom(viewport) {
    const content = viewport.querySelector('.map-zoom-content');
    const activePointers = new Map();
    const state = { content, scale: 1, pinchDistance: null };

    const applyZoom = () => {
        state.content.style.transform = `scale(${state.scale})`;
    };

    const wheel = event => {
        if (!event.target.closest('.hex-container')) {
            return;
        }

        event.preventDefault();
        state.scale = clampMapZoom(state.scale * Math.exp(-event.deltaY * 0.001));
        applyZoom();
    };

    const pointerDown = event => {
        if (event.pointerType !== 'touch' || !event.target.closest('.hex-container')) {
            return;
        }

        activePointers.set(event.pointerId, event);
        updatePinchDistance(state, activePointers);
    };

    const pointerMove = event => {
        if (!activePointers.has(event.pointerId)) {
            return;
        }

        activePointers.set(event.pointerId, event);

        if (activePointers.size !== 2 || state.pinchDistance === null) {
            return;
        }

        const currentDistance = getPointerDistance(activePointers);
        state.scale = clampMapZoom(state.scale * (currentDistance / state.pinchDistance));
        state.pinchDistance = currentDistance;
        applyZoom();
    };

    const pointerUp = event => {
        activePointers.delete(event.pointerId);
        updatePinchDistance(state, activePointers);
    };

    const doubleClick = () => {
        state.scale = 1;
        applyZoom();
    };

    viewport.addEventListener('wheel', wheel, { passive: false });
    viewport.addEventListener('pointerdown', pointerDown);
    viewport.addEventListener('pointermove', pointerMove);
    viewport.addEventListener('pointerup', pointerUp);
    viewport.addEventListener('pointercancel', pointerUp);
    viewport.addEventListener('dblclick', doubleClick);
    mapZoomStates.set(viewport, state);

    return {
        dispose: () => {
            viewport.removeEventListener('wheel', wheel);
            viewport.removeEventListener('pointerdown', pointerDown);
            viewport.removeEventListener('pointermove', pointerMove);
            viewport.removeEventListener('pointerup', pointerUp);
            viewport.removeEventListener('pointercancel', pointerUp);
            viewport.removeEventListener('dblclick', doubleClick);
            mapZoomStates.delete(viewport);
        },
    };
}

export function adjustMapZoom(viewport, factor) {
    const state = mapZoomStates.get(viewport);

    if (!state) {
        return;
    }

    state.scale = clampMapZoom(state.scale * factor);
    state.content.style.transform = `scale(${state.scale})`;
}

export function resetMapZoom(viewport) {
    const state = mapZoomStates.get(viewport);

    if (!state) {
        return;
    }

    state.scale = 1;
    state.content.style.transform = 'scale(1)';
}

function clampMapZoom(scale) {
    return Math.min(Math.max(scale, minimumMapZoom), maximumMapZoom);
}

function updatePinchDistance(state, activePointers) {
    state.pinchDistance = activePointers.size === 2 ? getPointerDistance(activePointers) : null;
}

function getPointerDistance(activePointers) {
    const [firstPointer, secondPointer] = activePointers.values();
    return Math.hypot(firstPointer.clientX - secondPointer.clientX, firstPointer.clientY - secondPointer.clientY);
}