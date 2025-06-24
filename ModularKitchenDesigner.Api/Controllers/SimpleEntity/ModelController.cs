using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("4.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ModelController : ControllerBase
    {
        public ModelController(IProcessorFactory modelProcessorFactory)
        {
            _modelProcessorFactory = modelProcessorFactory;
        }

        private readonly IProcessorFactory _modelProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(
                await _modelProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Model, ModelDto>, Model, ModelDto>()
                .ProcessAsync(predicate: x => x.Title != "default" && x.Enabled == true));

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(
                await _modelProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Model, ModelDto>, Model, ModelDto>()
                .ProcessAsync(predicate: x => x.Code == code && x.Enabled == true));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _modelProcessorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Model, ModelDto>, Model, ModelDto>()
                .ProcessAsync(predicate: x => x.Title == name && x.Enabled == true));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<ModelDto> models)
        => Ok(
            await _modelProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<Model, ModelDto>, Model, ModelDto>()
            .ProcessAsync(models));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<ModelDto> models)
           => Ok(
                await _modelProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<Model, ModelDto, ModelConverter>, Model, ModelDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<ModelDto> models)
            => Ok(
                await _modelProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Model, ModelDto, ModelConverter>, Model, ModelDto>()
                .ProcessAsync(models));
    }
}
