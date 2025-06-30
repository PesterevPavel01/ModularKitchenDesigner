using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("4.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ModuleController : ControllerBase
    {
        public ModuleController(IProcessorFactory moduleProcessorFactory)
        {
            _moduleProcessorFactory = moduleProcessorFactory;
        }

        private readonly IProcessorFactory _moduleProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _moduleProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Module, ModuleDto>, Module, ModuleDto>()
                .ProcessAsync(predicate: x => x.Enabled == true);

            if (result.IsSuccess) {            
                return Ok(
                    new CollectionResult<ModuleDto>() { 
                        Data = result.Data.OrderByDescending(x=>x.Width),
                        Count = result.Data.Count()
                    });        
            }

            return BadRequest();
        }

        [HttpGet("GetByType/{ModuleType}")]
        public async Task<IActionResult> GetByModuleType(String ModuleType)
        {
            var result = await _moduleProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Module, ModuleDto>, Module, ModuleDto>()
                .ProcessAsync(predicate: 
                    x => x.ModuleType.Title == ModuleType 
                    && x.Enabled == true);

            if (result.IsSuccess)
            {
                return Ok(
                    new CollectionResult<ModuleDto>()
                    {
                        Data = result.Data.OrderByDescending(x => x.Width),
                        Count = result.Data.Count()
                    });
            }

            return BadRequest();
        }

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetCreatorProcessor<CommonMultipleCreatorProcessor<Module, ModuleDto, ModuleConverter>, Module, ModuleDto>()
            .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> Update([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Module, ModuleDto, ModuleConverter>, Module, ModuleDto>()
            .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ModuleDto> models)
        => Ok(
            await _moduleProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<Module, ModuleDto>, Module, ModuleDto>()
            .ProcessAsync(models));
    }
}