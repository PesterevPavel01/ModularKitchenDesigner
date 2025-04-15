using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenCreators;
using ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenLoaders;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class KitchenController : ControllerBase
    {
        public KitchenController(IProcessorFactory<Kitchen, KitchenDto> kitchenProcessorFactory)
        {
            _kitchenProcessorFactory = kitchenProcessorFactory;
        }

        private readonly IProcessorFactory<Kitchen, KitchenDto> _kitchenProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<DefaultKitchenLoaderProcessor>().ProcessAsync());

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<DefaultKitchenLoaderProcessor>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByCode/{Code}")]
        public async Task<IActionResult> GetByCode(String Code)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<DefaultKitchenLoaderProcessor>().ProcessAsync(predicate: x => x.Code == Code));

        [HttpGet("GetByTitle/{Title}")]
        public async Task<IActionResult> GetByTitle(String Title)
    => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<DefaultKitchenLoaderProcessor>().ProcessAsync(predicate: x => x.Title == Title));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] KitchenDto model)
            => Ok(await _kitchenProcessorFactory.GetCreatorProcessor<SingleKitchenCreatorProcessor>().ProcessAsync(model));
    }
}