using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.BlockProcessor.BlockCreator;
using ModularKitchenDesigner.Application.Services.Processors.BlockProcessor.BlockLoader;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class BlockController : ControllerBase
    {
        public BlockController(IProcessorFactory<Block, BlockDto> componentProcessorFactory)
        {
            _componentProcessorFactory = componentProcessorFactory;
        }

        private readonly IProcessorFactory<Block, BlockDto> _componentProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultBlockLoaderProcessor>().ProcessAsync());

        [HttpGet("GetByModuleCode/{moduleCode}")]
        public async Task<IActionResult> GetByModuleCode(String moduleCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultBlockLoaderProcessor>().ProcessAsync(predicate: x => x.Module.Code == moduleCode));

        [HttpGet("GetByComponentCode/{componentCode}")]
        public async Task<IActionResult> GetByPriceSegmentTitle(String componentCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultBlockLoaderProcessor>().ProcessAsync(predicate: x => x.Component.Code == componentCode));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BlockDto model)
            => Ok(await _componentProcessorFactory.GetCreatorProcessor<SingleBlockCreatorProcessor>().ProcessAsync(model));
    }
}