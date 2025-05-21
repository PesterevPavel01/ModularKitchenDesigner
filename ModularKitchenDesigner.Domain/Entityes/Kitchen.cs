using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Kitchen : Identity, IAuditable, IConvertibleToDto<Kitchen, KitchenDto>
    {
        public DateTime CreatedAt { get ; set ; }
        public DateTime UpdatedAt { get ; set ; }
        public string UserLogin { get; set; }
        public string UserId { get; set; }
        public KitchenType KitchenType { get; set; }
        public Guid KitchenTypeId { get; set; }
        public List<Section> Sections { get; set; } = [];
        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; set; } = [];

        public static Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>> IncludeRequaredField()
            =>
                query => query
                    .Include(x => x.KitchenType);

        public Kitchen ConvertFromDtoWithRequiredFields(KitchenDto model)
        {
            model.Guid = Guid.NewGuid();
            Id = model.Guid;
            return this;
        }

        public KitchenDto ConvertToDto()
            => new()
            {
                UserLogin = UserLogin,
                UserId = UserId,
                KitchenType = KitchenType.Title,
                Guid = Id
            };
    }
}
