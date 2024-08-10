namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class QuickMenuItem
{
    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public int MaxWidth { get; set; } = 26;

    [Parameter]
    public EventCallback<IconType> OnClick { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string IconPath => PathProvider.GetIconPath(IconType);

    private void HandleClick()
    {
        OnClick.InvokeAsync(IconType);
    }
}