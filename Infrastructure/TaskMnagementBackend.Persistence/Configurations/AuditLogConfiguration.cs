using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(a => a.EntityType)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(a => a.EntityId)
                .HasMaxLength(100);

            builder.Property(a => a.UserEmail)
                .HasMaxLength(256);

            builder.Property(a => a.UserName)
                .HasMaxLength(256);

            builder.Property(a => a.IpAddress)
                .HasMaxLength(50);

            builder.Property(a => a.Timestamp)
                .IsRequired(true);

            builder.HasIndex(a => a.Action)
                .HasDatabaseName("IX_AuditLogs_Action");

            builder.HasIndex(a => a.EntityType)
                .HasDatabaseName("IX_AuditLogs_EntityType");

            builder.HasIndex(a => a.UserId)
                .HasDatabaseName("IX_AuditLogs_UserId");

            builder.HasIndex(a => a.Timestamp)
                .HasDatabaseName("IX_AuditLogs_Timestamp");
        }
    }
}
