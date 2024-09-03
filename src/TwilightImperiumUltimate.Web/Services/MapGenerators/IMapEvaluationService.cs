namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapEvaluationService
{
    Task<MapEvaluations> GetMapEvaluation();

    Task<MapEvaluations> GetMapEvaluation(MapTemplate mpaTemplate, IReadOnlyDictionary<int, SystemTileModel> map);
}
