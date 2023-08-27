namespace TwilightImperiumUltimate.Web.Services.Language;

public interface ICultureProvider
{
    Task<string> GetUserStoredCultureStringAsync();

    Task SetCultureAsync(string culture);

    Task SetUserStoredCultureOrDefaultAsync();
}
