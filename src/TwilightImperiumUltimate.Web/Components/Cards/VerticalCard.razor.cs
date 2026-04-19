namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class VerticalCard
{
    private bool _isImageLoaded;

    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string TypeOfCard { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string CardImagePath => PathProvider.GetCardImagePath(Name, TypeOfCard);

    private string GetContainerClass()
    {
        var baseClass = TypeOfCard == Strings.StrategyCard ? "strategy-image-item" : "image-item";
        return _isImageLoaded ? $"{baseClass} image-ready" : baseClass;
    }

    private string GetImageStyle() => _isImageLoaded
        ? "visibility: visible; opacity: 1;"
        : "visibility: hidden; opacity: 0;";

    protected override void OnParametersSet()
    {
        _isImageLoaded = false;
    }

    private void OnImageLoaded()
    {
        _isImageLoaded = true;
        StateHasChanged();
    }

    private void OnImageError()
    {
        _isImageLoaded = false;
        StateHasChanged();
    }
}