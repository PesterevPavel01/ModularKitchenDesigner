using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Module : Identity, IAuditable, IConvertibleToDto<Module, ModuleDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string PreviewImageSrc { get; set; }
        public double Width { get; set; }

        public ModuleType Type { get; set; }
        public Guid ModuleTypeId { get; set; }
        public List<Section> Sections { get; set; } = [];
        public List<ModelItem> ModelItems { get; set; }

        public static Func<IQueryable<Module>, IIncludableQueryable<Module, object>> IncludeRequaredField()
        => query => query.Include(x => x.Type);

        public ModuleDto ConvertToDto()
        => new()
        {
            Title = Title,
            Code = Code,
            PreviewImageSrc = PreviewImageSrc,
            Width = Width,
            Type = Type.Title
        };
    }
}
