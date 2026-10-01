using System.Globalization;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class BaseTechnologyTree : TwilightImperiumBaseComponent
{
    private bool _showNotes = true;
    private bool _showFaq;
    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    private string _currentBigImageCulture = string.Empty;

    private TechnologyModel _selectedTechnologyModel = new TechnologyModel();

    [Parameter]
    public TechnologyType SelectedTechnologyType { get; set; } = TechnologyType.None;

    [Parameter]
    public IReadOnlyCollection<TechnologyModel> Technologies { get; set; } = Array.Empty<TechnologyModel>();

    private List<FaqModel> Faqs { get; set; } = new List<FaqModel>();

    protected override async Task OnParametersSetAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq);
        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            var faqs = Mapper.Map<List<FaqModel>>(response!.Data!.Items);
            Faqs = faqs.Where(f => f.FaqStatus == FaqStatus.Approved).ToList();
        }
    }

    private static string GetTechnologyTileCssClass(TechnologyModel technology) =>
        IsFullWidthTechnologyLevel(technology.Level) ? "technology-tile technology-tile-full" : "technology-tile";

    private static bool IsFullWidthTechnologyLevel(TechnologyLevel level) =>
        level is TechnologyLevel.Level2 or TechnologyLevel.Level3;

    private void ShowBigImage(TechnologyModel technology)
    {
        _currentBigImageSrc = PathProvider.GetTechnologyImagePath(technology.TechnologyName);
        _currentBigImageCulture = CultureInfo.CurrentCulture.Name;
        _selectedTechnologyModel = technology;
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

    private List<TechnologyModel> GetTechnologiesInLevelOrder()
    {
        return Technologies
            .OrderBy(x => x.Level)
            .ThenBy(x => x.TechnologyName)
            .ToList();
    }

    private void ShowFaq()
    {
        _showNotes = false;
        _showFaq = true;
    }

    private void ShowNotes()
    {
        _showFaq = false;
        _showNotes = true;
    }

    private List<FaqModel> GetSpecificCardFaqs()
    {
        return Faqs.Where(f => f.ComponentName == _selectedTechnologyModel.TechnologyName.ToString() && f.FaqStatus == FaqStatus.Approved).ToList();
    }

    private void AddFaq()
    {
        NavigationManager.NavigateTo($"/faq/create-new-faq/{_selectedTechnologyModel.TechnologyName}");
    }

    private MarkupString GetSelectedCardNotes()
    {
        return (MarkupString)_selectedTechnologyModel.TechnologyName.ToString().GetComponentNotesText();
    }
}
