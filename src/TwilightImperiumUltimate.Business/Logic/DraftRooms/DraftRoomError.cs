using FluentResults;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// A typed draft-room operation failure.
/// </summary>
public sealed class DraftRoomError(DraftRoomErrorCode code, IReadOnlyList<string>? validationCodes = null)
    : Error(code.ToString())
{
    /// <summary>
    /// Gets the machine-readable error code.
    /// </summary>
    public DraftRoomErrorCode Code { get; } = code;

    /// <summary>Gets specific settings or engine refusal codes suitable for localized UI messages.</summary>
    public IReadOnlyList<string> ValidationCodes { get; } = Array.AsReadOnly((validationCodes ?? []).ToArray());
}
