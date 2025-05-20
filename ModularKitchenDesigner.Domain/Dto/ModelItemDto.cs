using ModularKitchenDesigner.Domain.Entityes;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ModelItemDto
    {
        public ModelItemDto(){}

        public ModelItemDto( ModelItem modelItem)
        {
            ModuleCode = modelItem.Module.Code;
            ModelCode = modelItem.Model.Code;
            Quantity = modelItem.Quantity;
        }

        [Required(ErrorMessage = "ModuleCode cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "ModelCode cannot be null or empty.")]
        public string ModelCode { get; set; }

        public short Quantity { get; set; }
    }
}
