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
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class MaterialSpecificationItemController : ControllerBase
    {
        public MaterialSpecificationItemController(IProcessorFactory materialSpecificationItemProcessorFactory)
        {
            _materialSpecificationItemProcessorFactory = materialSpecificationItemProcessorFactory;
        }

        private readonly IProcessorFactory _materialSpecificationItemProcessorFactory;

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _materialSpecificationItemProcessorFactory.GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>, MaterialSpecificationItem, MaterialSpecificationItemDto>().ProcessAsync());
        
        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleCreatorProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>, MaterialSpecificationItem, MaterialSpecificationItemDto>()
                .ProcessAsync(models));

        [HttpPost("UpdateMultiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] List<MaterialSpecificationItemDto> models)
            => Ok(
                await _materialSpecificationItemProcessorFactory
                .GetCreatorProcessor<CommonMultipleUpdaterProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto, MaterialSpecificationItemConverter>, MaterialSpecificationItem, MaterialSpecificationItemDto>()
                .ProcessAsync(models));

        [HttpPost("RemoveMultiple")]
        public async Task<IActionResult> Remove([FromBody] List<MaterialSpecificationItemDto> models)
        => Ok(
            await _materialSpecificationItemProcessorFactory
            .GetCreatorProcessor<CommonMultipleRemoveProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>, MaterialSpecificationItem, MaterialSpecificationItemDto>()
            .ProcessAsync(models));
    }
}
