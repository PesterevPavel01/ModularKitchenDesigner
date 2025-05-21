using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class KitchenDto : PrivateIdentity
    {
        public KitchenDto(){}

        public KitchenDto(Kitchen kitchen)
        {
            UserLogin = kitchen.UserLogin;
            UserId = kitchen.UserId;
            KitchenType = kitchen.KitchenType.Title;
            Guid = kitchen.Id;
            base.SetId(kitchen.Id);
        }

        [Required(ErrorMessage = "UserLogin cannot be null or empty.")]
        public string UserLogin { get; set; }

        [Required(ErrorMessage = "UserId cannot be null or empty.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "KitchenType cannot be null or empty.")]
        public string KitchenType { get; set; }

        public Guid Guid { get; set; }
    }
}
