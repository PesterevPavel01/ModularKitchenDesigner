using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
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
            Code = modelItem.Code;
        }

        [Required(ErrorMessage = "ModuleCode cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "ModelCode cannot be null or empty.")]
        public string ModelCode { get; set; }

        public short Quantity { get; set; }
        public String Code { get; set; }

    }
}
