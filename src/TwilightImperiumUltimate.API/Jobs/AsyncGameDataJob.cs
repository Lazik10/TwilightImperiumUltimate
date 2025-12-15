using Microsoft.Extensions.Options;
using Quartz;
using System.Net;
using System.Text.Json;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Business.Logic.Async;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

namespace TwilightImperiumUltimate.API.Jobs;

public class AsyncGameDataJob(
    ILogger<AsyncGameDataJob> logger,
    IOptions<AsyncStatsOptions> asyncOptions,
    IMediator mediator)
    : IJob
{
    private readonly ILogger<AsyncGameDataJob> _logger = logger;
    private readonly AsyncStatsOptions _asyncOptions = asyncOptions.Value;
    private readonly IMediator _mediator = mediator;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Data sync job started at: {Time}", DateTime.Now);

        try
        {
            using var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            using var client = new HttpClient(handler)
            {
                BaseAddress = _asyncOptions.Url,
            };

            client.DefaultRequestHeaders.UserAgent
                .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0 Safari/537.36");

            using var response = await client.GetAsync(
                new Uri(_asyncOptions.Statistics, UriKind.Relative),
                HttpCompletionOption.ResponseHeadersRead,
                context.CancellationToken);

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(context.CancellationToken);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                DefaultBufferSize = 32 * 1024, // smaller buffer to reduce transient memory
            };

            // Use small batches to cap peak memory.
            const int batchSize = 256;
            var batch = new List<GameData>(batchSize);

            await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<GameData>(stream, options, context.CancellationToken))
            {
                if (item is null)
                {
                    continue;
                }

                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    var ok = await _mediator.Send(new UpdateAsyncGameDataCommand(batch), context.CancellationToken);
                    batch.Clear();

                    if (!ok)
                    {
                        _logger.LogError("An error occurred while syncing JSON data batch.");
                    }
                }
            }

            if (batch.Count > 0)
            {
                var ok = await _mediator.Send(new UpdateAsyncGameDataCommand(batch), context.CancellationToken);
                if (!ok)
                {
                    _logger.LogError("An error occurred while syncing JSON data final batch.");
                }
            }

            _logger.LogInformation("JSON data synced successfully. {Time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while syncing JSON data.");
        }

        _logger.LogInformation("Data sync job ended at: {Time}", DateTime.Now);
    }
}
