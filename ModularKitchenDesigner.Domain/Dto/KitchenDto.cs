using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class KitchenDto
    {
        public KitchenDto(){}

        public KitchenDto(Kitchen kitchen)
        {
            UserLogin = kitchen.UserLogin;
            UserId = kitchen.UserId;
            KitchenType = kitchen.KitchenType.Title;
            Code = kitchen.Code;
            Title = kitchen.Title;
        }

        [Required(ErrorMessage = "UserLogin cannot be null or empty.")]
        public string UserLogin { get; set; }

        [Required(ErrorMessage = "UserId cannot be null or empty.")]
        public string UserId { get; set; }

        public string Title { get; set; }

        [Required(ErrorMessage = "KitchenType cannot be null or empty.")]
        public string KitchenType { get; set; }

        public String Code { get; set; } = Guid.NewGuid().ToString();
    }
}
