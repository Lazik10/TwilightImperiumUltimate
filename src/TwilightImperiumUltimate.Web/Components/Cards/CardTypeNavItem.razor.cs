namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class CardTypeNavItem
{
    [Parameter]
    public string TypeOfCard { get; set; } = string.Empty;

    [Parameter]
    public string IconPath { get; set; } = string.Empty;

    [Parameter]
    public string AltText { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public bool IsSelected { get; set; }

    [Parameter]
    public bool NoBorder { get; set; }

    [Parameter]
    public EventCallback<string> OnSelect { get; set; }

    private Task HandleClick() => OnSelect.InvokeAsync(TypeOfCard);
}
