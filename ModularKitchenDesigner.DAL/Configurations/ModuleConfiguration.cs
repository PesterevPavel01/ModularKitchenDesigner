using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ModuleConfiguration : BaseEntityConfiguration<Module>
    {
        protected override void AddBuilder(EntityTypeBuilder<Module> builder)
        {
            builder.Property(x => x.PreviewImageSrc).HasDefaultValue("N/A");
            builder.HasIndex(x => x.PreviewImageSrc);

            builder.HasOne(x => x.ModuleType)
                .WithMany(x => x.Modules)
                .HasForeignKey(x => x.ModuleTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Sections)
                .WithOne(x => x.Module)
                .HasForeignKey(x => x.ModuleId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ModelItems)
                .WithOne(x => x.Module)
                .HasForeignKey(x => x.ModuleId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "modules";
    }
}