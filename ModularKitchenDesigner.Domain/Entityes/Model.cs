using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Model : SimpleEntity, IDtoConvertible<Model, SimpleDto>
    {
        public List<ModelItem> ModelItems { get; set; } = [];
        public List<Component> Components { get; set; } = [];

        public static Expression<Func<Model, bool>> ContainsByUniqueKeyPredicate(List<SimpleDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);

        public static Func<IQueryable<Model>, IIncludableQueryable<Model, object>> IncludeRequaredField()
            => null;
    }
}