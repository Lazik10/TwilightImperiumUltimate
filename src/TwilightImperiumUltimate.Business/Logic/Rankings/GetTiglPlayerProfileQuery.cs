using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetTiglPlayerProfileQuery(int TiglUserId) : IRequest<TiglPlayerProfileDto>;
