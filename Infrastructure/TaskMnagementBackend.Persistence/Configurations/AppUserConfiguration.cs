using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Table name
            builder.ToTable("AppUsers");

            // Primary key is inherited from IdentityUser<Guid>
            builder.HasKey(u => u.Id);

            // Configure properties
            builder.Property(u => u.FullName)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.CompanyName)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(u => u.UserName)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.PasswordHash)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(u => u.NormalizedEmail)
                .HasDatabaseName("EmailIndex")
                .IsUnique(false);

            builder.HasIndex(u => u.NormalizedUserName)
                .HasDatabaseName("UserNameIndex")
                .IsUnique(true);

            builder.HasIndex(u => u.Email)
                .HasDatabaseName("IX_AppUsers_Email");

            // Audit fields (if added to IdentityUser in future)
            // builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            // builder.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Aktiv komanda konteksti (soft reference — komanda silinərsə null-a çəkilir)
            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(u => u.ActiveTeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Concurrency token
            builder.Property(u => u.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
