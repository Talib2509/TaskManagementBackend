using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.ToTable("UserSettings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Language)
                .HasMaxLength(10)
                .IsRequired(true)
                .HasDefaultValue("az");

            builder.Property(s => s.Theme)
                .HasMaxLength(20)
                .IsRequired(true)
                .HasDefaultValue("light");

            builder.HasOne(s => s.User)
                .WithOne(u => u.Settings)
                .HasForeignKey<UserSettings>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.UserId)
                .IsUnique();
        }
    }
}
