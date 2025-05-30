﻿using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class ModuleDto
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

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }

        private string _previewImageSrc;
        public string PreviewImageSrc 
        { 
            get => _previewImageSrc ?? "N/A";
            set => _previewImageSrc = value ?? "N/A";
        }
        public double Width { get; set; }
        public string Type { get; set; }
    }
}
