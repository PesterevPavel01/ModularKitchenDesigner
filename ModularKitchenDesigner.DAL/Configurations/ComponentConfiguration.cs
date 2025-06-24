using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularKitchenDesigner.DAL.Configurations.Base;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ComponentConfiguration : BaseEntityConfiguration<Component>
    {
        protected override void AddBuilder(EntityTypeBuilder<Component> builder)
        {
            builder.Property(x => x.Price).IsRequired();

            builder.HasOne(x => x.PriceSegment)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.PriceSegmentId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Material)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.MaterialId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "components";
    }
}
