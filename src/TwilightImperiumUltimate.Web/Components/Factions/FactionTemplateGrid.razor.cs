using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionTemplateGrid
{
    private FactionModel _selectedFaction = default!;

    private FactionInfoType _selectedFactionInfoType = FactionInfoType.Ability;

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    /// <summary>
    /// Gets the currently selected faction, or <see langword="null"/> before a faction has been selected.
    /// Used by the hosting page to build an SEO-friendly page title and meta description.
    /// </summary>
    public FactionModel? SelectedFaction => _selectedFaction;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, CreateInfoType());
        builder.AddAttribute(1, "Faction", _selectedFaction);
        builder.CloseComponent();
    };

    public void UpdateSelectedFaction(FactionModel faction)
    {
        _selectedFaction = faction;
        StateHasChanged();
    }

    public void SetFactionInfo(string factionInfoType)
    {
        if (Enum.TryParse<FactionInfoType>(factionInfoType, out var infoType))
        {
            SetFactionInfoType(infoType);
        }

        StateHasChanged();
    }

    private Type CreateInfoType()
    {
        var infoType = _selectedFactionInfoType switch
        {
            FactionInfoType.Ability => typeof(FactionAbilities),
            FactionInfoType.Setup => typeof(FactionSetup),
            FactionInfoType.Components => typeof(FactionComponents),
            FactionInfoType.Leaders => typeof(FactionLeaders),
            FactionInfoType.History => typeof(FactionLore),
            FactionInfoType.Faq => typeof(FactionFaq),
            _ => throw new NotImplementedException(),
        };

        return infoType;
    }

    private void ShowBigImage(bool front, bool isObsidian)
    {
        if (_selectedFaction is not null)
        {
            var factionName = _selectedFaction.FactionName.ToString();
            if (_selectedFaction.FactionName == FactionName.TheFirmamentTheObsidian)
            {
                if (isObsidian)
                    factionName = "TheObsidian";
                else
                    factionName = "TheFirmament";
            }

            currentBigImageSrc = PathProvider.GetFactionSheetPath(factionName, front);
            showBigImage = true;
        }
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private void SetFactionInfoType(FactionInfoType factionInfoType)
    {
        _selectedFactionInfoType = factionInfoType;
    }
}
