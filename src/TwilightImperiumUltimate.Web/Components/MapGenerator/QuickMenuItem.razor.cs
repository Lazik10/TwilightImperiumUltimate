using System.Text;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class QuickMenuItem
{
    private IconType _currentIconType;

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public IconType IconTypeClicked { get; set; }

    [Parameter]
    public int MaxWidth { get; set; } = 26;

    /// <summary>
    /// Gets or sets the accessible name announced for this icon-only control. When not set, the
    /// name is derived from <see cref="IconType"/> so the button is never left unlabeled.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public EventCallback<IconType> OnClick { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string IconPath => PathProvider.GetIconPath(_currentIconType);

    private string AccessibleName => string.IsNullOrWhiteSpace(AriaLabel)
        ? HumanizeIconType(IconType)
        : AriaLabel;

    protected override void OnInitialized()
    {
        _currentIconType = IconType;
    }

    private static string HumanizeIconType(IconType iconType)
    {
        var name = iconType.ToString();
        var builder = new StringBuilder(name.Length + 8);

        for (var i = 0; i < name.Length; i++)
        {
            if (i > 0 && char.IsUpper(name[i]))
                builder.Append(' ');

            builder.Append(name[i]);
        }

        return builder.ToString();
    }

    private async Task HandleClick()
    {
        _currentIconType = IconTypeClicked;
        StateHasChanged();

        await OnClick.InvokeAsync(IconType);

        await Task.Delay(1000);

        _currentIconType = IconType;
        StateHasChanged();
    }
}
