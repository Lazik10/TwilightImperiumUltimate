using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftContractSerializationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DraftSettings_WhenSerializedThroughBase_ShouldRoundTripWithStringEnums(bool apiOptions)
    {
        var options = CreateOptions(apiOptions);
        var settings = new FactionPickSettingsDto
        {
            BansMode = FactionPickBansMode.SimultaneousPlayerBans,
            BansPerPlayer = 3,
            HostBanCount = 2,
            PickOrderMode = FactionPickOrderMode.HostDefined,
            PicksPerPlayer = 5,
            Snake = false,
        };

        var json = JsonSerializer.Serialize<DraftSettingsDto>(settings, options);
        var restored = JsonSerializer.Deserialize<DraftSettingsDto>(json, options);

        restored.Should().BeOfType<FactionPickSettingsDto>().Which.Should().BeEquivalentTo(settings);
        using var document = JsonDocument.Parse(json);
        document.RootElement.GetProperty("type").GetString().Should().Be("faction-pick");
        document.RootElement.GetProperty("bansMode").GetString().Should().Be("SimultaneousPlayerBans");
        document.RootElement.GetProperty("pickOrderMode").GetString().Should().Be("HostDefined");
        document.RootElement.GetProperty("gameVersions").EnumerateArray()
            .Select(element => element.GetString())
            .Should().Equal("BaseGame", "ProphecyOfKings", "ThundersEdge");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DraftView_WhenSerializedThroughBase_ShouldRoundTrip(bool apiOptions)
    {
        var options = CreateOptions(apiOptions);
        DraftViewDto view = new FactionPickViewDto { Phase = FactionPickPhase.Picks };

        var json = JsonSerializer.Serialize(view, options);
        var restored = JsonSerializer.Deserialize<DraftViewDto>(json, options);

        restored.Should().BeOfType<FactionPickViewDto>().Which.Phase.Should().Be(FactionPickPhase.Picks);
        json.Should().Contain("\"phase\":\"Picks\"");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DraftActions_WhenSerializedThroughBase_ShouldRoundTripEveryRegisteredAction(bool apiOptions)
    {
        var options = CreateOptions(apiOptions);
        DraftActionDto[] actions =
        [
            new SelectBanAction { Faction = Enum.GetValues<FactionName>()[0] },
            new DeselectBanAction { Faction = Enum.GetValues<FactionName>()[1] },
            new LockInBansAction(),
            new ConfirmHostBansAction(),
            new DecideBansAction { Factions = Enum.GetValues<FactionName>().Take(2).ToArray() },
            new SelectPickAction { Faction = Enum.GetValues<FactionName>()[1] },
            new LockInPickAction { TurnIndex = 2 },
            new DecidePickAction { Faction = Enum.GetValues<FactionName>()[1], TurnIndex = 3 },
        ];

        foreach (var action in actions)
        {
            var json = JsonSerializer.Serialize(action, options);
            var restored = JsonSerializer.Deserialize<DraftActionDto>(json, options);

            restored.Should().BeOfType(action.GetType());
            JsonSerializer.Serialize(restored, options).Should().Be(json);
            if (action is SelectBanAction select)
            {
                restored.Should().BeOfType<SelectBanAction>().Which.Faction.Should().Be(select.Faction);
            }
            else if (action is DeselectBanAction deselect)
            {
                restored.Should().BeOfType<DeselectBanAction>().Which.Faction.Should().Be(deselect.Faction);
            }
            else if (action is DecideBansAction decide)
            {
                restored.Should().BeOfType<DecideBansAction>().Which.Factions.Should().Equal(decide.Factions);
            }

            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("faction", out var faction))
            {
                faction.ValueKind.Should().Be(JsonValueKind.String);
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DraftContracts_WhenDiscriminatorIsUnknown_ShouldRejectUnsupportedTypes(bool apiOptions)
    {
        var options = CreateOptions(apiOptions);
        const string json = """{"type":"unknown-draft"}""";

        var readSettings = () => JsonSerializer.Deserialize<DraftSettingsDto>(json, options);
        var readView = () => JsonSerializer.Deserialize<DraftViewDto>(json, options);
        var readAction = () => JsonSerializer.Deserialize<DraftActionDto>(json, options);

        readSettings.Should().Throw<JsonException>().WithMessage("*unrecognized type discriminator*");
        readView.Should().Throw<JsonException>().WithMessage("*unrecognized type discriminator*");
        readAction.Should().Throw<JsonException>().WithMessage("*unrecognized type discriminator*");
    }

    [Fact]
    public void FactionPickSettings_WhenCreated_ShouldUseSpecificationDefaults()
    {
        var settings = new FactionPickSettingsDto();

        settings.GameVersions.Should().Equal(
            GameVersion.BaseGame,
            GameVersion.ProphecyOfKings,
            GameVersion.ThundersEdge);
        settings.BansMode.Should().Be(FactionPickBansMode.None);
        settings.BansPerPlayer.Should().Be(1);
        settings.HostBanCount.Should().Be(0);
        settings.PickOrderMode.Should().Be(FactionPickOrderMode.Random);
        settings.PicksPerPlayer.Should().Be(1);
        settings.Snake.Should().BeTrue();
    }

    private static JsonSerializerOptions CreateOptions(bool apiOptions)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        if (apiOptions)
        {
            options.Converters.Add(new JsonStringEnumConverter());
        }

        return options;
    }
}
