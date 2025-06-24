using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Kitchen : BaseEntity, IAuditable, IDtoConvertible<Kitchen, KitchenDto>
    {
        public Kitchen(){}
        private Kitchen(string userLogin, string userId, string title, KitchenType kitchenType, string code)
        {
            UserLogin = userLogin;
            UserId = userId;
            Title = title ?? "N/A";
            KitchenTypeId = kitchenType.Id;
            Code = code ?? Guid.NewGuid().ToString();
        }
        public string UserLogin { get; private set; }
        public string UserId { get; private set; }

        public KitchenType KitchenType { get; private set; }
        public Guid KitchenTypeId { get; private set; }
        public List<Section> Sections { get; private set; } = [];
        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; private set; } = [];

        public static Kitchen Create(string userLogin, string userId, KitchenType kitchenType, string title = null, string code = null)
            => new(userLogin, userId, title, kitchenType, code);

        public Kitchen Update(string userLogin, string userId, KitchenType kitchenType, string title = null, string code = null)
        {
            UserLogin = userLogin;
            UserId = userId;
            Title = title ?? Title;
            KitchenTypeId = kitchenType.Id;
            Code = code ?? Code;

            return this;
        }

        public static Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>> IncludeRequaredField()
            => query => query
            .Include(x => x.KitchenType)
            .Include(x => x.Sections)
            .Include(x => x.MaterialSpecificationItems);

        public bool IsUniqueKeyEqual(KitchenDto model)
            => this.Code == model.Code;

        public Func<List<KitchenDto>, bool> containsByUniqueKey()
            => models 
                => models.Select(model => model.Code).Contains(this.Code);

        public static Expression<Func<Kitchen, bool>> ContainsByUniqueKeyPredicate(List<KitchenDto> models)
            => entity 
                => models.Select(model => model.Code).Contains(entity.Code);

        public KitchenDto ConvertToDto()
            => new()
            {
                UserLogin = this.UserLogin,
                UserId = this.UserId,
                KitchenType = this.KitchenType.Title,
                Title = this.Title,
                Code = this.Code
            };
    }
}
