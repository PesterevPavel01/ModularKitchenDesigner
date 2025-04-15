using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;

namespace ModularKitchenDesigner.Api.Controllers.SimpleEntity
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PriceSegmentController : ControllerBase
    {
        public PriceSegmentController(ISimpleEntityService simpleEntityService)
        {
            _creator = simpleEntityService.GetCreator<PriceSegment, SimpleDto>();
            _updater = simpleEntityService.GetUpdater<PriceSegment, SimpleDto>();
            _loader = simpleEntityService.GetLoader<PriceSegment, SimpleDto>();
        }

        private readonly ISimpleEntityCreator<PriceSegment, SimpleDto> _creator;
        private readonly ISimpleEntityUpdater<PriceSegment, SimpleDto> _updater;
        private readonly ISimpleEntityLoader<PriceSegment, SimpleDto> _loader;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
            => Ok($"URL: {Request.Path}");

        [HttpGet()]
        public async Task<IActionResult> GetAll()
            => Ok(await _loader.GetAll());

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
            => Ok(await _loader.GetByUniquePropertyAsync(predicate: x => x.Code == code));

        [HttpGet("GetByTitle/{name}")]
        public async Task<IActionResult> GetByTitle(string name)
            => Ok(await _loader.GetByUniquePropertyAsync(predicate: x => x.Title == name));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SimpleDto model)
            => Ok(await _creator.CreateAsync(model));

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SimpleDto model)
            => Ok(await _updater.UpdateAsync(model));
    }
}
