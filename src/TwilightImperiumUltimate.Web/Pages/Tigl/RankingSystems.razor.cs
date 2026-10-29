namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class RankingSystems
{
    private bool _showFractured;

    private string ActiveTabId => _showFractured
        ? "ranking-systems-tab-fractured"
        : "ranking-systems-tab-standard";

    private void ShowStandard() => _showFractured = false;

    private void ShowFractured() => _showFractured = true;
}
