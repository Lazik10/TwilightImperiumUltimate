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

    [Parameter]
    public EventCallback<IconType> OnClick { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string IconPath => PathProvider.GetIconPath(_currentIconType);

    protected override void OnInitialized()
    {
        _currentIconType = IconType;
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