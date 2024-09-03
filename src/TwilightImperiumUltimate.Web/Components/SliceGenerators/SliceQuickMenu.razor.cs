using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceQuickMenu
{
    private bool _showImportSliceDraftString;

    private string _importedSliceDraftString = string.Empty;

    [Inject]
    public ISlicesDraftToStringConverter SlicesDraftToStringConverter { get; set; } = default!;

    [Parameter]
    public EventCallback<IconType> OnMenuIconClick { get; set; }

    private async Task HandleMenuIconClick(IconType iconType)
    {
        if (iconType == IconType.ImportMapString)
        {
            _showImportSliceDraftString = !_showImportSliceDraftString;
            return;
        }

        await OnMenuIconClick.InvokeAsync(iconType);
    }

    private async Task LoadSlicesFromSlicesDraftString()
    {
        await SlicesDraftToStringConverter.ConvertStringToSlicesDraft(_importedSliceDraftString);
        await OnMenuIconClick.InvokeAsync(IconType.ImportMapString);
    }
}