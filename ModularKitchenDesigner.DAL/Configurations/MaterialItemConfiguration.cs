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

            builder.HasOne(x => x.KitchenType)
                .WithMany(x => x.MaterialItems)
                .HasForeignKey(x => x.KitchenTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
