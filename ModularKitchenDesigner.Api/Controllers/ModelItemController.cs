using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.ModelItemProcessors.ModelItemCreators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ModelItemController : ControllerBase
    {
        public ModelItemController(IProcessorFactory<ModelItem, ModelItemDto> modelItemProcessorFactory)
        {
            _modelItemProcessorFactory = modelItemProcessorFactory;
        }

        private readonly IProcessorFactory<ModelItem, ModelItemDto> _modelItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync());

        [HttpGet("GetByModule/{ModuleTitle}")]
        public async Task<IActionResult> GetByModule(String ModuleTitle)
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync(predicate: x => x.Module.Title == ModuleTitle));

        [HttpGet("GetByModuleCode/{ModuleCode}")]
        public async Task<IActionResult> GetByModuleModuleCode(String ModuleCode)
            => Ok(await _modelItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>>().ProcessAsync(predicate: x => x.Module.Code == ModuleCode));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ModelItemDto model)
            => Ok(await _modelItemProcessorFactory.GetCreatorProcessor<SingleModelItemCreatorProcessor, BaseResult<ModelItemDto>, ModelItemDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModelItemDto> models)
            => Ok(await _modelItemProcessorFactory.GetCreatorProcessor<MultipleModelItemCreatorProcessor, CollectionResult<ModelItemDto>, List<ModelItemDto>>().ProcessAsync(models));
    }
}