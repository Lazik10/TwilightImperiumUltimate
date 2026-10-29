export async function copyToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }

    try {
        await navigator.clipboard.writeText(text);
    } catch (error) {
        if (!(error instanceof DOMException)) {
            throw error;
        }
        fallbackCopyTextToClipboard(text);
    }
}

function fallbackCopyTextToClipboard(text) {
    const previousFocus = document.activeElement;
    const textArea = document.createElement('textarea');
    textArea.value = text;
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();
    try {
        if (!document.execCommand('copy')) {
            throw new Error('The browser did not permit copying to the clipboard.');
        }
    } finally {
        document.body.removeChild(textArea);
        if (previousFocus instanceof HTMLElement && previousFocus.isConnected) {
            previousFocus.focus();
        }
    }
}
