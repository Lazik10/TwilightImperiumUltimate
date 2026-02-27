using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileAchievementRow
{
    [Parameter]
    public TiglUserAchievementDto Achievement { get; set; } = default!;

    [Parameter]
    public double RarityPercent { get; set; }

    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    [Parameter]
    public bool IsEarned { get; set; } = true;

    private string RarityClass => GetRarityClass();

    private TextColor EffectiveTextColor => IsEarned ? GetRarityTextColor() : TextColor.Grey;

    private string GetRarityClass()
    {
        return RarityPercent switch
        {
            < 0.1 => "rarity-legendary",
            < 0.5 => "rarity-epic",
            < 5.0 => "rarity-rare",
            < 15.0 => "rarity-uncommon",
            < 30.0 => "rarity-common",
            _ => "rarity-default",
        };
    }

    private TextColor GetRarityTextColor()
    {
        return RarityPercent switch
        {
            < 0.1 => TextColor.Pink,
            < 0.5 => TextColor.Orange,
            < 5.0 => TextColor.Purple,
            < 15.0 => TextColor.LightBlue,
            < 30.0 => TextColor.Green,
            _ => TextColor,
        };
    }

    private static string FormatDate(long unixTimestamp)
    {
        if (unixTimestamp <= 0)
            return "N/A";

        var dt = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
        return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}
