using Interceptors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces.Base;

namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public class SimpleEntity : BaseEntity, ISimpleEntity, IAuditable
    {
        protected SimpleEntity(){ }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private SimpleEntity(string title, string code, bool enabled = true) 
        {
            Title = title;
            Code = code;
            Enabled = enabled;
        }

        public bool IsUniqueKeyEqual(SimpleDto model)
            => this.Code == model.Code;

        public SimpleDto ConvertToDto()
            => new(title:Title, code:Code);

        public static TEntity Create<TEntity>(string title, string code, bool enabled = true)
            where TEntity : BaseEntity, new()
            => new TEntity()
            {
                Code = code, 
                Title = title, 
                Enabled = enabled
            };

        public ISimpleEntity Update(string title, string code, bool enabled = true)
        {
            Title = title;
            Code = code;
            Enabled = enabled;

            return this;
        }
    }
}
