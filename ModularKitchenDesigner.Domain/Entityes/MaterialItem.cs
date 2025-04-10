using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialItem : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public KitchenType KitchenType { get; set; }
        public ComponentType ComponentType { get; set; }
        public List<Material> Materials { get; set; }
    }
}