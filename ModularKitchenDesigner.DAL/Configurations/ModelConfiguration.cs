using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ModelConfiguration : BaseEntityConfiguration<Model>
    {
        protected override void AddBuilder(EntityTypeBuilder<Model> builder)
        {
            builder.HasOne(x => x.ComponentType)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.ComponentTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ModelItems)
                .WithOne(x => x.Model)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Components)
                .WithOne(x => x.Model)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "models";
    }
}
