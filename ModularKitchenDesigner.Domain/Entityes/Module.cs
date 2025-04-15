using Interceptors;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Module : Identity, IAuditable
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
        public List<Block> Blocks { get; set; } = [];
        public List<ModelItem> ModelItems { get; set; }
    }
}
