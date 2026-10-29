using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Contains only public settings and the engine's personalized draft projection.</summary>
public sealed record DraftRoomDraftView(
    DraftTypeId Type,
    DraftSettingsDto Settings,
    DraftViewDto? Draft,
    bool CanStart,
    IReadOnlyList<string> StartBlockers,
    bool CanReset);
