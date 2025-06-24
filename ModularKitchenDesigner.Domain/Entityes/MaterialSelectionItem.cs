using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class MaterialSelectionItem : BaseEntity, IAuditable, IDtoConvertible<MaterialSelectionItem, MaterialSelectionItemDto>
    {
        private MaterialSelectionItem(){}

        private MaterialSelectionItem(ComponentType componentType, Material material, KitchenType kitchenType, string code, string title = null, bool enabled = true) 
        {
            ComponentTypeId = componentType.Id;
            KitchenTypeId = kitchenType.Id;
            MaterialId = material.Id;
            Code = code ?? Guid.NewGuid().ToString();
            Enabled = enabled;
            Title = title is null ? "N/A" : title;
        }
        public ComponentType ComponentType { get; private set; }
        public Guid ComponentTypeId { get; private set; }
        public Material Material { get; private set; }
        public Guid MaterialId { get; private set; }
        public KitchenType KitchenType { get; private set; }
        public Guid KitchenTypeId { get; private set; }

        public List<MaterialSpecificationItem> MaterialSpecificationItems { get; set; }

        public static Func<IQueryable<MaterialSelectionItem>, IIncludableQueryable<MaterialSelectionItem, object>> IncludeRequaredField()
        => query => query
        .Include(x => x.Material)
        .Include(x => x.ComponentType)
        .Include(x => x.KitchenType)
        .Include(x => x.MaterialSpecificationItems);

        public bool IsUniqueKeyEqual(MaterialSelectionItemDto model)
            => this.ComponentType.Title == model.ComponentType
            && this.Material.Title == model.Material
            && this.KitchenType.Title == model.KitchenType;

        public static Expression<Func<MaterialSelectionItem, bool>> ContainsByUniqueKeyPredicate(List<MaterialSelectionItemDto> models)
            => entity
                => models.Select(model => model.ComponentType).Contains(entity.ComponentType.Title)
                && models.Select(model => model.Material).Contains(entity.Material.Title)
                && models.Select(model => model.KitchenType).Contains(entity.KitchenType.Title);

        public MaterialSelectionItemDto ConvertToDto()
        => new()
        {
            ComponentType = ComponentType.Title,
            Material = Material.Title,
            KitchenType = KitchenType.Title,
            Code = Code,
            Title = Title
        };

        public static MaterialSelectionItem Create(ComponentType componentType, Material material, KitchenType kitchenType, string code = null, string title = null, bool enabled = true)
            => new(componentType, material, kitchenType, code, title, enabled);

        public MaterialSelectionItem Update(ComponentType componentType, Material material, KitchenType kitchenType, string code = null, string title = null, bool enabled = true) 
        {
            ComponentTypeId = componentType.Id;
            MaterialId = material.Id;
            KitchenTypeId = kitchenType.Id;
            Code = code ?? Code;
            Enabled = enabled;
            Title = title is null ? "N/A" : title;

            return this;
        }
    }
}