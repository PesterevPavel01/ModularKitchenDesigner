using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class PriceSegment : SimpleEntity, IDtoConvertible<PriceSegment, SimpleDto>
    {
        public PriceSegment(){}

        public List<Component> Components { get; set; } = [];
        public List<KitchenType> Types { get; set; } = [];

        public static Func<IQueryable<PriceSegment>, IIncludableQueryable<PriceSegment, object>> IncludeRequaredField()
            => null;

        public static Expression<Func<PriceSegment, bool>> ContainsByUniqueKeyPredicate(List<SimpleDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);

    }
}