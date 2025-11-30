using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record RemoveRankHistoryCommand(int RankHistoryId) : IRequest<RemoveRankHistoryResponse>;
