using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ComponentTypeConfiguration : IEntityTypeConfiguration<ComponentType>
    {
        public void Configure(EntityTypeBuilder<ComponentType> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.Title).IsUnique();

            builder.HasMany(x => x.Components)
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
    }
}

