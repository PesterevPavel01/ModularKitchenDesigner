﻿using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialSpecificationItem : Identity, IAuditable, IDtoConvertible<MaterialSpecificationItem, MaterialSpecificationItemDto>
    {
        private MaterialSpecificationItem(){}

        private MaterialSpecificationItem(ModuleType moduleType, MaterialSelectionItem materialSelectionItem, Kitchen kitchen, string code) 
        {
            ModuleTypeId = moduleType.Id;
            MaterialSelectionItemId = materialSelectionItem.Id;
            KitchenId = kitchen.Id;
            Code = code ?? Guid.NewGuid().ToString();
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Code { get; set; }

        public ModuleType ModuleType { get; private set; }
        public Guid ModuleTypeId { get; private set; }

        public MaterialSelectionItem MaterialSelectionItem { get; private set; }
        public Guid MaterialSelectionItemId { get; private set; }

        public Kitchen Kitchen { get; private set; }
        public Guid KitchenId { get; private set; }

        public static MaterialSpecificationItem Create(ModuleType moduleType, MaterialSelectionItem materialSelectionItem, Kitchen kitchen, string code = null)
            => new(moduleType, materialSelectionItem, kitchen, code);

        public MaterialSpecificationItem Update(ModuleType moduleType, MaterialSelectionItem materialSelectionItem, Kitchen kitchen, string code = null) 
        {
            ModuleTypeId = moduleType.Id;
            MaterialSelectionItemId = materialSelectionItem.Id;
            KitchenId = kitchen.Id;
            Code = code ?? Code;

            return this;
        }

        public static Func<IQueryable<MaterialSpecificationItem>, IIncludableQueryable<MaterialSpecificationItem, object>> IncludeRequaredField()
            => query => query
                .Include(x => x.ModuleType)
                .Include(x => x.MaterialSelectionItem)
                .Include(x => x.Kitchen);

        public bool IsUniqueKeyEqual(MaterialSpecificationItemDto model)
            => this.Kitchen.Code == model.KitchenCode
            && this.ModuleType.Title == model.ModuleType
            && this.MaterialSelectionItem.Code == model.MaterialSelectionItemCode;

        public static Expression<Func<MaterialSpecificationItem, bool>> ContainsByUniqueKeyPredicate(List<MaterialSpecificationItemDto> models)
        {
            var codeParam = Expression.Parameter(typeof(MaterialSpecificationItem), "entity");

            var orExpressions = models.Select(model =>
            {
                var moduleTypeProperty = Expression.Property(codeParam, nameof(MaterialSpecificationItem.ModuleType));
                var moduleTypeTitleProperty = Expression.Property(moduleTypeProperty, nameof(ModuleType.Title));

                var moduleTypeCondition = Expression.Equal(
                    moduleTypeTitleProperty,
                    Expression.Constant(model.ModuleType));

                var kitchenProperty = Expression.Property(codeParam, nameof(MaterialSpecificationItem.Kitchen));
                var kitchenCodeProperty = Expression.Property(kitchenProperty, nameof(Kitchen.Code));

                var kitchenCondition = Expression.Equal(
                    kitchenCodeProperty,
                    Expression.Constant(model.KitchenCode));

                var MaterialSelectionItemProperty = Expression.Property(codeParam, nameof(MaterialSpecificationItem.MaterialSelectionItem));
                var MaterialSelectionItemCodeProperty = Expression.Property(MaterialSelectionItemProperty, nameof(MaterialSelectionItem.Code));

                var MaterialSelectionItemCondition = Expression.Equal(
                    MaterialSelectionItemCodeProperty,
                    Expression.Constant(model.MaterialSelectionItemCode));

                return Expression.AndAlso(MaterialSelectionItemCondition,
                    Expression.AndAlso(moduleTypeCondition, kitchenCondition));
            });

            var finalCondition = orExpressions.Aggregate((accum, current) => Expression.OrElse(accum, current));

            return Expression.Lambda<Func<MaterialSpecificationItem, bool>>(finalCondition, codeParam);
        }

        public MaterialSpecificationItemDto ConvertToDto()
            => new()
                {
                    ModuleType = ModuleType.Title,
                    KitchenCode = Kitchen.Code,
                    MaterialSelectionItemCode = MaterialSelectionItem.Code,
                    Code = Code
                };
    }
}
