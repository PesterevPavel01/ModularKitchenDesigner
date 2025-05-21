using Microsoft.EntityFrameworkCore.Query;

namespace ModularKitchenDesigner.Domain.Interfaces
{
    public interface IConvertibleToDto <TEntity, TDto>
    {
        TDto ConvertToDto();
        TEntity ConvertFromDtoWithRequiredFields(TDto model);

        static abstract Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeRequaredField();
    }
}
