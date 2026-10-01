using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Components.Factions.Menu.DiscordantStars;
using TwilightImperiumUltimate.Web.Components.Factions.Menu.Official;

namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

public partial class FactionMenu : TwilightImperiumBaseComponent, IAsyncDisposable
{
    private FactionSource _source = FactionSource.Official;
    private FactionName _lastScrolledFactionName;
    private IJSObjectReference? _jsModule;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, GetFactionMenuType());
        builder.CloseComponent();
    };

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Runtime already gone; nothing left to clean up.
            }
        }

        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized()
    {
        _source = FactionProvider.CurrentSource;
        _lastScrolledFactionName = FactionProvider.CurrentFactionName;
    }

    // Keeps the menu pinned to the top of the viewport when another faction is opened, instead of
    // letting the router reset the scroll position to the very top of the page every time.
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender || _lastScrolledFactionName == FactionProvider.CurrentFactionName)
            return;

        _lastScrolledFactionName = FactionProvider.CurrentFactionName;

        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Factions/Menu/FactionMenu.razor.js");

        await _jsModule.InvokeVoidAsync("scrollToFactionMenu");
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
