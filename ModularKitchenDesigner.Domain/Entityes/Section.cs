using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Section : Identity, IAuditable
    {
        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public required short Quantity { get; set; }

        public Kitchen Kitchen { get; set; }
        public Guid KitchenId { get; set; }
        public Module Module { get; set; }
        public Guid ModuleId { get; set; }
    }
}
