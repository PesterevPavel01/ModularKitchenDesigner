using ModularKitchenDesigner.Domain.Entityes;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ComponentDto
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

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }
        
        public double Price { get; set; }
        
        [Required(ErrorMessage = "ComponentType cannot be null or empty.")]
        public string ComponentType { get; set; }

        [Required(ErrorMessage = "PriceSegment cannot be null or empty.")]
        public string PriceSegment { get; set; }

        [Required(ErrorMessage = "Material cannot be null or empty.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Model cannot be null or empty.")]
        public string Model { get; set; }
    }
}
