namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    Task<IReadOnlyDictionary<int, int>> GenerateMapAsync();
}
