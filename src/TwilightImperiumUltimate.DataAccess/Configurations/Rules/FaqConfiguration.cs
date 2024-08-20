namespace TwilightImperiumUltimate.DataAccess.Configurations.Rules;

internal class FaqConfiguration : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Faq, Schema.Rule);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnOrder(0);

        builder.Property(e => e.ComponentName)
            .IsRequired()
            .HasColumnName(nameof(Faq.ComponentName))
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(x => x.QuestionEnglish)
            .IsRequired()
            .HasColumnName(nameof(Faq.QuestionEnglish))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(2);

        builder.Property(x => x.AnswerEnglish)
            .IsRequired()
            .HasColumnName(nameof(Faq.AnswerEnglish))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(3);

        builder.Property(x => x.QuestionCzech)
            .IsRequired()
            .HasColumnName(nameof(Faq.QuestionCzech))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(4);

        builder.Property(x => x.AnswerCzech)
            .IsRequired()
            .HasColumnName(nameof(Faq.AnswerCzech))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(5);
    }
}
