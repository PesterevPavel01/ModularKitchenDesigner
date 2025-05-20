using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Application.Processors.MaterialSpecificationItemProcessors.MaterialSpecificationItemCreator;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class MaterialSpecificationItemController : ControllerBase
    {
        public MaterialSpecificationItemController(IProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto> materialSpecificationItemProcessorFactory)
        {
            _materialSpecificationItemProcessorFactory = materialSpecificationItemProcessorFactory;
        }

        private readonly IProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto> _materialSpecificationItemProcessorFactory;

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSpecificationItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>>().ProcessAsync());

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MaterialSpecificationItemDto model)
            => Ok(await _materialSpecificationItemProcessorFactory.GetCreatorProcessor<SingleMaterialSpecificationItemCreatorProcessor, BaseResult<MaterialSpecificationItemDto>, MaterialSpecificationItemDto>().ProcessAsync(model));

        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(await _materialSpecificationItemProcessorFactory.GetCreatorProcessor<MultipleMaterialSpecificationItemCreatorProcessor, CollectionResult<MaterialSpecificationItemDto>, List<MaterialSpecificationItemDto>>().ProcessAsync(models));
    }
}
