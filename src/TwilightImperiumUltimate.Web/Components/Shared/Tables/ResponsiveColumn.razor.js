// Drag-to-resize behavior for a single table column header. Purely client-side (no Blazor
// round-trips while dragging, for smooth per-frame updates) -- mutates the header <th>'s own
// inline width/min-width/max-width directly. Keyed by the handle element so `dispose` can find
// and remove exactly the listeners `initialize` attached for that instance.
const MIN_COLUMN_WIDTH_PX = 40;

const instances = new Map();

function applyWidth(header, widthPx) {
    const clamped = Math.max(MIN_COLUMN_WIDTH_PX, widthPx);
    header.style.width = `${clamped}px`;
    header.style.minWidth = `${clamped}px`;
    header.style.maxWidth = `${clamped}px`;
}

export function initialize(handle, header) {
    if (!handle || !header || instances.has(handle)) {
        return;
    }

    let startX = 0;
    let startWidth = 0;

    const onPointerMove = (event) => {
        applyWidth(header, startWidth + (event.clientX - startX));
    };

    const onPointerUp = (event) => {
        handle.releasePointerCapture(event.pointerId);
        handle.removeEventListener('pointermove', onPointerMove);
        handle.removeEventListener('pointerup', onPointerUp);
        document.body.style.cursor = '';
        document.body.style.userSelect = '';
    };

    const onPointerDown = (event) => {
        // Only the primary mouse button / a single touch/pen contact starts a resize.
        if (event.button !== undefined && event.button !== 0) {
            return;
        }

        event.preventDefault();
        startX = event.clientX;
        startWidth = header.getBoundingClientRect().width;
        handle.setPointerCapture(event.pointerId);
        document.body.style.cursor = 'col-resize';
        document.body.style.userSelect = 'none';
        handle.addEventListener('pointermove', onPointerMove);
        handle.addEventListener('pointerup', onPointerUp);
    };

    handle.addEventListener('pointerdown', onPointerDown);
    instances.set(handle, { header, onPointerDown });
}

export function resizeStep(header, deltaPx) {
    if (!header) {
        return;
    }

    applyWidth(header, header.getBoundingClientRect().width + deltaPx);
}

export function dispose(handle) {
    const instance = instances.get(handle);
    if (!instance) {
        return;
    }

    handle.removeEventListener('pointerdown', instance.onPointerDown);
    instances.delete(handle);
}
