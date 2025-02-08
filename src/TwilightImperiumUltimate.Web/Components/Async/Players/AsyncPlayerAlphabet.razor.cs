namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayerAlphabet
{
    private static readonly List<char> Alphabet = [.. "*1ABCDEFGHIJKLMNOPQRSTUVWXYZ"];

    [Parameter]
    public EventCallback<char> OnLetterClick { get; set; }

    private void SearchPlayer(char letter)
    {
        OnLetterClick.InvokeAsync(letter);
    }
}