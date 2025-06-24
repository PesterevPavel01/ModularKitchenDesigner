using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class ComponentType : SimpleEntity, IDtoConvertible<ComponentType, SimpleDto>
    {
        public ComponentType(){}

        public List<Model> Models { get; set; } = [];
        public List<MaterialSelectionItem> MaterialItems { get; set; } = [];

        public static Expression<Func<ComponentType, bool>> ContainsByUniqueKeyPredicate(List<SimpleDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);

        public static Func<IQueryable<ComponentType>, IIncludableQueryable<ComponentType, object>> IncludeRequaredField()
            => null;

    }
}