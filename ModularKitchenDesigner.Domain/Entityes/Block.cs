using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Block : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required short Quantity { get; set; }

        public Module Module { get; set; }
        public Guid ModuleId { get; set; }
        public Component Component { get; set; }
        public Guid ComponentId { get; set; }
    }
}
