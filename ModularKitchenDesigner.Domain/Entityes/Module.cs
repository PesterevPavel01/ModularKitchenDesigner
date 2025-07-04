﻿using System.Linq.Expressions;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public sealed class Module : BaseEntity, IAuditable, IDtoConvertible<Module, ModuleDto>
    {
        private Module(){}

        private Module(string title, string code, string previewImageSrc, double width, ModuleType moduleType, bool enabled = true)
        {
            Title = title;
            Code = code;
            PreviewImageSrc = previewImageSrc;
            Width = width;
            ModuleTypeId = moduleType.Id;
            Enabled = enabled;
        }
        public string PreviewImageSrc { get; private set; }
        public double Width { get; private set; }

        public ModuleType ModuleType { get; private set; }
        public Guid ModuleTypeId { get; private set; }
        public List<Section> Sections { get; private set; }
        public List<ModelItem> ModelItems { get; private set; }

        public static Func<IQueryable<Module>, IIncludableQueryable<Module, object>> IncludeRequaredField()
        => query => query.Include(x => x.ModuleType)
                         .Include(x => x.Sections)
                         .Include(x => x.ModelItems);

        public bool IsUniqueKeyEqual(ModuleDto model)
            => this.Code == model.Code;

        public bool containsByUniqueKey(List<ModuleDto> models)
           => models.Select(model => model.Code).Contains(this.Code);

        public static Expression<Func<Module, bool>> ContainsByUniqueKeyPredicate(List<ModuleDto> models)
            => entity
                => models.Select(model => model.Code).Contains(entity.Code);

        public ModuleDto ConvertToDto()
        => new()
        {
            Title = Title,
            Code = Code,
            PreviewImageSrc = PreviewImageSrc,
            Width = Width,
            Type = ModuleType.Title
        };

        public static Module Create(string title, string code, string previewImageSrc, double width, ModuleType moduleType, bool enabled = true)
            => new(title, code, previewImageSrc, width, moduleType, enabled);

        public Module Update(string title, string code, string previewImageSrc, double width, ModuleType moduleType, bool enabled = true)
        {
            Title = title;
            Code = code;
            PreviewImageSrc = previewImageSrc;
            Width = width;
            ModuleTypeId = moduleType.Id;
            Enabled = enabled;

            return this;
        }
    }
}
