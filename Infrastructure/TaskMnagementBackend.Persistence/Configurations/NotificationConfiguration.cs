using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(n => n.Text)
                .HasMaxLength(2000)
                .IsRequired(true);

            builder.Property(n => n.Type)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired(true);

            builder.Property(n => n.IsRead)
                .IsRequired(true)
                .HasDefaultValue(false);

            builder.Property(n => n.CreatedAt)
                .IsRequired(true);

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(n => new { n.UserId, n.IsRead })
                .HasDatabaseName("IX_Notifications_UserId_IsRead");

            builder.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_Notifications_CreatedAt");
        }
    }
}
