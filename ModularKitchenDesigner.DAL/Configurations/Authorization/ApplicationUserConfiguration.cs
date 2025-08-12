using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes.Authorization;

namespace ModularKitchenDesigner.DAL.Configurations.Authorization
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.Email)
                .HasMaxLength(20);

            builder
                .HasOne(x => x.UserToken)
                .WithOne(x => x.User);
        }
    }
}
