using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglGameHistoryTable
{
    [Parameter]
    public IReadOnlyList<TiglProfileGameDto> Games { get; set; } = [];

    [Parameter]
    public EventCallback<int> OnGameSelected { get; set; }

    private static string GetDuration(TiglProfileGameDto game)
    {
        if (game.StartTimestamp <= 0 || game.EndTimestamp <= 0)
            return "N/A";

        var duration = DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp) - DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp);
        return duration.TotalHours < 1 ? "<1 h" : $"{(int)duration.TotalHours} h";
    }
}
