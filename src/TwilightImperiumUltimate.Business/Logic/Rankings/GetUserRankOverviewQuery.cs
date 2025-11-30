using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetUserRankOverviewQuery(int TiglUserId) : IRequest<RankingsUserDto>;
