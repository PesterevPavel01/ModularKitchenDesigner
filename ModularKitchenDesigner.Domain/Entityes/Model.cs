using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Model : BaseEntity, IAuditable, IDtoConvertible<Model, ModelDto>
    {
        public Guid ComponentTypeId { get; private set; }
        public ComponentType ComponentType { get; private set; }
        public List<ModelItem> ModelItems { get; set; } = [];
        public List<Component> Components { get; set; } = [];

        private Model(string title, string code, ComponentType componentType, bool enabled = true) 
        {
            Title = title;
            Code = code;
            ComponentTypeId = componentType.Id;
            Enabled = enabled;
        }

        public Model() { }


        public ModelDto ConvertToDto()
            => new(this);

        public static Model Create(string title, string code, ComponentType componentType, bool enabled = true)
         => new(title, code, componentType, enabled);

        public Model Update(string title, string code, ComponentType componentType, bool enabled = true)
        {
            Title = title;
            Code = code;
            ComponentTypeId = componentType.Id;
            Enabled = enabled;

            return this;
        }

        public static Func<IQueryable<Model>, IIncludableQueryable<Model, object>> IncludeRequaredField()
            =>
            query => query
                .Include(x => x.ComponentType);

        public bool IsUniqueKeyEqual(ModelDto model)
            => this.Code == model.Code;

        public static Expression<Func<Model, bool>> ContainsByUniqueKeyPredicate(List<ModelDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);
    }
}