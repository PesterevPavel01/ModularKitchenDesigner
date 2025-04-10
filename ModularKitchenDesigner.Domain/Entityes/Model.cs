using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Model : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required  string Code { get; set; }
        public required string Title { get; set; }

        public List<ModelItem> ModelItems { get; set; } = [];
        public List<Component> Components { get; set; } = [];
    }
}