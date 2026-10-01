namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// Renders a list of <see cref="AccountLinkItem"/> entries, wrapping role-gated ones in
/// <c>AuthorizeView</c>. See the component markup for rationale.
/// </summary>
public partial class AccountLinksList
{
    /// <summary>
    /// Gets or sets the links to render, in order.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<AccountLinkItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the template used to render each link.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<AccountLinkItem> LinkTemplate { get; set; } = default!;
}
