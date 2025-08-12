using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Entityes.Authorization
{
    public class UserToken : Identity
    {
        public UserToken(Guid id) : base(id)
        {
        }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
