using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Services.Processors.MaterialItemProcessors.MaterialItemCreator;
using ModularKitchenDesigner.Application.Services.Processors.MaterialItemProcessors.MaterialItemLoader;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class MaterialItemController : ControllerBase
    {
        public MaterialItemController(IProcessorFactory<MaterialItem, MaterialItemDto> componentProcessorFactory)
        {
            _componentProcessorFactory = componentProcessorFactory;
        }

        private readonly IProcessorFactory<MaterialItem, MaterialItemDto> _componentProcessorFactory;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultMaterialItemLoaderProcessor>().ProcessAsync());

        [HttpGet("GetByKitchenType/{KitchenTypeTitle}")]
        public async Task<IActionResult> GetByKitchenType(String KitchenTypeTitle)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultMaterialItemLoaderProcessor>().ProcessAsync(predicate: x => x.KitchenType.Title == KitchenTypeTitle));

        [HttpGet("GetByComponentType/{ComponentTypeTitle}")]
        public async Task<IActionResult> GetByComponentType(String ComponentTypeTitle)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultMaterialItemLoaderProcessor>().ProcessAsync(predicate: x => x.ComponentType.Title == ComponentTypeTitle));

        [HttpGet("GetByKitchenTypeCode/{KitchenTypeCode}")]
        public async Task<IActionResult> GetByKitchenTypeCode(String KitchenTypeCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultMaterialItemLoaderProcessor>().ProcessAsync(predicate: x => x.KitchenType.Code == KitchenTypeCode));

        [HttpGet("GetByComponentTypeCode/{ComponentTypeCode}")]
        public async Task<IActionResult> GetByComponentTypeCode(String ComponentTypeCode)
            => Ok(await _componentProcessorFactory.GetLoaderProcessor<DefaultMaterialItemLoaderProcessor>().ProcessAsync(predicate: x => x.ComponentType.Code == ComponentTypeCode));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MaterialItemDto model)
            => Ok(await _componentProcessorFactory.GetCreatorProcessor<SingleMaterialItemCreatorProcessor>().ProcessAsync(model));
    }
}
