using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public class ModelDto : BaseDto, IExcangeDtoConvertable<ModelDto, NomanclatureDto>, IUniqueKeyQueryable<ModelDto>
    {
        public ModelDto(){}
        public ModelDto(Model model)
        {
            Title = model.Title;
            Code = model.Code;
            ComponentType = model.ComponentType.Title;
        }

        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

        public ModelDto Convert(NomanclatureDto dto)
        {
            Title = dto.Title;
            Code = dto.Code;
            ComponentType = dto.Parents?.Count >= 2
                ? dto.Parents[^2].Title
                : null;
            return this;
        }

        public bool HasMatchingUniqueKey(IEnumerable<ModelDto> models)
            =>
                models.Select(model => model.Code).Contains(this.Code);
    }
}
