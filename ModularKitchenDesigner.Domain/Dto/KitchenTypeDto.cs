using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class KitchenTypeDto : BaseDto, IExcangeDtoConvertable<KitchenTypeDto, NomanclatureDto>, IUniqueKeyQueryable<KitchenTypeDto>
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

        public KitchenTypeDto Convert(NomanclatureDto dto)
        {
            Code = dto.Code;
            Title = dto.Title;
            if (dto.Parents?.Count > 0)
                PriceSegment = dto.Parents[0].Title;

            return this;
        }

        public bool HasMatchingUniqueKey(IEnumerable<KitchenTypeDto> models)
            =>
                models.Select(model => model.Code).Contains(this.Code);
    }
}
