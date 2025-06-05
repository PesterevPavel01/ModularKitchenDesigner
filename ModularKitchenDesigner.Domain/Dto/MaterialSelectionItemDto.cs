using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSelectionItemDto : BaseDto, IExcangeDtoConvertable<MaterialSelectionItemDto, NomanclatureDto>, IUniqueKeyQueryable<MaterialSelectionItemDto>
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

        public bool HasMatchingUniqueKey(IEnumerable<MaterialSelectionItemDto> models)
            => models.Select(model => model.ComponentType).Contains(ComponentType)
                && models.Select(model => model.Material).Contains(Material)
                && models.Select(model => model.KitchenType).Contains(KitchenType);

        MaterialSelectionItemDto IExcangeDtoConvertable<MaterialSelectionItemDto, NomanclatureDto>.Convert(NomanclatureDto dto)
        {
            Code = dto.Code;
            
            if(dto.Parents?.Count > 2)
            {
                ComponentType = dto.Parents[2].Title;
                KitchenType = dto.Parents[0].Title;

            }

            Material = dto.Title;

            return this;
        }
    }
}
