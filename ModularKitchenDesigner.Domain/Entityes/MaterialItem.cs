using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialItem : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ComponentType ComponentType { get; set; }
        public Guid ComponentTypeId { get; set; }
        public Material Material { get; set; }
        public Guid MaterialId { get; set; }
        public List<KitchenType> KitchenTypes { get; set; }
    }
}