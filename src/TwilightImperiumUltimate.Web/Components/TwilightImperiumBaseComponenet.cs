using Blazored.LocalStorage;

namespace TwilightImperiumUltimate.Web.Components;

public class TwilightImperiumBaseComponenet : ComponentBase
{
    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Inject]
    protected IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    protected ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ILocalStorageService LocalStorageService { get; set; } = default!;

    [Inject]
    protected IMapper Mapper { get; set; } = default!;
}
