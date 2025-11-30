using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetAllTiglUsersQuery() : IRequest<ItemListDto<TiglUserLiteDto>>;
