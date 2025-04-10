using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModelItem : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required short Quantity { get; set; }
        public Module Module { get; set; }
        public Model Model { get; set; }

    }
}