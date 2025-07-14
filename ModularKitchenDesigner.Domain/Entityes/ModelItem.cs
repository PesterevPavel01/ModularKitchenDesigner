using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModelItem : BaseEntity, IAuditable, IDtoConvertible<ModelItem, ModelItemDto>
    {
        private ModelItem(){}

        private ModelItem(short quantity, Module module, Model model, string code, string title = null, bool enabled = true) 
        {
            Quantity = quantity;
            ModuleId = module.Id;
            ModelId = model.Id;
            Code = code ?? Guid.NewGuid().ToString();
            Title = title is null ? "N/A" : title;
            Enabled = enabled;
        }

        public short Quantity { get; private set; }

        public Module Module { get; private set; }
        public Guid ModuleId { get; private set; }
        public Model Model { get; private set; }
        public Guid ModelId { get; private set; }

        public static Func<IQueryable<ModelItem>, IIncludableQueryable<ModelItem, object>> IncludeRequaredField()
        => query => query
            .Include(x => x.Module)
            .Include(x => x.Model);

        public bool IsUniqueKeyEqual(ModelItemDto model)
            => this.Model.Code == model.ModelCode
            && this.Module.Code == model.ModuleCode;

        public static Expression<Func<ModelItem, bool>> ContainsByUniqueKeyPredicate(List<ModelItemDto> models)
        {
            var codeParam = Expression.Parameter(typeof(ModelItem), "entity");

            var orExpressions = models.Select(model =>
            {
                var modelProperty = Expression.Property(codeParam, nameof(ModelItem.Model));
                var modelCodeProperty = Expression.Property(modelProperty, nameof(Model.Code));

                var modelCondition = Expression.Equal(
                    modelCodeProperty,
                    Expression.Constant(model.ModelCode));

                var moduleProperty = Expression.Property(codeParam, nameof(ModelItem.Module));
                var moduleCodeProperty = Expression.Property(moduleProperty, nameof(Module.Code));

                var moduleCondition = Expression.Equal(
                    moduleCodeProperty,
                    Expression.Constant(model.ModuleCode));

                return Expression.AndAlso(modelCondition, moduleCondition);
            });

            var finalCondition = orExpressions.Aggregate((accum, current) => Expression.OrElse(accum, current));

            return Expression.Lambda<Func<ModelItem, bool>>(finalCondition, codeParam);
        }

        public ModelItemDto ConvertToDto()
        => new()
        {
            ModuleCode = Module.Code,
            ModelCode = Model.Code,
            Quantity = Quantity,
            Code = Code,
            Title = Title
        };

        public static ModelItem Create(short quantity, Module module, Model model, string code = null, string title = null, bool enabled = true)
            => new(quantity, module, model, code, title, enabled);

        public ModelItem Update(short quantity, Module module, Model model, string code = null, string title = null, bool enabled = true)
        {
            Quantity = quantity;
            ModuleId = module.Id;
            ModelId = model.Id;
            Code = code ?? Code;
            Enabled = enabled;
            Title = title is null ? "N/A" : title;

            return this;
        }
    }
}