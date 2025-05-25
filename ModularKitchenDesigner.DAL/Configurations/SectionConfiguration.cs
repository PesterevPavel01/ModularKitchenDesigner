using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasIndex(x => new { x.KitchenId, x.ModuleId }).IsUnique();

            builder.HasOne(x => x.Kitchen)
                .WithMany(x => x.Sections)
                .HasForeignKey(x => x.KitchenId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Module)
                .WithMany(x => x.Sections)
                .HasForeignKey(x => x.ModuleId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}