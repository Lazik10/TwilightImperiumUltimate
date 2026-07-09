namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// A single link rendered inside a desktop dropdown or a mobile collapsible navigation section.
/// </summary>
/// <param name="Href">The link destination.</param>
/// <param name="Key">A stable identifier used to track hover/active state for this link.</param>
/// <param name="Text">The localized link text.</param>
/// <param name="Target">An optional link target (for example "_blank" for external links).</param>
public sealed record NavLinkItem(string Href, string Key, string Text, string? Target = null);
