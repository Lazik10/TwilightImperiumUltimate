using TwilightImperiumUltimate.Core.Entities.Rules;
using TwilightImperiumUltimate.DataAccess.Tables.Rules;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Rules;

internal class RuleConfiguration : IEntityTypeConfiguration<Rule>
{
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Rules, Schema.Rule);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.RuleCategory)
            .IsRequired()
            .HasColumnName(nameof(Rule.RuleCategory))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Content)
            .IsRequired()
            .HasColumnName(nameof(Rule.Content))
            .HasColumnType("nvarchar(MAX)");
    }
}
