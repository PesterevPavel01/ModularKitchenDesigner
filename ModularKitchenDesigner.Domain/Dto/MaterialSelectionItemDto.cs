using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSelectionItemDto : BaseDto, IExcangeDtoConvertable<MaterialSelectionItemDto, NomanclatureDto>
    {
        public MaterialSelectionItemDto(){}

        public MaterialSelectionItemDto(MaterialSelectionItem materialSelectionItem)
        {
            ComponentType = materialSelectionItem.ComponentType.Title;
            Material = materialSelectionItem.Material.Title;
            KitchenType = materialSelectionItem.KitchenType.Title;
            Code = materialSelectionItem.Code;
        }

        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string KitchenType { get; set; }

        MaterialSelectionItemDto IExcangeDtoConvertable<MaterialSelectionItemDto, NomanclatureDto>.Convert(NomanclatureDto dto)
        {
            Code = dto.Code;
            ComponentType = dto.Parents[2].Title;
            Material = dto.Title;
            KitchenType = dto.Parents[0].Title;

            return this;
        }
    }
}
