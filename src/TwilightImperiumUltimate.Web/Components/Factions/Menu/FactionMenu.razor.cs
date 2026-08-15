using TwilightImperiumUltimate.Web.Components.Factions.Menu.Official;
using TwilightImperiumUltimate.Web.Services.Factions;

namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

public partial class FactionMenu : TwilightImperiumBaseComponent
{
    private FactionSource _source = FactionSource.Official;

    [Parameter]
    public EventCallback<FactionModel> OnClick { get; set; }

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, GetFactionMenuType());
        builder.AddAttribute(1, "OnFactionClick", OnClick);
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
