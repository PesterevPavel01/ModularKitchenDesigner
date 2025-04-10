using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class MaterialItemConfiguration : IEntityTypeConfiguration<MaterialItem>
    {
        public void Configure(EntityTypeBuilder<MaterialItem> builder)
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

            builder.HasMany(x => x.KitchenTypes)
                .WithOne(x => x.MaterialItem)
                .HasForeignKey(x => x.MaterialItemId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
