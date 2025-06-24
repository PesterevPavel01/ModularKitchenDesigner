using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class PriceSegmentConfiguration : BaseEntityConfiguration<PriceSegment>
    {
        protected override void AddBuilder(EntityTypeBuilder<PriceSegment> builder)
        {
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

        protected override string TableName()
            => "price_sigments";
    }
}