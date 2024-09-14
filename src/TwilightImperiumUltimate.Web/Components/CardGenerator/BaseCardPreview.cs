namespace TwilightImperiumUltimate.Web.Components.CardGenerator;

public abstract class BaseCardPreview : ComponentBase
{
    [Parameter]
    public virtual string CardTitle { get; set; } = string.Empty;

    [Parameter]
    public virtual string CardText { get; set; } = string.Empty;

    [Parameter]
    public CardGenerationType CardType { get; set; }

    [Inject]
    protected virtual IPathProvider PathProvider { get; set; } = null!;

    protected virtual string BackgroundImagePath => PathProvider
        .GetCardGeneratorImageBackground(CardType.ToString().ToLowerInvariant());
}
