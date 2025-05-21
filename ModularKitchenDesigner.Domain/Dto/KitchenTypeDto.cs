using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class KitchenTypeDto : PrivateIdentity, ISimpleEntity
    {
        public KitchenTypeDto(){}

        public KitchenTypeDto( KitchenType kitchenType)
        {
            Title = kitchenType.Title;
            Code = kitchenType.Code;
            PriceSegment = kitchenType.PriceSegment.Title;
        }

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }

        public string PriceSegment { get; set; }
    }
}
