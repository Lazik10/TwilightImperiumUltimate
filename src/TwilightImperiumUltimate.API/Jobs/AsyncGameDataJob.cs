using Microsoft.Extensions.Options;
using Quartz;
using System.Text.Json;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Business.Logic.Async;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

namespace TwilightImperiumUltimate.API.Jobs;

public class AsyncGameDataJob(
    ILogger<AsyncGameDataJob> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<AsyncStatsOptions> asyncOptions,
    IMediator mediator)
    : IJob
{
    private readonly ILogger<AsyncGameDataJob> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly AsyncStatsOptions _asyncOptions = asyncOptions.Value;
    private readonly IMediator _mediator = mediator;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Data sync job started at: {Time}", DateTime.Now);

        try
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = _asyncOptions.Url;
            client.DefaultRequestHeaders.UserAgent
                .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                                "(KHTML, like Gecko) Chrome/124.0 Safari/537.36");

            var response = await client.GetStringAsync(new Uri(_asyncOptions.Statistics, UriKind.Relative));
            var gameData = JsonSerializer.Deserialize<List<GameData>>(response);

            if (gameData is not null)
            {
                var success = await _mediator.Send(new UpdateAsyncGameDataCommand(gameData));
                if (success)
                {
                    _logger.LogInformation("JSON data synced successfully. {Time}", DateTime.Now);
                }
                else
                {
                    _logger.LogError("An error occurred while syncing JSON data.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while syncing JSON data.");
        }

        _logger.LogInformation("Data sync job ended at: {Time}", DateTime.Now);
    }
}
