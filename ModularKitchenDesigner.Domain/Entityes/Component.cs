using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Component : BaseEntity, IAuditable, IDtoConvertible<Component, ComponentDto>
    {
        private Component() { }
        private Component(string title, string code, double price, PriceSegment priceSegment, Material material, Model model, bool enabled = true)
        {
            Title = title;
            Code = code;
            Price = price;
            PriceSegmentId = priceSegment.Id;
            MaterialId = material.Id;
            ModelId = model.Id;
            Enabled = enabled;
        }

        public double Price { get; private set; }

        public PriceSegment PriceSegment { get; private set; }
        public Guid PriceSegmentId { get; private set; }
        public Material Material { get; private set; }
        public Guid MaterialId { get; private set; }
        public Model Model { get; private set; }
        public Guid ModelId { get; private set; }

        public ComponentDto ConvertToDto()
            => new()
            {
                Title = Title,
                Code = Code,
                Price = Price,
                PriceSegment = PriceSegment.Title,
                Material = Material.Title,
                Model = Model.Title,
            };

        public static Component Create(string title, string code, double price, PriceSegment priceSegment, Material material, Model model, bool enabled = true)
            => new (title, code, price, priceSegment, material, model, enabled);

        public Component Update(string title, string code, double price, PriceSegment priceSegment, Material material, Model model, bool enabled = true)
        {
            Title = title;
            Code = code;
            Price = price;
            PriceSegmentId = priceSegment.Id;
            MaterialId = material.Id;
            ModelId = model.Id;
            Enabled = enabled;

            return this;
        }

        public static Func<IQueryable<Component>, IIncludableQueryable<Component, object>> IncludeRequaredField()
            =>  
            query => query
                .Include(x => x.PriceSegment)
                .Include(x => x.Material)
                .Include(x => x.Model);

        public bool IsUniqueKeyEqual(ComponentDto model)
            => this.Code == model.Code;

        public static Expression<Func<Component, bool>> ContainsByUniqueKeyPredicate(List<ComponentDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);
    }
}