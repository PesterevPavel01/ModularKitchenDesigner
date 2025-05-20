using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.MaterialSelectionItemProcessors.MaterialSelectionItemCreators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class MaterialSelectionItemController : ControllerBase
    {
        public MaterialSelectionItemController(IProcessorFactory<MaterialSelectionItem, MaterialSelectionItemDto> materialSelectionItemProcessorFactory)
        {
            _materialSelectionItemProcessorFactory = materialSelectionItemProcessorFactory;
        }

        private readonly IProcessorFactory<MaterialSelectionItem, MaterialSelectionItemDto> _materialSelectionItemProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync());

        [HttpGet("GetByKitchenType/{KitchenTypeTitle}")]
        public async Task<IActionResult> GetByKitchenType(String KitchenTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.KitchenType.Title == KitchenTypeTitle));

        [HttpGet("GetByComponentType/{ComponentTypeTitle}")]
        public async Task<IActionResult> GetByComponentType(String ComponentTypeTitle)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.ComponentType.Title == ComponentTypeTitle));

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByComponentTypeCode/{ComponentTypeCode}")]
        public async Task<IActionResult> GetByComponentTypeCode(String ComponentTypeCode)
            => Ok(await _materialSelectionItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>>().ProcessAsync(predicate: x => x.ComponentType.Code == ComponentTypeCode));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MaterialSelectionItemDto model)
            => Ok(await _materialSelectionItemProcessorFactory.GetCreatorProcessor<SingleMaterialSelectionItemCreatorProcessor, BaseResult<MaterialSelectionItemDto>, MaterialSelectionItemDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSelectionItemDto> models)
            => Ok(await _materialSelectionItemProcessorFactory.GetCreatorProcessor<MultipleMaterialSelectionItemCreatorProcessor, CollectionResult<MaterialSelectionItemDto>, List<MaterialSelectionItemDto>>().ProcessAsync(models));
    }
}
