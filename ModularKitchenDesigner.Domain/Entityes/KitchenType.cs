using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class KitchenType : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public PriceSegment PriceSegment { get; set; }
        public Guid PriceSegmentId { get; set; }
        public List<MaterialItem> MaterialItems { get; set; }
        public List<Kitchen> Kitchens { get; set; } = [];
    }
}