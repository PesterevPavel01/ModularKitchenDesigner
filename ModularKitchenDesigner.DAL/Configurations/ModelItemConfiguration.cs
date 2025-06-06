﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.DAL.Configurations
{
    public class ModelItemConfiguration : IEntityTypeConfiguration<ModelItem>
    {
        public void Configure(EntityTypeBuilder<ModelItem> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();
            builder.Property(x => x.Title).HasMaxLength(255);
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
    }
}