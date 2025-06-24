using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularKitchenDesigner.DAL.Configurations.Base;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class KitchenTypeConfiguration : BaseEntityConfiguration<KitchenType>
    {
        protected override void AddBuilder(EntityTypeBuilder<KitchenType> builder)
        {

            builder.HasOne(x => x.PriceSegment)
                .WithMany(x => x.Types)
                .HasForeignKey(x => x.PriceSegmentId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialSelectionItems)
                .WithOne(x => x.KitchenType)
                .HasForeignKey(x => x.KitchenTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Kitchens)
                .WithOne(x => x.KitchenType)
                .HasForeignKey(x => x.KitchenTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "kitchen_types";
    }
}

