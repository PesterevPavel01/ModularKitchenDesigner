using System;
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
        public SectionController(IProcessorFactory sectionProcessorFactory)
        {
            _sectionProcessorFactory = sectionProcessorFactory;
        }

        private readonly IProcessorFactory _sectionProcessorFactory;

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _sectionProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Section, SectionDto>, Section, SectionDto>().ProcessAsync());

        [HttpGet("GetByKitchenCode/{KitchenCode}")]
        public async Task<IActionResult> GetByKitchenCode(string KitchenCode)
            => Ok(
                await _sectionProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Section, SectionDto>, Section, SectionDto>()
                .ProcessAsync(predicate: x => x.Kitchen.Code == KitchenCode));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<Section, SectionDto, SectionConverter>, Section, SectionDto>()
            .ProcessAsync(models));


        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Section, SectionDto, SectionConverter>, Section, SectionDto>()
            .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<SectionDto> models)
        => Ok(
            await _sectionProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<Section, SectionDto>, Section, SectionDto>()
            .ProcessAsync(models));
    }
}
