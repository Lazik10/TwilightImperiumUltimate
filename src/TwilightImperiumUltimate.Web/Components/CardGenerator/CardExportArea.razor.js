export function saveCardAsImage(elementId, imageName) {
    const element = document.getElementById(elementId);
    html2canvas(element).then((canvas) => {
        const imgData = canvas.toDataURL('image/png');
        const a = document.createElement('a');
        a.href = imgData;
        a.download = imageName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    });
}