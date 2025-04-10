using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModelItem : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required short Quantity { get; set; }

        public Module Module { get; set; }
        public Guid ModuleId { get; set; }
        public Model Model { get; set; }
        public Guid ModelId { get; set; }

    }
}