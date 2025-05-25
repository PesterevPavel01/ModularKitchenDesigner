using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSelectionItemDto
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

        public String Code { get; set; }

    }
}
