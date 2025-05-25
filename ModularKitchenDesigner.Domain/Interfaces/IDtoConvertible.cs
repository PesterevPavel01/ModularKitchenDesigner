using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ModularKitchenDesigner.Domain.Interfaces
{
    public interface IDtoConvertible <TEntity, TDto>
    {
        TDto ConvertToDto();
        static abstract Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeRequaredField();
        bool isUniqueKeyEqual(TDto model);
        static abstract Expression<Func<TEntity,bool>> ContainsByUniqueKeyPredicate(List<TDto> models);
    }
}
