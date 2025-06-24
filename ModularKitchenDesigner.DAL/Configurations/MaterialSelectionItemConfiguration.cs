using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class MaterialSelectionItemConfiguration : BaseEntityConfiguration<MaterialSelectionItem>
    {
        protected override void AddBuilder(EntityTypeBuilder<MaterialSelectionItem> builder)
        {
            builder.HasIndex(x => new { x.MaterialId, x.ComponentTypeId, x.KitchenTypeId }).IsUnique();
            builder.Property(x => x.Title).HasDefaultValue("N/A");

            builder.HasOne(x => x.ComponentType)
                .WithMany(x => x.MaterialItems)
                .HasForeignKey(x => x.ComponentTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Material)
                .WithMany(x => x.MaterialItems)
                .HasForeignKey(x => x.MaterialId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.KitchenType)
                .WithMany(x => x.MaterialSelectionItems)
                .HasForeignKey(x => x.KitchenTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialSpecificationItems)
                .WithOne(x => x.MaterialSelectionItem)
                .HasForeignKey(x => x.MaterialSelectionItemId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "material_selection_items";
    }
}
