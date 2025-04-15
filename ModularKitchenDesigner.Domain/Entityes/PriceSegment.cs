using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class PriceSegment : Identity, IAuditable, ISimpleEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public List<Component> Components { get; set; } = [];
        public List<KitchenType> Types { get; set; } = [];
    }
}