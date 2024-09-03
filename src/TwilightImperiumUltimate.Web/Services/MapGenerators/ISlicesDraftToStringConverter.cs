namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface ISlicesDraftToStringConverter
{
    IReadOnlyCollection<SliceModel> Slices { get; }

    Task InitializeSystemTiles();

    Task<string> ConvertSlicesDraftToString(IReadOnlyCollection<SliceModel> slices);

    Task<List<SliceModel>> ConvertStringToSlicesDraft(string slicesDraftString);

    Task<List<SliceModel>> GetSlicesFromSliceDraftString(string sliceDraftString);

    Task<string> CreateShareLink(string sliceDraftString);

    Task<string> GenerateSliceDraftArchiveLink(string sliceDraftName, string sliceDraftEvent);

    Task<string> ConvertSliceDraftStringToBase64String(string sliceDraftString);

    Task<string> ConvertBase64StringToSliceDraftString(string base64string);

    Task GenerateSlicesFromShareLink(string base64string);
}
