using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Factions;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionIconRow
{
    private List<FactionModel>? _factions = new List<FactionModel>();

    [Parameter]
    public EventCallback<FactionModel> OnFactionClickGetFaction { get; set; }

    [Parameter]
    public EventCallback<IReadOnlyCollection<FactionModel>> OnInitializeGetFactions { get; set; }

    [Parameter]
    public bool EnableBanMode { get; set; } = false;

    [Parameter]
    public bool BanAllFactions { get; set; } = false;

    public IReadOnlyCollection<FactionModel>? Factions => _factions;

    public void SetAllFactionsBanStatus(bool banStatus)
    {
        if (_factions is not null)
            _factions.ForEach(x => x.Banned = banStatus);
    }

    protected override async Task OnInitializedAsync()
    {
        _factions = await HttpClient.GetAsync<List<FactionModel>>(Paths.ApiPath_Factions);
        await OnInitializeGetFactions.InvokeAsync(Factions);

        if (BanAllFactions)
            SetAllFactionsBanStatus(true);
    }

    private void FactionClicked(FactionModel selectedFaction)
    {
        if (EnableBanMode)
            selectedFaction.Banned = !selectedFaction.Banned;

        OnFactionClickGetFaction.InvokeAsync(selectedFaction);
    }
}
