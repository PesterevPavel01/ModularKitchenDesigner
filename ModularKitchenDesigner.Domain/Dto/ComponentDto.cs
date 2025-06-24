using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ComponentDto : BaseDto, IExcangeDtoConvertable<ComponentDto, NomanclatureDto>, IUniqueKeyQueryable<ComponentDto>
    {
        public ComponentDto(Component newComponent)
        {
            Title = newComponent.Title;
            Code = newComponent.Code;
            Price = newComponent.Price;
            PriceSegment = newComponent.PriceSegment.Title;
            Material = newComponent.Material.Title;
            Model = newComponent.Model.Title;
        }

        public ComponentDto() { }
        
        public double Price { get; set; }

        [Required(ErrorMessage = "PriceSegment cannot be null or empty.")]
        public string PriceSegment { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Model cannot be null or empty.")]
        public string Model { get; set; }

        public ComponentDto Convert(NomanclatureDto dto)
        {
            Title = dto.Title;
            Code = dto.Code;
            Price = dto.Price;
            Model = dto.Template?.Title;

            if (dto.Parents?.Count > 1)
            {
                Material = dto.Parents[0].Title;
                PriceSegment = dto.Parents[1].Title;
            }
            return this;
        }

        public bool HasMatchingUniqueKey(IEnumerable<ComponentDto> models)
            =>
                models.Select(model => model.Code).Contains(this.Code);
    }
}
