using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Entityes.Authorization
{
    public class ApplicationUser : Identity
    {
        public ApplicationUser(Guid id) : base(id)
        {
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserToken UserToken { get; set; } = null!;
    }
}
