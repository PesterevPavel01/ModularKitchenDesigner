using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialSpecificationItem : Identity, IAuditable, IConvertibleToDto<MaterialSpecificationItem, MaterialSpecificationItemDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ModuleType ModuleType { get; set; }
        public Guid ModuleTypeId { get; set; }

        public MaterialSelectionItem MaterialSelectionItem { get; set; }
        public Guid MaterialSelectionItemId { get; set; }

        public Kitchen Kitchen { get; set; }
        public Guid KitchenId { get; set; }

        public static Func<IQueryable<MaterialSpecificationItem>, IIncludableQueryable<MaterialSpecificationItem, object>> IncludeRequaredField()
        => query => query
        .Include(x => x.ModuleType)
        .Include(x => x.MaterialSelectionItem)
        .Include(x => x.Kitchen);

        public MaterialSpecificationItem ConvertFromDtoWithRequiredFields(MaterialSpecificationItemDto model)
        {
            MaterialSelectionItemId = model.MaterialSelectionItemGuid;
            KitchenId = model.KitchenGuid;
            ModuleType = new() { Title = model.ModuleType };
            return this;
        }

        public MaterialSpecificationItemDto ConvertToDto()
        => new()
        {
            ModuleType = ModuleType.Title,
            KitchenGuid = Kitchen.KitchenTypeId,
            MaterialSelectionItemGuid = MaterialSelectionItemId,
        };
    }
}
