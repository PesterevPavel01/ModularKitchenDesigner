using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.SectionProcessors.SectionCretor;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SectionDto model)
            => Ok(await _sectionProcessorFactory.GetCreatorProcessor<SingleSectionCreatorProcessor, BaseResult<SectionDto>, SectionDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SectionDto> models)
            => Ok(await _sectionProcessorFactory.GetCreatorProcessor<MultipleSectionCreatorProcessor, CollectionResult<SectionDto>, List<SectionDto>>().ProcessAsync(models));


        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<SectionDto> models)
            => Ok(
                await _sectionProcessorFactory
                .GetUpdaterProcessor<CommonMultipleUpdaterProcessor<Section, SectionDto, SectionConverter>, CollectionResult<SectionDto>>()
                .ProcessAsync(
                    data: models,
                    predicate: entity => models.Select(model => model.KitchenGuid).Contains(entity.Kitchen.Id)
                            && models.Select(model => model.ModuleCode).Contains(entity.Module.Code)));
    }
}
