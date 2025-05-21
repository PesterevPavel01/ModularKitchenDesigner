using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class MaterialSelectionItemDto : PrivateIdentity
    {
        public MaterialSelectionItemDto(){}

        public MaterialSelectionItemDto(MaterialSelectionItem materialItem)
        {
            ComponentType = materialItem.ComponentType.Title;
            Material = materialItem.Material.Title;
            KitchenType = materialItem.KitchenType.Title;
            Guid = materialItem.Id;
            base.SetId(materialItem.Id);
        }

        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string KitchenType { get; set; }

        public Guid Guid { get; set; }

    }
}
