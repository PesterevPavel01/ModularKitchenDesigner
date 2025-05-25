using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Component : Identity, IAuditable, IDtoConvertible<Component, ComponentDto>
    {
        private Component() { }
        private Component(string title, string code, double price, ComponentType componentType, PriceSegment priceSegment, Material material, Model model)
        {
            Title = title;
            Code = code;
            Price = price;
            ComponentTypeId = componentType.Id;
            PriceSegmentId = priceSegment.Id;
            MaterialId = material.Id;
            ModelId = model.Id;

        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; private set; }
        public string Code { get; private set; }
        public double Price { get; private set; }

        public ComponentType ComponentType { get; private set; }
        public Guid ComponentTypeId { get; private set; }
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
                ComponentType = ComponentType.Title,
                PriceSegment = PriceSegment.Title,
                Material = Material.Title,
                Model = Model.Title,
            };

        public static Component Create(string title, string code, double price, ComponentType componentType, PriceSegment priceSegment, Material material, Model model)
        => new Component(title, code, price, componentType, priceSegment, material, model);

        public Component Update(string title, string code, double price, ComponentType componentType, PriceSegment priceSegment, Material material, Model model)
        {
            Title = title;
            Code = code;
            Price = price;
            ComponentTypeId = componentType.Id;
            PriceSegmentId = priceSegment.Id;
            MaterialId = material.Id;
            ModelId = model.Id;

            return this;
        }

        public static Func<IQueryable<Component>, IIncludableQueryable<Component, object>> IncludeRequaredField()
            =>  
            query => query
                .Include(x => x.ComponentType)
                .Include(x => x.PriceSegment)
                .Include(x => x.Material)
                .Include(x => x.Model);

        public bool isUniqueKeyEqual(ComponentDto model)
            => this.Code == model.Code;

        public static Expression<Func<Component, bool>> ContainsByUniqueKeyPredicate(List<ComponentDto> models)
            => entity => models.Select(model => model.Code).Contains(entity.Code);
    }
}