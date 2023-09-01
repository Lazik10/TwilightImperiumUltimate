namespace TwilightImperiumUltimate.Web.Services.Language;

public interface ICultureProvider
{
    Task SetCultureAsync(string culture);
}
