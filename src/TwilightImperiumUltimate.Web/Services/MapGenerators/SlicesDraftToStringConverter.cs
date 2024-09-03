using System.Text;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class SlicesDraftToStringConverter(
    ITwilightImperiumApiHttpClient httpClient,
    IMapper mapper,
    NavigationManager navigationManager)
    : ISlicesDraftToStringConverter
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;
    private readonly NavigationManager _navigationManager = navigationManager;

    private List<SystemTileModel> _systemTiles = new List<SystemTileModel>();
    private List<SliceModel> _slices = new List<SliceModel>();

    public IReadOnlyCollection<SliceModel> Slices => _slices;

    public Task<string> ConvertSlicesDraftToString(IReadOnlyCollection<SliceModel> slices)
    {
        StringBuilder sliceString = new();
        foreach (var slice in slices)
        {
            foreach (var systemTile in slice.SystemTiles.Skip(1))
            {
                if (systemTile.GameVersion == GameVersion.Custom)
                {
                    sliceString.Append('E');
                    sliceString.Append(',');
                }
                else
                {
                    sliceString.Append(systemTile.SystemTileCode);
                    sliceString.Append(',');
                }
            }

            sliceString.Append('\n');
        }

        return Task.FromResult(sliceString.ToString());
    }

    public Task<List<SliceModel>> GetSlicesFromSliceDraftString(string sliceDraftString)
    {
        throw new NotImplementedException();
    }

    public Task<List<SliceModel>> ConvertStringToSlicesDraft(string slicesDraftString)
    {
        var importedSlices = slicesDraftString
            .Trim()
            .Split('\n')
            .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
            .ToList();

        var slices = new List<SliceModel>();

        for (var i = 0; i < importedSlices.Count; i++)
        {
            var slice = new SliceModel()
            {
                Id = i,
                SystemTiles = new List<SystemTileModel>(),
            };

            var homePlaceholder = _systemTiles.Find(x => x.SystemTileName == SystemTileName.TileHome);
            if (homePlaceholder is not null)
            {
                slice.SystemTiles.Add(homePlaceholder);
            }

            var systemTileCodes = importedSlices[i]
                .Trim()
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (systemTileCodes.Count == 5)
            {
                foreach (var code in systemTileCodes)
                {
                    if (code == "E")
                    {
                        var systemTile = _systemTiles.Find(x => x.SystemTileName == SystemTileName.TileEmpty);
                        if (systemTile is not null)
                        {
                            slice.SystemTiles.Add(systemTile);
                        }
                    }
                    else
                    {
                        var systemTile = _systemTiles.Find(x => x.SystemTileCode == code);
                        if (systemTile is not null)
                        {
                            slice.SystemTiles.Add(systemTile);
                        }
                    }
                }
            }

            if (slice.SystemTiles.Count == 6)
                slices.Add(slice);
        }

        if (slices.Count > 9)
            _slices = slices.Take(9).ToList();
        else
            _slices = slices;

        return Task.FromResult(_slices);
    }

    public async Task<string> CreateShareLink(string sliceDraftString)
    {
        var base64string = await ConvertSliceDraftStringToBase64String(sliceDraftString);
        var baseAddress = _navigationManager.BaseUri;
        var sliceUrl = $"{baseAddress}tools/slice-generator?tiles={base64string}";
        return sliceUrl;
    }

    public Task<string> ConvertBase64StringToSliceDraftString(string base64string)
    {
        var bytes = Convert.FromBase64String(base64string);
        return Task.FromResult(Encoding.UTF8.GetString(bytes));
    }

    public Task<string> ConvertSliceDraftStringToBase64String(string sliceDraftString)
    {
        var base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(sliceDraftString));
        return Task.FromResult(base64string);
    }

    public async Task InitializeSystemTiles()
    {
        if (_systemTiles.Count > 0)
            return;

        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
        {
            _systemTiles = _mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
        }
    }

    public async Task GenerateSlicesFromShareLink(string base64string)
    {
        var slicesString = await ConvertBase64StringToSliceDraftString(base64string);
        await ConvertStringToSlicesDraft(slicesString);
    }

    public Task<string> GenerateSliceDraftArchiveLink(string sliceDraftName, string sliceDraftEvent)
    {
        var baseAddress = _navigationManager.BaseUri;
        var mapUrl = $"{baseAddress}community/slices-archive/slice-draft/";
        return Task.FromResult(mapUrl);
    }
}
