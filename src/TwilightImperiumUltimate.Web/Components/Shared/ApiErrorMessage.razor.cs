namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class ApiErrorMessage
{
    /// <summary>
    /// Gets or sets additional CSS classes applied to the rendered message, so callers can match
    /// their page's existing state-message styling (e.g. spacing/alignment) instead of every page
    /// having to redefine its own look for this generic error.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;
}
