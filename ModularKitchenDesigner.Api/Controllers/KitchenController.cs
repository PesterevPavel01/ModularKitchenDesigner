using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.KitchenProcessors.KitchenCreators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

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
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen,KitchenDto>>().ProcessAsync());

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByCode/{Code}")]
        public async Task<IActionResult> GetByCode(String Code)
            => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>>().ProcessAsync(predicate: x => x.UserId == Code));

        [HttpGet("GetByTitle/{Title}")]
        public async Task<IActionResult> GetByTitle(String Title)
    => Ok(await _kitchenProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>>().ProcessAsync(predicate: x => x.UserLogin == Title));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] KitchenDto model)
            => Ok(await _kitchenProcessorFactory.GetCreatorProcessor<SingleKitchenCreatorProcessor, BaseResult<KitchenDto>, KitchenDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<KitchenDto> models)
            => Ok(await _kitchenProcessorFactory.GetCreatorProcessor<MultipleKitchenCreatorProcessor, CollectionResult<KitchenDto>, List<KitchenDto>>().ProcessAsync(models));
    }
}