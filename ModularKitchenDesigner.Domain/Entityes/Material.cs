using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class Material : Identity, IAuditable, ISimpleEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public string Title { get; set; } = default;
        [Required]
        public string Code { get; set; } = default;

        public List<Component> Components { get; set; } = [];
        public List<MaterialItem> MaterialItems { get; set; } = [];

        public Material() { }
    }
}