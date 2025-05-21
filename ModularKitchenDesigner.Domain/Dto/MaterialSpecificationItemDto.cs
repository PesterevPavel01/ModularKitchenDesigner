using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSpecificationItemDto : PrivateIdentity
    {
        public MaterialSpecificationItemDto() { }

        public MaterialSpecificationItemDto(MaterialSpecificationItem materialItem)
        {
            ModuleType = materialItem.ModuleType.Title;
            KitchenGuid = materialItem.Kitchen.KitchenTypeId;
            MaterialSelectionItemGuid = materialItem.MaterialSelectionItemId;
        }

        [Required(ErrorMessage = "ModuleType cannot be null or empty.")]
        public string ModuleType { get; set; }

        [Required(ErrorMessage = "MaterialSelectionItem cannot be null or empty.")]
        public Guid MaterialSelectionItemGuid { get; set; }

        [Required(ErrorMessage = "KitchenGuid cannot be null or empty.")]
        public Guid KitchenGuid { get; set; }

    }
}

