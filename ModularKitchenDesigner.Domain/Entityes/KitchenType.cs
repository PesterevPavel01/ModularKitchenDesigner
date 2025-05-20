using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class KitchenType : Identity, IAuditable, IConvertibleToDto<KitchenType, KitchenTypeDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public PriceSegment PriceSegment { get; set; }
        public Guid PriceSegmentId { get; set; }
        public List<MaterialSelectionItem> MaterialSelectionItems { get; set; }
        public List<Kitchen> Kitchens { get; set; } = [];

        public static Func<IQueryable<KitchenType>, IIncludableQueryable<KitchenType, object>> IncludeRequaredField()
            =>
            query => query.Include(x => x.PriceSegment);

        public KitchenTypeDto ConvertToDto()
            => new()
            {
                Title = Title,
                Code = Code,
                PriceSegment = PriceSegment.Title
            };
    }
}