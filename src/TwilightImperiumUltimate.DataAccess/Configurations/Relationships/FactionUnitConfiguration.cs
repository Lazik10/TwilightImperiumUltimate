namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

internal sealed class FactionUnitConfiguration : IEntityTypeConfiguration<FactionUnit>
{
    public void Configure(EntityTypeBuilder<FactionUnit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(FactionUnit), Schema.Relationships);

        // Configure the composite key for the intermediary table
        builder.HasKey(fu => new { fu.FactionName, fu.UnitName });

        builder.Property(x => x.FactionName)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(0);

        builder.Property(x => x.UnitName)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(1);

        builder.Property(x => x.Count)
            .IsRequired()
            .HasColumnOrder(2);

        // Configure the many-to-many relationship for Faction
        builder.HasOne(fu => fu.Faction)
               .WithMany(f => f.FactionUnits)
               .HasForeignKey(fu => fu.FactionName)
               .HasPrincipalKey(f => f.FactionName);

        // Configure the many-to-many relationship for Unit
        builder.HasOne(fu => fu.Unit)
               .WithMany(u => u.FactionUnits)
               .HasForeignKey(fu => fu.UnitName)
               .HasPrincipalKey(u => u.UnitName);
    }
}
