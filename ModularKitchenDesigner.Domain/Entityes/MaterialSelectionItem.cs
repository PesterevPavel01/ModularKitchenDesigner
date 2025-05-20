using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialSelectionItem : Identity, IAuditable, IConvertibleToDto<MaterialSelectionItem, MaterialSelectionItemDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ComponentType ComponentType { get; set; }
        public Guid ComponentTypeId { get; set; }
        public Material Material { get; set; }
        public Guid MaterialId { get; set; }
        public KitchenType KitchenType { get; set; }
        public Guid KitchenTypeId { get; set; }

        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; set; }

        public static Func<IQueryable<MaterialSelectionItem>, IIncludableQueryable<MaterialSelectionItem, object>> IncludeRequaredField()
        => query => query
        .Include(x => x.Material)
        .Include(x => x.ComponentType)
        .Include(x => x.KitchenType);

        public MaterialSelectionItemDto ConvertToDto()
        => new()
        {
            ComponentType = ComponentType.Title,
            Material = Material.Title,
            KitchenType = KitchenType.Title,
            Guid = Id
        };
    }
}