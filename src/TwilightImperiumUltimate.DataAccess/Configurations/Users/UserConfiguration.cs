using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Tables.User;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Users;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Users, Schema.User);

        builder.HasKey(e => e.Id)
            .IsClustered();

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer");

        builder.Property(e => e.Nickname)
            .IsRequired()
            .HasColumnName(nameof(User.Nickname))
            .HasColumnType("nvarchar(30)");

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasColumnName(nameof(User.FirstName))
            .HasColumnType("nvarchar(50)");

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasColumnName(nameof(User.LastName))
            .HasColumnType("nvarchar(50)");

        builder.Property(e => e.Age)
            .IsRequired()
            .HasColumnName(nameof(User.Age))
            .HasColumnType("integer");

        builder.Property(e => e.Email)
            .IsRequired()
            .HasColumnName(nameof(User.Email))
            .HasColumnType("nvarchar(30)");
    }
}
