namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Planets
{
    private IReadOnlyCollection<PlanetModel> _planets = new List<PlanetModel>();

    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    private string _currentBigImageCulture = string.Empty;

    private GameVersion? _currentGameVersion;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InititalizePlanets();
    }

    private string GetPlanetImagePath(PlanetModel planet)
    {
        return PathProvider.GetPlanetImagePath(planet.PlanetName.ToString());
    }

    private void ShowBigImage(PlanetModel planet, string culture)
    {
        _currentBigImageSrc = GetPlanetImagePath(planet);
        _currentBigImageCulture = culture;
        _showBigImage = true;
    }

    private void HideBigImage()
    {
        _showBigImage = false;
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private void SetBigImageAddress(string culture)
    {
        _currentBigImageSrc = _currentBigImageSrc.Replace(_currentBigImageCulture, culture);
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
}
