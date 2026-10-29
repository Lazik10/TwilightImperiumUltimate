using Microsoft.AspNetCore.Identity;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Hashes draft room passwords with ASP.NET Core Identity's password hasher.
/// </summary>
public sealed class RoomPasswordHasher : IRoomPasswordHasher
{
    private static readonly DraftRoom DummyRoom = new();
    private static readonly string DummyPasswordHash =
        new PasswordHasher<DraftRoom>().HashPassword(DummyRoom, "draft-room-dummy-password");

    private readonly PasswordHasher<DraftRoom> _passwordHasher = new();

    /// <inheritdoc />
    public string Hash(DraftRoom room, string password)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(password);

        return _passwordHasher.HashPassword(room, password);
    }

    /// <inheritdoc />
    public bool Verify(DraftRoom room, string passwordHash, string password)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(passwordHash);
        ArgumentNullException.ThrowIfNull(password);

        var result = _passwordHasher.VerifyHashedPassword(room, passwordHash, password);

        return result != PasswordVerificationResult.Failed;
    }

    /// <inheritdoc />
    public bool VerifyDummyPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        return this.Verify(DummyRoom, DummyPasswordHash, password);
    }
}
