using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Code-behind for <see cref="ResponsiveColumnFilterControl{TItem}"/>.
/// </summary>
public partial class ResponsiveColumnFilterControl<TItem>
{
    /// <summary>
    /// Gets or sets the column this filter control belongs to.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ResponsiveTableColumn<TItem> Column { get; set; } = default!;

    /// <summary>
    /// Gets or sets the mutable filter state to read from and write to.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ResponsiveColumnFilterState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets a unique id to use for the underlying input(s), so labels/inputs stay
    /// associated even when multiple columns render the same filter type.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string InputId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a callback raised whenever the filter value changes.
    /// </summary>
    [Parameter]
    public EventCallback OnChanged { get; set; }

    private List<ResponsiveEnumFilterOption> EnumOptions => GetEnumOptions();

    private async Task OnTextValueChanged(string value)
    {
        State.TextValue = value;
        await OnChanged.InvokeAsync();
    }

    private async Task OnValueOperatorChanged(ResponsiveValueFilterOperator value)
    {
        State.ValueOperator = value;
        await OnChanged.InvokeAsync();
    }

    private async Task OnBooleanValueChanged(bool? value)
    {
        State.BooleanValue = value;
        await OnChanged.InvokeAsync();
    }

    private async Task OnEnumValueChanged(object value)
    {
        State.EnumValue = value;
        await OnChanged.InvokeAsync();
    }

    private List<ResponsiveEnumFilterOption> GetEnumOptions()
    {
        var enumType = Column.PropertyType;
        if (enumType is null)
            return [];

        var underlyingType = Nullable.GetUnderlyingType(enumType) ?? enumType;
        if (!underlyingType.IsEnum)
            return [];

        return Enum.GetValues(underlyingType)
            .Cast<object>()
            .Select(value => new ResponsiveEnumFilterOption(value.ToString() ?? string.Empty, value))
            .ToList();
    }
}
