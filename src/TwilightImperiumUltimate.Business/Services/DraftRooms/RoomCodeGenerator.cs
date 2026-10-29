using System.Security.Cryptography;
using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Generates cryptographically random room codes.
/// </summary>
public sealed class RoomCodeGenerator : IRoomCodeGenerator
{
    /// <inheritdoc />
    public string Generate()
    {
        var characters = RandomNumberGenerator.GetItems<char>(
            DraftRoomLimits.CodeAlphabet.AsSpan(),
            DraftRoomLimits.CodeLength);

        return new string(characters);
    }
}
