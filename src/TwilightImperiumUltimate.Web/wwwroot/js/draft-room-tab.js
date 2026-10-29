export function openDraftRoom(url) {
    const openedWindow = window.open("about:blank", "_blank");
    if (openedWindow === null) {
        return false;
    }

    openedWindow.opener = null;
    openedWindow.location.replace(url);
    return true;
}
