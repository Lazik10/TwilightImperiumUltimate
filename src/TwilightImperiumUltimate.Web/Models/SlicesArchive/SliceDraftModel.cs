namespace TwilightImperiumUltimate.Web.Models.SlicesArchive;

public class SliceDraftModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string EventName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string SliceDraftString { get; set; } = string.Empty;

    public string SliceNames { get; set; } = string.Empty;

    public int SliceCount { get; set; }

    public string SliceDraftGeneratorLink { get; set; } = string.Empty;

    public string SliceDraftArchiveLink { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public float Rating { get; set; }

    public int NumberOfVotes { get; set; }
}
