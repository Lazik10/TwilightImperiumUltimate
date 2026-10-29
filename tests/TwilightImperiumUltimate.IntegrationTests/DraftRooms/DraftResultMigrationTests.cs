using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftResultMigrationTests
{
    [Fact]
    public async Task Migrate_WhenResultsRolledBackAndReapplied_ShouldKeepSourceRoomAndRecreateIndependentResultTables()
    {
        await using var database = await TestDatabase.CreateAsync(useMigrations: true);
        await using var context = await database.ContextFactory.CreateDbContextAsync();
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "RESULT01",
            Name = "Results migration",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
        var migrator = context.GetService<IMigrator>();

        await migrator.MigrateAsync("20261017074043_AddFactionPickConfiguredOrder");
        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();
        var result = new DraftResult
        {
            Id = Guid.NewGuid(),
            SourceRoomId = room.Id,
            ShareToken = "abcdefghijklmnopqrstuv",
            RoomName = room.Name,
            PayloadJson = "{}",
            CreatedAtUtc = DateTime.UtcNow,
            FinishedAtUtc = DateTime.UtcNow,
        };
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();
        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();

        (await context.DraftResults.AnyAsync(item => item.Id == result.Id)).Should().BeTrue();
        context.Model.FindEntityType(typeof(DraftResult))!.GetForeignKeys()
            .Should().NotContain(key => key.Properties.Any(property => property.Name == nameof(DraftResult.SourceRoomId)));
    }
}
