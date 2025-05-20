using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class ComponentType : SimpleEntity<ComponentType>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Component> Components { get; set; } = [];
        public List<MaterialSelectionItem> MaterialItems { get; set; } = [];
    }
}