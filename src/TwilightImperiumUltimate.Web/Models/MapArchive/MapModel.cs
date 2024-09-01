namespace TwilightImperiumUltimate.Web.Models.MapArchive;

public class MapModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string EventName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public MapTemplate MapTemplate { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string TtsString { get; set; } = string.Empty;

    public string MapGeneratorLink { get; set; } = string.Empty;

    public string MapArchiveLink { get; set; } = string.Empty;

    public float Rating { get; set; }

    public int NumberOfVotes { get; set; }
}
