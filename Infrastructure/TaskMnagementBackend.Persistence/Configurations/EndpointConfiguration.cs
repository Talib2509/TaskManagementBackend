using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Persistence.Configurations
{
    public class EndpointConfiguration : IEntityTypeConfiguration<Endpoint>
    {
        public void Configure(EntityTypeBuilder<Endpoint> builder)
        {
            // Table name
            builder.ToTable("Endpoints");

            // Primary key
            builder.HasKey(e => e.Id);

            // Configure properties
            builder.Property(e => e.HttpMethod)
                .HasMaxLength(10)
                .IsRequired(true);

            builder.Property(e => e.RouteTemplate)
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(e => e.Code)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(e => e.Definition)
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(e => e.Menu)
                .HasMaxLength(100)
                .IsRequired(true);

         
            builder.HasIndex(e => e.Code)
                .HasDatabaseName("IX_Endpoints_Code")
                .IsUnique(true);

            builder.HasIndex(e => e.RouteTemplate)
                .HasDatabaseName("IX_Endpoints_RouteTemplate");

            builder.HasIndex(e => e.HttpMethod)
                .HasDatabaseName("IX_Endpoints_HttpMethod");

            builder.HasIndex(e => e.Menu)
                .HasDatabaseName("IX_Endpoints_Menu");

         
        }
    }
}
