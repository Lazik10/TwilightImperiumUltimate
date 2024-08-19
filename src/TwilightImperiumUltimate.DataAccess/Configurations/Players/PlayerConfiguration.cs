namespace TwilightImperiumUltimate.DataAccess.Configurations.Players;

internal sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Players, Schema.Player);

        builder.HasKey(x => x.Name);

        builder.Property(x => x.Id)
            .UseIdentityColumn()
            .HasColumnOrder(0);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(Player.Name))
            .HasMaxLength(Constraints.MaxPlayerNameLength)
            .HasColumnType("nvarchar")
            .HasColumnOrder(1);

        builder.Property(e => e.Color)
            .IsRequired()
            .HasColumnName(nameof(Player.Color))
            .HasConversion<int>()
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);
    }
}
