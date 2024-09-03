using TwilightImperiumUltimate.Web.Models.SlicesArchive;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Components.SlicesArchive;

public partial class AddToSlicesArchiveGrid
{
    private SliceDraftModel _sliceDraftModel = new SliceDraftModel();

    private List<SliceModel> _slices = new List<SliceModel>();

    private TwilightImperiumUser? _user;

    private bool _enableEditModeForDescription = true;

    private bool _showMapDuplicateError;

    [CascadingParameter(Name = "TilesValue")]
    public string TilesImported { get; set; } = string.Empty;

    private MarkupString MarkupStringDescription => (MarkupString)_sliceDraftModel.Description;

    private MarkupString SlicesString => CreatePreviewSlicesString();

    [Inject]
    private ISlicesDraftToStringConverter SlicesDraftToStringConverter { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(TilesImported))
        {
            var slicesDraftString = await SlicesDraftToStringConverter.ConvertBase64StringToSliceDraftString(TilesImported);
            _slices = await SlicesDraftToStringConverter.ConvertStringToSlicesDraft(slicesDraftString);

            _user = await UserService.GetCurrentUserAsync();

            await AssignSliceDraftModelStringsAndUserInfo();
        }
    }

    private MarkupString CreatePreviewSlicesString()
    {
        var slices = _sliceDraftModel.SliceDraftString.Replace("\n", "<br/>");
        return (MarkupString)slices;
    }

    private bool IsNameOrEventNotFilled()
    {
        return string.IsNullOrEmpty(_sliceDraftModel.EventName) || string.IsNullOrWhiteSpace(_sliceDraftModel.EventName)
            || string.IsNullOrEmpty(_sliceDraftModel.Name) || string.IsNullOrWhiteSpace(_sliceDraftModel.Name);
    }

    private async Task SaveSliceDraft()
    {
        _showMapDuplicateError = false;

        if (IsNameOrEventNotFilled())
            return;

        var sliceNames = _slices.Select(x => x.Name);
        var correctedSliceNames = sliceNames.Select(x => x.Replace(",", string.Empty)).ToList();
        _sliceDraftModel.SliceNames = string.Join(',', correctedSliceNames);
        _sliceDraftModel.SliceCount = _slices.Count;

        var sliceDraftDto = Mapper.Map<SliceDraftDto>(_sliceDraftModel);
        var (response, statusCode) = await HttpClient.PostAsync<SliceDraftDto, ApiResponse<SliceDraftDto>>(Paths.ApiPath_AddNewSliceDraftToArchive, sliceDraftDto);

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            NavigationManager.NavigateTo(Pages.Pages.SlicesArchive);
        }
        else if (statusCode == HttpStatusCode.OK && !response.Success)
        {
            _showMapDuplicateError = true;
        }
    }

    private async Task AssignSliceDraftModelStringsAndUserInfo()
    {
        _sliceDraftModel.SliceDraftString = await SlicesDraftToStringConverter.ConvertBase64StringToSliceDraftString(TilesImported);
        _sliceDraftModel.SliceDraftGeneratorLink = await SlicesDraftToStringConverter.CreateShareLink(_sliceDraftModel.SliceDraftString);
        _sliceDraftModel.SliceDraftArchiveLink = await SlicesDraftToStringConverter.GenerateSliceDraftArchiveLink(_sliceDraftModel.Name, _sliceDraftModel.EventName);
        _sliceDraftModel.UserName = _user?.UserName ?? string.Empty;
        _sliceDraftModel.UserId = _user?.Id ?? string.Empty;
    }

    private void ToggleEditDescription(bool editMode)
    {
        _enableEditModeForDescription = editMode;
        StateHasChanged();
    }

    private async Task HandleNameChange(ChangeEventArgs e)
    {
        _sliceDraftModel.Name = e?.Value?.ToString() ?? string.Empty;
        _sliceDraftModel.SliceDraftArchiveLink = await SlicesDraftToStringConverter.GenerateSliceDraftArchiveLink(_sliceDraftModel.Name, _sliceDraftModel.EventName);
        StateHasChanged();
    }

    private async Task HandleEventChange(ChangeEventArgs e)
    {
        _sliceDraftModel.EventName = e?.Value?.ToString() ?? string.Empty;
        _sliceDraftModel.SliceDraftArchiveLink = await SlicesDraftToStringConverter.GenerateSliceDraftArchiveLink(_sliceDraftModel.Name, _sliceDraftModel.EventName);
        StateHasChanged();
    }
}