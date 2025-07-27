using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglLeagueResolver
{
    TiglLeague Resolve(IGameReport gameReport);
}
