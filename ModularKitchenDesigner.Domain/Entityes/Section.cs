using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Section : Identity, IAuditable, IConvertibleToDto<Section, SectionDto>
    {
        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public short Quantity { get; set; }

        public Kitchen Kitchen { get; set; }
        public Guid KitchenId { get; set; }
        public Module Module { get; set; }
        public Guid ModuleId { get; set; }

        public static Func<IQueryable<Section>, IIncludableQueryable<Section, object>> IncludeRequaredField()
        => query => query
        .Include(x => x.Kitchen)
        .Include(x => x.Module);

        public SectionDto ConvertToDto()
        => new()
        {
            Quantity = Quantity,
            ModuleCode = Module.Code,
            KitchenGuid = Kitchen.Id
        };
    }
}
