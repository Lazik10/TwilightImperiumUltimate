using System.Text;
using System.Text.Json;
using Quartz;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.API.Jobs;

[DisallowConcurrentExecution]
public sealed class AsyncStatisticsSnapshotJob(
    ILogger<AsyncStatisticsSnapshotJob> logger,
    IAsyncStatisticsSnapshotBuilder builder,
    IAsyncStatisticsSnapshotRepository repository,
    IAsyncStatisticsSnapshotReader reader)
    : IJob
{
    private readonly ILogger<AsyncStatisticsSnapshotJob> _logger = logger;
    private readonly IAsyncStatisticsSnapshotBuilder _builder = builder;
    private readonly IAsyncStatisticsSnapshotRepository _repository = repository;
    private readonly IAsyncStatisticsSnapshotReader _reader = reader;

    public async Task Execute(IJobExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Only the startup trigger sets this key; recurring/manual triggers omit it, so a plain
            // GetBoolean lookup throws KeyNotFoundException and silently aborts every non-startup refresh.
            var skipIfPublished = context.Trigger.JobDataMap.TryGetValue("SkipIfPublished", out var skipIfPublishedValue)
                && skipIfPublishedValue is true;

            var publishedSnapshot = await _repository.GetPublishedAsync(context.CancellationToken);
            if (skipIfPublished
                && publishedSnapshot is not null
                && publishedSnapshot.SourceDataVersion == AsyncStatisticsSnapshotSchema.Version)
            {
                _logger.LogInformation("Async statistics snapshot already exists; skipping startup refresh");
                return;
            }

            if (skipIfPublished && publishedSnapshot is not null)
            {
                _logger.LogInformation(
                    "Published async snapshot schema version is incompatible. Found={FoundSchemaVersion}, Expected={ExpectedSchemaVersion}. Rebuilding snapshot.",
                    publishedSnapshot.SourceDataVersion ?? "<null>",
                    AsyncStatisticsSnapshotSchema.Version);
            }

            var buildStopwatch = System.Diagnostics.Stopwatch.StartNew();
            var snapshot = await _builder.BuildAsync(context.CancellationToken);
            buildStopwatch.Stop();

            var serializationStopwatch = System.Diagnostics.Stopwatch.StartNew();
            var payload = JsonSerializer.Serialize(snapshot);
            serializationStopwatch.Stop();
            var payloadSizeBytes = Encoding.UTF8.GetByteCount(payload);
            var persistenceStopwatch = System.Diagnostics.Stopwatch.StartNew();
            publishedSnapshot = await _repository.PublishAsync(DateTime.UtcNow, AsyncStatisticsSnapshotSchema.Version, payload, context.CancellationToken);
            persistenceStopwatch.Stop();
            _reader.Invalidate();

            stopwatch.Stop();
            _logger.LogInformation(
                "Async statistics snapshot published successfully. SnapshotVersion={SnapshotVersion}, CategoryCount={CategoryCount}, BuildDurationMilliseconds={BuildDurationMilliseconds}, SerializationDurationMilliseconds={SerializationDurationMilliseconds}, PersistenceDurationMilliseconds={PersistenceDurationMilliseconds}, TotalDurationMilliseconds={TotalDurationMilliseconds}, PayloadSizeBytes={PayloadSizeBytes}, GeneratedAtUtc={GeneratedAtUtc}",
                publishedSnapshot.SnapshotVersion,
                11,
                buildStopwatch.ElapsedMilliseconds,
                serializationStopwatch.ElapsedMilliseconds,
                persistenceStopwatch.ElapsedMilliseconds,
                stopwatch.ElapsedMilliseconds,
                payloadSizeBytes,
                publishedSnapshot.GeneratedAtUtc);
        }
        catch (OperationCanceledException ex) when (context.CancellationToken.IsCancellationRequested)
        {
            stopwatch.Stop();
            _logger.LogWarning(ex, "Async statistics snapshot refresh was cancelled after {ElapsedMilliseconds} ms; the last successful snapshot remains published", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Async statistics snapshot refresh failed after {ElapsedMilliseconds} ms; the last successful snapshot remains published", stopwatch.ElapsedMilliseconds);
        }
    }
}
