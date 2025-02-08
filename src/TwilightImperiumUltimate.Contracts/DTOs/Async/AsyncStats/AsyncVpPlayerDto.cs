namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncVpPlayerDto(
    int Id,
    string UserName,
    int Vp,
    int VpMax)
{
    public float VpPercentage => VpMax == 0 ? 0 : (float)Vp / VpMax * 100;
}
