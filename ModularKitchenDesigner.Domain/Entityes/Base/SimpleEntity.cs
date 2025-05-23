﻿using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;

namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public class SimpleEntity<TEntity> : Identity, ISimpleEntity, IConvertibleToDto<TEntity, SimpleDto>
        where TEntity : class, ISimpleEntity, new()
    {
        public string Title { get; set; }
        public string Code { get; set; }

        public static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeRequaredField()
            => null;

        public TEntity ConvertFromDtoWithRequiredFields(SimpleDto model)
        {
            return new() {Code = model.Code };
        }

        public SimpleDto ConvertToDto()
        => new()
        {
            Title = Title,
            Code = Code
        };
    }
}
