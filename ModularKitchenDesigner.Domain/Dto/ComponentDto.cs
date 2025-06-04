using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
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
            ComponentType = newComponent.ComponentType.Title;
            PriceSegment = newComponent.PriceSegment.Title;
            Material = newComponent.Material.Title;
            Model = newComponent.Model.Title;
        }

        public ComponentDto() { }
        
        public double Price { get; set; }
        
        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

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
            Model = dto.Template?.Title ?? "default";

            ComponentType = dto.Parents.Last().Title;

            if (dto.Parents.FindIndex(x => x.Code == "00080200115") == 3)
            {   //Фасады
                PriceSegment = dto.Parents[2].Title;
                Material = dto.Parents[0].Title;
                ComponentType = dto.Parents[3].Title;
            }
            else if (dto.Parents.FindIndex(x => x.Code == "00080200117") == 1)
            {   //Ящики
                PriceSegment = "default";
                Material = "default";
                ComponentType = dto.Parents[1].Title;
            }
            else if (dto.Parents.FindIndex(x => x.Code == "00080200116") == 1)
            {   //Полки
                PriceSegment = "default";
                Material = "default";
                ComponentType = dto.Parents[1].Title;
            }
            else if (dto.Parents.FindIndex(x => x.Code == "00080200112") == 1)
            {   //Корпуса
                PriceSegment = "default";
                Material = "default";
                ComponentType = dto.Parents[1].Title;
            }
            return this;
        }

        public bool HasMatchingUniqueKey(IEnumerable<ComponentDto> models)
            =>
                models.Select(model => model.Code).Contains(this.Code);
    }
}
