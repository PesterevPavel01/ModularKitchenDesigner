using System.ComponentModel.DataAnnotations;
using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Material : SimpleEntity<Material>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Component> Components { get; set; } = [];
        public List<MaterialSelectionItem> MaterialItems { get; set; } = [];

        public Material() { }
    }
}