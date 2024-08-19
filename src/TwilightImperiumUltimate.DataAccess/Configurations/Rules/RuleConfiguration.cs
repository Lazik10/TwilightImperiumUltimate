namespace TwilightImperiumUltimate.DataAccess.Configurations.Rules;

internal sealed class RuleConfiguration : IEntityTypeConfiguration<Rule>
{
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Rules, Schema.Rule);

        builder.HasKey(e => e.RuleCategory);

        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnOrder(0);

        builder.Property(e => e.RuleCategory)
            .IsRequired()
            .HasColumnName(nameof(Rule.RuleCategory))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(1);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasColumnName(nameof(Rule.Content))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(2);
    }
}
