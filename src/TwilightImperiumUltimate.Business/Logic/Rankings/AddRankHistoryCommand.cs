using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record AddRankHistoryCommand(int TiglUserId, TiglLeague League, TiglRankName Rank, long AchievedAt) : IRequest<AddRankHistoryResponse>;
