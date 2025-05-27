using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Section : Identity, IAuditable, IDtoConvertible<Section, SectionDto>
    {
        private Section(){}

        private Section(short quantity, Kitchen kitchen, Module module, string code) 
        {
            Quantity = quantity;
            KitchenId = kitchen.Id;
            ModuleId = module.Id;
            Code = code ?? Guid.NewGuid().ToString();
        }

        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public short Quantity { get; private set; }
        public string Code { get; set; }

        public Kitchen Kitchen { get; private set; }
        public Guid KitchenId { get; private set; }
        public Module Module { get; private set; }
        public Guid ModuleId { get; private set; }

        public static Func<IQueryable<Section>, IIncludableQueryable<Section, object>> IncludeRequaredField()
            => query => query
                .Include(x => x.Kitchen)
                .Include(x => x.Module);

        public bool isUniqueKeyEqual(SectionDto model)
            => this.Module.Code == model.ModuleCode
            && this.Kitchen.Code == model.KitchenCode;

        public static Expression<Func<Section, bool>> ContainsByUniqueKeyPredicate(List<SectionDto> models)
            => entity
                => models.Select(model => model.ModuleCode).Contains(entity.Module.Code)
                && models.Select(model => model.KitchenCode).Contains(entity.Kitchen.Code);

        public SectionDto ConvertToDto()
        => new()
        {
            Quantity = this.Quantity,
            ModuleCode = this.Module.Code,
            KitchenCode = this.Kitchen.Code,
            Code = this.Code
        };

        public static Section Create(short quantity, Kitchen kitchen, Module module, string code = null)
            => new(quantity, kitchen, module, code);

        public Section Update(short quantity, Kitchen kitchen, Module module, string code = null) 
        {
            Quantity = quantity;
            KitchenId = kitchen.Id;
            ModuleId = module.Id;
            Code = code ?? Code;

            return this;
        }
    }
}
