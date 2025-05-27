using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Api.Controllers.Exchange
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService<NomanclatureDto> _exchangeService;
        public ExchangeController(IExchangeService<NomanclatureDto> exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateMultiple([FromBody] List<NomanclatureDto> models)
            => Ok(await _exchangeService.ExchangeAsync(models));

    }
}
