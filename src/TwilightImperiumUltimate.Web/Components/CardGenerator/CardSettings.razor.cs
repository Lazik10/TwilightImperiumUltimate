using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.CardGenerator;

public partial class CardSettings
{
    [Parameter]
    public EventCallback<string> OnSettingsChangeRefreshComponent { get; set; } = default!;

    public string CardTitle { get; set; } = "Ancient Burial Sites";

    public int CardTitleFontSize { get; set; } = 28;

    public string CardText { get; set; } = "At the start of the agenda phase: Choose 1 player. " +
        "Exhaust each cultural planet owned by that player.";

    public int CardTextFontSize { get; set; } = 28;

    public string CardFlavoredText { get; set; } = "The images depicted a race that Rin had never seen before." +
        "Curisous.Could it be that this was a race that was exterminated by the Lazax?";

    public int CardFlavoredTextFontSize { get; set; } = 28;

    public string KeywordsText { get; set; } = "sustain damage";

    public CardGenerationType SelectedCardType { get; set; }

    protected void Refresh()
    {
        if (OnSettingsChangeRefreshComponent.HasDelegate)
            OnSettingsChangeRefreshComponent.InvokeAsync();
    }

    private void OnTitleInput(string? value)
    {
        CardTitle = value ?? string.Empty;
        Refresh();
    }

    private void OnTextInput(string? value)
    {
        CardText = value ?? string.Empty;
        Refresh();
    }

    private void OnFlavoredTextInput(string? value)
    {
        CardFlavoredText = value ?? string.Empty;
        Refresh();
    }

    private void OnKeywordsTextInput(string? value)
    {
        KeywordsText = value ?? string.Empty;
        if (KeywordsText != value)
            Refresh();
    }

    private void OnTitleFontSizeIncrease()
    {
        CardTitleFontSize++;
        Refresh();
    }

    private void OnTitleFontSizeDecrease()
    {
        CardTitleFontSize--;
        Refresh();
    }

    private void OnTextFontSizeIncrease()
    {
        CardTextFontSize++;
        Refresh();
    }

    private void OnTextFontSizeDecrease()
    {
        CardTextFontSize--;
        Refresh();
    }

    private void OnFlavoredTextFontSizeIncrease()
    {
        CardFlavoredTextFontSize++;
        Refresh();
    }

    private void OnFlavoredTextFontSizeDecrease()
    {
        CardFlavoredTextFontSize--;
        Refresh();
    }
}
