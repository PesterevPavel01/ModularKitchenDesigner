using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ModuleTypeConfiguration : BaseEntityConfiguration<ModuleType>
    {
        protected override void AddBuilder(EntityTypeBuilder<ModuleType> builder)
        {
            builder.HasMany(x => x.Modules)
                .WithOne(x => x.ModuleType)
                .HasForeignKey(x => x.ModuleTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialSpecificationItems)
                .WithOne(x => x.ModuleType)
                .HasForeignKey(x => x.ModuleTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "module_types";
    }
}