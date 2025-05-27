using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModuleType : SimpleEntity, IDtoConvertible<ModuleType, SimpleDto>
    {
        public ModuleType(){}

        public List<Module> Modules { get; set; } = [];
        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; set; } = [];

        public static Expression<Func<ModuleType, bool>> ContainsByUniqueKeyPredicate(List<SimpleDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);

        public static Func<IQueryable<ModuleType>, IIncludableQueryable<ModuleType, object>> IncludeRequaredField()
            => null;
    }
}
