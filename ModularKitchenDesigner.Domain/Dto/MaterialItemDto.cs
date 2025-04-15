using ModularKitchenDesigner.Domain.Entityes;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialItemDto
    {
        public MaterialItemDto(){}

        public MaterialItemDto(MaterialItem materialItem)
        {
            ComponentType = materialItem.ComponentType.Title;
            Material = materialItem.Material.Title;
            KitchenType = materialItem.KitchenType.Title;
        }

        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string KitchenType { get; set; }
    }
}
