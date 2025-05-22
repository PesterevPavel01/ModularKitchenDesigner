using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Component : Identity, IAuditable, IConvertibleToDto<Component, ComponentDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }

        public ComponentType ComponentType { get; set; }
        public Guid ComponentTypeId { get; set; }
        public PriceSegment PriceSegment { get; set; }
        public Guid PriceSegmentId { get; set; }
        public Material Material { get; set; }
        public Guid MaterialId { get; set; }
        public Model Model { get; set; }
        public Guid ModelId { get; set; }

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
        
        public Component ConvertFromDtoWithRequiredFields(ComponentDto model)
        {
            Code = model.Code;
            return this;
        }

        public static Func<IQueryable<Component>, IIncludableQueryable<Component, object>> IncludeRequaredField()
            =>  
            query => query
                .Include(x => x.ComponentType)
                .Include(x => x.PriceSegment)
                .Include(x => x.Material)
                .Include(x => x.Model);

    }
}