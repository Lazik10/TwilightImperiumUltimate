using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

public class DraftRoom
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DraftRoomStatus Status { get; set; }

    public string DraftType { get; set; } = nameof(DraftTypeId.FactionPick);

    public bool IsLocked { get; set; }

    public int Version { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime LastActivityUtc { get; set; }

    public Guid? ResultId { get; set; }

    public ICollection<DraftRoomParticipant> Participants { get; set; } = new List<DraftRoomParticipant>();

    public ICollection<DraftRoomSeat> Seats { get; set; } = new List<DraftRoomSeat>();

    public ICollection<DraftRoomChatMessage> ChatMessages { get; set; } = new List<DraftRoomChatMessage>();

    public ICollection<DraftRoomLogEntry> LogEntries { get; set; } = new List<DraftRoomLogEntry>();

    public FactionPickSettings? FactionPickSettings { get; set; }

    public FactionPickState? FactionPickState { get; set; }

    public ICollection<FactionPickTurnEntry> FactionPickTurnOrder { get; set; } = new List<FactionPickTurnEntry>();

    public ICollection<FactionPickSelection> FactionPickSelections { get; set; } = new List<FactionPickSelection>();
}
