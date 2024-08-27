using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftMainDraft
{
    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    [Inject]
    private IMiltyDraftSettingsService MiltyDraftSettingsService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private IReadOnlyCollection<SliceModel> Slices => GetUpdatedSlices();

    private IReadOnlyCollection<MiltyDraftInitiativeModel> Initiatives { get; set; } = new List<MiltyDraftInitiativeModel>();

    private MiltyDraftState DraftState => MiltyDraftService.State;

    private MiltyDraftPlayerModel ActivePlayer => MiltyDraftService.ActivePlayer;

    private DraftColor ActivePlayerColor => MiltyDraftService.ActivePlayer.PlayerColor;

    private IReadOnlyCollection<MiltyDraftPlayerModel> PlayersOrderOfPicks { get; set; } = new List<MiltyDraftPlayerModel>();

    private IReadOnlyCollection<MiltyDraftFactionModel> DraftedFactions { get; set; } = new List<MiltyDraftFactionModel>();

    public async Task NewPickOrder()
    {
        await MiltyDraftService.RegeneratePlayerOrder();
        await Refresh();
    }

    public async Task NewFactions()
    {
        await MiltyDraftService.RegenerateFactions();
        await Refresh();
    }

    public async Task NewSlices()
    {
        await MiltyDraftService.RegenerateSlices();
        await Refresh();
    }

    public async Task Refresh()
    {
        Initiatives = MiltyDraftService.Initiatives.Take(MiltyDraftSettingsService.NumberOfPlayers).ToList();
        PlayersOrderOfPicks = await MiltyDraftService.NextPicks();
        DraftedFactions = MiltyDraftService.DraftedFactions;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        if (MiltyDraftService.State == MiltyDraftState.Initialized)
        {
            await MiltyDraftService.ResetMiltyDraft();
            await MiltyDraftService.InitializeFactions();
        }

        Initiatives = MiltyDraftService.Initiatives.Take(MiltyDraftSettingsService.NumberOfPlayers).ToList();
        PlayersOrderOfPicks = await MiltyDraftService.NextPicks();
        DraftedFactions = MiltyDraftService.DraftedFactions;
    }

    private async Task GetNextPicks()
    {
        PlayersOrderOfPicks = await MiltyDraftService.NextPicks();
    }

    private IReadOnlyCollection<SliceModel> GetUpdatedSlices() => MiltyDraftService.Slices;

    private async Task StartMiltyDraft()
    {
        if (MiltyDraftService.State == MiltyDraftState.Initialized)
        {
            if (MiltyDraftService.NotEnoughFactionsPicked || await IsImportedSlicesEnabledAndImportedStringInvalid())
                return;

            await MiltyDraftService.StartMiltyDraft();
            await GetNextPicks();
            Initiatives = MiltyDraftService.Initiatives.Take(MiltyDraftSettingsService.NumberOfPlayers).ToList();
            DraftedFactions = MiltyDraftService.DraftedFactions;
            StateHasChanged();
        }
        else if (MiltyDraftService.State == MiltyDraftState.Started || MiltyDraftService.State == MiltyDraftState.Finished)
        {
            await MiltyDraftService.ResetMiltyDraft();
        }
    }

    private string GetButtonText()
    {
        if (MiltyDraftService.State == MiltyDraftState.Started || MiltyDraftService.State == MiltyDraftState.Finished)
        {
            return Strings.MiltyDraft_ResetDraft;
        }
        else if (MiltyDraftService.State == MiltyDraftState.Initialized)
        {
            if (MiltyDraftService.NotEnoughFactionsPicked)
            {
                return Strings.MiltyDraft_NotEnoughFactions;
            }

            return Strings.MiltyDraft_StartMiltyDraft;
        }

        return string.Empty;
    }

    private bool GetButtonStatus() => (MiltyDraftService.State == MiltyDraftState.Initialized
        && MiltyDraftService.NotEnoughFactionsPicked)
        || IsImportedSlicesEnabledAndImportedStringInvalid().Result;

    private string GetPlayerName(MiltyDraftPlayerModel player) => MiltyDraftSettingsService.EnablePlayerNames
        ? player.PlayerName
        : player.PlayerDefaultName;

    private async Task HandleDownloadImage(IconType iconType)
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MiltyDraft/MiltyDraftMainDraft.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "draftArea", $"TI4_Ultimate_MiltyDraft_{DateTime.Now.ToLocalTime().ToLongTimeString()}.png");
    }

    private async Task<bool> IsImportedSlicesEnabledAndImportedStringInvalid()
    {
        return MiltyDraftSettingsService.ImportSlices && !await MiltyDraftService.IsValidImportSlicesString();
    }
}