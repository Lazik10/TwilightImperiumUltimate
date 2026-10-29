using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

internal static class DraftRoomLogAssertions
{
    public static async Task AssertEntriesAsync(
        IDbContextFactory<TwilightImperiumDbContext> contextFactory,
        Guid roomId,
        params DraftLogEntryType[] expectedTypes)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entries = await context.DraftRoomLogEntries
            .AsNoTracking()
            .Where(entry => entry.RoomId == roomId)
            .OrderBy(entry => entry.Id)
            .ToArrayAsync();

        entries.Select(entry => entry.Type).Should().Equal(expectedTypes);
        foreach (var entry in entries)
        {
            using var json = JsonDocument.Parse(entry.DataJson);
            json.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
            entry.DataJson.Contains("secret", StringComparison.OrdinalIgnoreCase).Should().BeFalse();
            entry.DataJson.Contains("password", StringComparison.OrdinalIgnoreCase).Should().BeFalse();
        }
    }

    public static async Task AssertDoesNotContainAsync(
        IDbContextFactory<TwilightImperiumDbContext> contextFactory,
        Guid roomId,
        params string[] sensitiveValues)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var payloads = await context.DraftRoomLogEntries
            .AsNoTracking()
            .Where(entry => entry.RoomId == roomId)
            .Select(entry => entry.DataJson)
            .ToArrayAsync();

        foreach (var payload in payloads)
        {
            foreach (var sensitiveValue in sensitiveValues.Where(value => !string.IsNullOrEmpty(value)))
            {
                payload.Contains(sensitiveValue, StringComparison.Ordinal).Should().BeFalse();
            }
        }
    }
}
