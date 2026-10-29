using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a typed draft action. The caller's identity is resolved server-side.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(SelectBanAction), "faction-pick.select-ban")]
[JsonDerivedType(typeof(DeselectBanAction), "faction-pick.deselect-ban")]
[JsonDerivedType(typeof(LockInBansAction), "faction-pick.lock-in-bans")]
[JsonDerivedType(typeof(ConfirmHostBansAction), "faction-pick.confirm-host-bans")]
[JsonDerivedType(typeof(DecideBansAction), "faction-pick.decide-bans")]
[JsonDerivedType(typeof(SelectPickAction), "faction-pick.select-pick")]
[JsonDerivedType(typeof(LockInPickAction), "faction-pick.lock-in-pick")]
[JsonDerivedType(typeof(DecidePickAction), "faction-pick.decide-pick")]
public abstract class DraftActionDto
{
}
