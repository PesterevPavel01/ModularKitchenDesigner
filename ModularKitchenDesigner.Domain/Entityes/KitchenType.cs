using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class KitchenType : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }
        
        public PriceSegment PriceSegment { get; set; }
        public List<Kitchen> Kitchens { get; set; }
        public MaterialItem MaterialItem { get; set; }

    }
}