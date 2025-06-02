using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class KitchenType : BaseEntity, IAuditable, IDtoConvertible<KitchenType, KitchenTypeDto>
    {
        protected KitchenType(){}

        private KitchenType(string title, string code, PriceSegment priceSegment, bool enabled = true) 
        {
            Title = title;
            Code = code;
            PriceSegmentId = priceSegment.Id;
            Enabled = enabled;
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PriceSegment PriceSegment { get; private set; }
        public Guid PriceSegmentId { get; private set; }
        public List<MaterialSelectionItem> MaterialSelectionItems { get; set; } = [];
        public List<Kitchen> Kitchens { get; set; } = [];

        public static Func<IQueryable<KitchenType>, IIncludableQueryable<KitchenType, object>> IncludeRequaredField()
            =>
            query => query.Include(x => x.PriceSegment)
                          .Include(x => x.Kitchens)
                          .Include(x => x.MaterialSelectionItems);

        public bool IsUniqueKeyEqual(KitchenTypeDto model)
            => this.Code == model.Code;


        public static Expression<Func<KitchenType, bool>> ContainsByUniqueKeyPredicate(List<KitchenTypeDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);

        public KitchenTypeDto ConvertToDto()
            => new(title: Title,code: Code, priceSegment: PriceSegment.Title);

        public static KitchenType Create(String title, string code, PriceSegment priceSegment, bool enabled = true)
            => new(title, code, priceSegment, enabled);

        public KitchenType Update(String title, string code, PriceSegment priceSegment, bool enabled = true)
        {
            Title = title;
            Code = code;
            PriceSegmentId = priceSegment.Id;
            Enabled = enabled;

            return this;
        }
    }
}