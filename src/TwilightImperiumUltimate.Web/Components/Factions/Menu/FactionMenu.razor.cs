using TwilightImperiumUltimate.Web.Components.Factions.Menu.DiscordantStars;
using TwilightImperiumUltimate.Web.Components.Factions.Menu.Official;

namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

public partial class FactionMenu : TwilightImperiumBaseComponent
{
    private FactionSource _source = FactionSource.Official;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, GetFactionMenuType());
        builder.CloseComponent();
    };

    protected override async Task OnInitializedAsync()
    {
        _source = FactionProvider.CurrentSource;
    }

    private Type GetFactionMenuType() => _source switch
    {
        FactionSource.TwilightsFall => typeof(TwilightsFallFactionMenu),
        FactionSource.DiscordantStars => typeof(DiscordantStarsFactionMenu),
        FactionSource.BlueRiverie => typeof(BlueRiverieFactionMenu),
        FactionSource.WhispersFromTheVoid => typeof(WhispersFromTheVoidFactionMenu),
        _ => typeof(OfficialFactionMenu),
    };
}
