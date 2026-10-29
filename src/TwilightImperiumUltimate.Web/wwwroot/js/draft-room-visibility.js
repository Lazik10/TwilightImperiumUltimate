export async function observe(receiver) {
    const report = () => receiver.invokeMethodAsync("SetHidden", document.visibilityState === "hidden");
    await report();
    document.addEventListener("visibilitychange", report);
    return {
        dispose: () => document.removeEventListener("visibilitychange", report)
    };
}
