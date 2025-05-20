using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class MaterialSpecificationItemConfiguration : IEntityTypeConfiguration<MaterialSpecificationItem>
    {
        public void Configure(EntityTypeBuilder<MaterialSpecificationItem> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //builder.HasIndex(x => new { x.MaterialSelectionItem, x.MaterialSelectionItem.ComponentType }).IsUnique();

            builder.HasOne(x => x.ModuleType)
                .WithMany(x => x.MaterialSpecificationItems)
                .HasForeignKey(x => x.ModuleTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MaterialSelectionItem)
                .WithMany(x => x.MaterialSpecificationItems)
                .HasForeignKey(x => x.MaterialSelectionItemId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Kitchen)
                .WithMany(x => x.MaterialSpecificationItems)
                .HasForeignKey(x => x.MaterialSelectionItemId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
