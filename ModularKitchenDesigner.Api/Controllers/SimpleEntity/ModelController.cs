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
    public class ModelController : ControllerBase
    {
        public ModelController(ISimpleEntityService simpleEntityService)
        {
            _creator = simpleEntityService.GetCreator<Model, SimpleDto>();
            _updater = simpleEntityService.GetUpdater<Model, SimpleDto>();
            _loader = simpleEntityService.GetLoader<Model, SimpleDto>();
        }

        private readonly ISimpleEntityCreator<Model, SimpleDto> _creator;
        private readonly ISimpleEntityUpdater<Model, SimpleDto> _updater;
        private readonly ISimpleEntityLoader<Model, SimpleDto> _loader;

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
