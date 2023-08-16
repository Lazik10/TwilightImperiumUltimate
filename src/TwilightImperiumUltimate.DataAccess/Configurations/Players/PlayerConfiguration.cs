using TwilightImperiumUltimate.Core.Constraints;
using TwilightImperiumUltimate.DataAccess.Tables.Player;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Players;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Players, Schema.Player);

        builder.HasKey(x => x.Name);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(Player.Name))
            .HasMaxLength(Constraints.MaxPlayerNameLength)
            .HasColumnType("nvarchar");

        builder.Property(e => e.Color)
            .IsRequired()
            .HasColumnName(nameof(Player.Color))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
