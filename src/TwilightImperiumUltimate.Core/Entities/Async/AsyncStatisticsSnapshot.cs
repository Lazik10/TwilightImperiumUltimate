namespace TwilightImperiumUltimate.Core.Entities.Async;

public class AsyncStatisticsSnapshot
{
    public int Id { get; set; }

    public DateTime GeneratedAtUtc { get; set; }

    public long SnapshotVersion { get; set; }

    public string? SourceDataVersion { get; set; }

    public bool IsPublished { get; set; }

    public string Payload { get; set; } = string.Empty;
}
