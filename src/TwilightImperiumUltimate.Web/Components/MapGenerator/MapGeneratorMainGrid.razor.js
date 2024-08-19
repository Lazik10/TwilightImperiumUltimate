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