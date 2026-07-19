using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            // Table name
            builder.ToTable("AppRoles");

            // Primary key is inherited from IdentityRole<Guid>
            builder.HasKey(r => r.Id);

            // Configure properties
            builder.Property(r => r.Name)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(r => r.NormalizedName)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(r => r.ConcurrencyStamp)
                .IsConcurrencyToken();

            // Configure relationships
            builder.HasMany(r => r.Endpoint)
                .WithMany(e => e.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "AppRoleEndpoint",
                    j => j
                        .HasOne<Endpoint>()
                        .WithMany()
                        .HasForeignKey("EndpointId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<AppRole>()
                        .WithMany()
                        .HasForeignKey("AppRoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("AppRoleId", "EndpointId");
                        j.ToTable("AppRoleEndpoints");
                        j.HasIndex("EndpointId");
                    });

            // Indexes
            builder.HasIndex(r => r.NormalizedName)
                .HasDatabaseName("RoleNameIndex")
                .IsUnique(true);
        }
    }
}
