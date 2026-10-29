using System.Text.Json;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.Serialization;

/// <summary>
/// Serializes draft settings' game versions as strings without changing legacy enum serialization.
/// </summary>
public sealed class GameVersionListJsonConverter : JsonConverter<IReadOnlyList<GameVersion>>
{
    /// <inheritdoc />
    public override IReadOnlyList<GameVersion> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Game versions must be an array of enum names.");
        }

        var versions = new List<GameVersion>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return versions;
            }

            if (reader.TokenType != JsonTokenType.String
                || !Enum.TryParse<GameVersion>(reader.GetString(), out var version)
                || !Enum.IsDefined(version))
            {
                throw new JsonException("A game version must be a recognized enum name.");
            }

            versions.Add(version);
        }

        throw new JsonException("The game versions array is incomplete.");
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        IReadOnlyList<GameVersion> value,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartArray();
        foreach (var version in value)
        {
            if (!Enum.IsDefined(version))
            {
                throw new JsonException("A game version must be a recognized enum name.");
            }

            writer.WriteStringValue(version.ToString());
        }

        writer.WriteEndArray();
    }
}
