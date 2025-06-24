using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ComponentTypeConfiguration : BaseEntityConfiguration<ComponentType>
    {
        protected override void AddBuilder(EntityTypeBuilder<ComponentType> builder)
        {
            builder.HasMany(x => x.Models)
                .WithOne(x => x.ComponentType)
                .HasForeignKey(x => x.ComponentTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialItems)
                .WithOne(x => x.ComponentType)
                .HasForeignKey(x => x.ComponentTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "component_types";
    }
}

