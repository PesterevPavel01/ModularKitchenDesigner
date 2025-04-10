using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class PriceSegment : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }

        public List<Component> Components { get; set; }
        public List<KitchenType> Types { get; set; }
    }
}