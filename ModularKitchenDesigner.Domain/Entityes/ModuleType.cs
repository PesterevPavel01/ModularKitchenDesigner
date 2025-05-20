using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModuleType : SimpleEntity<ModuleType>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Module> Modules { get; set; } = [];
        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; set; } = [];
    }
}
