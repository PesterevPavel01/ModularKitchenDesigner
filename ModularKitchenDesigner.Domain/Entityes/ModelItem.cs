using System.Reflection;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class ModelItem : Identity, IAuditable, IConvertibleToDto<ModelItem, ModelItemDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public short Quantity { get; set; }

        public Module Module { get; set; }
        public Guid ModuleId { get; set; }
        public Model Model { get; set; }
        public Guid ModelId { get; set; }

        public static Func<IQueryable<ModelItem>, IIncludableQueryable<ModelItem, object>> IncludeRequaredField()
        => query => query
        .Include(x => x.Module)
        .Include(x => x.Model);

        public ModelItem ConvertFromDtoWithRequiredFields(ModelItemDto model)
        {
            Module = new() { Code = model.ModuleCode };
            Model = new() { Code = model.ModelCode };
            return this;
        }

        public ModelItemDto ConvertToDto()
        => new()
        {
            ModuleCode = Module.Code,
            ModelCode = Model.Code,
            Quantity = Quantity
        };
    }
}