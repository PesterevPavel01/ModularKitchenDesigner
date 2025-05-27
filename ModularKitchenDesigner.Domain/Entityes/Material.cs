using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Material : SimpleEntity, IDtoConvertible<Material, SimpleDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Component> Components { get; set; } = [];
        public List<MaterialSelectionItem> MaterialItems { get; set; } = [];

        public Material() { }

        public static Func<IQueryable<Material>, IIncludableQueryable<Material, object>> IncludeRequaredField()
            => null;

        public static Expression<Func<Material, bool>> ContainsByUniqueKeyPredicate(List<SimpleDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);
    }
}