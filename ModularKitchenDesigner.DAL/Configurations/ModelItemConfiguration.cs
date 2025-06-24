using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.DAL.Configurations.Base;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ModelItemConfiguration : BaseEntityConfiguration<ModelItem>
    {
        protected override void AddBuilder(EntityTypeBuilder<ModelItem> builder)
        {
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Title).HasDefaultValue("N/A");
            builder.HasIndex(x => new { x.ModelId, x.ModuleId }).IsUnique();

            builder.HasOne(x => x.Module)
                .WithMany(x => x.ModelItems)
                .HasForeignKey(x => x.ModuleId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.ModelItems)
                .HasForeignKey(x => x.ModelId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override string TableName()
            => "model_items";
    }
}