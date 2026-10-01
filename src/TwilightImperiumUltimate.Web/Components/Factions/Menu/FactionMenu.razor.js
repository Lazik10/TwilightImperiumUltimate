export function scrollToFactionMenu() {
    const element = document.querySelector('[data-container-name="faction-menu"]');

    if (element) {
        element.scrollIntoView({ behavior: "auto", block: "start" });
    }
}
