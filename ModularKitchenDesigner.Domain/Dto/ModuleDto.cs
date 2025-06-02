using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ModuleDto : BaseDto, IExcangeDtoConvertable<ModuleDto, NomanclatureDto>
    {
        public ModuleDto(){}
        public ModuleDto(Module module)
        {
            Title = module.Title;
            Code = module.Code;
            PreviewImageSrc = module.PreviewImageSrc;
            Width = module.Width;
            Type = module.ModuleType.Title;
        }

        private string _previewImageSrc;
        public string PreviewImageSrc 
        { 
            get => _previewImageSrc ?? "N/A";
            set => _previewImageSrc = value ?? "N/A";
        }
        public double Width { get; set; }
        public string Type { get; set; }

        public ModuleDto Convert(NomanclatureDto dto)
        {
            Title = dto.Title;
            Code = dto.Code;
            Width = dto.Widht;
            Type = dto.Parents[0].Title;

            return this;
        }
    }
}
