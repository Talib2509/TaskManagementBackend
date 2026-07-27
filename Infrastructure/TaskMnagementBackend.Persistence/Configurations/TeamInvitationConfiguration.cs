using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class TeamInvitationConfiguration : IEntityTypeConfiguration<TeamInvitation>
    {
        public void Configure(EntityTypeBuilder<TeamInvitation> builder)
        {
            builder.ToTable("TeamInvitations");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Email)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(i => i.Token)
                .HasMaxLength(64)
                .IsRequired(true);

            builder.Property(i => i.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired(true);

            builder.Property(i => i.CreatedAt)
                .IsRequired(true);

            builder.Property(i => i.ExpiresAt)
                .IsRequired(true);

            builder.HasOne(i => i.Team)
                .WithMany()
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.InvitedByUser)
                .WithMany()
                .HasForeignKey(i => i.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.InvitedUser)
                .WithMany()
                .HasForeignKey(i => i.InvitedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(i => i.Token)
                .IsUnique()
                .HasDatabaseName("IX_TeamInvitations_Token");

            builder.HasIndex(i => new { i.TeamId, i.Email })
                .HasDatabaseName("IX_TeamInvitations_TeamId_Email");

            // Eyni email-ə eyni komandaya təkrar Pending dəvət göndərilməsinin qarşısını
            // DB səviyyəsində də alan filtered unique index (SQL Server).
            builder.HasIndex(i => new { i.TeamId, i.Email })
                .IsUnique()
                .HasDatabaseName("IX_TeamInvitations_TeamId_Email_Pending")
                .HasFilter("[Status] = 'Pending'");

            builder.HasIndex(i => i.Status)
                .HasDatabaseName("IX_TeamInvitations_Status");

            builder.HasIndex(i => i.InvitedUserId)
                .HasDatabaseName("IX_TeamInvitations_InvitedUserId");
        }
    }
}
