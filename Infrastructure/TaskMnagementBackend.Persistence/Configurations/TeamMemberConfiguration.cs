using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired(true);

            builder.Property(m => m.JoinedAt)
                .IsRequired(true);

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

          
            builder.HasIndex(m => new { m.TeamId, m.UserId })
                .IsUnique()
                .HasDatabaseName("IX_TeamMembers_TeamId_UserId");

            builder.HasIndex(m => m.UserId)
                .HasDatabaseName("IX_TeamMembers_UserId");
        }
    }
}
