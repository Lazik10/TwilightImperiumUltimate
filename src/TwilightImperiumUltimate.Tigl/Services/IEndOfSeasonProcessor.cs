namespace TwilightImperiumUltimate.Tigl.Services;
public interface IEndOfSeasonProcessor
{
    Task<bool> ProcessEndOfSeason();
}
