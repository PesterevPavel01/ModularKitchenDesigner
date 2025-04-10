using Interceptors;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Section : Identity, IAuditable
    {
        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public required short Quantity { get; set; }
        public Kitchen Kitchen { get; set; }
        public Module Module { get; set; }
    }
}
