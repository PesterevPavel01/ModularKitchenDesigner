using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class ComponentType : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }
        public List<Component> Components { get; set; } = [];
        public List<MaterialItem> MaterialItems { get; set; } = [];

    }
}