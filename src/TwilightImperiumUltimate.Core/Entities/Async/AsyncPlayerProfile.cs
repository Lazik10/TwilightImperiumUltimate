using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Async;

public class AsyncPlayerProfile : IEntity
{
    public int Id { get; set; }

    public long DiscordUserId { get; set; }

    public string DiscordUserName { get; set; } = string.Empty;

    public ICollection<AsyncPlayerProfileGameStats> GameStatistics { get; set; } = new List<AsyncPlayerProfileGameStats>();

    public AsyncPlayerProfileSettings? ProfileSettings { get; set; }
}
