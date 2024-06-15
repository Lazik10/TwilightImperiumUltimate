using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Cards;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Planets
{
    private List<Planet> _planets = new();

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    private string currentBigImageCulture = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        _planets = await HttpClient.GetAsync<List<Planet>>(Paths.ApiPath_Planets);
    }

    private string GetPlanetImagePath(Planet planet)
    {
        return PathProvider.GetPlanetImagePath(planet.PlanetName.ToString());
    }

    private void ShowBigImage(Planet planet, string culture)
    {
        currentBigImageSrc = GetPlanetImagePath(planet);
        currentBigImageCulture = culture;
        showBigImage = true;
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private void SetBigImageAddress(string culture)
    {
        currentBigImageSrc = currentBigImageSrc.Replace(currentBigImageCulture, culture);
        currentBigImageCulture = culture;
        StateHasChanged();
    }
}
