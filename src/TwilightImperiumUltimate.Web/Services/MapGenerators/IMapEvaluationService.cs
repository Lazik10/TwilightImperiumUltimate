namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapEvaluationService
{
    Task<MapEvaluations> GetMapEvaluation();
}
