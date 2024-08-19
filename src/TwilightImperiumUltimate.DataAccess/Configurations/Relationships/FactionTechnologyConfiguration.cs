using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

internal sealed class FactionTechnologyConfiguration : IEntityTypeConfiguration<FactionTechnology>
{
    public void Configure(EntityTypeBuilder<FactionTechnology> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(FactionTechnology), Schema.Relationships);

        // Configure the composite key for the intermediary table
        builder.HasKey(ft => new { ft.FactionName, ft.TechnologyName });

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.TechnologyName)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.TechnologyName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(e => e.StartingTechnology)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.StartingTechnology))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(3);

        builder.HasOne(ft => ft.Faction)
            .WithMany(f => f.FactionTechnologies)
            .HasForeignKey(ft => ft.FactionName)
            .HasPrincipalKey(f => f.FactionName);

        builder.HasOne(ft => ft.Technology)
            .WithMany(t => t.FactionTechnologies)
            .HasForeignKey(ft => ft.TechnologyName)
            .HasPrincipalKey(t => t.TechnologyName);
    }
}
