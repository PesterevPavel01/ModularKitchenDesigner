using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class KitchenTypeDto : BaseEntity
    {
        public KitchenTypeDto(){}

        public KitchenTypeDto(string title, string code,string priceSegment)
        { 
            Title = title;
            Code = code;
            PriceSegment = priceSegment;       
        }

        public KitchenTypeDto( KitchenType kitchenType)
        {
            Title = kitchenType.Title;
            Code = kitchenType.Code;
            PriceSegment = kitchenType.PriceSegment.Title;
        }

        public string PriceSegment { get; set; }

    }
}
