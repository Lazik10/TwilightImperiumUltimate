export function buildFactionIndex(root) {
    try {
        const index = root?.querySelector("[data-faction-index]");
        const notes = root?.querySelector("[data-faction-notes]");
        if (!index || !notes) {
            return;
        }

        const sections = new Map();
        for (const heading of notes.querySelectorAll(".faq-markup h1")) {
            const label = heading.querySelector("sub")?.textContent
                ?.replace(/[()]/g, "")
                .trim() || "Abilities";
            const current = sections.get(label);
            const occurrence = (current?.occurrence || 0) + 1;
            heading.id = `faction-note-${slug(label)}-${occurrence}`;
            sections.set(label, {
                anchor: current?.anchor || heading.id,
                occurrence,
            });
        }

        const links = [...sections].map(([label, section]) =>
            createLink(label, section.anchor));
        const faq = root.querySelector("[data-faction-faq]");
        if (faq) {
            links.push(createLink("FAQ", faq.id));
        }

        index.replaceChildren(...links);
        index.hidden = links.length === 0;
        scrollToCurrentFragment(root);
    } catch {
        // Progressive enhancement only; the faction content remains available.
    }
}

function scrollToCurrentFragment(root) {
    const fragment = decodeURIComponent(window.location.hash.slice(1));
    if (!fragment) {
        return;
    }

    const target = document.getElementById(fragment);
    if (!target || !root.contains(target)) {
        return;
    }

    requestAnimationFrame(() => target.scrollIntoView());
}

function createLink(label, anchor) {
    const link = document.createElement("a");
    link.href = `${window.location.pathname}${window.location.search}#${anchor}`;
    link.textContent = label;
    return link;
}

function slug(value) {
    return value
        .normalize("NFKD")
        .replace(/[\u0300-\u036f]/g, "")
        .toLowerCase()
        .replace(/[^a-z0-9]+/g, "-")
        .replace(/^-|-$/g, "") || "section";
}
