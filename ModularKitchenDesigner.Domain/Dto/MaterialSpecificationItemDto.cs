using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSpecificationItemDto
    {
        public MaterialSpecificationItemDto() { }

        public MaterialSpecificationItemDto(MaterialSpecificationItem materialSpecificationItem)
        {
            ModuleType = materialSpecificationItem.ModuleType.Title;
            KitchenCode = materialSpecificationItem.Kitchen.Code;
            MaterialSelectionItemCode = materialSpecificationItem.MaterialSelectionItem.Code;
        }

        [Required(ErrorMessage = "ModuleType cannot be null or empty.")]
        public string ModuleType { get; set; }

        [Required(ErrorMessage = "MaterialSelectionItemCode cannot be null or empty.")]
        public string MaterialSelectionItemCode { get; set; }

        [Required(ErrorMessage = "KitchenCode cannot be null or empty.")]
        public string KitchenCode { get; set; }

        public string Code { get; set; }

    }
}

