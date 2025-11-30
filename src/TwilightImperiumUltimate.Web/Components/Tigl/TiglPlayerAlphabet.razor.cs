namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayerAlphabet
{
    private static readonly List<char> Alphabet = [.. "*1ABCDEFGHIJKLMNOPQRSTUVWXYZ"];

    [Parameter]
    public EventCallback<char> OnLetterClick { get; set; }

    private void SelectLetter(char letter) => OnLetterClick.InvokeAsync(letter);
}
