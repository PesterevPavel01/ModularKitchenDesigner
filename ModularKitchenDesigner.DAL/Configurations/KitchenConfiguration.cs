using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class KitchenConfiguration : BaseEntityConfiguration<Kitchen>
    {
        protected override void AddBuilder(EntityTypeBuilder<Kitchen> builder)
        {
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(50);
            builder.Property(x => x.UserLogin).IsRequired().HasMaxLength(255);

            builder.HasOne(x => x.KitchenType)
                .WithMany(x => x.Kitchens)
                .HasForeignKey(x => x.KitchenTypeId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Sections)
                .WithOne(x => x.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MaterialSpecificationItems)
                .WithOne(x => x.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "kitchens";
    }
}
