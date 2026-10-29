export function isAtBottom(element) {
    return element.scrollHeight - element.scrollTop <= element.clientHeight + 1;
}

export function scrollToBottom(element) {
    element.scrollTop = element.scrollHeight;
}
