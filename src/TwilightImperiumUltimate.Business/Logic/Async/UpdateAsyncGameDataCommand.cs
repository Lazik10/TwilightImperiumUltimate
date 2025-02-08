using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class UpdateAsyncGameDataCommand(
    IReadOnlyCollection<GameData> gameData)
    : IRequest<bool>
{
    public IReadOnlyCollection<GameData> GameData { get; set; } = gameData;
}
