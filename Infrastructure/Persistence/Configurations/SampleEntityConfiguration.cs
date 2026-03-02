using Kakeibo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kakeibo.Infrastructure.Persistence.Configurations;

/// <summary>
/// SampleEntity の EF Core マッピング
/// </summary>
public class SampleEntityConfiguration : IEntityTypeConfiguration<SampleEntity>
{
    public void Configure(EntityTypeBuilder<SampleEntity> builder)
    {
        builder.ToTable("SampleEntities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Amount)
            .HasPrecision(19, 4);

        builder.Property(e => e.CreatedAt);
        builder.Property(e => e.UpdatedAt);
    }
}
