using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class PlayerSeasonResultConfiguration : IEntityTypeConfiguration<PlayerSeasonResult>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonResult> builder)
    {
        builder.ToTable(TableName.PlayerSeasonResults, Schema.Tigl);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Season).HasDatabaseName("IX_PlayerSeasonResults_Season");
        builder.HasIndex(x => x.League).HasDatabaseName("IX_PlayerSeasonResults_League");
        builder.HasIndex(x => x.TiglUserId).HasDatabaseName("IX_PlayerSeasonResults_TiglUserId");

        builder.Property(x => x.TiglUserName)
            .HasMaxLength(100);

        builder.Property(x => x.League)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .HasConversion<string>();
    }
}
