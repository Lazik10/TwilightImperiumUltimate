using System.Text.Json;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Describes a public engine event for the application to persist in the same transaction as state.
/// </summary>
/// <param name="Code">The stable event name.</param>
/// <param name="Data">Explicitly projected public data, never an entire state object.</param>
public sealed record DraftLogEvent(string Code, JsonElement Data);
