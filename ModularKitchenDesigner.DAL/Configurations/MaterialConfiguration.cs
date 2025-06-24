using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class MaterialConfiguration : BaseEntityConfiguration<Material>
    {
        protected override void AddBuilder(EntityTypeBuilder<Material> builder)
        {
            builder.HasMany(x => x.Components)
                .WithOne(x => x.Material)
                .HasForeignKey(x => x.MaterialId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialItems)
                .WithOne(x => x.Material)
                .HasForeignKey(x => x.MaterialId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "materials";
    }
}
