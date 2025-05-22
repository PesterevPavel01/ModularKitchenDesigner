using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class SectionController : ControllerBase
    {
        public SectionController(IProcessorFactory<Section, SectionDto> sectionProcessorFactory)
        {
            _sectionProcessorFactory = sectionProcessorFactory;
        }

        private readonly IProcessorFactory<Section, SectionDto> _sectionProcessorFactory;

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        => Ok(await _sectionProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Section, SectionDto>>().ProcessAsync());

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<Section, SectionDto, SectionConverter>>()
            .ProcessAsync(
                data: models,
                predicate: entity => 
                    models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id)
                    && models.Select(model => model.ModuleCode).Contains(entity.Module.Code),
                findEntityByDto: model => entity => model.GetId() == entity.Id));


        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Section, SectionDto, SectionConverter>>()
            .ProcessAsync(
                data: models,
                predicate: entity =>
                    models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id)
                    && models.Select(model => model.ModuleCode).Contains(entity.Module.Code),
                findEntityByDto: model => entity => 
                    entity.KitchenId == model.KitchenGuid
                    && entity.Module.Code == model.ModuleCode));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetLoaderProcessor<CommonMultipleRemoveProcessor<Section, SectionDto>>()
            .ProcessAsync(
                predicate: entity =>
                    models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id)
                    && models.Select(model => model.ModuleCode).Contains(entity.Module.Code)));
    }
}
