namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Planets
{
    private const string ResourceFilterPrefix = "resource:";

    private const string InfluenceFilterPrefix = "influence:";

    private const string TechnologyFilterPrefix = "technology:";

    private const string TraitFilterPrefix = "trait:";

    private const string LegendaryFilterValue = "legendary";

    private const int PlanetsPageSize = 20;

    private IReadOnlyCollection<PlanetModel> _planets = new List<PlanetModel>();

    private GameVersion? _selectedGameVersion;

    private string _selectedPlanetFilter = string.Empty;

    private int _visiblePlanetsCount = PlanetsPageSize;

    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    private string _currentBigImageCulture = string.Empty;

    private string _currentBigImageAlt = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    private bool HasMorePlanetsToLoad => GetFilteredPlanetsSorted().Count() > _visiblePlanetsCount;

    protected override async Task OnInitializedAsync()
    {
        await InititalizePlanets();
    }

    private static string GetPlanetTraitDisplayName(PlanetTrait trait)
    {
        return trait switch
        {
            PlanetTrait.SpaceStation => "Space Station",
            _ => trait.ToString(),
        };
    }

    private static string GetTechnologyDisplayName(TechnologyType technologyType)
    {
        return technologyType switch
        {
            TechnologyType.Biotic => "Biotic",
            TechnologyType.Warfare => "Warfare",
            TechnologyType.Cybernetic => "Cybernetic",
            TechnologyType.Propulsion => "Propulsion",
            _ => technologyType.ToString(),
        };
    }

    private static bool MatchesTechnologyFilter(TechnologyType actualTechnology, TechnologyType selectedTechnology)
    {
        return selectedTechnology switch
        {
            TechnologyType.Cybernetic => actualTechnology == TechnologyType.Cybernetic
                || actualTechnology == TechnologyType.CyberneticWarfare
                || actualTechnology == TechnologyType.CyberneticCybernetic,
            TechnologyType.Warfare => actualTechnology == TechnologyType.Warfare
                || actualTechnology == TechnologyType.CyberneticWarfare,
            _ => actualTechnology == selectedTechnology,
        };
    }

    private static bool MatchesPlanetTraitFilter(PlanetTrait actualTrait, PlanetTrait selectedTrait)
    {
        return selectedTrait switch
        {
            PlanetTrait.Cultural => actualTrait == PlanetTrait.Cultural
                || actualTrait == PlanetTrait.CulturalHazardous
                || actualTrait == PlanetTrait.IndustrialCultural,
            PlanetTrait.Hazardous => actualTrait == PlanetTrait.Hazardous
                || actualTrait == PlanetTrait.CulturalHazardous
                || actualTrait == PlanetTrait.HazardousIndustrial,
            PlanetTrait.Industrial => actualTrait == PlanetTrait.Industrial
                || actualTrait == PlanetTrait.HazardousIndustrial
                || actualTrait == PlanetTrait.IndustrialCultural,
            _ => actualTrait == selectedTrait,
        };
    }

    private string GetPlanetImagePath(PlanetModel planet)
    {
        return PathProvider.GetPlanetImagePath(planet.PlanetName.ToString());
    }

    private IEnumerable<PlanetModel> GetFilteredPlanetsSorted()
    {
        var filteredPlanets = ApplyPlanetFilter(
            _selectedGameVersion.HasValue
                ? _planets.Where(x => x.GameVersion == _selectedGameVersion.Value)
                : _planets);

        return filteredPlanets
            .OrderBy(x => x.GameVersion)
            .ThenBy(x => x.PlanetName);
    }

    private IEnumerable<IGrouping<GameVersion, PlanetModel>> GetVisiblePlanets()
    {
        return GetFilteredPlanetsSorted()
            .Take(_visiblePlanetsCount)
            .GroupBy(x => x.GameVersion);
    }

    private Task LoadMorePlanets()
    {
        _visiblePlanetsCount += PlanetsPageSize;
        return Task.CompletedTask;
    }

    private IEnumerable<GameVersion> GetAvailableGameVersions()
    {
        return _planets
            .Select(x => x.GameVersion)
            .Distinct()
            .OrderBy(x => x);
    }

    private Task OnGameVersionFilterChanged(GameVersion? gameVersion)
    {
        _selectedGameVersion = gameVersion;
        _visiblePlanetsCount = PlanetsPageSize;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private List<PlanetFilterOption> GetPlanetFilterOptions()
    {
        var options = new List<PlanetFilterOption>
        {
            new(string.Empty, "All"),
        };

        var maxResources = _planets.Any() ? _planets.Max(x => x.Resources) : 0;
        for (int i = 0; i <= maxResources; i++)
        {
            options.Add(new($"{ResourceFilterPrefix}{i}", $"{i} Resources"));
        }

        var maxInfluence = _planets.Any() ? _planets.Max(x => x.Influence) : 0;
        for (int i = 0; i <= maxInfluence; i++)
        {
            options.Add(new($"{InfluenceFilterPrefix}{i}", $"{i} Influence"));
        }

        options.Add(new(LegendaryFilterValue, "Legendary"));

        var technologies = new[]
        {
            TechnologyType.Biotic,
            TechnologyType.Warfare,
            TechnologyType.Cybernetic,
            TechnologyType.Propulsion,
        };

        options.AddRange(technologies.Select(x => new PlanetFilterOption($"{TechnologyFilterPrefix}{x}", GetTechnologyDisplayName(x))));

        var traits = new[]
        {
            PlanetTrait.Industrial,
            PlanetTrait.Cultural,
            PlanetTrait.Hazardous,
            PlanetTrait.Relic,
            PlanetTrait.SpaceStation,
        };

        options.AddRange(traits.Select(x => new PlanetFilterOption($"{TraitFilterPrefix}{x}", GetPlanetTraitDisplayName(x))));

        return options;
    }

    private Task OnPlanetFilterChanged(object value)
    {
        _selectedPlanetFilter = value?.ToString() ?? string.Empty;
        _visiblePlanetsCount = PlanetsPageSize;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private IEnumerable<PlanetModel> ApplyPlanetFilter(IEnumerable<PlanetModel> planets)
    {
        if (string.IsNullOrWhiteSpace(_selectedPlanetFilter))
            return planets;

        if (string.Equals(_selectedPlanetFilter, LegendaryFilterValue, StringComparison.OrdinalIgnoreCase))
            return planets.Where(x => x.IsLegendary);

        if (_selectedPlanetFilter.StartsWith(ResourceFilterPrefix, StringComparison.Ordinal))
        {
            var value = _selectedPlanetFilter[ResourceFilterPrefix.Length..];
            if (int.TryParse(value, out var resources))
                return planets.Where(x => x.Resources == resources);

            return planets;
        }

        if (_selectedPlanetFilter.StartsWith(InfluenceFilterPrefix, StringComparison.Ordinal))
        {
            var value = _selectedPlanetFilter[InfluenceFilterPrefix.Length..];
            if (int.TryParse(value, out var influence))
                return planets.Where(x => x.Influence == influence);

            return planets;
        }

        if (_selectedPlanetFilter.StartsWith(TechnologyFilterPrefix, StringComparison.Ordinal))
        {
            var value = _selectedPlanetFilter[TechnologyFilterPrefix.Length..];
            if (Enum.TryParse<TechnologyType>(value, out var technologyType))
                return planets.Where(x => MatchesTechnologyFilter(x.TechnologySkip, technologyType));

            return planets;
        }

        if (_selectedPlanetFilter.StartsWith(TraitFilterPrefix, StringComparison.Ordinal))
        {
            var value = _selectedPlanetFilter[TraitFilterPrefix.Length..];
            if (Enum.TryParse<PlanetTrait>(value, out var planetTrait))
                return planets.Where(x => MatchesPlanetTraitFilter(x.PlanetTrait, planetTrait));

            return planets;
        }

        return planets;
    }

    private void ShowBigImage(PlanetModel planet, string culture)
    {
        _currentBigImageSrc = GetPlanetImagePath(planet);
        _currentBigImageCulture = culture;
        _currentBigImageAlt = planet.PlanetName.ToString();
        _showBigImage = true;
    }

    private void HideBigImage()
    {
        _showBigImage = false;
    }

    private void SetBigImageAddress(string culture)
    {
        _currentBigImageSrc = _currentBigImageSrc.Replace(_currentBigImageCulture, culture, StringComparison.Ordinal);
        _currentBigImageCulture = culture;
        StateHasChanged();
    }

    private async Task InititalizePlanets()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<PlanetDto>>>(Paths.ApiPath_Planets);
        if (statusCode == HttpStatusCode.OK)
        {
            var planets = Mapper.Map<List<PlanetModel>>(response!.Data!.Items);
            _planets = planets
                .Where(x => x.PlanetName != PlanetName.MalliceInactive)
                .OrderBy(x => x.GameVersion)
                .ThenBy(x => x.PlanetName)
                .ToList();
        }
    }

    private sealed record PlanetFilterOption(string Value, string Label);
}
