using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Kitchen : Identity, IAuditable
    {
        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public required string Title { get; set; }
        public required string Code { get; set; }

        public KitchenType KitchenType { get; set; }   
        public Guid KitchenTypeId { get; set; }
        public List<Section> Sections { get; set; } = [];
    }
}
