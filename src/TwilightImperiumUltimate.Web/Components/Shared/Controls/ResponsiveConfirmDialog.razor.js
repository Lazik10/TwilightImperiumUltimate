const cancelHandlers = new WeakMap();

export function showModal(dialog) {
    if (!dialog.open) {
        dialog.showModal();
    }

    const previousHandler = cancelHandlers.get(dialog);
    if (previousHandler) {
        dialog.removeEventListener("cancel", previousHandler);
    }

    const preventNativeCancel = event => event.preventDefault();
    dialog.addEventListener("cancel", preventNativeCancel);
    cancelHandlers.set(dialog, preventNativeCancel);
}

export function closeModal(dialog) {
    const cancelHandler = cancelHandlers.get(dialog);
    if (cancelHandler) {
        dialog.removeEventListener("cancel", cancelHandler);
        cancelHandlers.delete(dialog);
    }

    if (dialog.open) {
        dialog.close();
    }
}
