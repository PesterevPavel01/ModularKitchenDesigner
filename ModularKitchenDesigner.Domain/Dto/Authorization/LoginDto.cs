using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto.Authorization
{
    public sealed class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
