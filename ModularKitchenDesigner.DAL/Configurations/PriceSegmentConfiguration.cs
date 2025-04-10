using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class PriceSegmentConfiguration : IEntityTypeConfiguration<PriceSegment>
    {
        public void Configure(EntityTypeBuilder<PriceSegment> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.Title).IsUnique();
            builder.HasIndex(x => x.Title);

            builder.HasMany(x => x.Components)
                .WithOne(x => x.PriceSegment)
                .HasForeignKey(x => x.PriceSegmentId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Types)
                .WithOne(x => x.PriceSegment)
                .HasForeignKey(x => x.PriceSegmentId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}