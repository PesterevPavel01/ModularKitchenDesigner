using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModuleType : Identity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Title { get; set; }
        public required string Code { get; set; }

        public List<Module> Modules { get; set; } = [];
    }
}
