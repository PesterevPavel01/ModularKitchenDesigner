using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ModelItemDto : BaseDto, IExcangeDtoConvertable<ModelItemDto, NomanclatureDto>
    {
        public ModelItemDto(){}

        public ModelItemDto( ModelItem modelItem)
        {
            ModuleCode = modelItem.Module.Code;
            ModelCode = modelItem.Model.Code;
            Quantity = modelItem.Quantity;
            Code = modelItem.Code;
            Title = modelItem.Title;
        }

        [Required(ErrorMessage = "ModuleCode cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "ModelCode cannot be null or empty.")]
        public string ModelCode { get; set; }

        public short Quantity { get; set; }

        public ModelItemDto Convert(NomanclatureDto dto)
        {
            Quantity = dto.Models.First().Quantity;
            ModelCode = dto.Models.First().Code;
            ModuleCode = dto.Code;
            Title = dto.Title == "removed" ? "removed" : dto.Models.First().Title;

            return this;
        }
    }
}
