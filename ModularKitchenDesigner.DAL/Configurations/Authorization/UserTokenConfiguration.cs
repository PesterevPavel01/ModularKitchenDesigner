using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularKitchenDesigner.Domain.Entityes.Authorization;

namespace ModularKitchenDesigner.DAL.Configurations.Authorization
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("user_token");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.RefreshToken)
                .IsRequired()
                .HasMaxLength(255);

            builder
                .Property(x => x.RefreshTokenExpiryTime)
                .IsRequired();

            builder
                .HasOne(x => x.User)
                .WithOne(x => x.UserToken)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
