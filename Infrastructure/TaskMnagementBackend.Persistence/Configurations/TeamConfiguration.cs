using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(t => t.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                .IsRequired(true);

            builder.Property(t => t.IsDeleted)
                .IsRequired(true)
                .HasDefaultValue(false);

            builder.HasOne(t => t.TeamLead)
                .WithMany()
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(t => t.TeamMembers)
                .WithOne(m => m.Team)
                .HasForeignKey(m => m.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TaskItems)
                .WithOne(ti => ti.Team)
                .HasForeignKey(ti => ti.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.CompanyId)
                .HasDatabaseName("IX_Teams_CompanyId");

            builder.HasIndex(t => t.IsDeleted)
                .HasDatabaseName("IX_Teams_IsDeleted");

            builder.HasIndex(t => t.TeamLeadId)
                .HasDatabaseName("IX_Teams_TeamLeadId");
        }
    }
}
