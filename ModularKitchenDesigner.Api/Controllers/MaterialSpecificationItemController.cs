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
    [ApiVersion("1.0")]
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
        
        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<MaterialSpecificationItemDto> models)
        => Ok(
            await _materialSpecificationItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>>()
            .ProcessAsync(models));
    }
}
