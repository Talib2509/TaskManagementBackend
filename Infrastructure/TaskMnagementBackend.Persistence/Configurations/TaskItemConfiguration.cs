using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("TaskItems");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired(true);

            builder.Property(t => t.IsPrivate)
                .IsRequired(true)
                .HasDefaultValue(false);

            builder.Property(t => t.CreatedAt)
                .IsRequired(true);

            builder.HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(t => t.TeamId)
                .HasDatabaseName("IX_TaskItems_TeamId");

            builder.HasIndex(t => t.AssignedUserId)
                .HasDatabaseName("IX_TaskItems_AssignedUserId");

            builder.HasIndex(t => t.Status)
                .HasDatabaseName("IX_TaskItems_Status");
        }
    }
}
