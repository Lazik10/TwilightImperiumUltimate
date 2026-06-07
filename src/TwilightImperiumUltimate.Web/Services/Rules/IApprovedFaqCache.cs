namespace TwilightImperiumUltimate.Web.Services.Rules;

public interface IApprovedFaqCache
{
    long Version { get; }

    Task<ApprovedFaqSnapshot> GetAsync();

    Task<ApprovedFaqSnapshot> RetryAsync();

    void Invalidate();
}
