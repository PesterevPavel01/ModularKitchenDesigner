using ModularKitchenDesigner.Domain.Entityes;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class KitchenDto 
    {
        public KitchenDto(){}

        public KitchenDto(Kitchen kitchen)
        {
            Title = kitchen.Title;
            Code = kitchen.Code;
            KitchenType = kitchen.KitchenType.Title;
        }

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "KitchenType cannot be null or empty.")]
        public string KitchenType { get; set; }
    }
}
