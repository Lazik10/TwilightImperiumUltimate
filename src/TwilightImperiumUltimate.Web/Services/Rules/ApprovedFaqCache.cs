namespace TwilightImperiumUltimate.Web.Services.Rules;

public sealed class ApprovedFaqCache(
    ITwilightImperiumApiHttpClient httpClient,
    IMapper mapper) : IApprovedFaqCache
{
    private readonly object _sync = new();
    private Lazy<Task<ApprovedFaqSnapshot>>? _snapshot;
    private long _version;

    public long Version
    {
        get
        {
            lock (_sync)
                return _version;
        }
    }

    public Task<ApprovedFaqSnapshot> GetAsync()
    {
        lock (_sync)
        {
            _snapshot ??= CreateSnapshot();
            return AwaitAndResetOnFailure(_snapshot);
        }
    }

    public Task<ApprovedFaqSnapshot> RetryAsync()
    {
        Invalidate();
        return GetAsync();
    }

    public void Invalidate()
    {
        lock (_sync)
        {
            _version++;
            _snapshot = null;
        }
    }

    private Lazy<Task<ApprovedFaqSnapshot>> CreateSnapshot() =>
        new(LoadAsync, LazyThreadSafetyMode.ExecutionAndPublication);

    private async Task<ApprovedFaqSnapshot> AwaitAndResetOnFailure(
        Lazy<Task<ApprovedFaqSnapshot>> snapshot)
    {
        try
        {
            return await snapshot.Value;
        }
        catch
        {
            lock (_sync)
            {
                if (ReferenceEquals(_snapshot, snapshot))
                    _snapshot = null;
            }

            throw;
        }
    }

    private async Task<ApprovedFaqSnapshot> LoadAsync()
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq);
        if (result.StatusCode != HttpStatusCode.OK || result.Response?.Data?.Items is null)
            throw new InvalidOperationException("Approved FAQ data could not be loaded.");

        var approvedFaqs = mapper.Map<List<FaqModel>>(result.Response.Data.Items)
            .Where(faq => faq.FaqStatus == FaqStatus.Approved)
            .ToList();

        var faqByKey = approvedFaqs
            .GroupBy(faq => faq.ComponentName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<FaqModel>)group.ToList(),
                StringComparer.OrdinalIgnoreCase);

        var searchTextByKey = faqByKey.ToDictionary(
            pair => pair.Key,
            pair => string.Join(' ', pair.Value.Select(faq =>
                $"{faq.QuestionEnglish} {faq.AnswerEnglish} {faq.QuestionCzech} {faq.AnswerCzech}")),
            StringComparer.OrdinalIgnoreCase);

        return new ApprovedFaqSnapshot(faqByKey, searchTextByKey);
    }
}
