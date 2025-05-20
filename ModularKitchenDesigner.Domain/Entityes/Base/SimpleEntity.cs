using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public class SimpleEntity<TEntity> : Identity, ISimpleEntity, IConvertibleToDto<TEntity, SimpleDto>
        where TEntity : class
    {
        public string Title { get; set; }
        public string Code { get; set; }

        public static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeRequaredField()
            => null;

        public SimpleDto ConvertToDto()
        => new()
        {
            Title = Title,
            Code = Code
        };
    }
}
