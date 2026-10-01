namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Container for a set of <see cref="ResponsiveTab"/> items, rendered as an accessible
/// WAI-ARIA "tablist". Owns roving keyboard focus (Left/Right/Up/Down/Home/End) between its
/// registered tabs; each <see cref="ResponsiveTab"/> registers itself via a cascaded reference
/// to this instance so focus can move between sibling tab buttons without any JavaScript.
/// </summary>
public partial class ResponsiveTabMenu
{
    private readonly List<ResponsiveTab> _tabs = [];

    /// <summary>
    /// Gets or sets the accessible name for the tablist (e.g. "Admin sections").
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string AriaLabel { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal void Register(ResponsiveTab tab) => _tabs.Add(tab);

    internal void Unregister(ResponsiveTab tab) => _tabs.Remove(tab);

    internal ValueTask FocusPreviousAsync(ResponsiveTab current) => FocusRelativeAsync(current, -1);

    internal ValueTask FocusNextAsync(ResponsiveTab current) => FocusRelativeAsync(current, 1);

    internal ValueTask FocusFirstAsync() => _tabs.Count > 0 ? _tabs[0].FocusAsync() : ValueTask.CompletedTask;

    internal ValueTask FocusLastAsync() => _tabs.Count > 0 ? _tabs[^1].FocusAsync() : ValueTask.CompletedTask;

    private ValueTask FocusRelativeAsync(ResponsiveTab current, int delta)
    {
        var index = _tabs.IndexOf(current);

        if (index < 0 || _tabs.Count == 0)
            return ValueTask.CompletedTask;

        var nextIndex = (((index + delta) % _tabs.Count) + _tabs.Count) % _tabs.Count;

        return _tabs[nextIndex].FocusAsync();
    }
}
