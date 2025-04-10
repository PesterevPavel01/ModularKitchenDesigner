using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class KitchenType : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }
        public required string Code { get; set; }

        public PriceSegment PriceSegment { get; set; }
        public Guid PriceSegmentId { get; set; }
        public MaterialItem MaterialItem { get; set; }
        public Guid MaterialItemId { get; set; }
        public List<Kitchen> Kitchens { get; set; } = [];
    }
}