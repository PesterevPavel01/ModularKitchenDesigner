using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.DAL.Configurations.Base
{
    public abstract class BaseEntityConfiguration<Entity> : IEntityTypeConfiguration<Entity>
        where Entity : BaseEntity
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.ToTable(TableName());

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Code).IsUnique();

            AddBuilder(builder);
        }

        protected abstract void AddBuilder(EntityTypeBuilder<Entity> builder);

        protected abstract string TableName();
    }
}
