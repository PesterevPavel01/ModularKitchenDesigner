using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class Component : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }
        public required string Code { get; set; }
        public required double Price { get; set; }

        public ComponentType ComponentType { get; set; }
        public Guid ComponentTypeId { get; set; }
        public PriceSegment PriceSegment { get; set; }
        public Guid PriceSegmentId { get; set; }
        public Material Material { get; set; }
        public Guid MaterialId { get; set; }
        public Model Model { get; set; }
        public Guid ModelId { get; set; }
        public List<Block> Blocks { get; set; }

    }
}