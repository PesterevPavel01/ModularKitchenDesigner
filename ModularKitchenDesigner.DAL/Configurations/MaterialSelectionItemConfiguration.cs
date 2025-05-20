using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class MaterialSelectionItemConfiguration : IEntityTypeConfiguration<MaterialSelectionItem>
    {
        public void Configure(EntityTypeBuilder<MaterialSelectionItem> builder)
        {

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

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
    }
}
